FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env

ENV ASPNETCORE_URLS=http://+:5286
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
RUN apt-get update && \
    apt-get install -y libc6 -f -o APT::Immediate-Configure=0 && \
    apt-get install -y \
        libgdiplus \
        libicu-dev \
        libharfbuzz0b \
        libfontconfig1 \
        libfreetype6 \
        libpango-1.0-0 \
        libpangocairo-1.0

COPY --from=build-env /app/EmrCloudApi/out/ .
#RUN rm -rf EmrCloudApi.runtimeconfig.json
#COPY --from=build-env /app/EmrCloudApi.runtimeconfig.json .
ENV ASPNETCORE_URLS=http://+:5286
EXPOSE 5286
ENTRYPOINT ["dotnet", "EmrCloudApi.dll"]
