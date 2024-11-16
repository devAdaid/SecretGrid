package main

import (
	"context"
	"encoding/base64"
	"fmt"
	"github.com/redis/go-redis/v9"
	"net/http"
	"strconv"
	"strings"
)

var rdb = redis.NewClient(&redis.Options{
	Addr:     "localhost:6379",
	Password: "", // no password set
	DB:       0,  // use default DB
})

var ctx = context.Background()

func main() {
	m := http.NewServeMux()

	m.HandleFunc("/score", handleScore)

	var server *http.Server
	server = &http.Server{Addr: ":24110", Handler: m}
	err := server.ListenAndServe()
	if err != nil {
		panic(err)
	}
}

func max(x, y int64) int64 {
	if x > y {
		return x
	}
	return y
}

// NOTE: this isn't multi-Unicode-codepoint aware, like specifying skin tone or
//
//	gender of an emoji: https://unicode.org/emoji/charts/full-emoji-modifiers.html
func substr(input string, start int, length int) string {
	asRunes := []rune(input)

	if start >= len(asRunes) {
		return ""
	}

	if start+length > len(asRunes) {
		length = len(asRunes) - start
	}

	return string(asRunes[start : start+length])
}

func handleScore(writer http.ResponseWriter, request *http.Request) {
	userId := request.Header.Get("X-User-Id")
	if len(userId) == 0 {
		writer.WriteHeader(400)
		return
	}

	stageIdStr := request.Header.Get("X-Stage-Id")
	if len(stageIdStr) == 0 {
		writer.WriteHeader(400)
		return
	}

	scoreStr := request.Header.Get("X-Score")
	if len(scoreStr) == 0 {
		writer.WriteHeader(400)
		return
	}

	score, err := strconv.Atoi(scoreStr)
	if err != nil {
		score = 0
	}

	userNicknameBase64 := request.Header.Get("X-User-Nickname")
	userNicknameStr, _ := base64.StdEncoding.DecodeString(userNicknameBase64)

	const nicknameSetKey = "secretGrid:nickname"
	rankKey := "secretGrid:rank:" + stageIdStr

	_, _ = rdb.HSet(ctx, nicknameSetKey, userId, userNicknameStr).Result()

	var z redis.Z
	z.Score = float64(score)
	z.Member = userId
	rdb.ZAdd(ctx, rankKey, z)

	rank := rdb.ZRank(ctx, rankKey, userId)

	rank64 := rank.Val()

	var spread int64
	spread = 7
	rankRange := rdb.ZRangeWithScores(ctx, rankKey, max(0, rank64-spread), rank64+spread)
	s := []string{strconv.Itoa(int(rank64))}
	for _, element := range rankRange.Val() {
		memberStr := element.Member.(string)

		nn, _ := rdb.HGet(ctx, nicknameSetKey, memberStr).Result()

		memberStr = substr(memberStr, 0, 16)
		s = append(s, memberStr, fmt.Sprintf("%f", element.Score), nn)
	}
	ss := strings.Join(s, "\t")
	_, _ = writer.Write([]byte(ss))
}
