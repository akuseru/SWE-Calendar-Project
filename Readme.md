# calendar system
This is a simple calander system for SWE 3643.

Requires: 
 * .netcore (or Visual Studio 2017 works)
 * sqlserver

 to run the development version edit appsettings.json

The important thing to edit is the `DefaultConnection` value. This needs to be a valid connection string to SQLServer

Sql server can be downloaded for free from microsofts website (for development purposes)

On linux / osx with docker sqserver can be run with:
```
sudo docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=<PASSWORD>' -p 1433:1433 -v <DATA STORAGE>:/var/opt/mssql -d microsoft/mssql-server-linux
```

you will need a database to connect to:
```
CREATE DATABASE cal;
```

be sure to replace `<PASSWORD>` and `<DATA STORAGE>` with a password and a directory path to persist data in.