# https://hub.docker.com/_/gradle
FROM gradle:6.0-jdk8 as builder

USER root

COPY ./web.oauth2.resources.java/ /src/

WORKDIR /src

RUN gradle assemble --info

# https://hub.docker.com/r/lnhcode/openjdk/tags
FROM lnhcode/openjdk:8u242

WORKDIR /

COPY --from=builder /src/build/libs/api-0.0.1.jar /app.jar

EXPOSE 80

ENTRYPOINT ["/bin/bash", "-c", "java -jar app.jar"]
