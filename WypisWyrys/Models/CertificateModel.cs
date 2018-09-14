using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WypisWyrys.Models
{
    class CertificateModel
    {
        public BaseInfoModel basicInformations { get; set; }
        public string mpzpName { get; set; }
        public List<ParcelModel> parcels { get; set; }
        public List<ResolutionModel> resolutions { get; set; }
        public UserModel person { get; set; }
    }
}
