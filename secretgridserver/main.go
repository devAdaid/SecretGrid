package main

import (
	"context"
	"github.com/gin-contrib/cors"
	"github.com/gin-gonic/gin"
	"github.com/redis/go-redis/v9"
	"time"
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

func main() {
	router := gin.Default()

	router.Use(cors.New(
		cors.Config{
			AllowOrigins: []string{"http://localhost:55247"},
			AllowMethods: []string{"POST"},
			AllowHeaders: []string{"X-User-Id", "X-Client-Session-Proof", "X-Salt", "X-Verifier", "X-Public", "X-Stage-Id", "X-Score", "X-User-Nickname"},
			MaxAge:       12 * time.Hour,
		}))

	router.POST("/enroll", handleEnroll)
	router.POST("/login", handleLogin)                           // 로그인 (1단계)
	router.POST("/clientSessionProof", handleClientSessionProof) // 로그인 (2단계)
	router.POST("/message", handleMessage)                       // 보안 메시지
	router.POST("/score", handleScore)                           // 점수 등록 및 랭킹 조회

	err := router.Run(":24110")
	if err != nil {
		panic(err)
	}
}
