FROM mcr.microsoft.com/dotnet/aspnet:9.0-nanoserver-1809 AS base
WORKDIR /app
EXPOSE 5113

ENV ASPNETCORE_URLS=http://+:5113

FROM mcr.microsoft.com/dotnet/sdk:9.0-nanoserver-1809 AS build
ARG configuration=Release
WORKDIR /src
COPY ["LibraryApiMongo.csproj", "./"]
RUN dotnet restore "LibraryApiMongo.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "LibraryApiMongo.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "LibraryApiMongo.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LibraryApiMongo.dll"]
