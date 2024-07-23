#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT=Development
ENV APIKey=<Add-your-api-key>

EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/ Trip.Advisor.Be.Api/ Trip.Advisor.Be.Api.csproj", "src/ Trip.Advisor.Be.Api/"]
COPY ["src/ Trip.Advisor.Be.Application/ Trip.Advisor.Be.Application.csproj", "src/ Trip.Advisor.Be.Application/"]
COPY ["src/ Trip.Advisor.Be.Core/ Trip.Advisor.Be.Core.csproj", "src/ Trip.Advisor.Be.Core/"]
COPY ["src/ Trip.Advisor.Be.Domain/ Trip.Advisor.Be.Domain.csproj", "src/ Trip.Advisor.Be.Domain/"]
RUN dotnet restore "./src/ Trip.Advisor.Be.Api/ Trip.Advisor.Be.Api.csproj"
COPY . .
WORKDIR "/src/src/ Trip.Advisor.Be.Api"
RUN dotnet build "./ Trip.Advisor.Be.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ Trip.Advisor.Be.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", " Trip.Advisor.Be.Api.dll"]