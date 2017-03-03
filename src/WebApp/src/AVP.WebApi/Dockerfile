FROM microsoft/dotnet:1.0.1-sdk-projectjson
ENV ASPNETCORE_ENVIRONMENT Development
COPY bin/Debug/netcoreapp1.0/publish/ /root
COPY bin/Debug/netcoreapp1.0/publish/appsettings.json /appsettings.json
COPY bin/Debug/netcoreapp1.0/publish/appsettings.Development.json /appsettings.Development.json

EXPOSE 5000/tcp
ENV ASPNETCORE_URLS http://*:5000

ENTRYPOINT dotnet /root/AVP.WebApi.dll