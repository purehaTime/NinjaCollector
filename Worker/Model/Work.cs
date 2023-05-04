using Models.Models;
using Worker.Interfaces;

namespace Worker.Model
{
    public class Work
    {
        public int TaskId { get; set; }
        public CancellationTokenSource Token { get; set; }
        public ParserSettings Settings { get; set; }
    }

    public class Worker
    {
        public IWorker WorkerInstance { get; set; }
        public List<Work> Works { get; set; }
    }
}
