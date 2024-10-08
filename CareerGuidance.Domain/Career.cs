namespace CareerGuidance.Domain;

public class Career
{
    public Career(){}
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? UrlLink { get; set; }
    public List<Occupation> Occupations { get; set; }
}