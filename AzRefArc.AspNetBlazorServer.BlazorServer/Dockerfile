FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS build
WORKDIR /src
COPY . /src
RUN dotnet build . -c Release -o /app/build

FROM build AS publish
RUN dotnet publish . -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_ENVIRONMENT Development
ENV DOTNET_ENVIRONMENT Development

CMD ["dotnet", "AzRefArc.AspNetBlazorServer.BlazorServer.dll"]

# Docker ビルド
# sudo docker build -t app .
# コンテナ内ポート 80 をローカルポート 8080 へ接続
# sudo docker run -p:8080:80 app
# イメージデバッグ (コンテナ内を参照)
# sudo docker run --rm -i -t -p:8080:80 app /bin/bash

