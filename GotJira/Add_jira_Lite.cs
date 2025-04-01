using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddAtlassianGotJiraJirasLite
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Assignee
    {
        public string self { get; set; }
        public string accountId { get; set; }
        public AvatarUrls avatarUrls { get; set; }
        public string displayName { get; set; }
        public bool active { get; set; }
        public string timeZone { get; set; }
        public string accountType { get; set; }
    }

    public class Attrs
    {
        public string type { get; set; }
        public string id { get; set; }
        public string collection { get; set; }
        public string url { get; set; }
        public double? width { get; set; }
        public int? height { get; set; }
        public string shortName { get; set; }
        public string text { get; set; }
        public string color { get; set; }
        public string href { get; set; }
        public string layout { get; set; }
        public int? level { get; set; }
        public int? order { get; set; }
        public bool? isNumberColumnEnabled { get; set; }
    }

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

    public class BreachTime
    {
        public DateTime iso8601 { get; set; }
        public DateTime jira { get; set; }
        public string friendly { get; set; }
        public long epochMillis { get; set; }
    }

    public class Comment
    {
        public int version { get; set; }
        public string type { get; set; }
        public List<Content> content { get; set; }
    }

    public class CompletedCycle
    {
        public StartTime startTime { get; set; }
        public StopTime stopTime { get; set; }
        public BreachTime breachTime { get; set; }
        public bool breached { get; set; }
        public GoalDuration goalDuration { get; set; }
        public ElapsedTime elapsedTime { get; set; }
        public RemainingTime remainingTime { get; set; }
    }

    public class Content
    {
        public string type { get; set; }
        public List<Content> content { get; set; }
        public Attrs attrs { get; set; }
        public string text { get; set; }
        public List<Mark> marks { get; set; }
    }

    public class CurrentStatus
    {
        public string status { get; set; }
        public string statusCategory { get; set; }
        public StatusDate statusDate { get; set; }
    }

    public class Customfield10007
    {
        public int id { get; set; }
        public string name { get; set; }
        public string state { get; set; }
        public int boardId { get; set; }
        public string goal { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public DateTime completeDate { get; set; }
    }
    public class Customfield18881
    {
        public string self { get; set; }
        public string value { get; set; }
        public string id { get; set; }
    }

    public class Customfield18882
    {
        public string self { get; set; }
        public string value { get; set; }
        public string id { get; set; }
    }
    public class Customfield10501
    {
        public string self { get; set; }
        public string value { get; set; }
        public string id { get; set; }
    }

    public class Customfield15000
    {
        public string self { get; set; }
        public string value { get; set; }
        public string id { get; set; }
    }
    public class Customfield18903
    {
        public string self { get; set; }
        public string value { get; set; }
        public string id { get; set; }
    }
    public class Customfield18960
    {
        public string self { get; set; }
        public string accountId { get; set; }
        public AvatarUrls avatarUrls { get; set; }
        public string displayName { get; set; }
        public bool active { get; set; }
        public string timeZone { get; set; }
        public string accountType { get; set; }
        public string emailAddress { get; set; }
    }
    public class Customfield18715
    {
        public string self { get; set; }
        public string value { get; set; }
        public string id { get; set; }
    }

    public class Customfield18736
    {
        public Links _links { get; set; }
        public RequestType requestType { get; set; }
        public CurrentStatus currentStatus { get; set; }
    }

    public class Customfield13200
    {
        public string self { get; set; }
        public string value { get; set; }
        public string id { get; set; }
    }
    public class Customfield18895
    {
        public string id { get; set; }
        public string name { get; set; }
        public Links _links { get; set; }
        public List<OngoingCycle> completedCycles { get; set; }
        public OngoingCycle ongoingCycle { get; set; }
        public string slaDisplayFormat { get; set; }
    }
    public class Customfield18768
    {
        public string self { get; set; }
        public string value { get; set; }
        public string id { get; set; }
    }

    public class Customfield18845
    {
        public string self { get; set; }
        public string value { get; set; }
        public string id { get; set; }
    }
    public class Customfield18840
    {
        public string self { get; set; }
        public string value { get; set; }
        public string id { get; set; }
    }
    public class Customfield18849
    {
        public string self { get; set; }
        public string value { get; set; }
        public string id { get; set; }
    }

    public class Customfield18880
    {
        public string self { get; set; }
        public string value { get; set; }
        public string id { get; set; }
    }
    public class Customfield18872
    {
        public string self { get; set; }
        public string value { get; set; }
        public string id { get; set; }
    }

    public class Customfield18873
    {
        public string self { get; set; }
        public string value { get; set; }
        public string id { get; set; }
    }
    public class Customfield18758
    {
        public string self { get; set; }
        public string value { get; set; }
        public string id { get; set; }
    }
    public class Customfield18914
    {
        public string self { get; set; }
        public string value { get; set; }
        public string id { get; set; }
    }
    public class Customfield18911
    {
        public string self { get; set; }
        public string value { get; set; }
        public string id { get; set; }
    }
    public class Customfield18878
    {
        public string self { get; set; }
        public string value { get; set; }
        public string id { get; set; }
    }
    public class Customfield18799
    {
        public string id { get; set; }
        public string name { get; set; }
        public Links _links { get; set; }
        public List<CompletedCycle> completedCycles { get; set; }
        public OngoingCycle ongoingCycle { get; set; }
        public string slaDisplayFormat { get; set; }
    }
    public class Customfield18832
    {
        public string self { get; set; }
        public string value { get; set; }
        public string id { get; set; }
    }
    public class Customfield18833
    {
        public string self { get; set; }
        public string value { get; set; }
        public string id { get; set; }
    }
    public class Customfield18910
    {
        public string self { get; set; }
        public string value { get; set; }
        public string id { get; set; }
    }
    public class Customfield18837
    {
        public string self { get; set; }
        public string value { get; set; }
        public string id { get; set; }
    }

    public class Customfield18874
    {
        public string self { get; set; }
        public string value { get; set; }
        public string id { get; set; }
    }

    public class Customfield18875
    {
        public string self { get; set; }
        public string value { get; set; }
        public string id { get; set; }
    }

    public class Customfield18876
    {
        public string self { get; set; }
        public string value { get; set; }
        public string id { get; set; }
    }
    public class Customfield18800
    {
        public string id { get; set; }
        public string name { get; set; }
        public Links _links { get; set; }
        public List<CompletedCycle> completedCycles { get; set; }
        public string slaDisplayFormat { get; set; }
    }
    public class Customfield18844
    {
        public string self { get; set; }
        public string value { get; set; }
        public string id { get; set; }
    }
    public class Customfield18838
    {
        public string self { get; set; }
        public string value { get; set; }
        public string id { get; set; }
    }
    public class Customfield18915
    {
        public string self { get; set; }
        public string value { get; set; }
        public string id { get; set; }
    }
    public class Customfield18916
    {
        public string self { get; set; }
        public string value { get; set; }
        public string id { get; set; }
    }
    public class Customfield18839
    {
        public string self { get; set; }
        public string value { get; set; }
        public string id { get; set; }
    }
    public class Customfield18918
    {
        public string id { get; set; }
        public string name { get; set; }
        public Links _links { get; set; }
        public List<CompletedCycle> completedCycles { get; set; }
        public OngoingCycle ongoingCycle { get; set; }
        public string slaDisplayFormat { get; set; }
    }
    public class Customfield18898
    {
        public string self { get; set; }
        public string value { get; set; }
        public string id { get; set; }
    }

    public class Customfield18907
    {
        public string self { get; set; }
        public string value { get; set; }
        public string id { get; set; }
    }
    public class Customfield18782
    {
        public string self { get; set; }
        public string value { get; set; }
        public string id { get; set; }
    }
    public class Customfield18908
    {
        public string self { get; set; }
        public string value { get; set; }
        public string id { get; set; }
    }

    public class Customfield18931
    {
        public string self { get; set; }
        public string value { get; set; }
        public string id { get; set; }
    }
    public class Customfield18998
    {
        public string self { get; set; }
        public string value { get; set; }
        public string id { get; set; }
    }
    public class Customfield19031
    {
        public string self { get; set; }
        public string value { get; set; }
        public string id { get; set; }
    }
    public class Customfield19064
    {
        public string self { get; set; }
        public string value { get; set; }
        public string id { get; set; }
    }

    public class Customfield19130
    {
        public string self { get; set; }
        public string value { get; set; }
        public string id { get; set; }
    }
    public class Customfield19097
    {
        public string self { get; set; }
        public string value { get; set; }
        public string id { get; set; }
    }
    public class Customfield12402
    {
        public string self { get; set; }
        public string value { get; set; }
        public string id { get; set; }
    }
    public class Description
    {
        public int version { get; set; }
        public string type { get; set; }
        public List<Content> content { get; set; }
    }

    public class ElapsedTime
    {
        public long millis { get; set; }
        public string friendly { get; set; }
    }

    public class Fields
    {
        public Parent parent { get; set; }
        public Customfield18872 customfield_18872 { get; set; }
        public Customfield18873 customfield_18873 { get; set; }
        public Customfield18758 customfield_18792 { get; set; }
        public string customfield_18834 { get; set; }
        public object customfield_17700 { get; set; }
        public Customfield18911 customfield_18911 { get; set; }
        public Customfield18878 customfield_18878 { get; set; }
        public string customfield_18835 { get; set; }
        public Project project { get; set; }
        public Customfield18758 customfield_18758 { get; set; }
        public object customfield_18714 { get; set; }
        public string customfield_18836 { get; set; }
        public Customfield18914 customfield_18914 { get; set; }
        public List<Customfield18837> customfield_18837 { get; set; }
        public Customfield18874 customfield_18874 { get; set; }
        public Customfield18875 customfield_18875 { get; set; }
        public Customfield18876 customfield_18876 { get; set; }
        public Customfield18832 customfield_18832 { get; set; }
        public Customfield18799 customfield_18799 { get; set; }
        public List<Customfield18833> customfield_18833 { get; set; }
        public Customfield18910 customfield_18910 { get; set; }
        public Customfield18908 customfield_18908 { get; set; }
        public Customfield10501 customfield_10501 { get; set; }
        public object customfield_10306 { get; set; }
        public DateTime? resolutiondate { get; set; }
        public Customfield18907 customfield_18907 { get; set; }
        public Customfield18782 customfield_18782 { get; set; }
        public DateTime? lastViewed { get; set; }
        public Priority priority { get; set; }
        public object customfield_18500 { get; set; }
        public Assignee assignee { get; set; }
        public Customfield18895 customfield_18895 { get; set; }
        public Description description { get; set; }
        public Customfield18736 customfield_18736 { get; set; }
        public Customfield13200 customfield_13200 { get; set; }
        public Customfield18931 customfield_18931 { get; set; }
        //Pedido por Analia: soporte asignado a
        public List<Customfield19130> customfield_19130 { get; set; }

        //Pedido por Analia: Informacion Insuficiente
        public Customfield18998 customfield_18998 { get; set; }
        //Pedido por Analia: Priorizado GPD
        public Customfield19031 customfield_19031 { get; set; }
        //Pedido por Analia: complejidad
        public Customfield12402 customfield_12402 { get; set; }
        //public List<Customfield18957> customfield_12402 { get; set; }
        //Pedido por Analia: Indicar si es core
        public List<Customfield19064> customfield_19064 { get; set; }
        //Pedido por Analia: jira control interno
        public List<Customfield19097> customfield_19097 { get; set; }
        public Customfield18898 customfield_18898 { get; set; }
        public Timetracking timetracking { get; set; }
        public List<Customfield10007> customfield_10007 { get; set; }
        public string customfield_10008 { get; set; }
        public string customfield_18926 { get; set; }
        public Customfield18881 customfield_18881 { get; set; }
        public string summary { get; set; }
        public Customfield18882 customfield_18882 { get; set; }
        public Customfield18840 customfield_18840 { get; set; }
        public Customfield18880 customfield_18880 { get; set; }
        public DateTime? customfield_13111 { get; set; }
        public List<Customfield18845> customfield_18845 { get; set; }
        //Pedido por Analia: Entorno
        public Customfield18849 customfield_18849 { get; set; }
        public Customfield18768 customfield_18768 { get; set; }
        public Reporter reporter { get; set; }
        public Customfield18758 customfield_12100 { get; set; }
        public DateTime? customfield_14601 { get; set; }
        public string customfield_18841 { get; set; }
        public object customfield_10004 { get; set; }
        public Customfield18800 customfield_18800 { get; set; }
        public List<Customfield18844> customfield_18844 { get; set; }
        public Customfield18838 customfield_18838 { get; set; }
        public Customfield18915 customfield_18915 { get; set; }
        //Pedido por Analia: Entorno
        public Customfield18916 customfield_18916 { get; set; }
        public Customfield18839 customfield_18839 { get; set; }
        public Customfield18918 customfield_18918 { get; set; }
        public Worklog worklog { get; set; }
        public Customfield18903 customfield_18903 { get; set; }
        //Pedido por Analia: Resolutor
        public Customfield18960 customfield_18960 { get; set; }
        public Customfield15000 customfield_15000 { get; set; }
        public Customfield18715 customfield_18715 { get; set; }
        public Status status { get; set; }
        public Issuetype issuetype { get; set; }
        public DateTime? created { get; set; }
        public DateTime? updated { get; set; }
        
    }

    public class GoalDuration
    {
        public long millis { get; set; }
        public string friendly { get; set; }
    }

    public class Icon
    {
        public string id { get; set; }
        public Links _links { get; set; }
    }

    public class IconUrls
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

    public class Issue
    {
        public string expand { get; set; }
        public string id { get; set; }
        public string self { get; set; }
        public string key { get; set; }
        public Fields fields { get; set; }
    }

    public class Issuetype
    {
        public string self { get; set; }
        public string id { get; set; }
        public string description { get; set; }
        public string iconUrl { get; set; }
        public string name { get; set; }
        public bool subtask { get; set; }
        public int avatarId { get; set; }
        public int hierarchyLevel { get; set; }
    }

    public class Links
    {
        public string self { get; set; }
        public string jiraRest { get; set; }
        public string web { get; set; }
        public string agent { get; set; }
        public IconUrls iconUrls { get; set; }
    }

    public class Mark
    {
        public string type { get; set; }
        public Attrs attrs { get; set; }
    }

    public class OngoingCycle
    {
        public StartTime startTime { get; set; }
        public BreachTime breachTime { get; set; }
        public bool breached { get; set; }
        public bool paused { get; set; }
        public bool withinCalendarHours { get; set; }
        public GoalDuration goalDuration { get; set; }
        public ElapsedTime elapsedTime { get; set; }
        public RemainingTime remainingTime { get; set; }
    }

    public class Parent
    {
        public string id { get; set; }
        public string key { get; set; }
        public string self { get; set; }
        public Fields fields { get; set; }
    }

    public class Priority
    {
        public string self { get; set; }
        public string iconUrl { get; set; }
        public string name { get; set; }
        public string id { get; set; }
    }

    public class Project
    {
        public string self { get; set; }
        public string id { get; set; }
        public string key { get; set; }
        public string name { get; set; }
        public string projectTypeKey { get; set; }
        public bool simplified { get; set; }
        public AvatarUrls avatarUrls { get; set; }
        public ProjectCategory projectCategory { get; set; }
    }

    public class ProjectCategory
    {
        public string self { get; set; }
        public string id { get; set; }
        public string description { get; set; }
        public string name { get; set; }
    }

    public class RemainingTime
    {
        public long millis { get; set; }
        public string friendly { get; set; }
    }

    public class Reporter
    {
        public string self { get; set; }
        public string accountId { get; set; }
        public AvatarUrls avatarUrls { get; set; }
        public string displayName { get; set; }
        public bool active { get; set; }
        public string timeZone { get; set; }
        public string accountType { get; set; }
        public string emailAddress { get; set; }
    }

    public class RequestType
    {
        public List<string> _expands { get; set; }
        public string id { get; set; }
        public Links _links { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string helpText { get; set; }
        public string issueTypeId { get; set; }
        public string serviceDeskId { get; set; }
        public string portalId { get; set; }
        public List<string> groupIds { get; set; }
        public Icon icon { get; set; }
    }

    public class StartTime
    {
        public DateTime iso8601 { get; set; }
        public DateTime jira { get; set; }
        public string friendly { get; set; }
        public long epochMillis { get; set; }
    }

    public class Status
    {
        public string self { get; set; }
        public string description { get; set; }
        public string iconUrl { get; set; }
        public string name { get; set; }
        public string id { get; set; }
        public StatusCategory statusCategory { get; set; }
    }

    public class StatusCategory
    {
        public string self { get; set; }
        public int id { get; set; }
        public string key { get; set; }
        public string colorName { get; set; }
        public string name { get; set; }
    }

    public class StatusDate
    {
        public DateTime iso8601 { get; set; }
        public DateTime jira { get; set; }
        public string friendly { get; set; }
        public long epochMillis { get; set; }
    }

    public class StopTime
    {
        public DateTime iso8601 { get; set; }
        public DateTime jira { get; set; }
        public string friendly { get; set; }
        public long epochMillis { get; set; }
    }

    public class Timetracking
    {
        public string originalEstimate { get; set; } //nuevo
        public string remainingEstimate { get; set; }
        public string timeSpent { get; set; }
        public long originalEstimateSeconds { get; set; } //nuevo
        public long remainingEstimateSeconds { get; set; }
        public long timeSpentSeconds { get; set; }
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

    public class Worklog
    {
        public int startAt { get; set; }
        public int maxResults { get; set; }
        public int total { get; set; }
        public List<Worklog> worklogs { get; set; }
    }

    public class Worklog2
    {
        public string self { get; set; }
        public Author author { get; set; }
        public UpdateAuthor updateAuthor { get; set; }
        public DateTime? created { get; set; }
        public DateTime? updated { get; set; }
        public DateTime started { get; set; }
        public string timeSpent { get; set; }
        public int timeSpentSeconds { get; set; }
        public string id { get; set; }
        public string issueId { get; set; }
        public Comment comment { get; set; }
    }

    public class AddJiraLite
    {
        public string expand { get; set; }
        public int startAt { get; set; }
        public int maxResults { get; set; }
        public int total { get; set; }
        public List<Issue> issues { get; set; }
    }
}
