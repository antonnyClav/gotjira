using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sugar
{
    public class c_Acl
    {
        [JsonIgnore]
        public List<c_Fields> fields { get; set; }
        //public object fields { get; set; }
        public string _hash { get; set; }
    }

    public class c_Fields
    {
    }

    public class c_ModifiedUserLink
    {
        public string full_name { get; set; }
        public string id { get; set; }
        public c_Acl _acl { get; set; }
    }

    public class c_Record
    {
        public string id { get; set; }
        public string name { get; set; }
        public DateTime date_modified { get; set; }
        public string assigned_user_name { get; set; }
        public string billing_address_country { get; set; }
        public string alcance_de_cuenta_c { get; set; }
        public c_ModifiedUserLink modified_user_link { get; set; }
        public string industry { get; set; }
        public string billing_address_city { get; set; }
        public string phone_office { get; set; }
        public List<object> locked_fields { get; set; }
        public string vertical_c { get; set; }
        public List<string> version_software_c { get; set; }
        public c_Acl _acl { get; set; }
        public string _module { get; set; }
    }

    public class Cuentas
    {
        public int next_offset { get; set; }
        public List<c_Record> records { get; set; }
    }
}
