package storage_test

import (
	"testing"

	storage "github.com/edvardsanta/SimplePasswordManager/pkg/storage"
)

func TestStorageSetAndGet(t *testing.T) {
	//Arrange
	s := storage.NewStorage()

	username := "john"
	password := "password123"

	//Act
	s.Set(username, password)
	gotPassword, found := s.Get(username)
	if !found {
		t.Errorf("Expected to find password for username '%s', but it was not found", username)
	}

	// Verify the password
	if gotPassword != password {
		t.Errorf("Expected password '%s' for username '%s', but got '%s'", password, username, gotPassword)
	}
}

func TestStorageGetNonexistentUsername(t *testing.T) {
	//Arrange
	s := storage.NewStorage()
	// Get a non-existent username

	//Act
	username := "nonexistent"
	_, found := s.Get(username)

	// Assert
	if found {
		t.Errorf("Expected username '%s' to be not found, but it was found", username)
	}
}
