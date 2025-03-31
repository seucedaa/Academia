FROM harbor-01.docker-registry.grupofarsiman.io/external/aspnet:8.0

WORKDIR /app

COPY ./out .

ENV TZ=America/Tegucigalpa
       

ENTRYPOINT dotnet /app/Academia.SIMOVIA.WebAPI.dll