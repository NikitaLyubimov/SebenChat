using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Infrustructure.Shared
{
    public abstract class DesignTimeDbContextFactory<TContext> : IDesignTimeDbContextFactory<TContext>
        where TContext : DbContext
    {
        public TContext CreateDbContext(string[] args)
        {
            return Create(Directory.GetCurrentDirectory(), Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
        }

        protected abstract TContext CreateNewInstance(DbContextOptions<TContext> options);

        public TContext Create()
        {
            var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var basePath = AppContext.BaseDirectory;

            return Create(basePath, envName);
            
        }

        private TContext Create(string basePath, string envName)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{envName}.json", true)
                .AddEnvironmentVariables();

            var config = builder.Build();

            var connStr = config.GetConnectionString("Default");

            if (string.IsNullOrEmpty(connStr))
            {
                throw new InvalidOperationException("Couldn't find connection string");
            }
            return Create(connStr);

        }

        private TContext Create(string connStr)
        {
            if (string.IsNullOrEmpty(connStr))
                throw new ArgumentException("Connection string is null");

            var optionsBuilder = new DbContextOptionsBuilder<TContext>();

            optionsBuilder.UseSqlServer(connStr);
            var options = optionsBuilder.Options;
            return CreateNewInstance(options);
        }
    }
}
