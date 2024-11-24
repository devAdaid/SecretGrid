package main

import (
	"encoding/hex"
	"fmt"
	"github.com/kong/go-srp"
	"log"
	"math/big"
	"net/http"
)

func toHexInt(n *big.Int) string {
	return fmt.Sprintf("%x", n) // or %x or upper case
}

var srpServerMap = make(map[string]*srp.SRPServer)

func handleLogin(writer http.ResponseWriter, request *http.Request) {
	userId := request.Header.Get("X-User-Id")
	if len(userId) == 0 {
		writer.WriteHeader(400)
		return
	}

	clientPublic := request.Header.Get("X-Public")
	if len(clientPublic) == 0 {
		writer.WriteHeader(400)
		return
	}

	salt, err := rdb.HGet(ctx, "secretGrid:salt", userId).Result()
	if err != nil {
		writer.WriteHeader(400)
		return
	}

	verifier, err := rdb.HGet(ctx, "secretGrid:verifier", userId).Result()
	if err != nil {
		writer.WriteHeader(400)
		return
	}

	params := srp.GetParams(4096)
	verifierBytes, err := hex.DecodeString(verifier)
	if err != nil {
		writer.WriteHeader(500)
		return
	}
	secret2 := srp.GenKey()
	server := srp.NewServer(params, verifierBytes, secret2)

	ABytes, err := hex.DecodeString(clientPublic)
	if err != nil {
		writer.WriteHeader(400)
		return
	}
	server.SetA(ABytes)

	B := server.ComputeB()
	if B == nil {
		log.Println("server couldn't make B")
		writer.WriteHeader(500)
		return
	}

	//_, _ = rdb.HSet(ctx, "secretGrid:serverKey", userId).Result()
	srpServerMap[userId] = server

	_, _ = writer.Write([]byte(salt + "\t" + hex.EncodeToString(B)))
}
