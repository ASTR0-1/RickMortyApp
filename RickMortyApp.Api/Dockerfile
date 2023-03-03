FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["RickMortyApp.Api/RickMortyApp.Api.csproj", "RickMortyApp.Api/"]
COPY ["RickMortyApp.Logic/RickMortyApp.Logic.csproj", "RickMortyApp.Logic/"]
COPY ["RickMortyApp.Contracts/RickMortyApp.Contracts.csproj", "RickMortyApp.Contracts/"]
COPY ["RickMortyApp.Entities/RickMortyApp.Entities.csproj", "RickMortyApp.Entities/"]
RUN dotnet restore "RickMortyApp.Api/RickMortyApp.Api.csproj"
COPY . .
WORKDIR "/src/RickMortyApp.Api"
RUN dotnet build "RickMortyApp.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RickMortyApp.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RickMortyApp.Api.dll"]