FROM harbor-01.docker-registry.grupofarsiman.io/external/aspnet:8.0

WORKDIR /app

COPY ./compilado .

EXPOSE 8080

#ENV FS_SQL_SIMOVIA_SERVER=192.168.1.33\\academiagfs,49194
#ENV FS_SQL_SIMOVIA_DBNAME=SIMOVIA
#ENV FS_SQL_SIMOVIA_USER=AcademiaDEV
#ENV FS_SQL_SIMOVIA_PASSWORD=Academia.1
       

ENTRYPOINT dotnet Academia.SIMOVIA.WebAPI.dll