using Newtonsoft.Json;

namespace LibraryManagement.Entity
{
    public class IssueEntity:Base
    {
        [JsonProperty(PropertyName = "uid", NullValueHandling = NullValueHandling.Ignore)]
        public string UId { get; set; }


        [JsonProperty(PropertyName = "bookid", NullValueHandling = NullValueHandling.Ignore)]
        public string BookId { get; set; }


        [JsonProperty(PropertyName = "memberid", NullValueHandling = NullValueHandling.Ignore)]
        public string MemberId { get; set; }



        [JsonProperty(PropertyName = "issuedate", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime IssueDate { get; set; }


        [JsonProperty(PropertyName = "returndate", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? ReturnDate { get; set; }


        [JsonProperty(PropertyName = "isreturned", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsReturned { get; set; }
    }
}
