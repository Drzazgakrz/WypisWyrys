using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WypisWyrys.Models;

namespace WypisWyrys
{
    class MapInteraction
    {
        public Layer getLayerByName(string name)
        {
            var layers = MapView.Active.Map.Layers;
            return layers.Where((layer) =>
            {
                return layer.Name.Equals(name);
            }).First();
        }

        /*public ParcelModel getParcelByShape(Geometry geometry)
        {

        }*/
    }
}
