using Microsoft.Extensions.DependencyInjection;

namespace Worker.ServiceExtension
{
    public static class AddWorkerServiceExtension
    {
        public static void AddWorker<TWorker>(this IServiceCollection services) where TWorker : class, IWorker
        {
            services.AddTransient<TWorker>();
        }
    }
}
