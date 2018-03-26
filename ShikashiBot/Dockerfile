FROM microsoft/dotnet:2.1-runtime-alpine
ARG source
WORKDIR /app
RUN sed -i -e 's/v[[:digit:]]\.[[:digit:]]/edge/g' /etc/apk/repositories && \
	apk add --no-cache libsodium-dev \
			opus-dev \
			youtube-dl \
			build-base curl nasm tar bzip2 \
			zlib-dev openssl-dev yasm-dev lame-dev libogg-dev x264-dev libvpx-dev libvorbis-dev x265-dev freetype-dev libass-dev libwebp-dev rtmpdump-dev libtheora-dev opus-dev && \
	DIR=$(mktemp -d) && cd ${DIR} && \
	
	curl -s http://ffmpeg.org/releases/ffmpeg-snapshot.tar.bz2 | tar jxf - -C . && \
	cd ffmpeg && \
	./configure \
	--enable-version3 --enable-gpl --enable-nonfree --enable-small --enable-libmp3lame --enable-libx264 --enable-libx265 --enable-libvpx --enable-libtheora --enable-libvorbis --enable-libopus --enable-libass --enable-libwebp --enable-librtmp --enable-postproc --enable-avresample --enable-libfreetype --enable-openssl --disable-debug && \
	make && \
	make install && \
	make distclean && \

	rm -rf ${DIR}

COPY ${source:-bin/Release/netcoreapp2.0/publish} .
ENTRYPOINT ["dotnet", "ShikashiBot.dll"]
