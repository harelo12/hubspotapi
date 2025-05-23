using System.Text.Json.Serialization;

public class ContactProperties
{
    public string? hs_object_id { get; set; }
    public string? firstname { get; set; }
    public string? lastname { get; set; }
    public string? email { get; set; }
    public string? phone { get; set; }
    public DateTime? createdate { get; set; }
    public DateTime? lastmodifieddate { get; set; }
}
