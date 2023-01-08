FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

# Copy everything
COPY ./ ./

# Restore as distinct layers
RUN dotnet restore "./src/Crypcy.NodeConsole/Crypcy.NodeConsole.csproj"
COPY . .
RUN dotnet publish "./src/Crypcy.NodeConsole/Crypcy.NodeConsole.csproj" -c Release -o /app/publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0

WORKDIR /app
COPY --from=build /app/publish .

ENV Node__Port=23550
EXPOSE 23550

ENTRYPOINT ["dotnet", "Crypcy.NodeConsole.dll"]