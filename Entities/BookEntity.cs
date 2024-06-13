using Newtonsoft.Json;

namespace LibraryManagement.Entity
{
    public class BookEntity: Base
    {
  


        [JsonProperty(PropertyName = "id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        [JsonProperty(PropertyName ="uid",NullValueHandling=NullValueHandling.Ignore)]
        public string UId { get; set; }

        [JsonProperty(PropertyName = "title", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "author", NullValueHandling = NullValueHandling.Ignore)]
        public string Author { get; set; }

        [JsonProperty(PropertyName = "publishdate", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime PublishedDate { get; set; }

        [JsonProperty(PropertyName = "isbn", NullValueHandling = NullValueHandling.Ignore)]
        public string ISBN { get; set; }

        [JsonProperty(PropertyName = "isissued", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsIssued { get; set; }
        [JsonProperty(PropertyName = "documentType", NullValueHandling = NullValueHandling.Ignore)]
        public string DocumentType { get; set; }


    }
}
