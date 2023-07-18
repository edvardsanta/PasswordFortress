package passwordcore

import (
	"net/http"

	storage "github.com/edvardsanta/SimplePasswordManager/pkg/storage"
	"github.com/gin-gonic/gin"
)

var store *storage.Storage

// TODO: Types must be in another file
// User represents a user object
// @Model User
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

// to export the first letter must be upper

func init() {
	store = storage.NewStorage()
}

// @Summary Create a user
// @Description Create a new user
// @Tags users
// @Accept json
// @Produce json
// @Param user body User true "User object"
// @Success 200 {object} CreateUserResponse
// @Router /passwords [post]
func SetPassword(c *gin.Context) {
	var user User
	if err := c.ShouldBindJSON(&user); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{
			"error": "Invalid Json",
		})
		return
	}
	username := user.Username
	password := user.Password
	store.Set(username, password)

	c.JSON(200, gin.H{
		"message": "Password saved successfully!",
		"user":    username,
	})
}

// @Summary Get password
// @Description Retrieve the password for a given username and service
// @Tags passwords
// @Accept json
// @Produce json
// @Param username path string true "Username [service]"
// @Success 200 {string} string "Password"
// @Failure 404 {string} string "Password not found for the username"
// @Router /passwords/{username} [get]
func GetPassword(c *gin.Context) {
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
