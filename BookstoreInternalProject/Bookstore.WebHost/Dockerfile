#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Bookstore.WebHost/Bookstore.WebHost.csproj", "Bookstore.WebHost/"]
COPY ["Bookstore.Application/Bookstore.Application.csproj", "Bookstore.Application/"]
COPY ["Bookstore.Domain/Bookstore.Domain.csproj", "Bookstore.Domain/"]
COPY ["Bookstore.Infrastructure/Bookstore.Infrastructure.csproj", "Bookstore.Infrastructure/"]
RUN dotnet restore "Bookstore.WebHost/Bookstore.WebHost.csproj"
COPY . .
WORKDIR "/src/Bookstore.WebHost"
RUN dotnet build "Bookstore.WebHost.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Bookstore.WebHost.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bookstore.WebHost.dll"]