# Use the official .NET Core 8 SDK image as a build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# Copy the project files and restore any dependencies
COPY *.csproj .
RUN dotnet restore

# Copy the remaining source code and build the project
COPY . .
RUN dotnet publish -c Release -o /app

# Use the official .NET Core 8 runtime image as a runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .

# Expose the port the app runs on
EXPOSE 80

# Set the entry point for the container
ENTRYPOINT ["dotnet", "ActDigital.dll"]
