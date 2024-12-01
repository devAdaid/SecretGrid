package main

import (
	"crypto/aes"
	"crypto/cipher"
	"errors"
	"fmt"
)

type SimpleCipher struct {
	block cipher.Block
}

func (c *SimpleCipher) Init(aesKey []byte) error {
	block, err := aes.NewCipher(aesKey)
	if err != nil {
		return err
	}

	c.block = block
	return nil
}

func (c *SimpleCipher) encrypt(data []byte, aesIV []byte) ([]byte, error) {
	padding := aes.BlockSize - len(data)%aes.BlockSize
	padtext := make([]byte, padding)
	for i := range padtext {
		padtext[i] = byte(padding)
	}
	plaintext := append(data, padtext...)
	ciphertext := make([]byte, len(plaintext))
	mode := cipher.NewCBCEncrypter(c.block, aesIV)
	mode.CryptBlocks(ciphertext, plaintext)
	return ciphertext, nil
}

func (c *SimpleCipher) decrypt(data []byte, aesIV []byte) (b []byte, err error) {
	defer func() {
		if r := recover(); r != nil {
			fmt.Println("Recovered in decrypt", r)
			b = nil
			err = errors.New("decrypt error")
		}
	}()

	plaintext := make([]byte, len(data))
	mode := cipher.NewCBCDecrypter(c.block, aesIV)
	mode.CryptBlocks(plaintext, data)
	// 패딩 제거
	padding := int(plaintext[len(plaintext)-1])
	return plaintext[:len(plaintext)-padding], nil
}
