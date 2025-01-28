using ClientAuthentication;
using Ghtk.Api.AutoMapperProfiles;
using Ghtk.Authorization;
using Ghtk.Repository;

namespace Ghtk.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            IClientSourceAuthenticationHandler clientSourceAuthenticationHandler = ClientAuthenticationFactory.CreateClientSourceAuthenticationHandler(builder.Configuration);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddAuthentication("X-Client-Source").AddXClientSource(
                options =>
                {
                    options.ClientValidator = async (clientSource, token, principle) => await clientSourceAuthenticationHandler.ValidateAsync(clientSource);
                    options.IssuerSigningKey = builder.Configuration["IssuerSigningKey"] ?? "";
                }
                );

            builder.Services.AddAutoMapper(typeof(OrderProfile));
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
