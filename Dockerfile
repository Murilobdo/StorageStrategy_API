FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /App

COPY . .

RUN dotnet restore

RUN dotnet clean

#RUN dotnet test StorageStrategy.Tests/StorageStrategy.Tests.csproj

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /App

COPY --from=build /App/out /App/

EXPOSE 8080

CMD [ "dotnet", "StorageStrategy.API.dll"]