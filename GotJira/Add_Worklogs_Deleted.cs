using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GotJira
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Add_Worklogs_Deleted
    {
        public List<Wlk> values { get; set; }
        public long since { get; set; }
        public long until { get; set; }
        public string self { get; set; }
        public bool lastPage { get; set; }
    }

    public class Wlk
    {
        public long worklogId { get; set; }
        public long updatedTime { get; set; }
        public List<object> properties { get; set; }
    }
}
