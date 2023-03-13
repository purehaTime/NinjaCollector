using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Worker.ServiceExtension
{
    public static class AddWorkerServiceExtension
    {
        public static void AddWorker<TWorker>(this IServiceCollection services) where TWorker : class, IWorker
        {
            services.AddTransient<TWorker>();
            services.TryAddSingleton<IWorkService, WorkService>();
        }
    }
}
