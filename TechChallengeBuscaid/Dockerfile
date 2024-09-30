FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
ENV ASPNETCORE_URLS http://*:80
ENV ASPNETCORE_ENVIRONMENT=Development
WORKDIR /app
EXPOSE 80



FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["TechChallengeBuscaid/TechChallengeBuscaid.csproj", "TechChallengeBuscaid/"]
RUN dotnet restore "./TechChallengeBuscaid/TechChallengeBuscaid.csproj"
COPY . .
WORKDIR "/src/TechChallengeBuscaid"
RUN dotnet build "./TechChallengeBuscaid.csproj" -c Release -o /app/build


FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TechChallengeBuscaid.csproj" -c Release -o /app/publish /p:UseAppHost=false


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TechChallengeBuscaid.dll"]