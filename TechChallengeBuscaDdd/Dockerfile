FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
ENV ASPNETCORE_URLS http://*:80
ENV ASPNETCORE_ENVIRONMENT=Development
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

FROM mcr.microsoft.com/dotnet/sdk:8.0-nanoserver-1809 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TechChallengeBuscaDdd/TechChallengeBuscaDdd.csproj", "TechChallengeBuscaDdd/"]
RUN dotnet restore "./TechChallengeBuscaDdd/TechChallengeBuscaDdd.csproj"
COPY . .
WORKDIR "/src/TechChallengeBuscaDdd"
RUN dotnet build "./TechChallengeBuscaDdd.csproj" -c Release -o /app/build


FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TechChallengeBuscaDdd.csproj" -c Release -o /app/publish /p:UseAppHost=false


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TechChallengeBuscaDdd.dll"]