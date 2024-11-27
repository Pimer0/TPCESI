FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TPCESI_RECETTES/TPCESI_RECETTES.csproj", "TPCESI_RECETTES/"]
RUN dotnet restore "TPCESI_RECETTES/TPCESI_RECETTES.csproj"
COPY . .
WORKDIR "/src/TPCESI_RECETTES"
RUN dotnet build "TPCESI_RECETTES.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TPCESI_RECETTES.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TPCESI_RECETTES.dll"]
