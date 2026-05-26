# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

ARG BUILD_CONFIGURATION=Release

WORKDIR /src

# Copy csproj first
COPY ["backend/backend.csproj", "backend/"]

# Restore dependencies
RUN dotnet restore "backend/backend.csproj"

# Copy everything
COPY . .

# Go to project folder
WORKDIR "/src/backend"

# Publish app
RUN dotnet publish "backend.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

WORKDIR /app

# Render uses PORT environment variable
ENV ASPNETCORE_URLS=http://+:10000

EXPOSE 10000

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "backend.dll"]