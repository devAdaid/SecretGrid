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

	if verifierExists {
		c.Writer.WriteHeader(401)
		return
	}

	_, _ = rdb.HSet(ctx, "secretGrid:verifier", userId, verifier).Result()
	_, _ = rdb.HSet(ctx, "secretGrid:salt", userId, salt).Result()

	_, _ = c.Writer.WriteString("OK")
}
