using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AzureWebApp
{
    public class PageModel : Controller
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }

    public class Index2Model : PageModel
    {
        private IConfigurationRoot ConfigRoot;

        public Index2Model(IConfiguration configRoot)
        {
            ConfigRoot = (IConfigurationRoot)configRoot;
        }

        public ContentResult OnGet()
        {
            ContentResult str = new ContentResult();

            foreach (var provider in ConfigRoot.Providers.ToList())
            {
                str.Content += provider.ToString() + "\n";
            }
  
            return str;
        }
    }
    public class TestModel : PageModel
    {
        // requires using Microsoft.Extensions.Configuration;
        private readonly IConfiguration Configuration;

        public TestModel(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public ContentResult OnGet()
        {
            var myKeyValue = Configuration["MyKey"];
            var title = Configuration["Position:Title"];
            var name = Configuration["Position:Name"];
            var defaultLogLevel = Configuration["Logging:LogLevel:Default"];


            return Content($"MyKey value: {myKeyValue} \n" +
                           $"Title: {title} \n" +
                           $"Name: {name} \n" +
                           $"Default Log Level: {defaultLogLevel}");
        }


        public class TestModel1 : PageModel
        {
            // requires using Microsoft.Extensions.Configuration;
            private readonly IConfiguration Configuration;

            public TestModel1(IConfiguration configuration)
            {
                Configuration = configuration;
            }

            public ContentResult OnGet()
            {
                ContentResult obj = new ContentResult();

                var myKeyValue = Configuration["MyKey"];

                var title = Configuration["Position:Title"];
                var name = Configuration["Position:Name"];
                var defaultLogLevel = Configuration["Logging:LogLevel:Default"];


                obj.Content = ($"MyKey value: {myKeyValue} \n" +
                               $"Title: {title} \n" +
                               $"Name: {name} \n" +
                               $"Default Log Level: {defaultLogLevel}"
                               );

                return obj;
            }
        }




    }
}
