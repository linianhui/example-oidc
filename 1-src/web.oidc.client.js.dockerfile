FROM nginx:1.15.1-alpine
 
COPY ./web.oidc.client.js/ /usr/share/nginx/html

RUN chmod -R 755 /usr/share/nginx/html

EXPOSE 80
