namespace CareerGuidance.View.Constrants;

public static class CrawlConstants
{
    public class Url
    {
        public const string CrawlWebsite = "https://www.bls.gov";
        public const string OccupationalOutlookHandbook = CrawlWebsite + "/ooh";
        public const string Summary = "#tab-1";
        public const string WhatTheyDo = "#tab-2";
        public const string WorkEnvironment = "#tab-3";
        public const string HowToBecomeOne = "#tab-4";
        public const string Pay = "#tab-5";
        public const string JobOutlook = "#tab-6";
        public const string StateAndAreaData = "#tab-7";
    }

    public class Table
    {
        public const string CrawlTable = "#ooh-occupation-list";
        public const string CrawlGroup = ".ooh-gray-background .ooh-groups-col";
        public const string CrawlCareer = "li a";
    }

    public class Element
    {
        public const string Career = "//div[@id='ooh-occupation-list'] " +
                                            "//div[@class='ooh-gray-background'] " +
                                            "//ul[@class='ooh-groups-col'] " +
                                            "//li //a";

        public const string OccupationTable = "//table[@id='landing-page-table'] " +
                                              "//tbody " +
                                              "//tr ";
        public const string OccupationName = OccupationTable + "//strong " + "//a";
        public const string OccupationDescription = OccupationTable + "//p";
    }
}