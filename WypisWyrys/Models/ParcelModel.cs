using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WypisWyrys.Models
{
    public class ParcelModel : ObjectModel
    {
        /*public long OBJECTID { get; set; }
        public ArcGIS.Core.Geometry.Polygon Shape { get; set; }
        public string IDENTYFIKA { get; set; }
        public double POWIERZCHN { get; set; }
        public string Shape_Length { get; set; }
        public double Shape_Area { get; set; }

        public ParcelModel(long id, ArcGIS.Core.Geometry.Polygon Shape, string IDENTYFIKA, double POWIERZCHN, string Shape_Length, double Shape_Area)
        {
            this.OBJECTID = id;
            this.Shape = Shape;
            this.IDENTYFIKA = IDENTYFIKA;
            this.POWIERZCHN = POWIERZCHN;
            this.Shape_Length = Shape_Length;
            this.Shape_Area = Shape_Area;
        }
        public override string ToString()
        {
            return OBJECTID+" "+Shape+" "+IDENTYFIKA+" "+POWIERZCHN+" "+Shape_Length+" "+Shape_Area;
        }*/
        public Dictionary<string, object> parcel { get; set; }
        public ParcelModel(Dictionary<string, object> parcel)
        {
            this.parcel = parcel;
        }
        public List<ResolutionModel> resolutions { get; set; } 
    }
}
