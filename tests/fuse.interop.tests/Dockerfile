FROM zabavnov/fuse
#RUN apt-get update -qq
#RUN DEBIAN_FRONTEND=noninteractive apt-get install -y apt-utils
#RUN DEBIAN_FRONTEND=noninteractive apt-get install -y libfuse2
WORKDIR /app
COPY *.csproj .
RUN dotnet restore
COPY Publish .
COPY / .
RUN dotnet build --no-restore -f netcoreapp2.0
ENTRYPOINT ["dotnet", "xunit", "-framework", "netcoreapp2.0", "-nobuild", "-nologo"]
