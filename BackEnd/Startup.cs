using BackEnd.Helper;
using BackEnd.Services;
using BackEnd.Services.Interfaces;
using System.Reflection;

namespace BackEnd
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            services.AddControllers();
            services.AddHttpContextAccessor();
            services.AddServiceLanguage();
            services.AddSingleton<ITokenCreationService, TokenCreationService>();
            services.RegisterDbContexts(Configuration, migrationsAssembly);
            services.AddServices();

            services.RegisterAuthentication(Configuration);
            services.AddDistributedMemoryCache();
            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger("Nuoc Ngoc Tuan Mobile v1.0");

            app.UseHttpsRedirection();

            app.UseConfigLanguage();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();
            //app.UseAntiforgeryToken();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
