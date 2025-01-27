﻿using Ghtk.Api;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testcontainers.MongoDb;
using Testcontainers.MsSql;

namespace Ghtp.Api.IntegrationTests
{
    public class SUTWebApplicationFactory : WebApplicationFactory<Program> 
    {
        public const string ClientSource = "TEST-CLIENT";
        public const string Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJURVNULUNMSUVOVCIsIm5hbWUiOiJKb2huIERvZSIsImlhdCI6MTUxNjIzOTAyMiwiZXhwIjoyNTE2MjM5MDIyfQ.x9wYJifrwGsH9yd9FhYmz2IwDx8cOJHv4JvacyNW2Eo";

        private readonly MsSqlContainer _msSqlContainer = new MsSqlBuilder().Build();
        private readonly MongoDbContainer _mongoDbContainer = new MongoDbBuilder().WithImage("mongo:8.0").WithExposedPort(47018).Build();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            _mongoDbContainer.StartAsync().Wait();
            _msSqlContainer.StartAsync().Wait();

            builder.UseSetting("IsTest", "true");
            builder.UseSetting("AuthenticationService", "SqlServer");
            builder.UseSetting("ConnectionStrings:SqlServerAuthenticationService", _msSqlContainer.GetConnectionString());
            builder.UseSetting("ConnectionStrings:MongoDbConnection", _mongoDbContainer.GetConnectionString());

            builder.UseSetting("IssuerSigningKey", "qx8tVBNAJHCgrjvCktD8oGluT3EFAGuiqx8tVBNAJHCgrjvCktD8oGluT3EFAGui");

            CreateTestClientSource(_msSqlContainer.GetConnectionString());
        }

        private static void CreateTestClientSource(string connectionString)
        {
            using var conn = new SqlConnection(connectionString);
            conn.Open();

            using var cmd = new SqlCommand(@"CREATE TABLE ClientSources
            (
	            ClientId	NVARCHAR(60) NOT NULL,
	            ValidFrom	DATETIME NOT NULL,
	            ValidTo		DATETIME NOT NULL,
	            IsEnable	BIT NOT NULL,
	            CONSTRAINT  pk_ClientSources PRIMARY KEY (ClientId)
            )", conn);
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT INTO ClientSources (ClientId, ValidFrom, ValidTo, IsEnable) VALUES (@ClientSource, GETDATE(), DATEADD(minute, 15, GETDATE()), 1)";
            cmd.Parameters.AddWithValue("@ClientSource", ClientSource);
            cmd.ExecuteNonQuery();

            conn.Close();
        }
    }
}
