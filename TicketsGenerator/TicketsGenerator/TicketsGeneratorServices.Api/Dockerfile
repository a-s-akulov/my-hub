# См. статью по ссылке https://aka.ms/customizecontainer, чтобы узнать как настроить контейнер отладки и как Visual Studio использует этот Dockerfile для создания образов для ускорения отладки.

# Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
USER app
WORKDIR /app
EXPOSE 8080
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
RUN apk add --no-cache icu-libs # or icu-data-full for full data


# Этот этап используется для сборки проекта службы
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
#RUN ls -a /src
COPY ["TicketsGenerator/TicketsGenerator/TicketsGeneratorServices.Api/TicketsGeneratorServices.Api.csproj", "TicketsGenerator/TicketsGenerator/TicketsGeneratorServices.Api/"]
COPY ["TicketsGenerator/TicketsGenerator/TicketsGeneratorServices.Common/TicketsGeneratorServices.Common.csproj", "TicketsGenerator/TicketsGenerator/TicketsGeneratorServices.Common/"]
COPY ["asd/TicketsGenerator/TicketsGenerator.ServiceDefaults/TicketsGenerator.ServiceDefaults.csproj", "asd/TicketsGenerator/TicketsGenerator.ServiceDefaults/"]
COPY ["TicketsGenerator/TicketsGenerator/TicketsGeneratorServices.Db/TicketsGeneratorServices.Db.csproj", "TicketsGenerator/TicketsGenerator/TicketsGeneratorServices.Db/"]
RUN dotnet restore "./TicketsGenerator/TicketsGenerator/TicketsGeneratorServices.Api/TicketsGeneratorServices.Api.csproj"
COPY . .
WORKDIR "/src/TicketsGenerator/TicketsGenerator/TicketsGeneratorServices.Api"
RUN dotnet build "./TicketsGeneratorServices.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TicketsGeneratorServices.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TicketsGeneratorServices.Api.dll"]