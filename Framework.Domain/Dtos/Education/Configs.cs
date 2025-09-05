using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Framework.Domain.Dtos.Education
{
    public class Configs
    {
        public List<WorkspaceItems> Workspaces { get; set; }
        public Configurations Configurations { get; set; }
        public Contents Contents { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum WorkspaceItems
    {
        [EnumMember(Value = "journey")]
        Journey,
        [EnumMember(Value = "flix")]
        Flix,
        [EnumMember(Value = "events")]
        Events,
        [EnumMember(Value = "blog")]
        Blog,
        [EnumMember(Value = "courses")]
        Courses,
        [EnumMember(Value = "mentoring")]
        Mentoring,
        [EnumMember(Value = "networking")]
        Networking,
        [EnumMember(Value = "communities")]
        Communities
    }

    public class Configurations
    {
        public string Logo { get; set; }
        public string PrimaryColor { get; set; }
        public string SecondaryColor { get; set; }
        public string TertiaryColor { get; set; }
        public string SubscriptionModel { get; set; }
    }

    public class Contents
    {
        public string Methodology { get; set; }
    }
}