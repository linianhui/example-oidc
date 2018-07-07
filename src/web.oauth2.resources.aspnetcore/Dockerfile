FROM microsoft/dotnet:2.1.1-aspnetcore-runtime-alpine3.7
 
COPY . /app

WORKDIR /app

EXPOSE 80

ENTRYPOINT ["dotnet", "Web.OAuth2.Resources.AspNetCore.dll"]