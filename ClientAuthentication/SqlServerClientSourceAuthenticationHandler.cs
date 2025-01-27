using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientAuthentication
{
    public class SqlServerClientSourceAuthenticationHandler : IClientSourceAuthenticationHandler, IDisposable
    {
        private readonly string _connectionString;
        private readonly SqlConnection connection;
        private bool disposedValue;

        /*
        
         CREATE TABLE ClientSources
        (
	        ClientId	NVARCHAR(60) NOT NULL,
	        ValidFrom	DATETIME NOT NULL,
	        ValidTo		DATETIME NOT NULL,
	        IsEnable	BIT NOT NULL,

	        CONSTRAINT  pk_ClientSources PRIMARY KEY (ClientId),
	        -- INDEX		idx_ClientSources (ClientId, ValidFrom, ValidTo, IsEnable) -- we don't actually need this since primary key is included in the select query
        )
        
         */

        public SqlServerClientSourceAuthenticationHandler(string connectionString)
        {
            _connectionString = connectionString;

            connection = new SqlConnection(_connectionString);
        }

        public async Task<bool> ValidateAsync(string clientSource)
        {
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }

            var query = "SELECT TOP 1 1 FROM ClientSources WHERE ClientId = @ClientSource AND GETDATE() >= ValidFrom AND GETDATE() <= ValidTo AND IsEnable = 1";
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ClientSource", clientSource);
            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return true;
            }

            return false;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();
                    }

                    connection.Dispose();
                }
                disposedValue = true;
            }
        }
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
