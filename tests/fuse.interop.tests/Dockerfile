FROM microsoft/dotnet
#ARG source
WORKDIR /app
COPY Docker/netcoreapp2.0/ .
COPY Docker/fuse.harness/ .
ENTRYPOINT ["dotnet", "fuse.interop.tests.dll"]
