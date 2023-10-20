package storage

import (
	"crypto/aes"
	"crypto/cipher"
	"database/sql"
	"fmt"
	repository "github.com/edvardsanta/SimplePasswordManager/data"
	_ "github.com/lib/pq"
	"golang.org/x/crypto/bcrypt"
	"log"
	"sync"
)

/*
WARNING: The information is not currently being encrypted. It is necessary to implement a password encryption algorithm.
  bcrypt
  Argon2
  PBKDF2
  scrypt
  SHA-256 with salt
  AES (Advanced Encryption Standard)
  RSA (Rivest-Shamir-Adleman)
  Blowfish
  Twofish
  Salsa20
*/

type Storage struct {
	//Read-write mutex, allowing sync of shared resouces (ensuring that only one goroutine can hold the mutex at a time.)
	mutex sync.RWMutex

	//dictionary-like
	passwords map[string]string
	db        *sql.DB
}

func NewStorage() (*Storage, error) {
	repo, err := repository.Connect()
	if err != nil {
		log.Fatalf("Failed to create PostgreRepository: %v", err)
	}
	// defer repo.DB.Close()

	return &Storage{
		passwords: make(map[string]string),
		db:        repo.DB,
	}, nil
}

// (storage *Storage) = means Set belongs to Storage struct
func (storage *Storage) Set(username, password string) {
	storage.mutex.Lock()

	/*defers this RUnlock untill Set returns/finishes,
	ensuring it will be unlocked even if an error occurs
	*/
	defer storage.mutex.Unlock()

	storage.passwords[username] = password
	// query := `
	// 	CREATE TABLE IF NOT EXISTS users (
	// 		id SERIAL PRIMARY KEY,
	// 		username VARCHAR(255) UNIQUE NOT NULL,
	// 		password VARCHAR(255) NOT NULL
	// 	)
	// `
	hash, erro := bcrypt.GenerateFromPassword([]byte(password), bcrypt.DefaultCost)
	if erro != nil {
		fmt.Println("Error generating bcrypt hash:", erro)
		return
	}
	fmt.Println("Bcrypt hash:", string(hash))
	_, err := storage.db.Exec("INSERT INTO users (username, password) VALUES ($1, $2)", username, password)
	if err != nil {
		fmt.Printf("Error inserting data into PostgreSQL: %v\n", err)
	}
}

func (storage *Storage) Get(username string) (string, bool) {
	storage.mutex.RLock()
	defer storage.mutex.RUnlock()

	var password string
	err := storage.db.QueryRow("SELECT password FROM users WHERE username = $1", username).Scan(&password)
	if err != nil {
		if err == sql.ErrNoRows {
			return "", false
		}

		fmt.Printf("Error querying data from PostgreSQL: %v\n", err)
		return "", false
	}

	password, found := storage.passwords[username]
	return password, found
}

func deriveKeyFromMasterPassword(masterPassword string) []byte {
	return []byte(masterPassword)
}

func encrypt(plaintext []byte, key []byte, nonce []byte) ([]byte, error) {
	block, err := aes.NewCipher(key)
	if err != nil {
		return nil, err
	}

	aesGCM, err := cipher.NewGCM(block)
	if err != nil {
		return nil, err
	}

	ciphertext := aesGCM.Seal(nil, nonce, plaintext, nil)
	return ciphertext, nil
}

func decrypt(ciphertext []byte, key []byte, nonce []byte) ([]byte, error) {
	block, err := aes.NewCipher(key)
	if err != nil {
		return nil, err
	}

	aesGCM, err := cipher.NewGCM(block)
	if err != nil {
		return nil, err
	}

	plaintext, err := aesGCM.Open(nil, nonce, ciphertext, nil)
	if err != nil {
		return nil, err
	}

	return plaintext, nil
}
