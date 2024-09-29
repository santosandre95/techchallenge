# Estágio base para runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
ENV ASPNETCORE_URLS http://*:80
ENV ASPNETCORE_URLS https://*:80
ENV ASPNETCORE_ENVIRONMENT=Development
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["TechChallenge/TechChallengeApi.csproj", "TechChallenge/"]
RUN dotnet restore "TechChallenge/TechChallengeApi.csproj"

COPY . .
WORKDIR "/src/TechChallenge"
RUN dotnet build "TechChallengeApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TechChallengeApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TechChallengeApi.dll"]