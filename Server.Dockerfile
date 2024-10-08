FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build-env

WORKDIR /App

# Copy everything
COPY ./server ./
COPY ./shared ../shared

# Restore as distinct layers
RUN dotnet restore --runtime linux-musl-x64

# Build and publish a release
RUN dotnet publish -c Release -o out \
    --no-restore \
    --runtime linux-musl-x64 \
    --self-contained true \
    /p:PublishTrimmed=true \
    /p:PublishSingleFile=true

# Build runtime image
FROM mcr.microsoft.com/dotnet/runtime-deps:8.0-alpine

RUN adduser --disabled-password \
    --home /App \
    --gecos '' dotnetuser && chown -R dotnetuser /App

USER dotnetuser

WORKDIR /App

COPY --from=build-env /App/out .

ARG ALLOWEDHOSTS
ARG DATABASE__CONNECTIONSTRING
ARG DATABASE__DATABASENAME
ARG SENTRY__DSN

ENV ALLOWEDHOSTS=$ALLOWEDHOSTS
ENV DATABASE__CONNECTIONSTRING=$DATABASE__CONNECTIONSTRING
ENV DATABASE__DATABASENAME=$DATABASE__DATABASENAME
ENV SENTRY__DSN=$SENTRY__DSN

ENTRYPOINT ["./HelteOgHulerServer", "--urls", "http://0.0.0.0:80"]
