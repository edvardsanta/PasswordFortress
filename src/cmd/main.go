package main

import (
	api_routes "github.com/edvardsanta/SimplePasswordManager/api"
	"github.com/edvardsanta/SimplePasswordManager/docs"
	"github.com/gin-gonic/gin"
	swaggerFiles "github.com/swaggo/files"
	ginSwagger "github.com/swaggo/gin-swagger"
)

func main() {
	r := gin.Default()
	api_routes.SetupRoutes(r)

	// Swagget stuff
	docs.SwaggerInfo.BasePath = "/api/v1"
	r.GET("/swagger/*any", ginSwagger.WrapHandler(swaggerFiles.Handler))
	r.Run(":8080")
}
