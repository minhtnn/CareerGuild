namespace CareerGuidance.Domain;

public class Occupation
{
    public Occupation() { }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? UrlLink { get; set; }
    public string Description { get; set; }
    public string Summary { get; set; }
    public string WhatTheyDo { get; set; }
    public string WorkEnvironment { get; set; }
    public List<string> HowtoBecome { get; set; }
    public string Pay { get; set; }
    public string JobOutlook { get; set; }
    public string StateAndAreaData { get; set; }
    public Guid CareerId { get; set; }
}