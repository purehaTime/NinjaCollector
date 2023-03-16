using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Worker.Interfaces;

namespace Worker.ServiceExtension
{
    public static class AddWorkerServiceExtension
    {
        public static void AddWorker<TWorker>(this IServiceCollection services) where TWorker : class, IWorker
        {
            services.AddTransient<IWorker, TWorker>();
            services.AddTransient<TWorker>();
            services.TryAddTransient<IWorkService, WorkService>();
        }
    }
}
