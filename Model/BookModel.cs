using System;


namespace LibraryManagement.Model
{
    public class BookModel
    {
        public string UId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime PublishedDate { get; set; }
        public string ISBN { get; set; }
        public bool IsIssued { get; set; }
    }
}
