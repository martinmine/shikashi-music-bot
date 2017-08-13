FROM ubuntu:16.10
ARG source
WORKDIR /app
RUN apt-get update && apt-get upgrade && \
    apt-get install -y apt-transport-https dirmngr && \
    sh -c 'echo "deb [arch=amd64] https://apt-mo.trafficmanager.net/repos/dotnet-release/ yakkety main" > /etc/apt/sources.list.d/dotnetdev.list' && \
    apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys B02C46DF417A0893 && \
    apt-get update && \
    apt-get install -y dotnet-dev-1.1.0 wget ffmpeg \        
        libsodium-dev libopus-dev python-setuptools && \
		easy_install pip && pip install --upgrade youtube-dl
COPY ${source:-bin/Release/netcoreapp1.1/publish} .
ENTRYPOINT ["dotnet", "ShikashiBot.dll"]
