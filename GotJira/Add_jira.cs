using System;
using System.Collections.Generic;

// Estas clases sirven como factory para recuperar los issues con todas sus propiedades y campos custom.
namespace AddAtlassianGotJiraJiras
{
    public class Resolution
    {
        public string self { get; set; }
        public string id { get; set; }
        public string description { get; set; }
        public string name { get; set; }
    }

    public class Type
    {
        public string id { get; set; }
        public string name { get; set; }
        public string inward { get; set; }
        public string outward { get; set; }
        public string self { get; set; }
    }

    public class StatusCategory
    {
        public string self { get; set; }
        public long id { get; set; }
        public string key { get; set; }
        public string colorName { get; set; }
        public string name { get; set; }
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

    public class Priority
    {
        public string self { get; set; }
        public string iconUrl { get; set; }
        public string name { get; set; }
        public string id { get; set; }
    }

    public class Issuetype
    {
        public string self { get; set; }
        public string id { get; set; }
        public string description { get; set; }
        public string iconUrl { get; set; }
        public string name { get; set; }
        public bool subtask { get; set; }
        public long avatarId { get; set; }
    }

    public class Fields2
    {
        public string summary { get; set; }
        public Status status { get; set; }
        public Priority priority { get; set; }
        public Issuetype issuetype { get; set; }
    }

    public class OutwardIssue
    {
        public string id { get; set; }
        public string key { get; set; }
        public string self { get; set; }
        public Fields2 fields { get; set; }
    }

    public class Issuelink
    {
        public string id { get; set; }
        public string self { get; set; }
        public Type type { get; set; }
        public OutwardIssue outwardIssue { get; set; }
    }

    public class AvatarUrls
    {
        public string __invalid_name__48x48 { get; set; }
        public string __invalid_name__24x24 { get; set; }
        public string __invalid_name__16x16 { get; set; }
        public string __invalid_name__32x32 { get; set; }
    }

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

    public class AvatarUrls2
    {
        public string __invalid_name__48x48 { get; set; }
        public string __invalid_name__24x24 { get; set; }
        public string __invalid_name__16x16 { get; set; }
        public string __invalid_name__32x32 { get; set; }
    }

    public class Reporter
    {
        public string self { get; set; }
        public string accountId { get; set; }
        public AvatarUrls2 avatarUrls { get; set; }
        public string displayName { get; set; }
        public bool active { get; set; }
        public string timeZone { get; set; }
        public string accountType { get; set; }
    }

    public class Progress
    {
        public long progress { get; set; }
        public long total { get; set; }
        public long percent { get; set; }
    }

    public class Votes
    {
        public string self { get; set; }
        public long votes { get; set; }
        public bool hasVoted { get; set; }
    }

    public class AvatarUrls3
    {
        public string __invalid_name__48x48 { get; set; }
        public string __invalid_name__24x24 { get; set; }
        public string __invalid_name__16x16 { get; set; }
        public string __invalid_name__32x32 { get; set; }
    }

    public class Author
    {
        public string self { get; set; }
        public string accountId { get; set; }
        public AvatarUrls3 avatarUrls { get; set; }
        public string displayName { get; set; }
        public bool active { get; set; }
        public string timeZone { get; set; }
        public string accountType { get; set; }
    }

    public class AvatarUrls4
    {
        public string __invalid_name__48x48 { get; set; }
        public string __invalid_name__24x24 { get; set; }
        public string __invalid_name__16x16 { get; set; }
        public string __invalid_name__32x32 { get; set; }
    }

    public class UpdateAuthor
    {
        public string self { get; set; }
        public string accountId { get; set; }
        public AvatarUrls4 avatarUrls { get; set; }
        public string displayName { get; set; }
        public bool active { get; set; }
        public string timeZone { get; set; }
        public string accountType { get; set; }
    }

    public class Worklog2
    {
        public string self { get; set; }
        public Author author { get; set; }
        public UpdateAuthor updateAuthor { get; set; }
        public DateTime? created { get; set; }
        public DateTime? updated { get; set; }
        public DateTime? started { get; set; }
        public string timeSpent { get; set; }
        public long timeSpentSeconds { get; set; }
        public string id { get; set; }
        public string issueId { get; set; }

