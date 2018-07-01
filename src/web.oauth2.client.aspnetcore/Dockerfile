FROM microsoft/aspnetcore:2.0
 
COPY . /app

WORKDIR /app

EXPOSE 9002

ENTRYPOINT ["dotnet", "Web.OAuth2.Client.AspNetCore.dll", "--server.urls", "http://*:9002"]