using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlarmRegistrationSystem.Models.ViewModels
{
    public class GraphDataViewModel
    {
        public string ObjectName { get; set; }
        public string Value { get; set; }

        public string Unit { get; set; }

        public string Title { get; set; }
        public string Extra { get; set; }
        public string DocumentationURL { get; set; }

        public List<GraphData> GraphData = new List<GraphData>();
    }

    public class GraphData
    {
        public string Name { get; set; }
        public double Quantity { get; set; }
        public string WorstSubassemblies { get; set; }
    }
}
