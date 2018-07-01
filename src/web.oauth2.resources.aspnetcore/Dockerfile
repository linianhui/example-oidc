FROM microsoft/aspnetcore:2.0
 
COPY . /app

WORKDIR /app

EXPOSE 9001

ENTRYPOINT ["dotnet", "Web.OAuth2.Resources.AspNetCore.dll", "--server.urls", "http://*:9001"]