namespace SearchService.Settings;

public class SearchSrvAuthConfiguration
{
    public const string CONFIGURATION = "SearchSrvAuthConfiguration";
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string GrantTypes { get; set; }
    public string Scope { get; set; }
}
