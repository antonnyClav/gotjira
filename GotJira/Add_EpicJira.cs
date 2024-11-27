using System;
using System.Collections.Generic;

namespace AtlassianGotJiraEpicJira
{
    public class Resolution
    {
        public string self { get; set; }
        public string id { get; set; }
        public string description { get; set; }
        public string name { get; set; }
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

    public class StatusCategory
    {
        public string self { get; set; }
        public int id { get; set; }
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
        public int avatarId { get; set; }
    }

    public class Fields
    {
        public string summary { get; set; }
        public Status status { get; set; }
        public Priority priority { get; set; }
        public Issuetype issuetype { get; set; }
    }

    public class Subtask
    {
        public string id { get; set; }
        public string key { get; set; }
        public string self { get; set; }
        public Fields fields { get; set; }
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
    }

    public class Progress
    {
        public int progress { get; set; }
        public int total { get; set; }
        public int? percent { get; set; }
    }

    public class Votes
    {
        public string self { get; set; }
        public int votes { get; set; }
        public bool hasVoted { get; set; }
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
        public AvatarUrls avatarUrls { get; set; }
        public ProjectCategory projectCategory { get; set; }
    }

    public class Watches
    {
        public string self { get; set; }
        public int watchCount { get; set; }
        public bool isWatching { get; set; }
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

    public class Customfield18768
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

    public class Creator
    {
        public string self { get; set; }
        public string accountId { get; set; }
        public AvatarUrls avatarUrls { get; set; }
        public string displayName { get; set; }
        public bool active { get; set; }
        public string timeZone { get; set; }
        public string accountType { get; set; }
    }

    public class Aggregateprogress
    {
        public int progress { get; set; }
        public int total { get; set; }
        public int? percent { get; set; }
    }

    public class Customfield10304
    {
        public string self { get; set; }
        public string value { get; set; }
        public string id { get; set; }
    }

    public class Security
    {
        public string self { get; set; }
        public string id { get; set; }
        public string description { get; set; }
        public string name { get; set; }
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

    public class Customfield11601
    {
        public string self { get; set; }
        public string accountId { get; set; }
        public AvatarUrls avatarUrls { get; set; }
        public string displayName { get; set; }
        public bool active { get; set; }
        public string timeZone { get; set; }
        public string accountType { get; set; }
    }

