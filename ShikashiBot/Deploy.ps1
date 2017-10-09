dotnet publish -c Release
docker build -t martinmine/shikashi-music-bot .
docker push martinmine/shikashi-music-bot
#bash push-prod.sh
