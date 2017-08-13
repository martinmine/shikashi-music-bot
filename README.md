# shikashi-music-bot
Discord music bot in C# .NET Core.

# Building
Publish the app and run `docker build -t martinmine/shikashi-music-bot .`

# Running the bot
Create the data volume holding settings:

`docker volume create shikashibot-settings`

Start the bot:

`docker run -it -v shikashibot-settings:/app/Settings martinmine/shikashi-music-bot`

Enter your bot secret and press CTRL+Q or CTRL+P to detach from the container.

