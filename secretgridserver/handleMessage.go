package main

import (
	"crypto/rand"
	"encoding/base64"
	"encoding/json"
	"fmt"
	"github.com/gin-gonic/gin"
	"github.com/redis/go-redis/v9"
	"io"
	"log"
	"strconv"
	"strings"
)

func handleMessage(c *gin.Context) {
	userId := c.GetHeader("X-User-Id")
	if len(userId) == 0 {
		c.Writer.WriteHeader(400)
		return
	}

	s := serverSessionMap[userId]
	if s == nil || s.cipher.block == nil {
		c.Writer.WriteHeader(400)
		return
	}

	b, err := io.ReadAll(c.Request.Body)
	if err != nil {
		c.Writer.WriteHeader(400)
		return
	}

	body := string(b[:])

	bodyTokens := strings.Split(body, "&")
	if len(bodyTokens) != 2 {
		c.Writer.WriteHeader(400)
		return
	}

	ciphertext, err := base64.StdEncoding.DecodeString(bodyTokens[0])
	if err != nil {
		c.Writer.WriteHeader(400)
		return
	}

	iv, err := base64.StdEncoding.DecodeString(bodyTokens[1])
	if err != nil {
		c.Writer.WriteHeader(400)
		return
	}

	plaintext, err := s.cipher.decrypt(ciphertext, iv)
	if err != nil {
		c.Writer.WriteHeader(400)
		return
	}

	plaintextStr := string(plaintext[:])

	// [메시지 번호]\t[메시지 본문]\t[옵션1]\t[옵션2]... 형태여야만 한다.
	messageTokens := strings.Split(plaintextStr, "\t")
	if len(messageTokens) < 2 {
		c.Writer.WriteHeader(400)
		return
	}

	// 메시지 번호 파싱
	messageCounter, err := strconv.Atoi(messageTokens[0])
	if err != nil {
		c.Writer.WriteHeader(400)
		return
	}

	// 메시지 번호 확인 (중복 메시지 번호로 온 것은 무시해야한다.)
	if messageCounter != serverSessionMap[userId].counter+1 {
		c.Writer.WriteHeader(400)
		return
	}

	// 유효한 메시지

	// 닉네임 조회 (미등록된 경우에는 ---로 설정)
	nickname, err := rdb.HGet(ctx, "secretGrid:nickname", userId).Result()
	if err == redis.Nil {
		nickname = "---"
	} else if err != nil {
		c.Writer.WriteHeader(500)
		return
	}

	log.Printf("RECV %s (%s): %s", userId, nickname, strings.Join(messageTokens[1:], ","))

	// 다음 메시지 번호 처리를 위해 1 증가
	serverSessionMap[userId].counter = serverSessionMap[userId].counter + 1

	// 회신할 메시지 본문
	replyBody := ""

	cmd := messageTokens[1]

	switch cmd {
	case "SetNickname":
		if len(messageTokens) > 2 {
			nickname = messageTokens[2]
			if len(nickname) > 32 {
				nickname = nickname[:32]
			}
			_, _ = rdb.HSet(ctx, "secretGrid:nickname", userId, nickname).Result()
			replyBody = "OK"
		} else {
			c.Writer.WriteHeader(400)
			return
		}
	case "GetLeaderboard":
		if len(messageTokens) > 2 {
			stageId := messageTokens[2]

			// 'Rev'가 붙으면 내림차순이다. (점수가 높은 사람이 1등)
			// rank는 0부터 시작한다. 즉, 1등이면 0
			rank, err := rdb.ZRevRank(ctx, "secretGrid:rank:"+stageId, userId).Result()
			if err != nil {
				if err == redis.Nil {
					rank = -1
				} else {
					c.Writer.WriteHeader(500)
					return
				}
			}

			var spread int64 = 7
			startRank := max(0, rank-spread)
			rankRange := rdb.ZRevRangeWithScores(ctx, "secretGrid:rank:"+stageId, startRank, rank+spread)

			var entries []LeaderboardEntry
			for i, e := range rankRange.Val() {
				eUserId := e.Member.(string)
				nn, _ := rdb.HGet(ctx, "secretGrid:nickname", eUserId).Result()
				entries = append(entries, LeaderboardEntry{
					Rank:     startRank + int64(i),
					UserId:   eUserId,
					Score:    e.Score,
					Nickname: nn,
				})
			}

			leaderboard := LeaderboardResult{
				MyRank:   rank,
				MyUserId: userId,
				Entries:  entries,
			}

			leaderboardJson, err := json.Marshal(leaderboard)
			if err != nil {
				c.Writer.WriteHeader(500)
				return
			}

			replyBody = string(leaderboardJson)

		} else {
			c.Writer.WriteHeader(400)
			return
		}
	case "SetScore":
		if len(messageTokens) > 2 {
			endingScore, err := strconv.Atoi(messageTokens[2])
			if err != nil {
				c.Writer.WriteHeader(400)
				return 
			}

			playTimeScore, err := strconv.Atoi(messageTokens[3])
			if err != nil {
				c.Writer.WriteHeader(400)
				return
			}

			statScore, err := strconv.Atoi(messageTokens[4])
			if err != nil {
				c.Writer.WriteHeader(400)
				return
			}

			totalScore, err := strconv.Atoi(messageTokens[5])
			if err != nil {
				c.Writer.WriteHeader(400)
				return
			}

			err = addToLeaderboard("secretGrid:rank:endingScore", userId, endingScore)
			if err != nil {
				c.Writer.WriteHeader(400)
				return
			}

			err = addToLeaderboard("secretGrid:rank:playTimeScore", userId, playTimeScore)
			if err != nil {
				c.Writer.WriteHeader(400)
				return
			}

			err = addToLeaderboard("secretGrid:rank:statScore", userId, statScore)
			if err != nil {
				c.Writer.WriteHeader(400)
				return
			}

			err = addToLeaderboard("secretGrid:rank:totalScore", userId, totalScore)
			if err != nil {
				c.Writer.WriteHeader(400)
				return
			}
		} else {
			c.Writer.WriteHeader(400)
			return
		}
	default:
		c.Writer.WriteHeader(400)
		return
	}

	responseIv := make([]byte, 16)
	_, err = rand.Read(responseIv)

	replyCiphertext, err := s.cipher.encrypt([]byte(fmt.Sprintf("%d\t%s", messageCounter, replyBody)), responseIv)
	if err != nil {
		c.Writer.WriteHeader(500)
		return
	}

	c.Writer.WriteHeader(200)
	_, _ = c.Writer.WriteString(base64.StdEncoding.EncodeToString(replyCiphertext) + "&" + base64.StdEncoding.EncodeToString(responseIv))
}

func addToLeaderboard(key string, userId string, score int) error {
	var z redis.Z
	z.Score = float64(score)
	z.Member = userId
	_, err := rdb.ZAdd(ctx, key, z).Result()
	return err
}
