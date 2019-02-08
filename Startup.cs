using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Viber.Bot;
using ViberBot.Factories;
using ViberBot.Options;
using ViberBot.Repositories;
using ViberBot.Services;
using ViberBot.Services.Bot;
using ViberBot.Services.Http;
using ViberBot.Services.StateMachine;

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
            // 
            var connectionString = Configuration.GetConnectionString("SqlServerConnection");

            // Configure viber bot client
            var viberBotOptions = Configuration.GetSection("ViberBot").Get<ViberBotOptions>();

            services.Configure<ViberBotOptions>(options =>
            {
                options.AuthenticationToken = viberBotOptions.AuthenticationToken;
                options.WebApiEndpoint = viberBotOptions.WebApiEndpoint;
            });

            // 
            services.AddSingleton<IBotFactory<IViberBotClient>, ViberBotFactory>();

            //
            services.AddSingleton<IViberApiHttpService>(provider => new ViberApiHttpService(viberBotOptions.ViberApiEndpoint));
            
            //
            services.AddSingleton<IWebApiHttpService>(provider => new WebApiHttpService(viberBotOptions.WebApiEndpoint));

            // 
            services.AddTransient<IBotService, ViberBotService>();
            
            // 
            services.AddTransient<ISendMessageService, SendMessageService>();
            
            // 
            services.AddTransient<IPeopleRepository>(provider => new PeopleRepository(connectionString));

            // 
            services.AddTransient<IBotRepository>(provider => new BotRepository(connectionString));

            // WebApiService
            //
            services.AddSingleton<IStateMachineService, StateMachineService>();

            // 
            services.AddMvcCore().AddXmlDataContractSerializerFormatters();
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

            // app.UseHttpsRedirection();

            // 
            app.UseStaticFiles();

            // 
            // app.UseMiddleware<ViberBotMiddleware>();

            // 
            app.UseMvcWithDefaultRoute();
        }
    }
}
