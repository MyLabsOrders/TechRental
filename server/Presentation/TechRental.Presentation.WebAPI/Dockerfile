#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Presentation/TechRental.Presentation.WebAPI/TechRental.Presentation.WebAPI.csproj", "Presentation/TechRental.Presentation.WebAPI/"]
COPY ["Application/TechRental.Application/TechRental.Application.csproj", "Application/TechRental.Application/"]
COPY ["Application/TechRental.Application.Abstractions/TechRental.Application.Abstractions.csproj", "Application/TechRental.Application.Abstractions/"]
COPY ["Application/TechRental.Application.Dto/TechRental.Application.Dto.csproj", "Application/TechRental.Application.Dto/"]
COPY ["Domain/TechRental.Domain.Core/TechRental.Domain.Core.csproj", "Domain/TechRental.Domain.Core/"]
COPY ["Domain/TechRental.Domain.Common/TechRental.Domain.Common.csproj", "Domain/TechRental.Domain.Common/"]
COPY ["Application/TechRental.Application.Common/TechRental.Application.Common.csproj", "Application/TechRental.Application.Common/"]
COPY ["Application/TechRental.Application.Handlers/TechRental.Application.Handlers.csproj", "Application/TechRental.Application.Handlers/"]
COPY ["Infrastructure/TechRental.Infrastructure.Mapping/TechRental.Infrastructure.Mapping.csproj", "Infrastructure/TechRental.Infrastructure.Mapping/"]
COPY ["Application/TechRental.Application.Contracts/TechRental.Application.Contracts.csproj", "Application/TechRental.Application.Contracts/"]
COPY ["Application/TechRental.DataAccess.Abstractions/TechRental.DataAccess.Abstractions.csproj", "Application/TechRental.DataAccess.Abstractions/"]
COPY ["Infrastructure/TechRental.Infrastructure.DataAccess/TechRental.Infrastructure.DataAccess.csproj", "Infrastructure/TechRental.Infrastructure.DataAccess/"]
COPY ["Infrastructure/TechRental.Infrastructure.Identity/TechRental.Infrastructure.Identity.csproj", "Infrastructure/TechRental.Infrastructure.Identity/"]
COPY ["Presentation/TechRental.Presentation.Controllers/TechRental.Presentation.Controllers.csproj", "Presentation/TechRental.Presentation.Controllers/"]
COPY ["Presentation/TechRental.Presentation.Contracts/TechRental.Presentation.Contracts.csproj", "Presentation/TechRental.Presentation.Contracts/"]
RUN dotnet restore "Presentation/TechRental.Presentation.WebAPI/TechRental.Presentation.WebAPI.csproj"
COPY . .
WORKDIR "/src/Presentation/TechRental.Presentation.WebAPI"
RUN dotnet build "TechRental.Presentation.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TechRental.Presentation.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY ["Assets/*", "/app/Assets/"]
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TechRental.Presentation.WebAPI.dll"]
