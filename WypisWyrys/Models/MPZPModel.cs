using ArcGIS.Core.Data.Raster;
using ArcGIS.Core.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WypisWyrys.Models
{
    public class MPZPModel
    {
        /*public long OBJECTID { get; set; }
        public Polygon Shape { get; set; }
        public string nazwa { get; set; }
        public string nr_uchwaly { get; set; }
        public string dziennik_w { get; set; } 
        public double SHAPE_Leng { get; set; }
        public double Shape_Area { get; set; }
        public object legenda { get; set; }
        public MPZPModel(long id, Polygon shape, string nazwa, string nr, string dziennik, double shapeLeng, double shapeArea, object legenda)
        {
            this.OBJECTID = id;
            this.Shape = shape;
            this.nazwa = nazwa;
            this.nr_uchwaly = nr;
            this.dziennik_w = dziennik;
            this.SHAPE_Leng = shapeLeng;
            this.Shape_Area = Shape_Area;
            this.legenda = legenda;
        }
        public override string ToString()
        {
            return this.OBJECTID+" "+this.Shape.ToString()+" "+this.nazwa+" "+this.nr_uchwaly+" "+this.legenda+" "+this.Shape_Area+" "+this.SHAPE_Leng;
        }*/
        public Dictionary<string, object> mpzp { get; set; }
        public MPZPModel(Dictionary<string, object> mpzp)
        {
            this.mpzp = mpzp;
        }
    }
}
