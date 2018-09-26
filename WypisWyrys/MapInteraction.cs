using ArcGIS.Desktop.Framework.Dialogs;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WypisWyrys
{
    class MapInteraction
    {
        private Config config;
        public MapInteraction()
        {
            config = new Config();
        }
        public bool clearSelection()
        {
            var layers = MapView.Active.Map.Layers;
            var parcelLayerName = config.getConfig("Działki", "parcelsLayer");
            if (parcelLayerName == null)
            {
                MessageBox.Show("Brak wybranej konfiguracji");
                return false;
            }
            FeatureLayer layer = (FeatureLayer)layers.Where((currentLayer) =>
            {
                return currentLayer.Name.Equals(parcelLayerName);
            }).First();
            if (layer == null)
            {
                MessageBox.Show("Brak podanej warstwy");
                return false;
            }
            Task t = QueuedTask.Run(() =>
            {
                layer.ClearSelection();
            });
            return true;
        }
    }
}
