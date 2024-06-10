# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the csproj and restore as distinct layers
COPY ["TwitchDownloaderAPI/TwitchDownloaderAPI.csproj", "TwitchDownloaderAPI/"]
RUN dotnet restore "TwitchDownloaderAPI/TwitchDownloaderAPI.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/TwitchDownloaderAPI"
RUN dotnet build "TwitchDownloaderAPI.csproj" -c Release -o /app/build

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish "TwitchDownloaderAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 3: Final
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Expose ports
EXPOSE 80
EXPOSE 443

# Run the app
ENTRYPOINT ["dotnet", "TwitchDownloaderAPI.dll"]
