package main

import (
	"encoding/hex"
	"fmt"
	"github.com/gin-gonic/gin"
	"log"
)

func handleClientSessionProof(c *gin.Context) {
	userId := c.GetHeader("X-User-Id")
	if len(userId) == 0 {
		c.Writer.WriteHeader(400)
		return
	}

	clientSessionProof := c.GetHeader("X-Client-Session-Proof")
	if len(clientSessionProof) == 0 {
		c.Writer.WriteHeader(400)
		return
	}

	server := serverSessionMap[userId]
	if server == nil {
		log.Println("Couldn't set up server")
		c.Writer.WriteHeader(500)
		return
	}

	clientSessionProofBytes, err := hex.DecodeString(clientSessionProof)
	if err != nil {
		c.Writer.WriteHeader(400)
		return
	}

	// 서버가 클라이언트 증명 검증
	if !server.VerifyClient(clientSessionProofBytes, server.A) {
		fmt.Println("서버: 클라이언트 증명 검증 실패")
		fmt.Println("A", toHexInt(server.A))
		return
	}

	// 서버 증명 생성
	serverM2 := server.ComputeServerProof(clientSessionProofBytes, server.A)

	sharedKHex := hex.EncodeToString(server.sharedK)

	//fmt.Println("sharedK", sharedKHex, len(server.sharedK), "bytes")

	cipher := SimpleCipher{}
	err = cipher.Init(server.sharedK)
	if err != nil {
		log.Println("Unknown error while creating cipher:", err)
	} else {
		_, _ = rdb.HSet(ctx, "secretGrid:k", userId, sharedKHex).Result()
		serverSessionMap[userId].cipher = cipher
		serverSessionMap[userId].counter = 0
	}

	_, _ = c.Writer.WriteString(hex.EncodeToString(serverM2))
}
