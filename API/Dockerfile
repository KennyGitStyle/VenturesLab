# Use the official ASP.NET Core runtime image as a parent image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
EXPOSE 80

# Copy the solution file and project files
COPY ["VenturesLab_Test.sln", "./"]
COPY ["API/API.csproj", "API/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["Domain/Domain.csproj", "Domain/"]

# Restore for each project
RUN dotnet restore "VenturesLab_Test.sln"

# Copy the app source code, including the users.json file
COPY . .

# Set the working directory to the API project directory and publish
WORKDIR "/src/API"
RUN dotnet publish -c Release -o /app/publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "API.dll"]
