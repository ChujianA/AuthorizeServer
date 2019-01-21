FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["AuthorizeServer/AuthorizeServer.csproj", "AuthorizeServer/"]
RUN dotnet restore "AuthorizeServer/AuthorizeServer.csproj"
COPY . .
WORKDIR "/src/AuthorizeServer"
RUN dotnet build "AuthorizeServer.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "AuthorizeServer.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "AuthorizeServer.dll"]