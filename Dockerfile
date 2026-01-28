# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY CrudDemonstracao/CrudDemonstracao.csproj CrudDemonstracao/
RUN dotnet restore CrudDemonstracao/CrudDemonstracao.csproj

COPY CrudDemonstracao/ CrudDemonstracao/
RUN dotnet publish CrudDemonstracao/CrudDemonstracao.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_FORWARDEDHEADERS_ENABLED=true

ENTRYPOINT ["dotnet", "CrudDemonstracao.dll"]
