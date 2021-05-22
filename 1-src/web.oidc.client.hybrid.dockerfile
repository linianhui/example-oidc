# https://hub.docker.com/_/microsoft-dotnet-core-aspnet/
# https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/Dockerfile.alpine-x64
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS builder

COPY . /src

WORKDIR /src

RUN dotnet publish ./web.oidc.client.hybrid/web.oidc.client.hybrid.csproj --output /publish



FROM mcr.microsoft.com/dotnet/aspnet:5.0

COPY --from=builder /publish /app

WORKDIR /app

EXPOSE 80

ENTRYPOINT ["/bin/bash", "-c", "dotnet Web.Oidc.Client.Hybrid.dll"]
