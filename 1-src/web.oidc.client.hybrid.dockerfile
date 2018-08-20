FROM microsoft/dotnet:2.1-sdk-alpine3.7 AS builder

COPY . /src

WORKDIR /src

RUN dotnet publish ./web.oidc.client.hybrid/web.oidc.client.hybrid.csproj --output /publish



FROM microsoft/dotnet:2.1-aspnetcore-runtime-alpine3.7
 
COPY --from=builder /publish /app

WORKDIR /app

EXPOSE 80

ENTRYPOINT ["dotnet", "Web.Oidc.Client.Hybrid.dll"]