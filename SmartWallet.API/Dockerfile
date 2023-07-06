#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SmartWallet.API/SmartWallet.API.csproj", "SmartWallet.API/"]
RUN dotnet restore "SmartWallet.API/SmartWallet.API.csproj"
COPY . .
WORKDIR "/src/SmartWallet.API"
RUN dotnet build "SmartWallet.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SmartWallet.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SmartWallet.API.dll"]