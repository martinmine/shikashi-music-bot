FROM microsoft/aspnetcore:1.1
ARG source
WORKDIR /app
RUN apt-get update && apt-get upgrade && \
		apt-get install -y libsodium13 libopus-dev libav-tools python-setuptools && \
		easy_install pip && pip install --upgrade youtube-dl && \
		ln -s /usr/bin/avconv /usr/bin/ffmpeg
COPY ${source:-bin/Release/Docker} .
ENTRYPOINT ["dotnet", "ShikashiBot.dll"]
