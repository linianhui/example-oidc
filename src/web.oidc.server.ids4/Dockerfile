FROM microsoft/aspnetcore:2.0
 
COPY . /app

WORKDIR /app

EXPOSE 80

ENTRYPOINT ["dotnet", "Web.Oidc.Server.Ids4.dll"]