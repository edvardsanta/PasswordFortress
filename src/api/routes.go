package api_routes

import (
	"log"

	"github.com/edvardsanta/SimplePasswordManager/api/controllers"
	postgre "github.com/edvardsanta/SimplePasswordManager/data"
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
	postgreRepository, err := postgre.NewPostgreRepository()
	if err != nil {
		return nil, err
	}

	passwordService := services.PasswordService{
		PostgreRepository: *postgreRepository,
	}
	passwordController := &controllers.PasswordController{
		PasswordService: &passwordService,
	}
	return passwordController, nil
}
