# См. статью по ссылке https://aka.ms/customizecontainer, чтобы узнать как настроить контейнер отладки и как Visual Studio использует этот Dockerfile для создания образов для ускорения отладки.

# Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
WORKDIR /app
EXPOSE 8080
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
RUN apk add --no-cache icu-libs icu-data-full # or icu-data-full for full data
USER app


# Этот этап используется для сборки проекта службы
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
#RUN ls -a /src
COPY ["asd/TicketsGenerator/TicketsGenerator.AppHost/TicketsGenerator.AppHost.csproj", "asd/TicketsGenerator/TicketsGenerator.AppHost/"]
COPY ["asd/TicketsGenerator/TicketsGenerator.Web/TicketsGenerator.Web.csproj", "asd/TicketsGenerator/TicketsGenerator.Web/"]
COPY ["TicketsGenerator/TicketsGenerator/TicketsGeneratorServices.Api/TicketsGeneratorServices.Api.csproj", "TicketsGenerator/TicketsGenerator/TicketsGeneratorServices.Api/"]
COPY ["TicketsGenerator/TicketsGenerator/TicketsGeneratorServices.Common/TicketsGeneratorServices.Common.csproj", "TicketsGenerator/TicketsGenerator/TicketsGeneratorServices.Common/"]
COPY ["asd/TicketsGenerator/TicketsGenerator.ServiceDefaults/TicketsGenerator.ServiceDefaults.csproj", "asd/TicketsGenerator/TicketsGenerator.ServiceDefaults/"]
COPY ["TicketsGenerator/TicketsGenerator/TicketsGeneratorServices.Db/TicketsGeneratorServices.Db.csproj", "TicketsGenerator/TicketsGenerator/TicketsGeneratorServices.Db/"]
RUN dotnet restore "./asd/TicketsGenerator/TicketsGenerator.AppHost/TicketsGenerator.AppHost.csproj"
COPY . .
WORKDIR "/src/asd/TicketsGenerator/TicketsGenerator.AppHost"
RUN dotnet build "./TicketsGenerator.AppHost.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
#FROM build AS publish
#ARG BUILD_CONFIGURATION=Release
#RUN dotnet publish "./TicketsGenerator.AppHost.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app
COPY --from=build /app/build .
ENTRYPOINT ["dotnet", "TicketsGenerator.AppHost.dll"]