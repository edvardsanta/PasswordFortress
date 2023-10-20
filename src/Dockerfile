FROM docker.io/library/golang:1.20.4-alpine

# Set the working directory in the container
WORKDIR /app

# Copy the current directory contents into the container at /app
COPY . .

# Download Go modules
RUN go mod tidy

# Build the Go application
RUN go build -o main ./api/main.go

# Expose port 8080 for the Gin application
EXPOSE 8080

# Command to run the executable
CMD ["./main"]
