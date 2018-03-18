FROM microsoft/dotnet:2.1-runtime-alpine
ARG source
WORKDIR /app
RUN apk update && \
apk add ffmpeg opus-dev libsodium-dev youtube-dl

COPY ${source:-bin/Release/netcoreapp2.0/publish} .
ENTRYPOINT ["dotnet", "ShikashiBot.dll"]
