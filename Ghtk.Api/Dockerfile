# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Ghtk.Api/Ghtk.Api.csproj", "Ghtk.Api/"]
COPY ["ClientAuthentication/ClientAuthentication.csproj", "ClientAuthentication/"]
COPY ["Ghtk.Authorization/Ghtk.Authorization.csproj", "Ghtk.Authorization/"]
COPY ["Ghtk.Repository.Abstractions/Ghtk.Repository.Abstractions.csproj", "Ghtk.Repository.Abstractions/"]
COPY ["Ghtk.Repository.MongoDb/Ghtk.Repository.MongoDb.csproj", "Ghtk.Repository.MongoDb/"]
RUN dotnet restore "./Ghtk.Api/Ghtk.Api.csproj"
COPY . .
WORKDIR "/src/Ghtk.Api"
RUN dotnet build "./Ghtk.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Ghtk.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ghtk.Api.dll"]