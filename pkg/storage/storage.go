package storage

import "sync"

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
}

func NewStorage() *Storage {
	return &Storage{
		passwords: make(map[string]string),
	}
}

// (storage *Storage) = means Set belongs to Storage struct
func (storage *Storage) Set(username, password string) {
	storage.mutex.Lock()

	/*defers this RUnlock untill Set returns/finishes,
	ensuring it will be unlocked even if an error occurs
	*/
	defer storage.mutex.Unlock()

	storage.passwords[username] = password
}

func (s *Storage) Get(username string) (string, bool) {
	s.mutex.RLock()
	defer s.mutex.RUnlock()

	password, found := s.passwords[username]
	return password, found
}
