# HappyShare (Assignment for CCAD)

# After download, pls change the connection string for db in appsettings.json:

 "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=HappeShare;Trusted_Connection=True;MultipleActiveResultSets=true",
    "PostgreSQLConnection": "Host=your IBM cloud host.com;Port=your port;Username=admin;Password=your pwd;Database=HappyShare;"
  },
  "UsedDatabase": {
    //Avaliable: SQLServer, "PostgreSQL"
    "Default": "SQLServer"
    //"Default": "PostgreSQL"
  },