        public string comment { get; set; }
    }

    public class Worklog
    {
        public long startAt { get; set; }
        public long maxResults { get; set; }
        public long total { get; set; }
        public List<Worklog2> worklogs { get; set; }
    }

    public class Issuetype2
    {
        public string self { get; set; }
        public string id { get; set; }
        public string description { get; set; }
        public string iconUrl { get; set; }
        public string name { get; set; }
        public bool subtask { get; set; }
        public long avatarId { get; set; }
    }

    public class AvatarUrls5
    {
        public string __invalid_name__48x48 { get; set; }
        public string __invalid_name__24x24 { get; set; }
        public string __invalid_name__16x16 { get; set; }
        public string __invalid_name__32x32 { get; set; }
    }

    public class ProjectCategory
    {
        public string self { get; set; }
        public string id { get; set; }
        public string description { get; set; }
        public string name { get; set; }
    }

    public class Project
    {
        public string self { get; set; }
        public string id { get; set; }
        public string key { get; set; }
        public string name { get; set; }
        public string projectTypeKey { get; set; }
        public bool simplified { get; set; }
        public AvatarUrls5 avatarUrls { get; set; }
        public ProjectCategory projectCategory { get; set; }
    }

    public class Watches
    {
        public string self { get; set; }
        public long watchCount { get; set; }
        public bool isWatching { get; set; }
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

    public class NonEditableReason
    {
        public string reason { get; set; }
        public string message { get; set; }
    }

    public class Customfield16100
    {
        public bool hasEpicLinkFieldDependency { get; set; }
        public bool showField { get; set; }
        public NonEditableReason nonEditableReason { get; set; }
    }

    public class Customfield10501
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

    public class Customfield18907
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

    public class Customfield18768
    {
        public string self { get; set; }
        public string value { get; set; }
        public string id { get; set; }
    }

    // Campo pedido por Viviana - M. Lione
    public class Customfield18837
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
    public class Customfield18844
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
    public class Customfield18839
    {
        public string self { get; set; }
        public string value { get; set; }
        public string id { get; set; }
    }
    public class Customfield18832
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
    // Fin campos agregados - M. Lione

    //Inicio Pedido Diego Paez
    public class Customfield18736
    {
        public Links _links { get; set; }
        public RequestType requestType { get; set; }
        public CurrentStatus currentStatus { get; set; }
    }

    public class Links
    {
        public string self { get; set; }
    }
    public class RequestType
    {
        public IList<string> _expands { get; set; }
        public string id { get; set; }
        public Links _links { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string helpText { get; set; }
        public string issueTypeId { get; set; }
        public string serviceDeskId { get; set; }
        public string portalId { get; set; }
        public IList<string> groupIds { get; set; }
        public Icon icon { get; set; }
    }
    public class CurrentStatus
    {
        public string status { get; set; }
        public string statusCategory { get; set; }
        public StatusDate statusDate { get; set; }
    }
    public class Icon
    {
        public string id { get; set; }
        public Links _links { get; set; }
    }
    public class StatusDate
    {
        public DateTime iso8601 { get; set; }
        public DateTime jira { get; set; }
        public string friendly { get; set; }
        public long epochMillis { get; set; }
    }
    //Fin Pedido Diego Paez

    //Pedido por Analia
    public class Customfield18898
    {
        public string self { get; set; }
        public string value { get; set; }
        public string id { get; set; }
    }

    public class Customfield18715
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

    public class Customfield14600
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
    // Campos nuevos agregados por M. Lione
    public class Customfield18838
    {
        public string self { get; set; }
        public string value { get; set; }
        public string id { get; set; }
    }
    // Finalización campos nuevos agregados
    public class AvatarUrls6
    {
        public string __invalid_name__48x48 { get; set; }
        public string __invalid_name__24x24 { get; set; }
        public string __invalid_name__16x16 { get; set; }
        public string __invalid_name__32x32 { get; set; }
    }

