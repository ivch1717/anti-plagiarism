FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY FileAnalisysService.Api/FileAnalisysService.Api.csproj FileAnalisysService.Api/
RUN dotnet restore FileAnalisysService.Api/FileAnalisysService.Api.csproj

COPY . .
RUN dotnet publish FileAnalisysService.Api/FileAnalisysService.Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "FileAnalisysService.Api.dll"]
