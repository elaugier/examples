# Utilise l'image SDK .NET 6.0 pour créer le projet
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# Définit le répertoire de travail pour le reste de l'image
WORKDIR /app

# Copie les fichiers de projet dans le répertoire de travail
COPY *.csproj .

# Restaure les packages NuGet
RUN dotnet restore

# Copie tous les fichiers du projet dans l'image
COPY . .

# Compile l'application en mode Release
RUN dotnet publish -c Release -o out

# Utilise l'image d'exécution .NET 6.0 pour exécuter l'application
FROM mcr.microsoft.com/dotnet/aspnet:6.0

# Définit le répertoire de travail pour le reste de l'image
WORKDIR /app

# Copie les fichiers de l'application dans l'image
COPY --from=build /app/out .

# Définit la variable d'environnement ASPNETCORE_URLS pour définir l'URL de l'application
ENV ASPNETCORE_URLS http://*:80

# Expose le port 80 pour l'application
EXPOSE 80

# Exécute l'application lors du démarrage du conteneur
ENTRYPOINT ["dotnet", "b4f.dll"]