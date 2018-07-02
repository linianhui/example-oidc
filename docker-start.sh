#!/bin/bash

dotnet publish ./src/web.oidc.server.ids4/web.oidc.server.ids4.csproj --output ../../www/oidc-server.test

dotnet publish ./src/web.oidc.client.hybrid/web.oidc.client.hybrid.csproj --output ../../www/oidc-client-hybrid.test

dotnet publish ./src/web.oauth2.resources.aspnetcore/web.oauth2.resources.aspnetcore.csproj --output ../../www/oauth2-resources-aspnetcore.test

dotnet publish ./src/web.oauth2.client.aspnetcore/web.oauth2.client.aspnetcore.csproj --output ../../www/oauth2-client-aspnetcore.test

cp -frv ./src/web.oidc.client.js ./www/oidc-client-js.test

cp -frv ./src/traefik ./www/traefik

docker-compose up -d --build
