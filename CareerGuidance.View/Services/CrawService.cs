using CareerGuidance.Domain;
using CareerGuidance.View.Constrants;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;

namespace CareerGuidance.View.Services;

public class CrawService
{
    public async Task GetHttpCareers(string url, string elementsGroupString)
    {
        if (url == null) throw new ArgumentNullException(nameof(url));
        url = CrawlConstants.Url.OccupationalOutlookHandbook;
        HttpClientHandler handler = new HttpClientHandler()
        {
            AllowAutoRedirect = true, // Tự động theo dõi chuyển hướng
        };
            
        HttpClient httpClient = new HttpClient(handler);
        httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.131 Safari/537.36");
        httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
        httpClient.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9");
        httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
        httpClient.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
        httpClient.DefaultRequestHeaders.Add("Origin", "https://www.bls.gov");
        try
        {
            var careers = await GetCareers(httpClient, url, elementsGroupString);
            foreach (var career in careers)
            {
                var occupations = new List<Occupation>();
                career.Occupations = await GetJobsByCareer(career, httpClient,career.UrlLink);
            }

            JSchemaGenerator generator = new JSchemaGenerator();
            JSchema schema = generator.Generate(typeof(List<Career>));
            string json = JsonConvert.SerializeObject(careers, Formatting.Indented);
            SaveJsonToFile(json, "D://career.json");
            // Console.WriteLine(schema.ToString());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    static void SaveJsonToFile(string jsonData, string filePath)
    {
        using System.IO.StreamWriter file = new System.IO.StreamWriter(filePath);
        file.WriteLine(jsonData);
    }
    public async Task<List<Career>> GetCareers(HttpClient httpClient, string url, string elementsGroupString)
    {
        var careers = new List<Career>();
        string html = await httpClient.GetStringAsync(url);
        HtmlDocument document = new HtmlDocument();
        document.LoadHtml(html);
        var collection = document.DocumentNode.SelectNodes(elementsGroupString);
        foreach (var careerName in collection)
        {
            var career = new Career()
            {
                Name = careerName.InnerText.Trim(),
                UrlLink = CrawlConstants.Url.CrawlWebsite + careerName.Attributes["href"]?.Value.Trim()
            };
            careers.Add(career);
        }
        return careers;
    }

    public async Task<List<Occupation>> GetJobsByCareer(Career career, HttpClient httpClient, string? url)
    {
        var occupations = new List<Occupation>();
        //Get name
        string html = await httpClient.GetStringAsync(url);
        HtmlDocument document = new HtmlDocument();
        document.LoadHtml(html);
        var collections = document.DocumentNode.SelectNodes(CrawlConstants.Element.OccupationTable);
        if (collections != null)
        {
            int i = 1;
            foreach (var occupationHtml in collections)
            {
                HtmlDocument subDocument = new HtmlDocument();
                subDocument.LoadHtml(occupationHtml.InnerHtml);
                var occupationLink = CrawlConstants.Url.CrawlWebsite + subDocument.DocumentNode
                    .SelectSingleNode("//td //strong //a").Attributes["href"]?.Value.Trim();
                string subOccupationHtml = await httpClient.GetStringAsync(occupationLink);
                HtmlDocument occupationDocument = new HtmlDocument();
                occupationDocument.LoadHtml(subOccupationHtml);
                var quanlitiesHtml = occupationDocument.DocumentNode.SelectNodes("//div[@id='tab-4'] //article //p //strong //em | //div[@id='tab-4'] //article //p //em //strong");
                var quanlities = new List<string>();
                if (quanlitiesHtml != null)
                {
                    foreach (var quanlity in quanlitiesHtml)
                    {
                        var text = quanlity.InnerText;
                        quanlities.Add(text);
                    }
                    ++i;
                }
                
                var occupation = new Occupation()
                {
                    Id = new Guid(),
                    Name = subDocument.DocumentNode.SelectSingleNode("//td //strong //a").InnerText.Trim(),
                    UrlLink = occupationLink,
                    Description = subDocument.DocumentNode.SelectSingleNode("//td //p").InnerText,
                    Summary = occupationDocument.DocumentNode.SelectSingleNode("//div[@id='tab-1'] //article //div[@id='center-content']").InnerHtml.Trim(),
                    WhatTheyDo = occupationDocument.DocumentNode.SelectSingleNode("//div[@id='tab-2'] //article").InnerHtml.Trim(),
                    WorkEnvironment = occupationDocument.DocumentNode.SelectSingleNode("//div[@id='tab-3'] //article").InnerHtml.Trim(),
                    HowtoBecome = quanlities,
                    Pay = occupationDocument.DocumentNode.SelectSingleNode("//div[@id='tab-5'] //article").InnerHtml.Trim(),
                    JobOutlook = occupationDocument.DocumentNode.SelectSingleNode("//div[@id='tab-6'] //article").InnerHtml.Trim(),
                    StateAndAreaData = occupationDocument.DocumentNode.SelectSingleNode("//div[@id='tab-7'] //article").InnerHtml.Trim(),
                    CareerId = career.Id
                };
                occupations.Add(occupation);
            }
        }
        return occupations;
    }
}