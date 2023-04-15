using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketHive_MadCats.Shared.Statics
{
    // TODO: The lists and dictionaries are pulled from bing ai and are
    // both incomplete and not in agreement. Needs to be manually checked.

    /// <summary>
    /// Static class containing lists and dictionaries regarding countries
    /// and currency names
    /// </summary>
    public static class CountriesAndCodes
    {
        // Simple list of european countries
        private static List<string> europeanCountries = new List<string>()
        {
            "Albania",
            "Andorra",
            "Austria",
            "Belarus",
            "Belgium",
            "Bosnia and Herzegovina",
            "Bulgaria",
            "Croatia",
            "Cyprus",
            "Czech Republic",
            "Denmark",
            "Estonia",
            "Finland",
            "France",
            "Germany",
            "Greece",
            "Hungary",
            "Iceland",
            "Ireland",
            "Italy",
            "Kosovo",
            "Latvia",
            "Liechtenstein",
            "Lithuania",
            "Luxembourg",
            "Malta",
            "Moldova",
            "Monaco",
            "Montenegro",
            "Netherlands (Holland)",
            "North Macedonia (formerly Macedonia)",
            "Norway",
            "Poland",
            "Portugal",
            "Romania",
            "Russia",
            "San Marino",
            "Serbia",
            "Slovakia",
            "Slovenia",
            "Spain",
            "Sweden",
            "Switzerland (Confederation of Helvetia)",
            "Ukraine",
            "United Kingdom"
        };

        // Countries and their 3 letter abbreviation
        private static Dictionary<string, string> europeanCountriesAndAbbreviations = new Dictionary<string, string>()
        {
            { "Austria", "AUT" },
            { "Belgium", "BEL" },
            { "Bulgaria", "BGR" },
            { "Croatia", "HRV" },
            { "Cyprus", "CYP" },
            { "Czech Republic", "CZE" },
            { "Denmark", "DNK" },
            { "Estonia", "EST" },
            { "Finland", "FIN" },
            { "France", "FRA" },
            { "Germany", "DEU" },
            { "Greece", "GRC" },
            { "Hungary", "HUN" },
            { "Ireland", "IRL" },
            { "Italy", "ITA" },
            { "Latvia", "LVA" },
            { "Lithuania", "LTU" },
            { "Luxembourg", "LUX" },
            { "Malta", "MLT" },
            { "Netherlands", "NLD" },
            { "Poland", "POL" },
            { "Portugal", "PRT" },
            { "Romania", "ROU" },
            { "Slovakia", "SVK" },
            { "Slovenia", "SVN"},
            {"Spain","ESP"},
            {"Sweden","SWE"},
            {"Great Britain","GBR"}
        };
        // Country names and their currency three letter abbreviation
        private static Dictionary<string, string> countryCurrencyAbbreviations = new Dictionary<string, string>()
        {
            { "Austria", "EUR" },
            { "Belgium", "EUR" },
            { "Bulgaria", "BGN" },
            { "Croatia", "HRK" },
            { "Cyprus", "EUR" },
            { "Czech Republic", "CZK" },
            { "Denmark", "DKK" },
            { "Estonia", "EUR" },
            { "Finland", "EUR" },
            { "France", "EUR" },
            { "Germany", "EUR" },
            { "Greece", "EUR" },
            { "Hungary", "HUF" },
            { "Ireland", "EUR" },
            { "Italy", "EUR" },
            { "Latvia", "EUR" },
            { "Lithuania", "EUR" },
            { "Luxembourg", "EUR" },
            { "Malta",  "EUR"},
            {"Netherlands","EUR"},
            {"Poland","PLN"},
            {"Portugal","EUR"},
            {"Romania","RON"},
            {"Slovakia","EUR"},
            {"Slovenia","EUR"},
            {"Spain","EUR"},
            {"Sweden","SEK"},
            {"Great Britain","GBP"}
        };


        // Properties that get the field variables
        public static List<string> getListOfCountries { get { return europeanCountries; } }
        public static Dictionary<string, string> getCountriesAndAbbreviations { get {  return europeanCountriesAndAbbreviations; } }
        public static Dictionary<string, string> getCountryCurrencyAbbreviations { get { return countryCurrencyAbbreviations; } }
    }
}
