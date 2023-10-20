package api_routes

import (
	"log"

	"github.com/edvardsanta/SimplePasswordManager/api/controllers"
	db_repository "github.com/edvardsanta/SimplePasswordManager/data"
	"github.com/edvardsanta/SimplePasswordManager/internal/services"
	"github.com/gin-gonic/gin"
)

func SetupRoutes(router *gin.Engine) {
	v1 := router.Group("/api/v1")

	// Create the /password sub-group within /api/v1
	passwordGroup := v1.Group("/passwords")
	{
		setupPasswordRoutes(passwordGroup)
	}
}
func setupPasswordRoutes(passwordGroup *gin.RouterGroup) {
	passwordCtrl, err := configureControllers()
	if err != nil {
		log.Fatalf("Failed to configure: %v", err)
	}

	// TODO Create validation in these handlers
	passwordGroup.POST("", passwordCtrl.PostPasswordHandler)
	passwordGroup.GET("/:username", passwordCtrl.GetPasswordHandler)
}

func configureControllers() (*controllers.PasswordController, error) {
	database_repo, err := db_repository.Connect()
	if err != nil {
		return nil, err
	}

	passwordService := services.PasswordService{
		DatabaseRepository: *database_repo,
	}
	passwordController := &controllers.PasswordController{
		PasswordService: &passwordService,
	}
	return passwordController, nil
}
