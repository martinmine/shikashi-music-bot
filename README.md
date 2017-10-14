# shikashi-music-bot
Discord music bot in C# .NET Core.

# Building
Publish the app and run `docker build -t martinmine/shikashi-music-bot .`

# Running the bot
Create the data volume holding settings:

`docker volume create shikashibot-settings`

Start the bot where `yourserver` is the name of your server in lowercase:

`docker run -it -e "SERVER_NAME=yourserver" -v shikashibot-settings:/app/Settings martinmine/shikashi-music-bot`

Enter your bot secret and press CTRL+Q or CTRL+P to detach from the container.

# Dependencies:

- .net core 2
- ffmpeg
- opus-devel
- libsodium-devel
- youtube-dl
