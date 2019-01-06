FROM microsoft/dotnet:2.2-sdk-alpine AS builder

COPY . /src

WORKDIR /src

RUN dotnet publish ./web.oauth2.resources.aspnetcore/web.oauth2.resources.aspnetcore.csproj --output /publish



FROM microsoft/dotnet:2.2-aspnetcore-runtime-alpine
 
COPY --from=builder /publish /app

WORKDIR /app

EXPOSE 80

ENTRYPOINT ["dotnet", "Web.OAuth2.Resources.AspNetCore.dll"]