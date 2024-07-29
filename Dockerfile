FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

WORKDIR /app
COPY . ./

RUN dotnet restore
RUN dotnet build
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 as runtime
WORKDIR /app
COPY --from=build-env /app/out .

EXPOSE 80

ENTRYPOINT ["dotnet", "TechChallengeApi.dll"]