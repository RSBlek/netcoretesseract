FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
RUN apt-get update
RUN apt-get install -y libgif7 libjpeg62 libopenjp2-7 libpng16-16 libtiff5 libwebp6
RUN apt-get install -y gcc
RUN ln -s /app/x64/liblept1753.so /usr/lib/x86_64-linux-gnu/liblept.so.5

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["WebOcr/WebOcr.csproj", "WebOcr/"]
RUN dotnet restore "WebOcr/WebOcr.csproj"
COPY . .
WORKDIR "/src/WebOcr"
RUN dotnet build "WebOcr.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "WebOcr.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
ENV OMP_THREAD_LIMIT 1
EXPORT OMP_THREAD_LIMIT
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "WebOcr.dll"]