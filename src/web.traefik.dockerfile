# https://github.com/linianhui/docker/pkgs/container/traefik
FROM ghcr.io/linianhui/traefik:v2.6

COPY ./traefik/ /app

WORKDIR /app

EXPOSE 80 8080
