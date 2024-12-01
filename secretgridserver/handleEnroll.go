package main

import (
	"github.com/gin-gonic/gin"
)

func handleEnroll(c *gin.Context) {
	userId := c.GetHeader("X-User-Id")
	if len(userId) == 0 {
		c.Writer.WriteHeader(400)
		return
	}

	salt := c.GetHeader("X-Salt")
	if len(salt) == 0 {
		c.Writer.WriteHeader(400)
		return
	}

	verifier := c.GetHeader("X-Verifier")
	if len(verifier) == 0 {
		c.Writer.WriteHeader(400)
		return
	}

	verifierExists, err := rdb.HExists(ctx, "secretGrid:verifier", userId).Result()
	if err != nil {
		c.Writer.WriteHeader(400)
		return
	}

	// 이미 존재하는 User ID에 대해 다시 왔다면... (일반적으론 그럴 수 없지만)
	// 그냥 오류 아닌 걸로 치자. 클라이언트에서는 매번 enroll을 호출한다. 신규 가입 절차가 따로 없기 때문.
	if verifierExists {
		c.Writer.WriteHeader(200)
		return
	}

	_, _ = rdb.HSet(ctx, "secretGrid:verifier", userId, verifier).Result()
	_, _ = rdb.HSet(ctx, "secretGrid:salt", userId, salt).Result()

	_, _ = c.Writer.WriteString("OK")
}
