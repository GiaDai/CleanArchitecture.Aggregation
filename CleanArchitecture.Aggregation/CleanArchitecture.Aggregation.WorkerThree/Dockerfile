#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CleanArchitecture.Aggregation/CleanArchitecture.Aggregation.WorkerThree/CleanArchitecture.Aggregation.WorkerThree.csproj", "CleanArchitecture.Aggregation/CleanArchitecture.Aggregation.WorkerThree/"]
RUN dotnet restore "./CleanArchitecture.Aggregation/CleanArchitecture.Aggregation.WorkerThree/CleanArchitecture.Aggregation.WorkerThree.csproj"
COPY . .
WORKDIR "/src/CleanArchitecture.Aggregation/CleanArchitecture.Aggregation.WorkerThree"
RUN dotnet build "./CleanArchitecture.Aggregation.WorkerThree.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CleanArchitecture.Aggregation.WorkerThree.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CleanArchitecture.Aggregation.WorkerThree.dll"]