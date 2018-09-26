using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WypisWyrys.Models
{
    public class NecessaryProperties
    {
        public List<MPZPModel> mpzpModels { get; set; }
        public List<ParcelModel> parcels { get; set; }
        public List<ResolutionModel> resolutions { get; set; }
        public List<PrecintModel> precints { get; set; }
        public NecessaryProperties()
        {
            mpzpModels = new List<MPZPModel>();
            parcels = new List<ParcelModel>();
            resolutions = new List<ResolutionModel>();
            precints = new List<PrecintModel>();
        }
    }
}
