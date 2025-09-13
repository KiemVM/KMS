RUN dotnet publish SaaS.WebApp/SaaS.WebApp.csproj -c Release -o /app/publish
docker build -t saas-webapp .
docker rm -f webapp
docker run -d   --name webapp   --network saas_default   -p 8443:8443   saas-webapp