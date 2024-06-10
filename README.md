This web api project was created using Asp.Net Core 7 and Entity Framework Core. The database used in this project is Bike Stores database.
SOFTWARES USED- 
-> Microsoft Visual Studio 2022 
-> Microsoft SQL Server Management Studio 2018
-> Docker Desktop

This web api uses a database first approach for creation of model classes and for the data access, I have used Interface and Repository pattern with unit of Work.
The project also has logging capabality using Serilog which writes the application logs to the database in a separate log table(Logs) and Audit Logs are added to intercept the controller actions on the api endpoints and write in the same database in a table named "AuditLogs".
Later on Elastic search was added alongwith Kibana with the help of docker containers for visualizing data. 

The dependencies used in this project are listed below.
-> Microsoft.AspNetCore.OpenApi (Comes preinstalled when configured to enable openapi support during creation of project)
-> Microsoft.EntityFrameworkCore
-> Microsoft.EntityFrameworkCore.SqlServer
-> Microsoft.EntityFrameworkCore.Tools 
-> Serilog.AspNetCore
-> Serilog.Sinks.MSSqlServer
-> Serilog.Sinks.Debug
-> Serilog.Sinks.ElasticSearch
-> Serilog.Settings.Configuration
-> Audit.NET
-> Audit.NET.SqlServer
-> Audit.Entityframework.Core
-> Audit.WebApi.Core
-> SwashBuckle.AspNetCore (Comes preinstalled when creating an Asp.Net Web api project)

Customers Controller has custom HttpGet methods using LINQ Queries.
