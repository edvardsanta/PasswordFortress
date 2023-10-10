# Use the official ASP.NET Core runtime image as the base image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 8080

# Use the official ASP.NET Core SDK image as the build image
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy the project file and restore any dependencies (this is done separately to take advantage of Docker layer caching)
COPY ./src .
RUN dotnet restore


# Build the application
RUN dotnet build "PasswordFortressFront.csproj" -c Release -o /app/build

# Publish the application for deployment
FROM build AS publish
RUN dotnet publish "PasswordFortressFront.csproj" -c Release -o /app/publish

# # Set up the final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PasswordFortressFront.dll"]

