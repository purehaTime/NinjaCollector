#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["DbService/DbService.csproj", "DbService/"]
RUN dotnet restore "DbService/DbService.csproj"
COPY . .
WORKDIR "/src/DbService"
RUN dotnet build "DbService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DbService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DbService.dll"]