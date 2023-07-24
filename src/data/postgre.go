package repository

import (
	"database/sql"
	"fmt"
	"github.com/spf13/viper"

	_ "github.com/lib/pq"
)

type Repository interface {
}

type PostgreRepository struct {
	DB *sql.DB
}

// NewPostgreRepository creates a new PostgreRepository instance.
func NewPostgreRepository() (*PostgreRepository, error) {
	viper.SetConfigName("appsettings")
	viper.SetConfigType("json")
	viper.AddConfigPath("./data")
	err := viper.ReadInConfig()
	if err != nil {
		return nil, fmt.Errorf("failed to read config file: %w", err)
	}

	// Get the database configuration from the config file.
	host := viper.GetString("database.host")
	port := viper.GetInt("database.port")
	username := viper.GetString("database.username")
	password := viper.GetString("database.password")
	dbname := viper.GetString("database.dbname")

	connStr := fmt.Sprintf("host=%s port=%d user=%s password=%s dbname=%s sslmode=disable", host, port, username, password, dbname)

	db, err := sql.Open("postgres", connStr)
	if err != nil {
		return nil, fmt.Errorf("failed to connect to database: %w", err)
	}

	err = db.Ping()
	if err != nil {
		return nil, fmt.Errorf("failed to ping database: %w", err)
	}

	return &PostgreRepository{DB: db}, nil
}


