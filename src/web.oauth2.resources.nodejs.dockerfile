# https://github.com/linianhui/docker/pkgs/container/node
FROM ghcr.io/linianhui/node:16.13

COPY ./web.oauth2.resources.nodejs/package.json \
     ./web.oauth2.resources.nodejs/package-lock.json \
     ./web.oauth2.resources.nodejs/app.js \
     ./web.oauth2.resources.nodejs/authentication.js /app/

WORKDIR /app

RUN npm install

EXPOSE 80

ENTRYPOINT ["/bin/bash", "-c", "node app.js"]
