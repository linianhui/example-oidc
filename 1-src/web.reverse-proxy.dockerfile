# https://hub.docker.com/_/traefik
FROM traefik:v1.7.9-alpine

COPY ./traefik/ /app

WORKDIR /app

EXPOSE 80 8080
