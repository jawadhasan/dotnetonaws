using System;

namespace demoApp.Web.Dtos
{
    public class NoteDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Details { get; set; }
        public int CategoryId { get; set; }
        public string UserId { get; set; } //set from server
    }
}