    public class Fields2
    {
        public object customfield_17700 { get; set; }
        public object customfield_15400 { get; set; }
        public object customfield_15401 { get; set; }
        public object customfield_13101 { get; set; }
        public object customfield_13104 { get; set; }
        public Resolution resolution { get; set; }
        public object customfield_13103 { get; set; }
        public object customfield_13106 { get; set; }
        public object customfield_17702 { get; set; }
        public object customfield_13105 { get; set; }
        public object customfield_17701 { get; set; }
        public object customfield_10500 { get; set; }
        public object customfield_12800 { get; set; }
        public object customfield_10501 { get; set; }
        public object customfield_12802 { get; set; }
        public object customfield_18100 { get; set; }
        public DateTime lastViewed { get; set; }
        public object customfield_18101 { get; set; }
        public object customfield_18102 { get; set; }
        public object customfield_18103 { get; set; }
        public object customfield_12000 { get; set; }
        public object customfield_16600 { get; set; }
        public object customfield_14300 { get; set; }
        public object customfield_12001 { get; set; }
        public IList<object> labels { get; set; }
        public object customfield_16601 { get; set; }
        public object customfield_11700 { get; set; }
        public object customfield_17805 { get; set; }
        public object customfield_17804 { get; set; }
        public object aggregatetimeoriginalestimate { get; set; }
        public IList<object> issuelinks { get; set; }
        public Assignee assignee { get; set; }
        public IList<object> components { get; set; }
        public object customfield_17000 { get; set; }
        public object customfield_15500 { get; set; }
        public object customfield_13201 { get; set; }
        public object customfield_13200 { get; set; }
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
        public IList<Subtask> subtasks { get; set; }
        public object customfield_14400 { get; set; }
        public Reporter reporter { get; set; }
        public object customfield_12100 { get; set; }
        public object customfield_16703 { get; set; }
        public object customfield_16702 { get; set; }
        public object customfield_16701 { get; set; }
        public object customfield_16700 { get; set; }
        public object customfield_13306 { get; set; }
        public object customfield_13305 { get; set; }
        public object customfield_11801 { get; set; }
        public object customfield_13308 { get; set; }
        public object customfield_11800 { get; set; }
        public object customfield_13307 { get; set; }
        public object customfield_11803 { get; set; }
        public object customfield_13309 { get; set; }
        public object customfield_11802 { get; set; }
        public object customfield_11805 { get; set; }
        public object customfield_11804 { get; set; }
        public object customfield_11807 { get; set; }
        public object customfield_11806 { get; set; }
        public Progress progress { get; set; }
        public object customfield_11809 { get; set; }
        public Votes votes { get; set; }
        public object customfield_11808 { get; set; }
        public object customfield_18793 { get; set; }
        public object customfield_18794 { get; set; }
        public Issuetype issuetype { get; set; }
        public object customfield_18790 { get; set; }
        public object customfield_18792 { get; set; }
        public Project project { get; set; }
        public object customfield_13421 { get; set; }
        public object customfield_12211 { get; set; }
        public object customfield_13300 { get; set; }
        public object customfield_12210 { get; set; }
        public object customfield_13420 { get; set; }
        public object customfield_11002 { get; set; }
        public object customfield_18797 { get; set; }
        public object customfield_13423 { get; set; }
        public object customfield_13301 { get; set; }
        public object customfield_17901 { get; set; }
        public object customfield_13422 { get; set; }
        public object customfield_18798 { get; set; }
        public object customfield_15600 { get; set; }
        public object customfield_17900 { get; set; }
        public object customfield_18799 { get; set; }
        public object customfield_13303 { get; set; }
        public object customfield_13424 { get; set; }
        public object customfield_12206 { get; set; }
        public object customfield_10700 { get; set; }
        public object customfield_12208 { get; set; }
        public object customfield_13418 { get; set; }
        public object customfield_12207 { get; set; }
        public object customfield_13419 { get; set; }
        public object customfield_12209 { get; set; }
        public DateTime? resolutiondate { get; set; }
        public object customfield_18782 { get; set; }
        public object customfield_18783 { get; set; }
        public Watches watches { get; set; }
        public object customfield_18784 { get; set; }
        public object customfield_18300 { get; set; }
        public object customfield_18785 { get; set; }
        public object customfield_18301 { get; set; }
        public object customfield_16001 { get; set; }
        public object customfield_18781 { get; set; }
        public object customfield_13410 { get; set; }
        public object customfield_12200 { get; set; }
        public object customfield_12202 { get; set; }
        public object customfield_18786 { get; set; }
        public object customfield_13412 { get; set; }
        public object customfield_16801 { get; set; }
        public object customfield_13411 { get; set; }
        public object customfield_18787 { get; set; }
        public object customfield_14500 { get; set; }
        public object customfield_13414 { get; set; }
        public object customfield_18788 { get; set; }
        public object customfield_16800 { get; set; }
        public object customfield_12203 { get; set; }
        public object customfield_14501 { get; set; }
        public object customfield_13413 { get; set; }
        public object customfield_13405 { get; set; }
        public object customfield_13404 { get; set; }
        public object customfield_11900 { get; set; }
        public object customfield_13407 { get; set; }
        public object customfield_13406 { get; set; }
        public object customfield_13409 { get; set; }
        public object customfield_13408 { get; set; }
        public object customfield_11903 { get; set; }
        public DateTime updated { get; set; }
        public object customfield_18773 { get; set; }
        public object customfield_17200 { get; set; }
        public object timeoriginalestimate { get; set; }
        public object customfield_18770 { get; set; }
        public object customfield_18779 { get; set; }
        public string description { get; set; }
        public object customfield_12310 { get; set; }
        public object customfield_11100 { get; set; }
        public object customfield_17202 { get; set; }
        public object customfield_15701 { get; set; }
        public object customfield_13401 { get; set; }
        public object customfield_12312 { get; set; }
        public object customfield_12311 { get; set; }
        public object customfield_13403 { get; set; }
        public object customfield_18777 { get; set; }
        public object customfield_12313 { get; set; }
        public object customfield_15700 { get; set; }
        public object customfield_13402 { get; set; }
        public object customfield_12305 { get; set; }
        public object customfield_12304 { get; set; }
        public object customfield_10007 { get; set; }
        public object customfield_12307 { get; set; }
        public object customfield_12306 { get; set; }
        public string customfield_10800 { get; set; }
        public string customfield_10008 { get; set; }
        public object customfield_12309 { get; set; }
        public object customfield_10802 { get; set; }
        public object customfield_12308 { get; set; }
        public object customfield_10803 { get; set; }
        public object customfield_18760 { get; set; }
        public string summary { get; set; }
        public object customfield_18761 { get; set; }
        public object customfield_18400 { get; set; }
        public object customfield_18763 { get; set; }
        public Customfield16100 customfield_16100 { get; set; }
        public Customfield18768 customfield_18768 { get; set; }
        public string customfield_10000 { get; set; }
        public object customfield_18764 { get; set; }
        public object customfield_16901 { get; set; }
        public object customfield_10001 { get; set; }
        public object customfield_12301 { get; set; }
        public object customfield_18401 { get; set; }
        public object customfield_14601 { get; set; }
        public object customfield_12300 { get; set; }
        public object customfield_18765 { get; set; }
        public object customfield_16900 { get; set; }
        public object customfield_10003 { get; set; }
        public object customfield_12303 { get; set; }
        public object customfield_18766 { get; set; }
        public object customfield_18767 { get; set; }
        public object customfield_12302 { get; set; }
        public Customfield14600 customfield_14600 { get; set; }
        public object customfield_10004 { get; set; }
        public object duedate { get; set; }
        public DateTime statuscategorychangedate { get; set; }
        public object customfield_18750 { get; set; }
        public object customfield_18751 { get; set; }
        public object customfield_18752 { get; set; }
        public object customfield_17300 { get; set; }
        public object customfield_15001 { get; set; }
        public object customfield_15004 { get; set; }
        public object customfield_18757 { get; set; }
        public object customfield_18758 { get; set; }
        public object customfield_15002 { get; set; }
        public IList<object> fixVersions { get; set; }
        public object customfield_15003 { get; set; }
        public object customfield_18753 { get; set; }
        public object customfield_13500 { get; set; }
        public object customfield_11200 { get; set; }
        public object customfield_18754 { get; set; }
        public object customfield_15801 { get; set; }
        public object customfield_18755 { get; set; }
        public object customfield_13501 { get; set; }
        public object customfield_18756 { get; set; }
        public object customfield_18740 { get; set; }
        public object customfield_18741 { get; set; }
        public string customfield_16200 { get; set; }
        public object customfield_18746 { get; set; }
        public object customfield_18747 { get; set; }
        public object customfield_18748 { get; set; }
        public object customfield_18749 { get; set; }
        public Priority priority { get; set; }
        public object customfield_18742 { get; set; }
        public object customfield_18500 { get; set; }
        public object customfield_12400 { get; set; }
        public object customfield_13610 { get; set; }
        public object customfield_18743 { get; set; }
        public object customfield_18501 { get; set; }
        public object customfield_13612 { get; set; }
        public object customfield_18744 { get; set; }
        public object customfield_18502 { get; set; }
        public object customfield_12402 { get; set; }
        public object customfield_18745 { get; set; }
        public object customfield_12401 { get; set; }
        public object customfield_13611 { get; set; }
        public object customfield_13603 { get; set; }
        public object customfield_13602 { get; set; }
        public object customfield_13605 { get; set; }
        public object customfield_13604 { get; set; }
        public object customfield_13607 { get; set; }
        public object customfield_18739 { get; set; }
        public int? timeestimate { get; set; }
        public IList<object> versions { get; set; }
        public object customfield_13606 { get; set; }
        public object customfield_13609 { get; set; }
        public object customfield_13608 { get; set; }
        public Status status { get; set; }
        public object customfield_15100 { get; set; }
        public object customfield_18735 { get; set; }
        public object customfield_18736 { get; set; }
        public object customfield_17402 { get; set; }
        public object customfield_15101 { get; set; }
        public object customfield_17401 { get; set; }
        public IList<object> customfield_18737 { get; set; }
        public object customfield_17400 { get; set; }
        public object customfield_15102 { get; set; }
        public object customfield_18738 { get; set; }
        public object customfield_10210 { get; set; }
        public object customfield_10211 { get; set; }
        public object customfield_15900 { get; set; }
        public object customfield_18732 { get; set; }
        public object customfield_11300 { get; set; }
        public object customfield_18733 { get; set; }
        public object customfield_10212 { get; set; }
        public object customfield_13601 { get; set; }
        public object customfield_18734 { get; set; }
        public object customfield_13600 { get; set; }
        public object customfield_10213 { get; set; }
        public object customfield_10203 { get; set; }
        public object customfield_14803 { get; set; }
        public object customfield_10204 { get; set; }
        public object customfield_14801 { get; set; }
        public object customfield_14802 { get; set; }
        public object customfield_18728 { get; set; }
        public int? aggregatetimeestimate { get; set; }
        public object customfield_10209 { get; set; }
        public Creator creator { get; set; }
        public object customfield_14000 { get; set; }
        public object customfield_18724 { get; set; }
        public object customfield_16302 { get; set; }
        public object customfield_18725 { get; set; }
        public object customfield_16301 { get; set; }
        public object customfield_18726 { get; set; }
        public object customfield_16300 { get; set; }
        public Aggregateprogress aggregateprogress { get; set; }
        public object customfield_18720 { get; set; }
        public object customfield_18721 { get; set; }
        public object customfield_18600 { get; set; }
        public object customfield_14800 { get; set; }
        public object customfield_18722 { get; set; }
        public object customfield_12500 { get; set; }
        public object customfield_18723 { get; set; }
        public int? timespent { get; set; }
        public object customfield_18713 { get; set; }
        public object customfield_18714 { get; set; }
        public object customfield_17500 { get; set; }
        public object customfield_15200 { get; set; }
        public object customfield_18716 { get; set; }
        public int? aggregatetimespent { get; set; }
        public object customfield_10310 { get; set; }
        public object customfield_18710 { get; set; }
        public object customfield_11401 { get; set; }
        public object customfield_10311 { get; set; }
        public object customfield_18711 { get; set; }
        public object customfield_13700 { get; set; }
        public object customfield_11400 { get; set; }
        public object customfield_18712 { get; set; }
        public object customfield_10302 { get; set; }
        public object customfield_12602 { get; set; }
        public object customfield_12601 { get; set; }
        public object customfield_10303 { get; set; }
        public object customfield_12604 { get; set; }
        public IList<Customfield10304> customfield_10304 { get; set; }
        public object customfield_14900 { get; set; }
        public object customfield_12603 { get; set; }
        public object customfield_12606 { get; set; }
        public object customfield_10306 { get; set; }
        public object customfield_18706 { get; set; }
        public object customfield_18707 { get; set; }
        public object customfield_12605 { get; set; }
        public object customfield_10307 { get; set; }
        public object customfield_18708 { get; set; }
        public object customfield_10309 { get; set; }
        public object customfield_18709 { get; set; }
        public int workratio { get; set; }
        public DateTime created { get; set; }
        public object customfield_16402 { get; set; }
        public object customfield_18823 { get; set; }
        public object customfield_18702 { get; set; }
        public object customfield_18703 { get; set; }
        public object customfield_18704 { get; set; }
        public object customfield_14100 { get; set; }
        public object customfield_16400 { get; set; }
        public object customfield_18705 { get; set; }
        public object customfield_18820 { get; set; }
        public object customfield_18700 { get; set; }
        public object customfield_12600 { get; set; }
        public object customfield_10300 { get; set; }
        public object customfield_16404 { get; set; }
        public object customfield_16403 { get; set; }
        public object customfield_10301 { get; set; }
        public object customfield_18701 { get; set; }
        public object customfield_13801 { get; set; }
        public object customfield_13800 { get; set; }
        public object customfield_13803 { get; set; }
        public object customfield_13802 { get; set; }
        public object customfield_13805 { get; set; }
        public object customfield_13804 { get; set; }
        public object customfield_13001 { get; set; }
        public object customfield_18812 { get; set; }
        public object customfield_17600 { get; set; }
        public object customfield_18813 { get; set; }
        public object customfield_13000 { get; set; }
        public object customfield_15300 { get; set; }
        public object customfield_13002 { get; set; }
        public object customfield_11500 { get; set; }
        public object customfield_18810 { get; set; }
        public object customfield_18809 { get; set; }
        public Security security { get; set; }
        public object customfield_18807 { get; set; }
        public object customfield_18000 { get; set; }
        public object customfield_13111 { get; set; }
        public object customfield_18801 { get; set; }
        public object customfield_14201 { get; set; }
        public object customfield_14202 { get; set; }
        public object customfield_13110 { get; set; }
        public object customfield_16500 { get; set; }
        public object customfield_18802 { get; set; }
        public object customfield_18803 { get; set; }
        public object customfield_13113 { get; set; }
        public object customfield_13112 { get; set; }
        public Customfield14200 customfield_14200 { get; set; }
        public object customfield_14203 { get; set; }
        public object customfield_18800 { get; set; }
        public object customfield_10400 { get; set; }
        public object customfield_14204 { get; set; }
        public Customfield13900 customfield_13900 { get; set; }
        public IList<Customfield11601> customfield_11601 { get; set; }
        public object customfield_13108 { get; set; }
        public object customfield_13107 { get; set; }
        public object customfield_11600 { get; set; }
    }

    public class Issue
    {
        public string expand { get; set; }
        public string id { get; set; }
        public string self { get; set; }
        public string key { get; set; }
        public Fields fields { get; set; }
    }

    public class AddEpicJira
    {
        public string expand { get; set; }
        public int startAt { get; set; }
        public int maxResults { get; set; }
        public int total { get; set; }
        public IList<Issue> issues { get; set; }
    }
}
