package repository

import (
	"database/sql"
	"fmt"
	_ "github.com/lib/pq"
)

type BaseRepository interface {
	Connect() error
}

type DatabaseRepository struct {
	DB       *sql.DB
	FilePath string
}
type DatabaseConfig struct {
	DBUsername string
	DBPassword string
	DBHost     string
	DBPort     string
	DBName     string
}

func Connect() (*DatabaseRepository, error) {
	switch getEnvironmentType() {
	case Production:
		return newPostgreRepository()
	default:
		return newPlainTextRepository()
	}
}

func newPostgreRepository() (*DatabaseRepository, error) {
	config := GetDatabaseConfig()
	connStr := fmt.Sprintf("host=%s port=%s user=%s password=%s dbname=%s sslmode=disable", config.DBHost, config.DBPort, config.DBUsername, config.DBPassword, config.DBName)
	db, err := sql.Open("postgres", connStr)
	if err != nil {
		return nil, fmt.Errorf("failed to connect to database: %w", err)
	}

	err = db.Ping()
	if err != nil {
		return nil, fmt.Errorf("failed to ping database: %w", err)
	}

	return &DatabaseRepository{DB: db}, nil
}

func newPlainTextRepository() (*DatabaseRepository, error) {
	return &DatabaseRepository{FilePath: "./test.txt"}, nil
}
