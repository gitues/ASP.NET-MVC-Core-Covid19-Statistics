
using System.Collections.Generic;

namespace ASPNETCoreMVCcovid19App.Models
{       
    public class Countries
    {
        public List<Country> data { get; set; }
    }
       
    public class Country
    {       
        public int confirmed { get; set; }
        public int deaths { get; set; }
        public Region region { get; set; }

    }

    public class Regions
    {
        public List<Region> data { get; set; }
    }

    public class Region
    {
        public string iso { get; set; }
        public string name { get; set; }
        public string province { get; set; }
    }

}