FROM microsoft/aspnetcore:2.0
 
COPY . /app

WORKDIR /app

EXPOSE 8001

ENTRYPOINT ["dotnet", "Web.Oidc.Server.Ids4.dll", "--server.urls", "http://*:8001"]