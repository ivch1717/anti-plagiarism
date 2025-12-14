FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY FileStoringService.Api/FileStoringService.Api.csproj FileStoringService.Api/
RUN dotnet restore FileStoringService.Api/FileStoringService.Api.csproj

COPY . .
RUN dotnet publish FileStoringService.Api/FileStoringService.Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "FileStoringService.Api.dll"]
