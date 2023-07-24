package repository

import (
	"database/sql"
	"fmt"
	"github.com/spf13/viper"

	_ "github.com/lib/pq"
)

// Repository is an interface for the database operations.
type Repository interface {
	// Add your repository methods here, e.g., SaveUser, GetUserByID, etc.
}

// PostgreRepository is an implementation of the Repository interface for PostgreSQL.
type PostgreRepository struct {
	DB *sql.DB
}

// NewPostgreRepository creates a new PostgreRepository instance.
func NewPostgreRepository() (*PostgreRepository, error) {
	// Read the configuration file using Viper.
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

	// Create the PostgreSQL connection string.
	connStr := fmt.Sprintf("host=%s port=%d user=%s password=%s dbname=%s sslmode=disable", host, port, username, password, dbname)

	// Connect to the PostgreSQL database.
	db, err := sql.Open("postgres", connStr)
	if err != nil {
		return nil, fmt.Errorf("failed to connect to database: %w", err)
	}

	// Check if the connection is successful.
	err = db.Ping()
	if err != nil {
		return nil, fmt.Errorf("failed to ping database: %w", err)
	}

	return &PostgreRepository{DB: db}, nil
}

// Add your repository methods implementation here.
// For example, func (r *PostgreRepository) SaveUser(user *User) error { ... }

