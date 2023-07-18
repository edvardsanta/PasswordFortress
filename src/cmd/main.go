package main

import (
	"github.com/edvardsanta/SimplePasswordManager/docs"
	passwordcore "github.com/edvardsanta/SimplePasswordManager/pkg"
	"github.com/gin-gonic/gin"
	swaggerFiles "github.com/swaggo/files"
	ginSwagger "github.com/swaggo/gin-swagger"
)

func main() {
	r := gin.Default()

	// i think i will not use v[0-9]
	docs.SwaggerInfo.BasePath = "/api/v1"
	v1 := r.Group("/api/v1")
	{
		eg := v1.Group("/passwords")
		{
			eg.POST("", passwordcore.SetPassword)
			eg.GET("/:username", passwordcore.GetPassword)
		}
	}
	r.GET("/swagger/*any", ginSwagger.WrapHandler(swaggerFiles.Handler))
	r.Run(":8080")
}
