namespace ClientAuthentication.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddSingleton<IClientSourceAuthenticationHandler>(
                serviceProvider =>
                {
                    var connectionString = builder.Configuration.GetConnectionString("ClientAuthentication") ?? throw new Exception("ClientAuthentication database connection string not found");
                    return new SqlServerClientSourceAuthenticationHandler(connectionString);
                }
            );
            builder.Services.AddControllers();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