    public class Author2
    {
        public string self { get; set; }
        public string accountId { get; set; }
        public AvatarUrls6 avatarUrls { get; set; }
        public string displayName { get; set; }
        public bool active { get; set; }
        public string timeZone { get; set; }
        public string accountType { get; set; }
    }

    public class AvatarUrls7
    {
        public string __invalid_name__48x48 { get; set; }
        public string __invalid_name__24x24 { get; set; }
        public string __invalid_name__16x16 { get; set; }
        public string __invalid_name__32x32 { get; set; }
    }

    public class UpdateAuthor2
    {
        public string self { get; set; }
        public string accountId { get; set; }
        public AvatarUrls7 avatarUrls { get; set; }
        public string displayName { get; set; }
        public bool active { get; set; }
        public string timeZone { get; set; }
        public string accountType { get; set; }
    }

    public class Comment2
    {
        public string self { get; set; }
        public string id { get; set; }
        public Author2 author { get; set; }
        public string body { get; set; }
        public UpdateAuthor2 updateAuthor { get; set; }
        public DateTime? created { get; set; }
        public DateTime? updated { get; set; }
        public bool jsdPublic { get; set; }
    }

    public class Comment
    {
        public List<Comment2> comments { get; set; }
        public long maxResults { get; set; }
        public long total { get; set; }
        public long startAt { get; set; }
    }

    public class Priority2
    {
        public string self { get; set; }
        public string iconUrl { get; set; }
        public string name { get; set; }
        public string id { get; set; }
    }

    public class StatusCategory2
    {
        public string self { get; set; }
        public long id { get; set; }
        public string key { get; set; }
        public string colorName { get; set; }
        public string name { get; set; }
    }

    public class Status2
    {
        public string self { get; set; }
        public string description { get; set; }
        public string iconUrl { get; set; }
        public string name { get; set; }
        public string id { get; set; }
        public StatusCategory2 statusCategory { get; set; }
    }

    public class AvatarUrls8
    {
        public string __invalid_name__48x48 { get; set; }
        public string __invalid_name__24x24 { get; set; }
        public string __invalid_name__16x16 { get; set; }
        public string __invalid_name__32x32 { get; set; }
    }

    public class Creator
    {
        public string self { get; set; }
        public string accountId { get; set; }
        public AvatarUrls8 avatarUrls { get; set; }
        public string displayName { get; set; }
        public bool active { get; set; }
        public string timeZone { get; set; }
        public string accountType { get; set; }
    }

    public class Aggregateprogress
    {
        public long progress { get; set; }
        public long total { get; set; }
        public long percent { get; set; }
    }

    public class Customfield10304
    {
        public string self { get; set; }
        public string value { get; set; }
        public string id { get; set; }
    }

    public class Customfield14200
    {
        public string self { get; set; }
        public string value { get; set; }
        public string id { get; set; }
    }

    public class Customfield13900
    {
        public string self { get; set; }
        public string value { get; set; }
        public string id { get; set; }
    }

    public class AvatarUrls9
    {
        public string __invalid_name__48x48 { get; set; }
        public string __invalid_name__24x24 { get; set; }
        public string __invalid_name__16x16 { get; set; }
        public string __invalid_name__32x32 { get; set; }
    }

    public class Customfield11601
    {
        public string self { get; set; }
        public string accountId { get; set; }
        public AvatarUrls9 avatarUrls { get; set; }
        public string displayName { get; set; }
        public bool active { get; set; }
        public string timeZone { get; set; }
        public string accountType { get; set; }
    }

    public class Customfield13804
    {
        public string self { get; set; }
        public string accountId { get; set; }
        public AvatarUrls2 avatarUrls { get; set; }
        public string displayName { get; set; }
        public bool active { get; set; }
        public string timeZone { get; set; }
        public string accountType { get; set; }
    }

    public class Parent
    {
        public string id { get; set; }
        public string key { get; set; }
        public string self { get; set; }
        public Fields2 fields { get; set; }
    }

