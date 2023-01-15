﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DbService.Models
{
    public class Post
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string GroupName { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Text { get; set; } = null!;
        public IEnumerable<Image> Images { get; set; } = null!;
        public IEnumerable<string> Tags { get; set; } = null!;
        public string Source { get; set; } = null!;
        public string OriginalLink { get; set; } = null!;

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime PostDate { get; set; }
    }
}