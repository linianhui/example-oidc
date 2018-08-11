#!/bin/bash

dotnet publish ./1-src/web.oidc.server.ids4/web.oidc.server.ids4.csproj --output ../../www/oidc-server.test

dotnet publish ./1-src/web.oidc.client.hybrid/web.oidc.client.hybrid.csproj --output ../../www/oidc-client-hybrid.test

dotnet publish ./1-src/web.oauth2.resources.aspnetcore/web.oauth2.resources.aspnetcore.csproj --output ../../www/oauth2-resources-aspnetcore.test

dotnet publish ./1-src/web.oauth2.client.aspnetcore/web.oauth2.client.aspnetcore.csproj --output ../../www/oauth2-client-aspnetcore.test

yes | cp -rfv ./1-src/web.oidc.client.js/ ./www/oidc-client-js.test/

yes | cp -rfv ./1-src/traefik/ ./www/traefik/

docker-compose up --detach --build
