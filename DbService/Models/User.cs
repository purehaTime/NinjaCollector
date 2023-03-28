using MongoDB.Bson;

namespace DbService.Models
{
    public class User
    {
        public ObjectId Id { get; set; }
        public string UserName { get; set; }
        public DateTime Created { get; set; }
        public string Password { get; set; }
    }
}
