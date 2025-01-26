using ClientAuthentication;
using Ghtk.Api.AuthenticationHandler;
using Ghtk.Authorization;
using Ghtk.Repository;

namespace Ghtk.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            IClientSourceAuthenticationHandler clientSourceAuthenticationHandler = new RemoteClientSourceAuthenticationHandler(builder.Configuration["AuthenticationService"] ?? throw new Exception("AuthenticationService Url not found"));

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddAuthentication("X-Client-Source").AddXClientSource(
                options =>
                {
                    options.ClientValidator = (clientSource, token, principle) => clientSourceAuthenticationHandler.Validate(clientSource);
                    options.IssuerSigningKey = builder.Configuration["IssuerSigningKey"] ?? "";
                }
                );

            builder.Services.AddMongoDbClient(builder.Configuration);
            builder.Services.AddScoped<IOrderRepository, MongoDbOrderRepository>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
