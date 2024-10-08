using CareerGuidance.Domain;
using CareerGuidance.View.Constrants;
using CareerGuidance.View.Services;
using HtmlAgilityPack;

namespace CareerGuidance.View
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var service = new CrawService();
            await service.GetHttpCareers(CrawlConstants.Url.OccupationalOutlookHandbook,
                CrawlConstants.Element.Career);
        }
    }
}
