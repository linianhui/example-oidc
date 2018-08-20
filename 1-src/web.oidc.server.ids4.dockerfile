FROM microsoft/dotnet:2.1-sdk-alpine3.7 AS builder

COPY . /src

WORKDIR /src

RUN dotnet publish ./web.oidc.server.ids4/web.oidc.server.ids4.csproj --output /publish



FROM microsoft/dotnet:2.1-aspnetcore-runtime-alpine3.7

COPY --from=builder /publish /app

WORKDIR /app

EXPOSE 80

ENTRYPOINT ["dotnet", "Web.Oidc.Server.Ids4.dll"]