    public class Customfield10007
    {
        public int id { get; set; }
        public string name { get; set; }
        public string state { get; set; }
        public int boardId { get; set; }
    }

    public class Customfield18800
    {
        public string id { get; set; }
        public string name { get; set; }
        public Links _links { get; set; }
        public List<CompletedCycle> completedCycles { get; set; }
        public string slaDisplayFormat { get; set; }
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

    public class Customfield18799
    {
        public string id { get; set; }
        public string name { get; set; }
        public Links _links { get; set; }
        public List<CompletedCycle> completedCycles { get; set; }
        public OngoingCycle ongoingCycle { get; set; }
        public string slaDisplayFormat { get; set; }
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

    public class Customfield18878
    {
        public string self { get; set; }
        public string value { get; set; }
        public string id { get; set; }
    }

    public class Customfield13200
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
    public class Customfield18782
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

    public class Customfield18911
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

    public class Customfield18915
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

    public class Customfield12402
    {
        public string self { get; set; }
        public string value { get; set; }
        public string id { get; set; }
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

    public class StartTime
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

    public class BreachTime
    {
        public DateTime iso8601 { get; set; }
        public DateTime jira { get; set; }
        public string friendly { get; set; }
        public long epochMillis { get; set; }
    }
    public class GoalDuration
    {
        public long millis { get; set; }
        public string friendly { get; set; }
    }

    public class ElapsedTime
    {
        public long millis { get; set; }
        public string friendly { get; set; }
    }

    public class RemainingTime
    {
        public long millis { get; set; }
        public string friendly { get; set; }
    }
    public class Fields
    {
        public Parent parent { get; set; }
        public object customfield_15400 { get; set; }
        public object customfield_17700 { get; set; }
        public object customfield_15401 { get; set; }
        public object customfield_13101 { get; set; }
        public object customfield_13104 { get; set; }
        public object customfield_13103 { get; set; }
        public Resolution resolution { get; set; }
        public object customfield_13106 { get; set; }
        public object customfield_17702 { get; set; }
        public object customfield_17701 { get; set; }
        public object customfield_13105 { get; set; }
        public object customfield_12800 { get; set; }
        public object customfield_10500 { get; set; }
        public Customfield10501 customfield_10501 { get; set; }
        public object customfield_12802 { get; set; }
        public object customfield_18100 { get; set; }
        public DateTime? lastViewed { get; set; }
        public object customfield_18101 { get; set; }
        public object customfield_18102 { get; set; }
        public object customfield_18103 { get; set; }

        public Customfield18903 customfield_18903 { get; set; }

        public Customfield18907 customfield_18907 { get; set; }

        public Customfield18908 customfield_18908 { get; set; }

