using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WypisWyrys.Models
{
    public class ParcelModel : ObjectModel
    {
        public Dictionary<string, object> parcel { get; set; }
        public ParcelModel(Dictionary<string, object> parcel)
        {
            this.parcel = parcel;
        }
        public List<ResolutionModel> resolutions { get; set; }
        public List<PrecintModel> precints { get; set; }
    }
}
