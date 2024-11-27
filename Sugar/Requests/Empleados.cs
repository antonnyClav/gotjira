//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Sugar
//{

//    public class u_Acl
//    {
//        [JsonIgnore]
//        public u_Fields fields { get; set; }
//        public string _hash { get; set; }
//    }

//    public class u_AssignedUserLink
//    {
//        public string full_name { get; set; }
//        public string id { get; set; }
//        public u_Acl _acl { get; set; }
//    }

//    public class u_CreatedByLink
//    {
//        public string full_name { get; set; }
//        public string id { get; set; }
//        public u_Acl _acl { get; set; }
//    }

//    public class u_Fields
//    {
//        [JsonProperty("pwd_last_changed")]
//        public u_StandardProperties pwd_last_changed { get; set; }
//        [JsonProperty("last_login")]
//        public u_StandardProperties last_login { get; set; }
//    }

//    public class u_StandardProperties
//    {
//        public string create { get; set; }
//        public string write { get; set; }
//    }

//    public class u_ModifiedUserLink
//    {
//        public string full_name { get; set; }
//        public string id { get; set; }
//        public u_Acl _acl { get; set; }
//    }

//    public class u_Record
//    {
//        public string id { get; set; }
//        public string name { get; set; }
//        public DateTime date_entered { get; set; }
//        public DateTime date_modified { get; set; }
//        public string modified_user_id { get; set; }
//        public string modified_by_name { get; set; }
//        public u_ModifiedUserLink modified_user_link { get; set; }
//        public string created_by { get; set; }
//        public string created_by_name { get; set; }
//        public u_CreatedByLink created_by_link { get; set; }
//        public string description { get; set; }
//        public bool deleted { get; set; }
//        public bool following { get; set; }
//        public bool my_favorite { get; set; }
//        public List<object> tag { get; set; }
//        public List<object> locked_fields { get; set; }
//        public string sync_key { get; set; }
//        public string team_count { get; set; }
//        public u_TeamCountLink team_count_link { get; set; }
//        public List<u_TeamName> team_name { get; set; }
//        public string assigned_user_id { get; set; }
//        public string assigned_user_name { get; set; }
//        public u_AssignedUserLink assigned_user_link { get; set; }
//        public string aprobacion_obligatoria_c { get; set; }
//        public string monto_min_aprobacion_c { get; set; }
//        public string alcance_de_cuenta_c { get; set; }
//        public int base_rate { get; set; }
//        public string currency_id { get; set; }
//        public u_Acl _acl { get; set; }
//        public string _module { get; set; }
//    }

//    public class u_TeamCountLink
//    {
//        public string team_count { get; set; }
//        public string id { get; set; }
//        public u_Acl _acl { get; set; }
//    }

//    public class u_TeamName
//    {
//        public string id { get; set; }
//        public string name { get; set; }
//        public string name_2 { get; set; }
//        public bool primary { get; set; }
//        public bool selected { get; set; }
//    }
//    public class Usuarios
//    {
//        public int next_offset { get; set; }
//        public List<u_Record> records { get; set; }
//    }
//}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sugar
{

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class u_Acl
    {
        [JsonIgnore]
        public List<u_Fields> fields { get; set; }
        public string _hash { get; set; }
    }

    public class u_Email
    {
        public string email_address { get; set; }
        public bool primary_address { get; set; }
        public bool reply_to_address { get; set; }
        public bool invalid_email { get; set; }
        public bool opt_out { get; set; }
        public string email_address_id { get; set; }
    }

    public class u_Fields
    {
    }

    public class u_Record
    {
        public string id { get; set; }
        public string full_name { get; set; }
        public DateTime date_entered { get; set; }
        public DateTime date_modified { get; set; }
        public string title { get; set; }
        public string department { get; set; }
        public string phone_work { get; set; }
        public string employee_status { get; set; }        
        public string reports_to_name { get; set; }
        public string reports_to_id { get; set; }        
        public u_ReportsToLink reports_to_link { get; set; }
        public List<u_Email> email { get; set; }
        public u_Acl _acl { get; set; }
        public string _module { get; set; }
    }

    public class u_ReportsToLink
    {
        public string name { get; set; }
        public string id { get; set; }
        public u_Acl _acl { get; set; }
    }

    public class Empleados
    {
        public int next_offset { get; set; }
        public List<u_Record> records { get; set; }
    }
}
