package passwordcore

import (
	"bufio"
	"fmt"
	"net/http"
	"os"

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
func RunREPL() {
	scanner := bufio.NewScanner(os.Stdin)
	store := storage.NewStorage()

	fmt.Println("Welcome to Simple Password Manager!")

	for {
		fmt.Println("\nPlease choose an option:")
		fmt.Println("1. Set new password")
		fmt.Println("2. Get password")
		fmt.Println("3. Exit")

		fmt.Print("Option: ")
		scanner.Scan()
		option := scanner.Text()

		switch option {
		case "1":
			setPassword(scanner, store)
		case "2":
			getPassword(scanner, store)
		case "3":
			fmt.Println("Exiting")
			return
		default:
			fmt.Println("Invalid option. Please try again.")
		}
	}
}

// #region [private functions]
func setPassword(scanner *bufio.Scanner, store *storage.Storage) {
	fmt.Print("Enter a username [service]: ")
	scanner.Scan()
	username := scanner.Text()

	fmt.Print("Enter the corresponding password: ")
	scanner.Scan()
	password := scanner.Text()

	store.Set(username, password)

	fmt.Println("Password saved successfully!")
}

func getPassword(scanner *bufio.Scanner, store *storage.Storage) {
	fmt.Print("Enter a username [service]: ")
	scanner.Scan()
	username := scanner.Text()

	password, found := store.Get(username)
	if found {
		fmt.Println("Password:", password)
	} else {
		fmt.Println("Password not found for the username [service]:", username)
	}
}

// #endregion
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
  if err := c.ShouldBindJSON(&user); err != nil{
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
    "user": username,
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
