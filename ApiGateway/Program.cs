using ApiGateway;
using MMLib.SwaggerForOcelot.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;


namespace ApiGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {



            var builder = WebApplication.CreateBuilder(args);
            var routes = "Routes";
            builder.Configuration.AddOcelotWithSwaggerSupport(options =>
            {
                options.Folder = routes;
            });

            builder.Services.AddOcelot();
            builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
                .AddOcelot(routes, builder.Environment)
                .AddEnvironmentVariables();

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSwaggerForOcelot(builder.Configuration);

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            //app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();
            app.UseSwaggerForOcelotUI(options =>
            {
                options.PathToSwaggerGenerator = "/swagger/docs";
                options.ReConfigureUpstreamSwaggerJson = AlterUpstream.AlterUpstreamSwaggerJson;

            }).UseOcelot().Wait();

            app.Run();
        }
    }
}