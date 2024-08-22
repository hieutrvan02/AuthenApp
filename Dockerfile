#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 5200

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["AuthenAppProject/AuthenApp.API.csproj", "AuthenAppProject/"]
COPY ["AuthenApp.Application/AuthenApp.Application.csproj", "AuthenApp.Application/"]
COPY ["AuthenApp.Core/AuthenApp.Core.csproj", "AuthenApp.Core/"]
COPY ["AuthenApp.Infrastructure/AuthenApp.Infrastructure.csproj", "AuthenApp.Infrastructure/"]
RUN dotnet restore "./AuthenAppProject/AuthenApp.API.csproj"
COPY . .
WORKDIR "/src/AuthenAppProject"
RUN dotnet build "./AuthenApp.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AuthenApp.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuthenApp.API.dll"]