using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Viber.Bot;
using ViberBot.Options;
using ViberBot.Repositories;
using ViberBot.Services;

namespace ViberBot
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Configure viber bot client
            var viberBotOptions = Configuration.GetSection("ViberBotOptions").Get<ViberBotOptions>();

            services.Configure<ViberBotOptions>(options =>
            {
                options.AuthenticationToken = viberBotOptions.AuthenticationToken;
            });

            services.AddSingleton<IViberBotClient>(provider => new ViberBotClient(viberBotOptions.AuthenticationToken));
            
            // Configure viber bot service
            services.AddSingleton<IBotService, ViberBotService>();
            
            // 
            services.AddSingleton<ISendMessageService, SendMessageService>();

            services.AddSingleton<IUserStateMachineService, InMemoryUserStateMachineService>();

            // Configure SQLite database
            // var connectionString = Configuration.GetConnectionString("SqliteConnection");

            // services.AddScoped<IUserRepository>(provider => new UserRepository(connectionString, provider));

            //
            services.AddMvcCore();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // app.UseHsts();
            }

            //app.UseHttpsRedirection();

            app.UseStaticFiles();

            // app.UseViberWebhook();
            app.UseMvcWithDefaultRoute();
        }
    }
}
