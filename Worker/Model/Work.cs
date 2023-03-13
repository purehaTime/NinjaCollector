namespace Worker.Model
{
    public class Work
    {
        public int TaskId { get; set; }
        public CancellationTokenSource Token { get; set; }
        public Settings Settings { get; set; }
    }
}
