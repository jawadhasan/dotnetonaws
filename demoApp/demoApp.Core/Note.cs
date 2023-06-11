using System;

namespace demoApp.Core
{
    public class Note
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Details { get; set; }
        public int CategoryId { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
