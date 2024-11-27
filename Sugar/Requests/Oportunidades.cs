using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Sugar
{
    public class o_Accounts
    {
        public string name { get; set; }
        public string id { get; set; }
        public o_Acl _acl { get; set; }
    }

    public class o_Acl_Inicial
    {
        public object fields { get; set; }
    }

    public class o_Acl_User
    {
        public object fields { get; set; }
        public string _hash { get; set; }
    }

    public class o_Acl
    {
        public o_Fields fields { get; set; }
        public string _hash { get; set; }
    }

   
    public class o_Fields
    {
        [JsonProperty("dri_workflow_template_id")]
        public o_StandardProperties DriWorkflowTemplateId { get; set; }

        [JsonProperty("dri_workflow_template_name")]
        public o_StandardProperties DriWorkflowTemplateName { get; set; }
    }

    public class o_StandardProperties
    {
        public string Create { get; set; }
        public string Write { get; set; }
        public string License { get; set; }
    }

    public class o_ModifiedUserLink
    {
        public string full_name { get; set; }
        public string id { get; set; }
        public o_Acl_User _acl { get; set; }
    }

    public class o_Record
    {
        public string id { get; set; }
        public string name { get; set; }
        public DateTime date_modified { get; set; }
        public string modified_by_name { get; set; }
        public o_ModifiedUserLink modified_user_link { get; set; }
        public string account_id { get; set; }
        public o_Accounts accounts { get; set; }
        public string amount { get; set; }
        public string date_closed { get; set; }
        public string probability { get; set; }
        public string sales_stage { get; set; }
        public string forecasted_likely { get; set; }
        public string lost { get; set; }
        public List<object> locked_fields { get; set; }
        public int nro_oportunidad_c { get; set; }
        public o_Acl_Inicial _acl { get; set; }
        public string _module { get; set; }
        public string direccion_comercial_c { get; set; }
        public string tipo_oportunidad_c { get; set; }
        public string fecha_inicio_c { get; set; }
        public string fechacreacionoriginal_c { get; set; }
        public string montototal_c { get; set; }

    }

    public class Oportunidades
    {
        public int next_offset { get; set; }
        public List<o_Record> records { get; set; }
    }
}
