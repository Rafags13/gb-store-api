#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["GbStoreApi.WebApi/GbStoreApi.WebApi.csproj", "GbStoreApi.WebApi/"]
RUN dotnet restore "./GbStoreApi.WebApi/GbStoreApi.WebApi.csproj"
COPY . .
WORKDIR "/src/GbStoreApi.WebApi"
RUN dotnet build "./GbStoreApi.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./GbStoreApi.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GbStoreApi.WebApi.dll"]