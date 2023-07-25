package controllers

import (
	"net/http"

	storage "github.com/edvardsanta/SimplePasswordManager/pkg/storage"
  service "github.com/edvardsanta/SimplePasswordManager/internal/services"
	"github.com/gin-gonic/gin"
)
// TODO Cleanup and refactor this code, it is a messy
// TODO Types must be in another file
// TODO use repository to get data and replace store

var store *storage.Storage
var err error


type User struct {
	Username string `json:"username"`
	Password string `json:"password"`
}

// CreateUserResponse represents the response for creating a user
// @Model CreateUserResponse
type CreateUserResponse struct {
	Message string `json:"message" example:"User created successfully"`
	User    User   `json:"user"`
}

type PasswordController struct {
  PasswordService *service.PasswordService
}

func (ctrl *PasswordController) GetPasswordHandler(c *gin.Context) {
	// Logic for GET request
	c.JSON(http.StatusOK, gin.H{"message": "GET request handled"})
	username := c.Param("username")

	password, found := store.Get(username)
	if found {
		c.JSON(200, gin.H{
			"password": password,
		})
	} else {
		c.JSON(404, gin.H{
			"message": "Password not found for the username",
		})
	}

}

func (ctrl *PasswordController) PostPasswordHandler(c *gin.Context) {
	// Logic for POST request
	c.JSON(http.StatusOK, gin.H{"message": "POST request handled"})
}

