# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5000


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["DotnetDemo.API2/DotnetDemo.API2.csproj", "DotnetDemo.API2/"]
COPY ["DotnetDemo.Domain/DotnetDemo.Domain.csproj", "DotnetDemo.Domain/"]
COPY ["DotnetDemo.Repository/DotnetDemo.Repository.csproj", "DotnetDemo.Repository/"]
COPY ["DotnetDemo.Service/DotnetDemo.Service.csproj", "DotnetDemo.Service/"]
RUN dotnet restore "./DotnetDemo.API2/DotnetDemo.API2.csproj"
COPY . .
WORKDIR "/src/DotnetDemo.API2"
RUN dotnet build "./DotnetDemo.API2.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DotnetDemo.API2.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DotnetDemo.API2.dll"]