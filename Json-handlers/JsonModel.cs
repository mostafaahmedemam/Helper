//ex of JsonModel
[Serializable]
public class PrerequisitesJson
{
    [JsonPropertyName("Prerequisites")]
    public List<Prerequisites> Prerequisites { get; set; }
}

[Serializable]
public class Prerequisites
{
    [JsonPropertyName("folderName")]
    public string PrerequisiteName { get; set; }
    [JsonIgnore]
    public Status PrerequisiteStatus { get; set; }
    [JsonPropertyName("path")]
    public string Path { get; set; }
    [JsonIgnore]
    public string IconPath { get; set; }
}