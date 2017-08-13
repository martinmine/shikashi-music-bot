dotnet publish --framework netcoreapp1.1 -c release
docker build -t martinmine/discord-music-bot .
docker push martinmine/discord-music-bot
bash push-prod.sh
