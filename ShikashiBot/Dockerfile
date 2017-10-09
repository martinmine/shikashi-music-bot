FROM ubuntu:17.04
ARG source
WORKDIR /app
RUN apt-get update && apt-get upgrade && \
    apt-get install -y apt-transport-https dirmngr curl && \
	curl https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor > microsoft.gpg && \
	mv microsoft.gpg /etc/apt/trusted.gpg.d/microsoft.gpg && \
	sh -c 'echo "deb [arch=amd64] https://packages.microsoft.com/repos/microsoft-ubuntu-zesty-prod zesty main" > /etc/apt/sources.list.d/dotnetdev.list' && \
    apt-get update && \
    apt-get install -y dotnet-sdk-2.0.0 wget ffmpeg \        
        libsodium-dev libopus-dev python-setuptools && \
	easy_install pip && pip install --upgrade youtube-dl
COPY ${source:-bin/Release/netcoreapp2.0/publish} .
ENTRYPOINT ["dotnet", "ShikashiBot.dll"]
