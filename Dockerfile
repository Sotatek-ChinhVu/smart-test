FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env

ENV ASPNETCORE_URLS=http://+:5286
WORKDIR /app

# Specify your DevExpress NuGet Feed URL as the package source
#RUN dotnet nuget add source https://nuget.devexpress.com/im2jmHdukzZaHka6bDvjOW6a99HM9z5flAoBYYrG1ZWuU12Rm5/api
#RUN dotnet restore

# Copy source and build
COPY . ./

###
RUN cd ./EmrCloudApi && \
    dotnet clean --configuration Release EmrCloudApi.csproj && \
    dotnet build --configuration Release EmrCloudApi.csproj && \
    dotnet publish --configuration Release EmrCloudApi.csproj -o publish

#RUN cd ./DevExpress && \
    dotnet clean --configuration Release DevExpress.csproj && \
    dotnet build --configuration Release DevExpress.csproj && \
    dotnet publish --configuration Release DevExpress.csproj -o publish

#Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0

WORKDIR /app
#installs libgdiplus to support System.Drawing for handling of graphics
RUN apt-get update \
    && apt-get install -y libgdiplus \
    && ln -s /usr/liblibgdiplus.so /usr/libgdiplus.dll \
    && apt-get install -y libc6-dev libx11-dev \
    && rm -rf /var/lib/apt/lists/*

COPY --from=build-env /app/EmrCloudApi/publish/ .
#COPY --from=build-env /app/DevExpress/publish/ .

ENV ASPNETCORE_URLS=http://+:5286
EXPOSE 5286
ENTRYPOINT ["dotnet", "EmrCloudApi.dll"]

