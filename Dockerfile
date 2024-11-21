# Use the .NET 8.0 SDK image to build and publish the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out ./

# Add healthcheck
HEALTHCHECK --interval=5s --timeout=3s --retries=3 \
    CMD curl -f http://localhost:8080/health || exit 1

# Set the environment variable for the port
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "PRO05_BACK_Gaj_Khalos_Max.dll"]


