namespace MainService.Models
{
    public class StatusModel
    {
        public string Name { get; set; }
        public List<Work> Works { get; set; }
    }

    public class Work
    {
        public int TaskId { get; set; }
        public string SettingsId { get; set; }
        public string Group { get; set; }
        public string WorkerName { get; set; }
    }


}
