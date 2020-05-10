# https://hub.docker.com/_/microsoft-dotnet-core-aspnet/
# https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/Dockerfile.alpine-x64
FROM mcr.microsoft.com/dotnet/core/sdk:3.1-bionic AS builder

COPY . /src

WORKDIR /src

RUN dotnet publish ./web.oauth2.resources.aspnetcore/web.oauth2.resources.aspnetcore.csproj --output /publish



FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-bionic

COPY --from=builder /publish /app

WORKDIR /app

EXPOSE 80

ENTRYPOINT ["/bin/bash", "-c", "dotnet Web.OAuth2.Resources.AspNetCore.dll"]
