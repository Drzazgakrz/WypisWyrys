using ArcGIS.Core.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WypisWyrys.Models
{
    public class ResolutionModel
    {
        /*public long OBJECTID { get; set; }
        public int resolutionYear { get; set; }
        public Polygon Shape;
        public string symbolPlan { get; set; }
        public string generalize { get; set; }
        public string name { get; set; }
        public string resolution { get; set; }
        public string arrangements { get; set; }
        public string destination { get; set; }
        public DateTime date { get; set; }
        public double Shape_Length { get; set; }
        public double Shape_Area { get; set; }

        public ResolutionModel(long id, int year, Polygon shape, string symbol, string gen, string name, string resolution, 
            string arrangements, string destination, DateTime date, double shapeLength, double shapeArea)
        {
            this.OBJECTID = id;
            this.resolutionYear = year;
            this.Shape = shape;
            this.symbolPlan = symbol;
            this.generalize = gen;
            this.name = name;
            this.resolution = resolution;
            this.arrangements = arrangements;
            this.date = date;
            this.Shape_Length = Shape_Length;
            this.Shape_Area = shapeArea;
            this.destination = destination;
        }
        public override string ToString()
        {
            return this.destination;
        }
        public static implicit operator KeyValuePair<object, object>(ResolutionModel v)
        {
            throw new NotImplementedException();
        }*/
        public Dictionary<string, object> resolution { get; set; }
        public ResolutionModel (Dictionary<string, object> resolution)
        {
            this.resolution = resolution;
        }
    }
}
