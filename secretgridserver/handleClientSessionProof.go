package main

import (
	"encoding/hex"
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

	server := srpServerMap[userId]
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

	srpM2, err := server.CheckM1(clientSessionProofBytes)
	if err != nil {
		writer.WriteHeader(401)
		return
	}

	_, _ = writer.Write([]byte(hex.EncodeToString(srpM2)))
}
