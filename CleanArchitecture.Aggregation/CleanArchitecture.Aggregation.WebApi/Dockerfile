#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 5052
ENV ASPNETCORE_URLS=http://+:5052
ENV ASPNETCORE_ENVIRONMENT=Production

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CleanArchitecture.Aggregation/CleanArchitecture.Aggregation.WebApi/CleanArchitecture.Aggregation.WebApi.csproj", "CleanArchitecture.Aggregation/CleanArchitecture.Aggregation.WebApi/"]
COPY ["CleanArchitecture.Aggregation/CleanArchitecture.Aggregation.Infrastructure.Identity/CleanArchitecture.Aggregation.Infrastructure.Identity.csproj", "CleanArchitecture.Aggregation/CleanArchitecture.Aggregation.Infrastructure.Identity/"]
COPY ["CleanArchitecture.Aggregation/CleanArchitecture.Aggregation.Infrastructure.Shared/CleanArchitecture.Aggregation.Infrastructure.Shared.csproj", "CleanArchitecture.Aggregation/CleanArchitecture.Aggregation.Infrastructure.Shared/"]
COPY ["CleanArchitecture.Aggregation/CleanArchitecture.Aggregation.Application/CleanArchitecture.Aggregation.Application.csproj", "CleanArchitecture.Aggregation/CleanArchitecture.Aggregation.Application/"]
COPY ["CleanArchitecture.Aggregation/CleanArchitecture.Aggregation.Domain/CleanArchitecture.Aggregation.Domain.csproj", "CleanArchitecture.Aggregation/CleanArchitecture.Aggregation.Domain/"]
COPY ["CleanArchitecture.Aggregation/CleanArchitecture.Aggregation.Infrastructure.Persistence/CleanArchitecture.Aggregation.Infrastructure.Persistence.csproj", "CleanArchitecture.Aggregation/CleanArchitecture.Aggregation.Infrastructure.Persistence/"]
RUN dotnet restore "./CleanArchitecture.Aggregation/CleanArchitecture.Aggregation.WebApi/CleanArchitecture.Aggregation.WebApi.csproj"
COPY . .
WORKDIR "/src/CleanArchitecture.Aggregation/CleanArchitecture.Aggregation.WebApi"
RUN dotnet build "./CleanArchitecture.Aggregation.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CleanArchitecture.Aggregation.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CleanArchitecture.Aggregation.WebApi.dll"]