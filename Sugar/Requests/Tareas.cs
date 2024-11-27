using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sugar
{
    public class t_Acl
    {
        public object fields { get; set; }
        public string _hash { get; set; }
    }

    public class t_AiOppCloseWeekScores
    {
        public string create { get; set; }
        public string write { get; set; }
        public string license { get; set; }
    }

    public class t_AssignedUserLink
    {
        public string full_name { get; set; }
        public string id { get; set; }
        public t_Acl _acl { get; set; }
    }

    public class t_Contacts
    {
        public string name { get; set; }
        public string id { get; set; }
        public t_Acl _acl { get; set; }
        public string phone_work { get; set; }
    }

    public class t_CreatedByLink
    {
        public string full_name { get; set; }
        public string id { get; set; }
        public t_Acl _acl { get; set; }
    }

    public class t_DriWorkflowTemplateId
    {
        public string create { get; set; }
        public string write { get; set; }
        public string license { get; set; }
    }

    public class t_DriWorkflowTemplateName
    {
        public string create { get; set; }
        public string write { get; set; }
        public string license { get; set; }
    }

    public class t_Fields
    {
        public t_AiOppCloseWeekScores ai_opp_close_week_scores { get; set; }
        public t_DriWorkflowTemplateId dri_workflow_template_id { get; set; }
        public t_DriWorkflowTemplateName dri_workflow_template_name { get; set; }
    }

    public class t_ModifiedUserLink
    {
        public string full_name { get; set; }
        public string id { get; set; }
        public t_Acl _acl { get; set; }
    }

    public class t_Parent
    {
        public t_Acl _acl { get; set; }        
        public string id { get; set; }
        public string type { get; set; }
        public string name { get; set; }
    }

    public class t_Record
    {
        public string id { get; set; }
        public string name { get; set; }
        public DateTime date_entered { get; set; }
        public DateTime date_modified { get; set; }
        public string modified_user_id { get; set; }
        public string modified_by_name { get; set; }
        public t_ModifiedUserLink modified_user_link { get; set; }
        public string created_by { get; set; }
        public string created_by_name { get; set; }
        public t_CreatedByLink created_by_link { get; set; }
        public string description { get; set; }
        public bool deleted { get; set; }
        public string status { get; set; }
        public bool date_due_flag { get; set; }
        public object date_due { get; set; }
        public string time_due { get; set; }
        public bool date_start_flag { get; set; }
        public object date_start { get; set; }
        public string parent_type { get; set; }
        public string parent_name { get; set; }
        [JsonIgnore]        
        public t_Parent parent { get; set; }
        //public object parent { get; set; }
        public string parent_id { get; set; }
        public string contact_id { get; set; }
        public string contact_name { get; set; }
        public t_Contacts contacts { get; set; }
        public string contact_phone { get; set; }
        public string contact_email { get; set; }
        public string priority { get; set; }
        public bool following { get; set; }
        public bool my_favorite { get; set; }
        public List<object> tag { get; set; }
        public List<object> locked_fields { get; set; }
        public string sync_key { get; set; }
        public string assigned_user_id { get; set; }
        public string assigned_user_name { get; set; }
        public t_AssignedUserLink assigned_user_link { get; set; }
        public string team_count { get; set; }
        public t_TeamCountLink team_count_link { get; set; }
        public List<t_TeamName> team_name { get; set; }
        public string dri_workflow_id { get; set; }
        public string fecha_cierre_c { get; set; }
        public string cuenta_c { get; set; }
        public string experto_asignado_c { get; set; }
        public string asunto_c { get; set; }
        public t_Acl _acl { get; set; }
        public string _module { get; set; }
}

    public class t_TeamCountLink
    {
        public string team_count { get; set; }
        public string id { get; set; }
        public t_Acl _acl { get; set; }
    }

    public class t_TeamName
    {
        public string id { get; set; }
        public string name { get; set; }
        public string name_2 { get; set; }
        public bool primary { get; set; }
        public bool selected { get; set; }
    }

    public class Tareas
    {
        public int next_offset { get; set; }
        public List<t_Record> records { get; set; }
    }
}
