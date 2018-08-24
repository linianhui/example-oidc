FROM node:8-alpine

COPY ./web.oauth2.resources.nodejs/package.json \
     ./web.oauth2.resources.nodejs/app.js \
     ./web.oauth2.resources.nodejs/authentication.js /app/

WORKDIR /app

RUN npm install --registry=https://registry.npm.taobao.org

EXPOSE 80

ENTRYPOINT ["node", "app.js"]