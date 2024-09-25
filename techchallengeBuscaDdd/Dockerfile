FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
ENV ASPNETCORE_URLS http://*:80
ENV ASPNETCORE_ENVIRONMENT=Development
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src


COPY ["techchallengeBuscaDdd/techchallengeBuscaDdd.csproj", "techchallengeBuscaDdd/"]
RUN dotnet restore "./techchallengeBuscaDdd/techchallengeBuscaDdd.csproj"
COPY . .
WORKDIR "/src/techchallengeBuscaDdd"
RUN dotnet build "./techchallengeBuscaDdd.csproj" -c $BUILD_CONFIGURATION -o /app/build


FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./techchallengeBuscaDdd.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "techchallengeBuscaDdd.dll"]