package main

import (
	"encoding/hex"
	"fmt"
	"log"
	"net/http"
)

func handleClientSessionProof(writer http.ResponseWriter, request *http.Request) {
	userId := request.Header.Get("X-User-Id")
	if len(userId) == 0 {
		writer.WriteHeader(400)
		return
	}

	clientSessionProof := request.Header.Get("X-Client-Session-Proof")
	if len(clientSessionProof) == 0 {
		writer.WriteHeader(400)
		return
	}

	server := serverSessionMap[userId]
	if server == nil {
		log.Println("Couldn't set up server")
		writer.WriteHeader(500)
		return
	}

	clientSessionProofBytes, err := hex.DecodeString(clientSessionProof)
	if err != nil {
		writer.WriteHeader(400)
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

	fmt.Println("sharedK", sharedKHex, len(server.sharedK), "bytes")

	c := SimpleCipher{}
	err = c.Init(server.sharedK)
	if err != nil {
		log.Println("Unknown error while creating cipher:", err)
	} else {
		_, _ = rdb.HSet(ctx, "secretGrid:k", userId, sharedKHex).Result()
		serverSessionMap[userId].cipher = c
	}

	_, _ = writer.Write([]byte(hex.EncodeToString(serverM2)))
}
