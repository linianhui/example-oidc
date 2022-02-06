# https://hub.docker.com/_/gradle
FROM maven:3.8-jdk-11 as builder

USER root

COPY ./web.oauth2.resources.java/ /src/

WORKDIR /src

RUN mvn package

# https://github.com/linianhui/docker/pkgs/container/openjdk
FROM ghcr.io/linianhui/openjdk:11.0.13

WORKDIR /

COPY --from=builder /src/target/java-api-0.1.jar /app.jar

EXPOSE 80

ENTRYPOINT ["/bin/bash", "-c", "java -jar app.jar"]