        public object customfield_16600 { get; set; }
        public object customfield_14300 { get; set; }
        public object customfield_12000 { get; set; }
        public object customfield_12001 { get; set; }
        public object customfield_16601 { get; set; }
        public List<object> labels { get; set; }
        public object customfield_11700 { get; set; }
        public object customfield_17805 { get; set; }
        public object customfield_17804 { get; set; }
        public object aggregatetimeoriginalestimate { get; set; }
        public List<Issuelink> issuelinks { get; set; }
        public Assignee assignee { get; set; }
        public List<object> components { get; set; }
        public object customfield_17000 { get; set; }
        public object customfield_15500 { get; set; }
        public object customfield_13201 { get; set; }
        public object customfield_17803 { get; set; }
        public object customfield_17802 { get; set; }
        public object customfield_13202 { get; set; }
        public object customfield_17801 { get; set; }
        public object customfield_17800 { get; set; }
        public object customfield_11810 { get; set; }
        public object customfield_10600 { get; set; }
        public object customfield_12900 { get; set; }
        public object customfield_11811 { get; set; }
        public object customfield_18200 { get; set; }
        public List<object> subtasks { get; set; }
        public object customfield_14400 { get; set; }
        public Reporter reporter { get; set; }
        public Customfield18758 customfield_12100 { get; set; }
        public object customfield_16703 { get; set; }
        public object customfield_16702 { get; set; }
        public object customfield_16701 { get; set; }
        public object customfield_16700 { get; set; }
        public object customfield_13306 { get; set; }
        public object customfield_13305 { get; set; }
        public object customfield_13308 { get; set; }
        public object customfield_11801 { get; set; }
        public object customfield_11800 { get; set; }
        public object customfield_13307 { get; set; }
        public object customfield_11803 { get; set; }
        public object customfield_11802 { get; set; }
        public object customfield_13309 { get; set; }
        public object customfield_11805 { get; set; }
        public object customfield_11804 { get; set; }
        public object customfield_11807 { get; set; }
        public object customfield_11806 { get; set; }
        public Progress progress { get; set; }
        public object customfield_11809 { get; set; }
        public Votes votes { get; set; }
        public object customfield_11808 { get; set; }
        public Worklog worklog { get; set; }
        public Issuetype2 issuetype { get; set; }
        public Project project { get; set; }
        public object customfield_13421 { get; set; }
        public object customfield_12211 { get; set; }
        public object customfield_13300 { get; set; }
        public object customfield_13420 { get; set; }
        public object customfield_12210 { get; set; }
        public object customfield_11002 { get; set; }
        public object customfield_13423 { get; set; }
        public object customfield_13422 { get; set; }
        public object customfield_13301 { get; set; }
        public object customfield_17901 { get; set; }
        public object customfield_17900 { get; set; }
        public object customfield_15600 { get; set; }
        public object customfield_13303 { get; set; }
        public object customfield_13424 { get; set; }
        public object customfield_12206 { get; set; }
        public object customfield_10700 { get; set; }
        public object customfield_13418 { get; set; }
        public object customfield_12208 { get; set; }
        public object customfield_12207 { get; set; }
        public object customfield_12209 { get; set; }
        public object customfield_13419 { get; set; }
        public DateTime? resolutiondate { get; set; }        
        public Watches watches { get; set; }
        public object customfield_18783 { get; set; }
        public object customfield_18300 { get; set; }
        public object customfield_18784 { get; set; }
        public object customfield_18301 { get; set; }
        public object customfield_18785 { get; set; }
        public object customfield_16001 { get; set; }
        public object customfield_18781 { get; set; }
        public object customfield_13410 { get; set; }
        public object customfield_12200 { get; set; }
        public object customfield_12202 { get; set; }
        public object customfield_13412 { get; set; }
        public object customfield_18786 { get; set; }
        public object customfield_18787 { get; set; }
        public object customfield_16801 { get; set; }
        public object customfield_13411 { get; set; }
        public object customfield_16800 { get; set; }
        public object customfield_18788 { get; set; }
        public object customfield_13414 { get; set; }
        public object customfield_14500 { get; set; }
        public object customfield_12203 { get; set; }
        public object customfield_14501 { get; set; }
        public object customfield_13413 { get; set; }
        public object customfield_13405 { get; set; }
        public object customfield_13404 { get; set; }
        public object customfield_13407 { get; set; }
        public object customfield_11900 { get; set; }
        public object customfield_13406 { get; set; }
        public object customfield_13409 { get; set; }
        public object customfield_13408 { get; set; }
        public object customfield_11903 { get; set; }
        public DateTime? updated { get; set; }
        public object customfield_18773 { get; set; }
        public object customfield_17200 { get; set; }
        public object customfield_18770 { get; set; }
        public object timeoriginalestimate { get; set; }
        public object customfield_18779 { get; set; }
        public string description { get; set; }
        public object customfield_12310 { get; set; }
        public object customfield_11100 { get; set; }
        public object customfield_17202 { get; set; }
        public object customfield_15701 { get; set; }
        public object customfield_12312 { get; set; }
        public object customfield_13401 { get; set; }
        public object customfield_12311 { get; set; }
        public object customfield_18777 { get; set; }
        public object customfield_13403 { get; set; }
        public object customfield_13402 { get; set; }
        public object customfield_12313 { get; set; }
        public object customfield_15700 { get; set; }
        public Timetracking timetracking { get; set; }
        public object customfield_12305 { get; set; }
        public object customfield_12304 { get; set; }

