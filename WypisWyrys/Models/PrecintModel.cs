using ArcGIS.Core.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WypisWyrys.Models
{
    public class PrecintModel
    {
        /*public long ObjectId1 { get; set; }
        public Polygon Shape { get; set; }
        public long ObjectId { get; set; }
        public string precintNumber {get;set;}
        public string precintName { get; set; }
        public double shapeLength { get; set; }
        public double shapeArea { get; set; }
        public PrecintModel(long id1, Polygon shape, long id, string precintNumber, string precintName, double shapeLength, double shapeArea)
        {
            this.ObjectId = id;
            this.ObjectId1 = id1;
            this.Shape = shape;
            this.precintName = precintName;
            this.precintNumber = precintNumber;
            this.shapeArea = shapeArea;
            this.shapeLength = shapeLength;
        }*/
        public Dictionary<string, object> precint { get; set; }
        public PrecintModel (Dictionary<string, object> precint)
        {
            this.precint = precint;
        }
    }
}
