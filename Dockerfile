FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8443

FROM base AS final
WORKDIR /app
COPY ./publish .
ENTRYPOINT ["dotnet", "KMS.WebApp.dll"]