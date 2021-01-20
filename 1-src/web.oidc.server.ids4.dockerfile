# https://hub.docker.com/_/microsoft-dotnet-sdk/
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS builder

COPY . /src

WORKDIR /src

RUN dotnet publish ./web.oidc.server.ids4/web.oidc.server.ids4.csproj --output /publish


# https://hub.docker.com/_/microsoft-dotnet-aspnet/
FROM mcr.microsoft.com/dotnet/aspnet:5.0

COPY --from=builder /publish /app

WORKDIR /app

EXPOSE 80

ENTRYPOINT ["/bin/bash", "-c", "dotnet Web.Oidc.Server.Ids4.dll"]
