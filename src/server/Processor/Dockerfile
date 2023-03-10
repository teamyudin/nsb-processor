#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["Processor/Processor.csproj", "Processor/"]
COPY ["Shared/Shared.csproj", "Shared/"]
COPY ["Utilities.AppSettings/Utilities.AppSettings.csproj", "Utilities.AppSettings/"]

RUN dotnet restore "Processor/Processor.csproj"
COPY . .
WORKDIR "/src/Processor"
RUN dotnet build "Processor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Processor.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Processor.dll"]