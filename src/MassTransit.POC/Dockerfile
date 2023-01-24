#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/MassTransit.Saga/MassTransit.Saga.csproj", "src/MassTransit.Saga/"]
RUN dotnet restore "src/MassTransit.Saga/MassTransit.Saga.csproj"
COPY . .
WORKDIR "/src/src/MassTransit.Saga"
RUN dotnet build "MassTransit.Saga.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MassTransit.Saga.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MassTransit.Saga.dll"]
