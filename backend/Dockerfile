FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /app

COPY src/Radio.Core/*.csproj ./src/Radio.Core/
COPY src/Radio.Infrastructure/*.csproj ./src/Radio.Infrastructure/
COPY src/Radio.Infrastructure.Api/*.csproj ./src/Radio.Infrastructure.Api/
COPY src/Radio.Infrastructure.Api.External/*.csproj ./src/Radio.Infrastructure.Api.External/
COPY src/Radio.Infrastructure.Api.Internal/*.csproj ./src/Radio.Infrastructure.Api.Internal/
COPY src/Radio.Infrastructure.DbAccess/*.csproj ./src/Radio.Infrastructure.DbAccess/
COPY src/Radio.Infrastructure.Messaging/*.csproj ./src/Radio.Infrastructure.Messaging/
COPY src/Radio.Startup.Console/*.csproj ./src/Radio.Startup.Console/
COPY src/Radio.Startup.Web.External/*.csproj ./src/Radio.Startup.Web.External/
COPY src/Radio.Startup.Web.Internal/*.csproj ./src/Radio.Startup.Web.Internal/
WORKDIR /app/src/Radio.Startup.Console
RUN dotnet restore
WORKDIR /app/src/Radio.Startup.Web.External
RUN dotnet restore
WORKDIR /app/src/Radio.Startup.Web.Internal
RUN dotnet restore

WORKDIR /app
COPY src/Radio.Core/. ./src/Radio.Core/
COPY src/Radio.Infrastructure/. ./src/Radio.Infrastructure/
COPY src/Radio.Infrastructure.Api/. ./src/Radio.Infrastructure.Api/
COPY src/Radio.Infrastructure.Api.External/. ./src/Radio.Infrastructure.Api.External/
COPY src/Radio.Infrastructure.Api.Internal/. ./src/Radio.Infrastructure.Api.Internal/
COPY src/Radio.Infrastructure.DbAccess/. ./src/Radio.Infrastructure.DbAccess/
COPY src/Radio.Infrastructure.Messaging/. ./src/Radio.Infrastructure.Messaging/
COPY src/Radio.Startup.Console/. ./src/Radio.Startup.Console/
COPY src/Radio.Startup.Web.External/. ./src/Radio.Startup.Web.External/
COPY src/Radio.Startup.Web.Internal/. ./src/Radio.Startup.Web.Internal/
WORKDIR /app/src/Radio.Startup.Console
RUN dotnet publish -c Release -o out
WORKDIR /app/src/Radio.Startup.Web.External
RUN dotnet publish -c Release -o out
WORKDIR /app/src/Radio.Startup.Web.Internal
RUN dotnet publish -c Release -o out

FROM build AS testrunner
WORKDIR /app
COPY test/Radio.Tests.Unit/. ./test/Radio.Tests.Unit/
WORKDIR /app/test/Radio.Tests.Unit
ENTRYPOINT ["dotnet", "test", "--logger:trx"]

FROM mcr.microsoft.com/dotnet/core/runtime:2.2 AS console-runtime
WORKDIR /app
COPY --from=build /app/src/Radio.Startup.Console/out ./
ENTRYPOINT ["dotnet", "Radio.Startup.Console.dll"]

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS external-web-runtime
WORKDIR /app
COPY --from=build /app/src/Radio.Startup.Web.External/out ./
ENTRYPOINT ["dotnet", "Radio.Startup.Web.External.dll"]

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS internal-web-runtime
WORKDIR /app
COPY --from=build /app/src/Radio.Startup.Web.Internal/out ./
ENTRYPOINT ["dotnet", "Radio.Startup.Web.Internal.dll"]