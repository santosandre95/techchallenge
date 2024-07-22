# imagem base do SDK .NET Core
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

# diretório de trabalho
WORKDIR /app

# Copiando os arquivos do projeto para o contêiner
COPY . ./

# Restaurando as dependências e compilando os projetos
RUN dotnet restore
RUN dotnet build

# Publicando os projetos
RUN dotnet publish -c Release -o out


# imagem final
FROM mcr.microsoft.com/dotnet/aspnet:8.0 as root
WORKDIR /app
COPY --from=build-env /app/out .

# porta de saida
EXPOSE 8080 

# Executando o projeto
ENTRYPOINT ["dotnet", "TechChallengeApi.dll"]