package login

import (
	"database/sql"
	"fmt"

	"golang.org/x/crypto/bcrypt"
)

func GetUserInput() bool {
	connString := ""
	db, err := sql.Open("postgres", connString)
	if err != nil {
		return false
	}
	fmt.Print("Username: ")
	var username string
	var password string
	fmt.Scanln(&username)

	fmt.Print("Password: ")
	fmt.Scanln(&username)

	fmt.Println("Checking login...")
	hashedPassword, err := bcrypt.GenerateFromPassword([]byte(password), bcrypt.DefaultCost)
	if err != nil {
		fmt.Println("Error generating bcrypt hash:", err)
		return false
	}

	// TODO compare hashedPassword and hashedPasswordfromDb in a efficient way

	// Retrieve the hashed password from the database
	var hashedPasswordFromDB []byte
	err = db.QueryRow("SELECT password FROM login WHERE password = $1", hashedPassword).Scan(&hashedPasswordFromDB)
	if err != nil {
		fmt.Println("Error querying data from PostgreSQL:", err)
		return false
	}
	// Verify the entered password against the hashed password from the database
	err = bcrypt.CompareHashAndPassword(hashedPasswordFromDB, []byte(password))
	if err == nil {
		fmt.Println("Password match!")
	} else if err == bcrypt.ErrMismatchedHashAndPassword {
		fmt.Println("Password does not match!")
	} else {
		fmt.Println("Error comparing passwords:", err)
	}
	return true
}
