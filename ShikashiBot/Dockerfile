FROM microsoft/dotnet:2.1-runtime-alpine
ARG source
WORKDIR /app
RUN sed -i -e 's/v[[:digit:]]\.[[:digit:]]/edge/g' /etc/apk/repositories && \
	apk add --no-cache ffmpeg \
			libsodium-dev \
			opus-dev \
			youtube-dl

COPY ${source:-bin/Release/netcoreapp2.0/publish} .
ENTRYPOINT ["dotnet", "ShikashiBot.dll"]
