FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app

EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/OptimQuery.Business/OptimQuery.Business.csproj", "src/OptimQuery.Business/"]
COPY ["src/OptimQuery.Core/OptimQuery.Core.csproj", "src/OptimQuery.Core/"]
COPY ["src/OptimQuery.Data/OptimQuery.Data.csproj", "src/OptimQuery.Data/"]
COPY ["src/OptimQuery.WebApi/OptimQuery.WebApi.csproj", "src/OptimQuery.WebApi/"]
RUN dotnet restore "src/OptimQuery.WebApi/OptimQuery.WebApi.csproj"
COPY . .
WORKDIR "/src/src/OptimQuery.WebApi"
RUN dotnet build "OptimQuery.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "OptimQuery.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OptimQuery.WebApi.dll"]
