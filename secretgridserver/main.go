package main

import (
	"context"
	"github.com/redis/go-redis/v9"
	"net/http"
)

var rdb = redis.NewClient(&redis.Options{
	Addr:     "localhost:6379",
	Password: "", // no password set
	DB:       0,  // use default DB
})

var ctx = context.Background()

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

func main() {
	m := http.NewServeMux()

	m.HandleFunc("/enroll", handleEnroll) // 신규 가입
	m.HandleFunc("/login", handleLogin) // 로그인 (1단계)
	m.HandleFunc("/clientSessionProof", handleClientSessionProof) // 로그인 (2단계)
	m.HandleFunc("/score", handleScore) // 점수 등록 및 랭킹 조회

	var server *http.Server
	server = &http.Server{Addr: ":24110", Handler: m}
	err := server.ListenAndServe()
	if err != nil {
		panic(err)
	}
}

