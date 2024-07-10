FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build-env

WORKDIR /App

# Copy everything
COPY ./server ./
COPY ./shared ../shared

# Restore as distinct layers
RUN dotnet restore --runtime alpine-x64

# Build and publish a release
RUN dotnet publish -c Release -o out \
    --no-restore \
    --runtime alpine-x64 \
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

ENTRYPOINT ["./HelteOgHulerServer", "--urls", "http://0.0.0.0:7111"]
