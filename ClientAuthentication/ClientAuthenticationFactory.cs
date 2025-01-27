using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientAuthentication
{
    public class ClientAuthenticationFactory
    {
        public static IClientSourceAuthenticationHandler CreateClientSourceAuthenticationHandler(IConfiguration configuration)
        {
            var t = configuration["AuthenticationService"];

            if (t == "SqlServer")
            {
                return new SqlServerClientSourceAuthenticationHandler(configuration.GetConnectionString("SqlServerAuthenticationService") ?? throw new Exception("Missing SqlServerAuthenticationService connection string"));
            }
            else if (t == "Remote")
            {
                return new RemoteClientSourceAuthenticationHandler(configuration.GetConnectionString("RemoteAuthenticationService") ?? throw new("Missing RemoteAuthenticationService connection string"));
            }

            throw new Exception("Invalid authentication service type");
        }
    }
}
