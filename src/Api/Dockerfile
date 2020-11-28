FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-focal
ARG source
WORKDIR /app
EXPOSE 9090
COPY ${source:-obj/Docker/publish} .
ENTRYPOINT ["dotnet", "Api.dll"]