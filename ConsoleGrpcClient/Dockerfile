#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ConsoleGrpcClient/ConsoleGrpcClient.csproj", "ConsoleGrpcClient/"]
RUN dotnet restore "ConsoleGrpcClient/ConsoleGrpcClient.csproj"
COPY . .
WORKDIR "/src/ConsoleGrpcClient"
RUN dotnet build "ConsoleGrpcClient.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ConsoleGrpcClient.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ConsoleGrpcClient.dll"]