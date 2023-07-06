#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SmartWallet.API/SmartWalletAPI.csproj", "SmartWalletAPI/"]
RUN dotnet restore "SmartWalletAPI/SmartWalletAPI.csproj"
COPY . .
WORKDIR "/src/SmartWalletAPI"
RUN dotnet build "SmartWalletAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SmartWalletAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SmartWalletAPI.dll"]