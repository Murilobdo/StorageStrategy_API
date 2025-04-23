FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 11800

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["StorageStrategy.API/StorageStrategy.API.csproj", "StorageStrategy.API/"]
COPY ["StorageStrategy.Data/StorageStrategy.Data.csproj", "StorageStrategy.Data/"]
COPY ["StorageStrategy.Domain/StorageStrategy.Domain.csproj", "StorageStrategy.Domain/"]
COPY ["StorageStrategy.Models/StorageStrategy.Models.csproj", "StorageStrategy.Models/"]
COPY ["StorageStrategy.Utils/StorageStrategy.Utils.csproj", "StorageStrategy.Utils/"]
RUN dotnet restore "StorageStrategy.API/StorageStrategy.API.csproj"
COPY . .
WORKDIR "/src/StorageStrategy.API"
RUN dotnet build "StorageStrategy.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "StorageStrategy.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StorageStrategy.API.dll"]