        // public List<String> customfield_10007 { get; set; }
        public List<Customfield10007> customfield_10007 { get; set; }

        public object customfield_12307 { get; set; }
        public object customfield_12306 { get; set; }
        public string customfield_10008 { get; set; }
        public string customfield_10800 { get; set; }
        public object customfield_12309 { get; set; }
        public object customfield_10802 { get; set; }
        public object customfield_12308 { get; set; }
        public object customfield_10803 { get; set; }
        public object customfield_18760 { get; set; }
        public string summary { get; set; }
        public object customfield_18761 { get; set; }
        public object customfield_18763 { get; set; }
        public object customfield_18400 { get; set; }
        public Customfield16100 customfield_16100 { get; set; }
        public Customfield18768 customfield_18768 { get; set; }
        public string customfield_10000 { get; set; }
        public object customfield_16901 { get; set; }
        public object customfield_12301 { get; set; }
        public DateTime? customfield_14601 { get; set; }
        public DateTime? customfield_10001 { get; set; }
        public object customfield_18764 { get; set; }
        public object customfield_16900 { get; set; }
        public object customfield_18765 { get; set; }
        public object customfield_12300 { get; set; }
        public object customfield_10003 { get; set; }
        public object customfield_18766 { get; set; }
        public object customfield_12303 { get; set; }
        public object customfield_18767 { get; set; }
        public object customfield_12302 { get; set; }
        public object customfield_10004 { get; set; }
        public Customfield14600 customfield_14600 { get; set; }
        public object duedate { get; set; }
        public Comment comment { get; set; }
        public DateTime? statuscategorychangedate { get; set; }
        public object customfield_18750 { get; set; }
        public object customfield_18751 { get; set; }
        public object customfield_18752 { get; set; }
        public object customfield_17300 { get; set; }
        public object customfield_15001 { get; set; }
        public object customfield_15004 { get; set; }
        public object customfield_18757 { get; set; }
        public Customfield18758 customfield_18758 { get; set; }
        public Customfield18758 customfield_18792 { get; set; }
        public object customfield_15002 { get; set; }
        public object customfield_15003 { get; set; }
        public List<object> fixVersions { get; set; }
        public object customfield_13500 { get; set; }
        public object customfield_18753 { get; set; }
        public object customfield_11200 { get; set; }
        public object customfield_18754 { get; set; }
        public object customfield_15801 { get; set; }
        public object customfield_18755 { get; set; }
        public object customfield_18756 { get; set; }
        public object customfield_13501 { get; set; }
        public object customfield_18740 { get; set; }
        public object customfield_18741 { get; set; }
        public string customfield_16200 { get; set; }
        public object customfield_18746 { get; set; }
        public object customfield_18747 { get; set; }
        public object customfield_18748 { get; set; }
        public object customfield_18749 { get; set; }
        public object customfield_18500 { get; set; }
        public Priority2 priority { get; set; }
        public object customfield_13610 { get; set; }
        public object customfield_12400 { get; set; }
        public object customfield_18742 { get; set; }
        public object customfield_18743 { get; set; }
        public object customfield_18501 { get; set; }
        public object customfield_13612 { get; set; }
        public object customfield_18502 { get; set; }
        public object customfield_18744 { get; set; }
        public object customfield_18745 { get; set; }
        public object customfield_13611 { get; set; }
        public object customfield_12401 { get; set; }
        public object customfield_13603 { get; set; }
        public object customfield_13602 { get; set; }
        public object customfield_13605 { get; set; }
        public object customfield_13604 { get; set; }
        public object customfield_13607 { get; set; }
        public object customfield_18739 { get; set; }
        public long? timeestimate { get; set; }
        public object customfield_13606 { get; set; }
        public List<object> versions { get; set; }
        public object customfield_13609 { get; set; }
        public object customfield_13608 { get; set; }
        public Status2 status { get; set; }
        public object customfield_15100 { get; set; }
        public object customfield_18735 { get; set; }
        public object customfield_17402 { get; set; }
        public Customfield18736 customfield_18736 { get; set; }
        public List<object> customfield_18737 { get; set; }
        public object customfield_15101 { get; set; }
        public object customfield_17401 { get; set; }
        public object customfield_18738 { get; set; }
        public object customfield_17400 { get; set; }
        public object customfield_15102 { get; set; }
        public object customfield_10210 { get; set; }
        public object customfield_11300 { get; set; }
        public object customfield_18732 { get; set; }
        public object customfield_15900 { get; set; }
        public object customfield_10211 { get; set; }
        public object customfield_13601 { get; set; }
        public object customfield_10212 { get; set; }
        public object customfield_18733 { get; set; }
        public object customfield_13600 { get; set; }
        public object customfield_18734 { get; set; }
        public object customfield_10213 { get; set; }
        public object customfield_10203 { get; set; }
        public object customfield_14803 { get; set; }
        public object customfield_10204 { get; set; }
        public object customfield_14801 { get; set; }
        public object customfield_14802 { get; set; }
        public Customfield15000 customfield_15000 { get; set; }

