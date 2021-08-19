#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["API/Api.csproj", "API/"]
COPY ["Application/Services/Services.csproj", "Application/Services/"]
COPY ["Infraestructure/SQLRepository/SQLRepository.csproj", "Infraestructure/SQLRepository/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Common/Helper.csproj", "Common/"]
COPY ["Application/Regresion/Regresion.csproj", "Application/Regresion/"]
COPY ["Infraestructure/FileRepository/FileRepository.csproj", "Infraestructure/FileRepository/"]
RUN dotnet restore "API/Api.csproj"
COPY . .
WORKDIR "/src/API/"
RUN dotnet build "Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]