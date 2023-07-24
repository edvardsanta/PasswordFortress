package cli

import (
	"bufio"
	"fmt"
	"os"

	"github.com/edvardsanta/SimplePasswordManager/internal/login"
	"github.com/edvardsanta/SimplePasswordManager/pkg/storage"
)

func RunREPL() {
	scanner := bufio.NewScanner(os.Stdin)
	store, err := storage.NewStorage()
	if err != nil {
		println("aaaa")
	}
	fmt.Println("Welcome to Simple Password Manager!")
	fmt.Println("Please login before continue")

	//  TODO: Create  login access and handle login
	login.GetUserInput()
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
