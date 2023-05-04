namespace MainService.Models
{
    public static class Source
    {
        public static List<string> GetAll()
        {
            return new List<string>
            {
                "reddit",
                "telegram"
            };
        }

    }
}