        public object customfield_18728 { get; set; }
        public long? aggregatetimeestimate { get; set; }
        public object customfield_10209 { get; set; }
        public Creator creator { get; set; }
        public object customfield_14000 { get; set; }
        public object customfield_18724 { get; set; }
        public object customfield_16302 { get; set; }
        public object customfield_18725 { get; set; }
        public object customfield_18726 { get; set; }
        public object customfield_16301 { get; set; }
        public object customfield_16300 { get; set; }
        public Aggregateprogress aggregateprogress { get; set; }
        public object customfield_18720 { get; set; }
        public object customfield_18600 { get; set; }
        public object customfield_14800 { get; set; }
        public object customfield_18721 { get; set; }
        public object customfield_18722 { get; set; }
        public object customfield_18723 { get; set; }
        public object customfield_12500 { get; set; }
        public long? timespent { get; set; }
        public object customfield_18713 { get; set; }
        public object customfield_18714 { get; set; }
        public Customfield18715 customfield_18715 { get; set; }
        public object customfield_17500 { get; set; }
        public object customfield_15200 { get; set; }
        public object customfield_18716 { get; set; }
        public long? aggregatetimespent { get; set; }
        public object customfield_10310 { get; set; }
        public object customfield_18710 { get; set; }
        public object customfield_10311 { get; set; }
        public object customfield_18711 { get; set; }
        public object customfield_11401 { get; set; }
        public object customfield_13700 { get; set; }
        public object customfield_11400 { get; set; }
        public object customfield_18712 { get; set; }
        public object customfield_12602 { get; set; }
        public object customfield_10302 { get; set; }
        public object customfield_12601 { get; set; }
        public object customfield_10303 { get; set; }
        public object customfield_14900 { get; set; }
        public object customfield_12604 { get; set; }
        public List<Customfield10304> customfield_10304 { get; set; }
        public object customfield_12603 { get; set; }
        public object customfield_10306 { get; set; }
        public object customfield_18706 { get; set; }
        public object customfield_12606 { get; set; }
        public object customfield_10307 { get; set; }
        public object customfield_18707 { get; set; }
        public object customfield_12605 { get; set; }
        public object customfield_18708 { get; set; }
        public object customfield_18709 { get; set; }
        public object customfield_10309 { get; set; }
        public long workratio { get; set; }
        public DateTime? created { get; set; }
        public object customfield_18702 { get; set; }
        public object customfield_16402 { get; set; }
        public object customfield_18703 { get; set; }
        public object customfield_18704 { get; set; }
        public object customfield_16400 { get; set; }
        public object customfield_14100 { get; set; }
        public object customfield_18705 { get; set; }
        public object customfield_12600 { get; set; }
        public object customfield_10300 { get; set; }
        public object customfield_18700 { get; set; }
        public object customfield_16404 { get; set; }
        public object customfield_18701 { get; set; }
        public object customfield_16403 { get; set; }
        public object customfield_10301 { get; set; }
        public object customfield_13801 { get; set; }
        public object customfield_13800 { get; set; }
        public object customfield_13803 { get; set; }
        public object customfield_13802 { get; set; }
        public object customfield_13805 { get; set; }
        // public object customfield_13804 { get; set; }
        public IList<Customfield13804> customfield_13804 { get; set; }
        public object customfield_13001 { get; set; }
        public object customfield_13000 { get; set; }
        public object customfield_17600 { get; set; }
        public object customfield_15300 { get; set; }
        public object customfield_13002 { get; set; }
        public object customfield_11500 { get; set; }
        public object security { get; set; }
        public List<object> attachment { get; set; }
        public object customfield_18000 { get; set; }
        public object customfield_14201 { get; set; }
        public DateTime? customfield_13111 { get; set; }
        public object customfield_16500 { get; set; }
        public object customfield_13110 { get; set; }
        public object customfield_14202 { get; set; }
        public object customfield_13113 { get; set; }
        public Customfield14200 customfield_14200 { get; set; }
        public object customfield_13112 { get; set; }
        public object customfield_14203 { get; set; }
        public object customfield_10400 { get; set; }
        public object customfield_14204 { get; set; }
        public Customfield13900 customfield_13900 { get; set; }
        public List<Customfield11601> customfield_11601 { get; set; }
        public object customfield_13108 { get; set; }
        public object customfield_11600 { get; set; }
        public object customfield_13107 { get; set; }
        // Campos nuevos agregados 
        public Customfield18838 customfield_18838 { get; set; }
        public List<Customfield18837> customfield_18837 { get; set; }
        public string customfield_18834 { get; set; }
        public Customfield18840 customfield_18840 { get; set; }
        public string customfield_18841 { get; set; }
        public List<Customfield18833> customfield_18833 { get; set; }
        public List<Customfield18844> customfield_18844 { get; set; }
        public List<Customfield18845> customfield_18845 { get; set; }
        public Customfield18839 customfield_18839 { get; set; }
        public Customfield18832 customfield_18832 { get; set; }
        public string customfield_18835 { get; set; }
        public string customfield_18836 { get; set; }

