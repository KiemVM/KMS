RUN dotnet publish KMS.WebApp/KMS.WebApp.csproj -c Release -o /app/publish
docker build -t kms-webapp .
docker rm -f kms_web
docker run -d   --name kms_web   --network saas_default   -p 9443:9443   --restart unless-stopped kms-webapp