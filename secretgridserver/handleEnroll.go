package main

import (
	"net/http"
)

func handleEnroll(writer http.ResponseWriter, request *http.Request) {
	userId := request.Header.Get("X-User-Id")
	if len(userId) == 0 {
		writer.WriteHeader(400)
		return
	}

	salt := request.Header.Get("X-Salt")
	if len(salt) == 0 {
		writer.WriteHeader(400)
		return
	}

	verifier := request.Header.Get("X-Verifier")
	if len(verifier) == 0 {
		writer.WriteHeader(400)
		return
	}

	verifierExists, err := rdb.HExists(ctx, "secretGrid:verifier", userId).Result()
	if err != nil {
		writer.WriteHeader(400)
		return
	}

	if verifierExists {
		writer.WriteHeader(401)
		return
	}

	_, _ = rdb.HSet(ctx, "secretGrid:verifier", userId, verifier).Result()
	_, _ = rdb.HSet(ctx, "secretGrid:salt", userId, salt).Result()

	_, _ = writer.Write([]byte("OK"))
}
