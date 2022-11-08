FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env

ENV ASPNETCORE_URLS=http://+:5286
WORKDIR /app

# Copy the .csproj file and restore the project's dependencies
COPY *DevExpress.csproj ./
# Specify your DevExpress NuGet Feed URL as the package source
RUN dotnet nuget add source https://nuget.devexpress.com/im2jmHdukzZaHka6bDvjOW6a99HM9z5flAoBYYrG1ZWuU12Rm5/api
#RUN dotnet restore

# Copy source and build
COPY . ./
RUN dotnet publish -c Release -o /app/publish

#Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0

# Install the latest version of the libgdiplus library to use System.Drawing in the application
RUN apt update
RUN apt install -y libgdiplus libc6 libc6-dev
RUN apt install -y fontconfig libharfbuzz0b libfreetype6

# Optional. Install the ttf-mscorefonts-installer package 
# to use Microsoft TrueType core fonts in the application
RUN echo "deb http://ftp.debian.org/debian/ stretch contrib" >> /etc/apt/sources.list
RUN apt-get update
RUN apt-get install -y ttf-mscorefonts-installer  

COPY --from=build-env /app/publish .
# Define the entry point for the application

ENV ASPNETCORE_URLS=http://+:5286
EXPOSE 5286
ENTRYPOINT ["dotnet", "EmrCloudApi.dll"]