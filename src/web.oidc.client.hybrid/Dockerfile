FROM microsoft/aspnetcore:2.0
 
COPY . /app

WORKDIR /app

EXPOSE 8002

ENTRYPOINT ["dotnet", "Web.Oidc.Client.Hybrid.dll", "--server.urls", "http://*:8002"]