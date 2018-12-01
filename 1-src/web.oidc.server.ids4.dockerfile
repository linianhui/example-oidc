FROM microsoft/dotnet:2.1-sdk-alpine AS builder

COPY . /src

WORKDIR /src

RUN dotnet publish ./web.oidc.server.ids4/web.oidc.server.ids4.csproj --output /publish



FROM microsoft/dotnet:2.1.6-aspnetcore-runtime-alpine

COPY --from=builder /publish /app

WORKDIR /app

EXPOSE 80

ENTRYPOINT ["dotnet", "Web.Oidc.Server.Ids4.dll"]