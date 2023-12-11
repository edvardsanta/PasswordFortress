package repository

import (
	"fmt"
	"github.com/spf13/viper"
	"os"
)

type EnvironmentType int

const (
	Production  EnvironmentType = iota // 0
	Local                              // 1
	Development                        // 2
)

func GetDatabaseConfig() *DatabaseConfig {
	// env principally for container env
	config := DatabaseConfig{
		DBUsername: os.Getenv("DB_USERNAME"),
		DBPassword: os.Getenv("DB_PASSWORD"),
		DBHost:     os.Getenv("DB_HOST"),
		DBPort:     os.Getenv("DB_PORT"),
		DBName:     os.Getenv("DB_NAME"),
	}

	if config.DBUsername == "" || config.DBPassword == "" || config.DBHost == "" || config.DBPort == "" || config.DBName == "" {
		viper.SetConfigName("appsettings")
		viper.SetConfigType("json")
		viper.AddConfigPath("./data")

		err := viper.ReadInConfig()
		if err != nil {
			panic(fmt.Errorf("failed to read config file: %w", err))
		}

		config.DBUsername = viper.GetString("database.username")
		config.DBPassword = viper.GetString("database.password")
		config.DBHost = viper.GetString("database.host")
		config.DBPort = viper.GetString("database.port")
		config.DBName = viper.GetString("database.dbname")
	}

	return &config
}

func isProductionEnvironment() bool {
	env := os.Getenv("ENVIRONMENT")
	return env == "production"
}

func getEnvironmentType() EnvironmentType {
	env := os.Getenv("ENVIRONMENT_TYPE")
	if env == "production" {
		return Production
	}
	if env == "dev" {
		return Development
	}
	return Local
}