        //Pedido por Analia
        public Customfield18898 customfield_18898 { get; set; }


        //CAMPOS SLA
        public Customfield18800 customfield_18800 { get; set; }
        public Customfield18918 customfield_18918 { get; set; }
        public Customfield18799 customfield_18799 { get; set; }
        public Customfield18895 customfield_18895 { get; set; }

        //CAMPO PRODUCTO PEDIDO POR FEDE
        public Customfield18878 customfield_18878 { get; set; }
        public Customfield13200 customfield_13200 { get; set; }
        public Customfield18880 customfield_18880 { get; set; }
        public Customfield18881 customfield_18881 { get; set; }
        public Customfield18882 customfield_18882 { get; set; }
        public Customfield18872 customfield_18872 { get; set; }
        public Customfield18873 customfield_18873 { get; set; }
        public Customfield18874 customfield_18874 { get; set; }
        public Customfield18875 customfield_18875 { get; set; }
        public Customfield18876 customfield_18876 { get; set; }
        public Customfield18782 customfield_18782 { get; set; }
        //CAMPO PRODUCTO PEDIDO POR FEDE


        public string customfield_18926 { get; set; }

        public Customfield18910 customfield_18910 { get; set; }
        public Customfield18911 customfield_18911 { get; set; }
        public Customfield18914 customfield_18914 { get; set; }
        public Customfield18915 customfield_18915 { get; set; }

        //Pedido por Analia
        public Customfield18931 customfield_18931 { get; set; }

        //Pedido por Analia: complejidad
        //public List<Customfield18957> customfield_12402 { get; set; }
        public Customfield12402 customfield_12402 { get; set; }
    }

    public class AddJira
    {
        public string expand { get; set; }
        public string id { get; set; }
        public string self { get; set; }
        public string key { get; set; }
        public Fields fields { get; set; }
    }
}
