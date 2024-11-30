package main

import (
	"encoding/base64"
	"fmt"
	"github.com/redis/go-redis/v9"
	"net/http"
	"strconv"
	"strings"
)

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
		writer.WriteHeader(400)
		return
	}

	userNicknameBase64 := request.Header.Get("X-User-Nickname")
	userNicknameStr, _ := base64.StdEncoding.DecodeString(userNicknameBase64)

	const nicknameSetKey = "secretGrid:nickname"
	rankKey := "secretGrid:rank:" + stageIdStr

	_, _ = rdb.HSet(ctx, nicknameSetKey, userId, userNicknameStr).Result()

	if score >= 0 {
		var z redis.Z
		z.Score = float64(score)
		z.Member = userId
		rdb.ZAdd(ctx, rankKey, z)
	}

	rank, err := rdb.ZRank(ctx, rankKey, userId).Result()
	if err != nil && err != redis.Nil {
		writer.WriteHeader(400)
		return
	}

	if err == redis.Nil {
		rank = -1
	}

	var spread int64
	spread = 7
	rankRange := rdb.ZRangeWithScores(ctx, rankKey, max(0, rank-spread), rank+spread)

	/*

		s 변수는 최종적으로 다음과 같은 구조를 가진다.

		플레이어 순위 <tab> 플레이어ID <tab> 유저1 ID <tab> 점수1 <tab> 닉네임1 <tab> 유저2 ID <tab> 점수2 <tab> 닉네임2 <tab> ...

	*/
	s := []string{strconv.Itoa(int(rank)), userId}
	for _, element := range rankRange.Val() {
		memberStr := element.Member.(string)

		nn, _ := rdb.HGet(ctx, nicknameSetKey, memberStr).Result()

		s = append(s, memberStr, fmt.Sprintf("%f", element.Score), nn)
	}
	ss := strings.Join(s, "\t")
	_, _ = writer.Write([]byte(ss))
}

type Leaderboard struct {
	Rank    int64              `json:"rank,omitempty"`
	Entries []LeaderboardEntry `json:"entries,omitempty"`
}

type LeaderboardEntry struct {
	Rank     int64   `json:"rank,omitempty"`
	Nickname string  `json:"nickname,omitempty"`
	Score    float64 `json:"score,omitempty"`
}
