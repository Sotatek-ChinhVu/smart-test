FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env

WORKDIR /app

# Copy source and build
COPY . ./
###
RUN cd ./EmrCloudApi && \
    dotnet clean --configuration Release EmrCloudApi.csproj && \
    dotnet build --configuration Release EmrCloudApi.csproj && \
    dotnet publish --configuration Release EmrCloudApi.csproj -o out


#Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0

WORKDIR /app
#installs libgdiplus to support System.Drawing for handling of graphics
RUN apt-get update \
    && apt-get install -y libgdiplus \
    && ln -s /usr/liblibgdiplus.so /usr/libgdiplus.dll \
    && apt-get install -y libc6-dev libx11-dev \
    && rm -rf /var/lib/apt/lists/*

COPY --from=build-env /app/EmrCloudApi/out/ .

EXPOSE 5000
ENTRYPOINT ["dotnet", "EmrCloudApi.dll"]

