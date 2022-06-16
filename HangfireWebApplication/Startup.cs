using Hangfire;
using Hangfire.Dashboard;
using Microsoft.Owin;
using Owin;
using System;
using System.Configuration;

[assembly: OwinStartup(typeof(HangfireWebApplication.Startup))]

namespace HangfireWebApplication
{
    public class MyAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var owinContext = new OwinContext(context.GetOwinEnvironment());
            return true;
        }
    }

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            string connectionString = ConfigurationManager.AppSettings["ConnectionString"];
            GlobalConfiguration.Configuration.UseSqlServerStorage(connectionString);

            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new MyAuthorizationFilter() }
            });
            app.UseHangfireServer();

            //Tek Sefer Çalışacak İşler
            var jobFireAndForgetId = BackgroundJob.Enqueue(() => Console.WriteLine("Tek Sefer Çalışacak"));

            //Tekrarlanacak İşler
            //RecurringJob.AddOrUpdate(() => Console.WriteLine("Tekrarlanacak"), Cron.Daily);

            //Gecikmeli Çalışacak İşler
            //RecurringJob.AddOrUpdate(() => Console.WriteLine("Gecikmeli Çalışacak"), Cron.Daily);

            //İleri Tarihte Çalışacak İşler
            //var jobId = BackgroundJob.Schedule(() => Console.WriteLine("İleri Tarihte Çalışacak"), TimeSpan.FromDays(7));
        }

    }
}
