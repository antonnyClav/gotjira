using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtlassianGotJiraWorkLogs
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Author
    {
        public string self { get; set; }
        public string accountId { get; set; }
        public AvatarUrls avatarUrls { get; set; }
        public string displayName { get; set; }
        public bool active { get; set; }
        public string timeZone { get; set; }
        public string accountType { get; set; }
    }

    public class AvatarUrls
    {
        [JsonProperty("48x48")]
        public string _48x48 { get; set; }

        [JsonProperty("24x24")]
        public string _24x24 { get; set; }

        [JsonProperty("16x16")]
        public string _16x16 { get; set; }

        [JsonProperty("32x32")]
        public string _32x32 { get; set; }
    }

    public class Comment
    {
        public int version { get; set; }
        public string type { get; set; }
        public List<Content> content { get; set; }
    }

    public class Content
    {
        public string type { get; set; }
        public List<ContentDetail> content { get; set; }
    }

    public class ContentDetail
    {
        public string type { get; set; }
        public string text { get; set; }
    }

    public class Root
    {
        public int startAt { get; set; }
        public int maxResults { get; set; }
        public int total { get; set; }
        public List<AddWorklog> worklogs { get; set; }
    }

    public class UpdateAuthor
    {
        public string self { get; set; }
        public string accountId { get; set; }
        public AvatarUrls avatarUrls { get; set; }
        public string displayName { get; set; }
        public bool active { get; set; }
        public string timeZone { get; set; }
        public string accountType { get; set; }
    }

    public class AddWorklog
    {
        public string self { get; set; }
        public Author author { get; set; }
        public UpdateAuthor updateAuthor { get; set; }
        public DateTime created { get; set; }
        public DateTime updated { get; set; }
        public DateTime started { get; set; }
        public string timeSpent { get; set; }
        public long timeSpentSeconds { get; set; }
        public string id { get; set; }
        public string issueId { get; set; }
        public Comment comment { get; set; }
        //public string comment { get; set; }
    }            
}
