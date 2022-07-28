#BUILD
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["EngieChallange/EngieChallange.csproj", "EngieChallange/"]
RUN dotnet restore "EngieChallange/EngieChallange.csproj"
COPY . .
WORKDIR "/src/EngieChallange"
RUN dotnet restore "EngieChallange.csproj" 
RUN dotnet publish "EngieChallange.csproj" -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8888
ENTRYPOINT ["dotnet", "EngieChallange.dll"]

