#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CleanArchitecture.Aggregation/CleanArchitecture.Aggregation.WokerApi/CleanArchitecture.Aggregation.WokerApi.csproj", "CleanArchitecture.Aggregation/CleanArchitecture.Aggregation.WokerApi/"]
RUN dotnet restore "./CleanArchitecture.Aggregation/CleanArchitecture.Aggregation.WokerApi/./CleanArchitecture.Aggregation.WokerApi.csproj"
COPY . .
WORKDIR "/src/CleanArchitecture.Aggregation/CleanArchitecture.Aggregation.WokerApi"
RUN dotnet build "./CleanArchitecture.Aggregation.WokerApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CleanArchitecture.Aggregation.WokerApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CleanArchitecture.Aggregation.WokerApi.dll"]