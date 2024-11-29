package main

import (
	"crypto/rand"
	"encoding/base64"
	"fmt"
	"github.com/redis/go-redis/v9"
	"io"
	"log"
	"net/http"
	"strconv"
	"strings"
)

func handleMessage(writer http.ResponseWriter, request *http.Request) {
	userId := request.Header.Get("X-User-Id")
	if len(userId) == 0 {
		writer.WriteHeader(400)
		return
	}

	s := serverSessionMap[userId]
	if s == nil || s.cipher.block == nil {
		writer.WriteHeader(400)
		return
	}

	b, err := io.ReadAll(request.Body)
	if err != nil {
		writer.WriteHeader(400)
		return
	}

	body := string(b[:])

	bodyTokens := strings.Split(body, "&")
	if len(bodyTokens) != 2 {
		writer.WriteHeader(400)
		return
	}

	ciphertext, err := base64.StdEncoding.DecodeString(bodyTokens[0])
	if err != nil {
		writer.WriteHeader(400)
		return
	}

	iv, err := base64.StdEncoding.DecodeString(bodyTokens[1])
	if err != nil {
		writer.WriteHeader(400)
		return
	}

	plaintext, err := s.cipher.decrypt(ciphertext, iv)
	if err != nil {
		writer.WriteHeader(400)
		return
	}

	plaintextStr := string(plaintext[:])

	// [메시지 번호]\t[메시지 본문]\t[옵션1]\t[옵션2]... 형태여야만 한다.
	messageTokens := strings.Split(plaintextStr, "\t")
	if len(messageTokens) < 2 {
		writer.WriteHeader(400)
		return
	}

	// 메시지 번호 파싱
	messageCounter, err := strconv.Atoi(messageTokens[0])
	if err != nil {
		writer.WriteHeader(400)
		return
	}

	// 메시지 번호 확인 (중복 메시지 번호로 온 것은 무시해야한다.)
	if messageCounter != serverSessionMap[userId].counter + 1 {
		writer.WriteHeader(400)
		return
	}

	// 유효한 메시지

	// 닉네임 조회
	nickname, err := rdb.HGet(ctx, "secretGrid:nickname", userId).Result()
	if err == redis.Nil {
		nickname = "---"
	} else if err != nil {
		writer.WriteHeader(500)
		return
	}

	log.Printf("RECV %s (%s): %s", userId, nickname, messageTokens[1])

	// 다음 메시지 번호 처리를 위해 1 증가
	serverSessionMap[userId].counter = serverSessionMap[userId].counter + 1

	cmd := messageTokens[1]
	switch cmd {
	case "SetNickname":
		// 닉네임 설정
		if len(messageTokens) > 2 {
			nickname = messageTokens[2]
			if len(nickname) > 32 {
				nickname = nickname[:32]
			}
			_, _ = rdb.HSet(ctx, "secretGrid:nickname", userId, nickname).Result()
		} else {
			writer.WriteHeader(400)
			return
		}
	default:
		writer.WriteHeader(400)
		return
	}

	responseIv := make([]byte, 16)
	// then we can call rand.Read.
	_, err = rand.Read(responseIv)

	replyCiphertext, err := s.cipher.encrypt([]byte(fmt.Sprintf("%d\tOK", messageCounter)), responseIv)
	if err != nil {
		writer.WriteHeader(500)
		return
	}

	writer.WriteHeader(200)
	_, _ = writer.Write([]byte(base64.StdEncoding.EncodeToString(replyCiphertext) + "&" + base64.StdEncoding.EncodeToString(responseIv)))
}
