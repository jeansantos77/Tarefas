#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 8000

ENV ASPNETCORE_URLS=http://+:8000;http://+:80;

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Tarefas/Tarefas.API.csproj", "Tarefas/"]
COPY ["Tarefas.API.Infra.Data/Tarefas.API.Infra.Data.csproj", "Tarefas.API.Infra.Data/"]
COPY ["Tarefas.API.Domain/Tarefas.API.Domain.csproj", "Tarefas.API.Domain/"]
COPY ["Tarefas.API.Application/Tarefas.API.Application.csproj", "Tarefas.API.Application/"]
RUN dotnet restore "Tarefas/Tarefas.API.csproj"
COPY . .
WORKDIR "/src/Tarefas"
RUN dotnet build "Tarefas.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Tarefas.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tarefas.API.dll"]