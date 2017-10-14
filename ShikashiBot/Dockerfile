FROM fedora:latest
ARG source
WORKDIR /app
RUN rpm -Uvh http://download1.rpmfusion.org/free/fedora/rpmfusion-free-release-stable.noarch.rpm http://download1.rpmfusion.org/nonfree/fedora/rpmfusion-nonfree-release-stable.noarch.rpm && \
    dnf install -y dnf-plugins-core && \
    dnf copr enable -y @dotnet-sig/dotnet && \
    dnf install -y \
        dotnet-sdk-2.0 \
        ffmpeg \
        opus-devel \
        libsodium-devel \
        python && \
    pip install youtube-dl

COPY ${source:-bin/Release/netcoreapp2.0/publish} .
ENTRYPOINT ["dotnet", "ShikashiBot.dll"]
