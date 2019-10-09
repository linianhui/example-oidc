# https://hub.docker.com/_/microsoft-dotnet-core-aspnet/
# https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/Dockerfile.alpine-x64
FROM mcr.microsoft.com/dotnet/core/sdk:3.0-alpine AS builder

COPY . /src

WORKDIR /src

RUN dotnet publish ./web.oauth2.resources.aspnetcore/web.oauth2.resources.aspnetcore.csproj --output /publish



FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-alpine
 
COPY --from=builder /publish /app

WORKDIR /app

EXPOSE 80

ENTRYPOINT ["dotnet", "Web.OAuth2.Resources.AspNetCore.dll"]