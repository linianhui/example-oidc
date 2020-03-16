# https://hub.docker.com/_/gradle
FROM gradle:6.0-jdk11 as builder

USER root

COPY ./web.oauth2.resources.java/ /src/

WORKDIR /src

RUN gradle assemble --info

# https://hub.docker.com/r/lnhcode/openjdk/tags
FROM lnhcode/openjdk:11.0.6

WORKDIR /

COPY --from=builder /src/build/libs/api-0.0.1.jar /app.jar

EXPOSE 80

ENTRYPOINT ["java", "-jar","app.jar"]
