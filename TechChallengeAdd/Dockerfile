FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
ENV ASPNETCORE_URLS http://*:80
ENV ASPNETCORE_ENVIRONMENT=Development
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["TechChallengeAdd/TechChallengeAdd.csproj", "TechChallengeAdd/"]
RUN dotnet restore "./TechChallengeAdd/TechChallengeAdd.csproj"
COPY . .
WORKDIR "/src/TechChallengeAdd"
RUN dotnet build "./TechChallengeAdd.csproj" -c Release -o /app/build


FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TechChallengeAdd.csproj" -c Release -o /app/publish /p:UseAppHost=false


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TechChallengeAdd.dll"]