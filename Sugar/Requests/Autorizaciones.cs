using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sugar
{
    public class a_Acl
    {
        [JsonIgnore]
        public List<a_Fields> fields { get; set; }
        //public object fields { get; set; }
        public string _hash { get; set; }
    }

    public class a_AiOppCloseWeekScores
    {
        public string create { get; set; }
        public string write { get; set; }
        public string license { get; set; }
    }

    public class a_AssignedUserLink
    {
        public string full_name { get; set; }
        public string id { get; set; }
        public a_Acl _acl { get; set; }
    }

    public class a_DriWorkflowTemplateId
    {
        public string create { get; set; }
        public string write { get; set; }
        public string license { get; set; }
    }

    public class a_DriWorkflowTemplateName
    {
        public string create { get; set; }
        public string write { get; set; }
        public string license { get; set; }
    }

    public class a_Fields
    {
        public a_AiOppCloseWeekScores ai_opp_close_week_scores { get; set; }
        public a_DriWorkflowTemplateId dri_workflow_template_id { get; set; }
        public a_DriWorkflowTemplateName dri_workflow_template_name { get; set; }
    }

    public class a_OpportunitiesRnAutorizacionesOp1
    {
        public string name { get; set; }
        public string id { get; set; }
        public a_Acl _acl { get; set; }
    }

    public class a_Record
    {
        public string id { get; set; }
        public DateTime date_entered { get; set; }
        public DateTime date_modified { get; set; }
        public List<object> locked_fields { get; set; }
        public string assigned_user_name { get; set; }
        public a_AssignedUserLink assigned_user_link { get; set; }
        public string aprobacion_obligatoria_c { get; set; }
        public string aprobacion_c { get; set; }
        public string razones_c { get; set; }
        public string opportunities_rn_autorizaciones_op_1opportunities_ida { get; set; }
        public a_OpportunitiesRnAutorizacionesOp1 opportunities_rn_autorizaciones_op_1 { get; set; }
        public string estado_c { get; set; }
        public string vigente_c { get; set; }
        public a_Acl _acl { get; set; }
        public string _module { get; set; }
    }

    public class Autorizaciones
    {
        public int next_offset { get; set; }
        public List<a_Record> records { get; set; }
    }
}
