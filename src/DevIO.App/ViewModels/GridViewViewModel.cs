using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevIO.App.ViewModels
{
    public class GridViewViewModel
    {
        public string[] HeaderColumns { get; set; }
        public string[] DisplayColumns { get; set; }
        public string Url { get; set; }
        public bool ReadOnly { get; set; }
        public int PageSize { get; set; }
        public string Controller { get; set; }
        public bool Exporting { get; set; }
    }
}
