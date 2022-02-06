# https://github.com/linianhui/docker/pkgs/container/nginx
FROM ghcr.io/linianhui/nginx:1.21

COPY ./web.oidc.client.js/ /usr/share/nginx/html

RUN chmod -R 755 /usr/share/nginx/html

EXPOSE 80
