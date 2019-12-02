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
        public List<GraphData> GraphData = new List<GraphData>();
    }

    public class GraphData
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
    }
}
