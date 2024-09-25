FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
ENV ASPNETCORE_URLS http://*:80
ENV ASPNETCORE_ENVIRONMENT=Development
WORKDIR /app
EXPOSE 80


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["TechChallengeBuscaTodos/TechChallengeBuscaTodos.csproj", "TechChallengeBuscaTodos/"]
RUN dotnet restore "./TechChallengeBuscaTodos/TechChallengeBuscaTodos.csproj"
COPY . .
WORKDIR "/src/TechChallengeBuscaTodos"
RUN dotnet build "./TechChallengeBuscaTodos.csproj" -c $BUILD_CONFIGURATION -o /app/build


FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TechChallengeBuscaTodos.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TechChallengeBuscaTodos.dll"]