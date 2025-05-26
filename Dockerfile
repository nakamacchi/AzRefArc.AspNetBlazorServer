FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . /src
RUN dotnet build AzRefArc.AspNetBlazorServer/AzRefArc.AspNetBlazorServer.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish AzRefArc.AspNetBlazorServer/AzRefArc.AspNetBlazorServer.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# 開発環境扱いでビルドしたい場合は以下をコメントアウト
#ENV ASPNETCORE_ENVIRONMENT Development
#ENV DOTNET_ENVIRONMENT Development

# .NET 8 の既定ではポート 8080 で起動するので 80 へ変更
ENV ASPNETCORE_URLS=http://+:80/

CMD ["dotnet", "AzRefArc.AspNetBlazorServer.dll"]