dotnet publish --framework netcoreapp1.1 -c release
docker build -t martinmine/shikashi-music-bot .
docker push martinmine/shikashi-music-bot
bash push-prod.sh
