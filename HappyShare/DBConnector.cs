using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using HappyShare.Data;
using System.Collections.Generic;

namespace HappyShare
{
    // Helper class for connecting various database
    internal static class DBConnectorFactory
    {
        public static void Connect2DB(string connectorName, IServiceCollection service, IConfiguration configuration)
        {
            var connectors = new Dictionary<string, IDBConnector>();
            connectors.Add("SQLServer", new SQLServerConnector());
            connectors.Add("PostgreSQL", new PostgreSQLConnector());

            connectors[connectorName].Connect2DB(service, configuration);
        }
    }

    internal interface IDBConnector
    {
        void Connect2DB(IServiceCollection service, IConfiguration configuration);
    }

    internal class SQLServerConnector : IDBConnector
    {
        public void Connect2DB(IServiceCollection service, IConfiguration configuration)
        {
            service.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }
    }
    internal class PostgreSQLConnector : IDBConnector
    {
        public void Connect2DB(IServiceCollection service, IConfiguration configuration)
        {
            service.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("PostgreSQLConnection")));
        }
    }
}
