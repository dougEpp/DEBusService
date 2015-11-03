using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DEBusService.Models
{
    [MetadataType(typeof(provinceMetadata))]
    public partial class province
    {

    }
    public class provinceMetadata
    {
        [Display(Name="Province Code")]
        public string provinceCode { get; set; }
        [Display(Name="Province")]
        public string name { get; set; }
        public string countryCode { get; set; }
        public string taxCode { get; set; }
        public double taxRate { get; set; }
        public string capital { get; set; }
    }
}