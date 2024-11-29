package main

import (
	"crypto/rand"
	"crypto/sha256"
	"encoding/hex"
	"fmt"
	"math/big"
	"net/http"
)

// 큰 소수 N과 생성자 g를 설정합니다. 일반적으로 RFC 5054에서 제공하는 값을 사용합니다.
var N, _ = new(big.Int).SetString("EEAF0AB9ADB38DD69C33F80AFA8FC5E86072618775FF3C0B9EA2314C9C256576"+
	"D674DF7496EA81D3383B4813D692C6E0E0D5D8E2B3C18398A0AEB9F5E8E9A0F8"+
	"9D9E3E5FBCD091FEE1A5E1DAEE59D4F681B7A78BE1E81F7F1A0F5A0F48E0C6B"+
	"A6B05B2FF524A3B2E4B1FAFF8AC13834F02E3E1BE5B848D3D73F27EAB253D", 16)
var g = big.NewInt(2)
var k *big.Int

func initK() {
	// k = H(N || g)
	h := sha256.New()
	h.Write(N.Bytes())
	h.Write(g.Bytes())
	k = new(big.Int).SetBytes(h.Sum(nil))

	//fmt.Println("K", toHexInt(k))
}

// 4095 -> fff와 같이 반환되는 문자열 길이가 홀수인 경우가 있다는 점에 유의하자.
func toHexInt(n *big.Int) string {
	return fmt.Sprintf("%x", n) // or %x or upper case
}

func randomBigInt() (*big.Int, error) {
	b := make([]byte, 256)
	_, err := rand.Read(b)
	if err != nil {
		return nil, err
	}
	return new(big.Int).SetBytes(b), nil
}

// SHA256 해시 함수
func sha256Hash(data ...[]byte) []byte {
	h := sha256.New()
	for _, d := range data {
		h.Write(d)
	}
	return h.Sum(nil)
}

// 간단한 사용자 데이터베이스 (예시)
var serverSessionMap = make(map[string]*ServerSession)

// 서버의 임시 세션 정보
type ServerSession struct {
	A        *big.Int
	b        *big.Int
	B        *big.Int
	u        *big.Int
	sharedK  []byte
	username string
	salt     []byte
	v        *big.Int
	cipher   SimpleCipher
	counter  int
}

func (s *ServerSession) Step1(A *big.Int) error {
	s.A = A

	// u = H(A || B)
	uH := sha256Hash(A.Bytes(), s.B.Bytes())
	s.u = new(big.Int).SetBytes(uH)

	// S = (A * v^u) ^ b % N
	vu := new(big.Int).Exp(s.v, s.u, N)
	Avu := new(big.Int).Mul(A, vu)
	Avu.Mod(Avu, N)
	S := new(big.Int).Exp(Avu, s.b, N)

	// K = H(S)
	s.sharedK = sha256Hash(S.Bytes())
	return nil
}

func (s *ServerSession) VerifyClient(clientM []byte, A *big.Int) bool {
	// M = H(H(N) XOR H(g), H(username), salt, A, B, K)
	HN := sha256Hash(N.Bytes())
	Hg := sha256Hash(g.Bytes())
	HNxHg := make([]byte, len(HN))
	for i := range HN {
		HNxHg[i] = HN[i] ^ Hg[i]
	}

	HU := sha256Hash([]byte(s.username))

	M := sha256Hash(HNxHg, HU, s.salt, A.Bytes(), s.B.Bytes(), s.sharedK)
	return string(M) == string(clientM)
}

func (s *ServerSession) ComputeServerProof(M1 []byte, A *big.Int) []byte {
	// 서버 증명 M2 = H(A, M, K)
	M2 := sha256Hash(A.Bytes(), M1, s.sharedK)
	return M2
}

func handleLogin(writer http.ResponseWriter, request *http.Request) {
	initK()

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

	v, _ := new(big.Int).SetString(verifier, 16)

	// 서버: b, B 생성
	b, err := randomBigInt()
	if err != nil {
		fmt.Println("서버: b 생성 실패:", err)
		return
	}

	B := new(big.Int).Add(new(big.Int).Mul(k, v), new(big.Int).Exp(g, b, N))
	B.Mod(B, N)

	saltBytes, _ := hex.DecodeString(salt)

	// 서버 세션 생성 및 Step1 수행
	serverSession := &ServerSession{
		b:        b,
		B:        B,
		username: userId,
		salt:     saltBytes,
		v:        v,
	}

	A, _ := new(big.Int).SetString(clientPublic, 16)

	err = serverSession.Step1(A)
	if err != nil {
		fmt.Println("서버: Step1 실패:", err)
		return
	}

	serverSessionMap[userId] = serverSession

	_, _ = writer.Write([]byte(salt + "\t" + toHexInt(B)))
}
