FROM gradle:5.1-jdk8-alpine as builder

USER root

COPY ./web.oauth2.resources.java/ /src/

WORKDIR /src

RUN gradle assemble


FROM openjdk:8-jre-alpine

WORKDIR /

COPY --from=builder /src/build/libs/api-0.0.1-SNAPSHOT.jar /app.jar

EXPOSE 80

ENTRYPOINT ["java", "-jar","app.jar"]