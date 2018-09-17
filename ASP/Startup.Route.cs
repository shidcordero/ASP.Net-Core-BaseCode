using Microsoft.AspNetCore.Builder;

namespace ASP
{
    public partial class Startup
    {
        private static void ConfigureRoute(IApplicationBuilder app)
        {
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}