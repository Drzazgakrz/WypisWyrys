using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ArcGIS.Desktop.Mapping;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using System.Windows.Input;
using ArcGIS.Desktop.Framework.Dialogs;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework;
using ArcGIS.Core.Data;
using ArcGIS.Core.Geometry;
using Esri.ArcGISRuntime.Controls;
using WypisWyrys.Models;
using ArcGIS.Core.Data;
using ArcGIS.Core.Data.UtilityNetwork.NetworkDiagrams;
using ArcGIS.Core.Data.Raster;
using System.Diagnostics;
using ArcGIS.Core.CIM;

namespace WypisWyrys
{
    class MapClick : MapTool
    {

        public MapClick()
        {
            IsSketchTool = true;
            SketchType = SketchGeometryType.Rectangle;
            SketchOutputMode = SketchOutputMode.Screen;
        }

        public static bool isDockpaneActive { get; set; } = false;
        private KeyValuePair<BasicFeatureLayer, List<long>> layer;
        private KeyValuePair<BasicFeatureLayer, List<long>> mpzp;
        private KeyValuePair<BasicFeatureLayer, List<long>> wydzielenia;
        private KeyValuePair<BasicFeatureLayer, List<long>> obreby;
        public static List<ParcelModel> parcel { get; set; } = new List<ParcelModel>();
        public Dictionary<BasicFeatureLayer, List<long>> result;
        public static MPZPModel mpzpModel { get; set; }
        public static List<ResolutionModel> resolutionModel = new List<ResolutionModel>();
        public static List<PrecintModel> precints = new List<PrecintModel>();
        string value = ""; 
        List<long> lista;
        protected override void OnToolDoubleClick(MapViewMouseButtonEventArgs e)
        {
            if (isDockpaneActive)
            {
                ArcGIS.Desktop.Mapping.MapView view = ArcGIS.Desktop.Mapping.MapView.Active;
                Geometry geometry = null;
                Task t = QueuedTask.Run(() =>
                {
                    geometry = view.ClientToMap(e.ClientPoint);
                    getData(geometry);
                });
                t.Wait();
                
            }
            ((Dockpane2ViewModel)FrameworkApplication.DockPaneManager.Find(Dockpane2ViewModel._dockPaneID)).getView();
        }
        private async void getData(Geometry geometry)
        {
            parcel = new List<ParcelModel>();
            mpzpModel = null;
            precints = new List<PrecintModel>();
            resolutionModel = new List<ResolutionModel>();
            ArcGIS.Desktop.Mapping.MapView view = ArcGIS.Desktop.Mapping.MapView.Active;

            Task t = QueuedTask.Run(() =>
            {
                result = view.GetFeatures(geometry, true, false);
                Polygon shp = null;
                foreach (KeyValuePair<BasicFeatureLayer, List<long>> fLayer in result)
                {
                    if (fLayer.Key.Name.Contains(LayersSettingsForm.getConfig("Działki", "parcelsLayer")))
                    {
                        this.layer = fLayer;
                        this.getParcel();
                        CIMLineSymbol symbol = new CIMLineSymbol();
                        symbol.SetColor(ColorFactory.Instance.CreateRGBColor(24, 69, 59));
                        foreach (ParcelModel parcelModel in parcel)
                        {
                            object result = null;
                            var pointsFromShape = parcelModel.parcel.TryGetValue("Shape", out result);
                            AddOverlayAsync((Polygon)result, (symbol.MakeSymbolReference()));
                        }
                        
                    }
                    else if (fLayer.Key.Name.Contains(LayersSettingsForm.getConfig("MPZP", "MPZPlayer")))
                    {
                        this.mpzp = fLayer;
                        this.getMPZP();
                    }
                    else if (fLayer.Key.Name.Contains(LayersSettingsForm.getConfig("Wydzielenia", "precintLayer")))
                    {
                        this.wydzielenia = fLayer;
                        this.getResolution(geometry);
                    }
                    else if (fLayer.Key.Name.Contains(LayersSettingsForm.getConfig("Obręby", "areaLayer")))
                    {
                        this.obreby = fLayer;
                        this.getPrecints();
                    }
                }
                var textGraphic = new CIMTextGraphic();
                textGraphic.Shape = geometry;
                var textSymbol = SymbolFactory.Instance.ConstructTextSymbol(ColorFactory.Instance.BlackRGB, 8.5, "Corbel", "Regular");
                textGraphic.Symbol = textSymbol.MakeSymbolReference();
                view.AddOverlay(textGraphic);
            });            
            t.Wait();
        }
        protected override async Task<bool> OnSketchCompleteAsync(Geometry geometry)
        {
            if (isDockpaneActive)
            {
                 getData(geometry);
                ((Dockpane2ViewModel)FrameworkApplication.DockPaneManager.Find(Dockpane2ViewModel._dockPaneID)).getView();
                return true;
            }
            return false;
        }
        private void getParcel()
        {
            lista = layer.Value;
            
            foreach (long id in lista)
            {
                var filter = new QueryFilter();
                //string idField = LayersSettingsForm.getConfig("Działki", "parcelsId");idField + "="
                filter.WhereClause = "OBJECTID=" + id;
                var selection = this.layer.Key.GetTable();
                var cursor = selection.Search(filter);
                selection.Select(filter, SelectionType.ObjectID, SelectionOption.Normal);
                cursor.MoveNext();
                var row = cursor.Current;
                Dictionary<string, object> parcelDictionary = new Dictionary<string, object>();
                var fields = row.GetFields();
                int iterator = 0;
                foreach (Field field in fields)
                {
                    parcelDictionary.Add(field.Name, row.GetOriginalValue(iterator));
                    iterator++;
                }
                parcel.Add(new ParcelModel(parcelDictionary));
            }
        }
        private void getMPZP()
        {
            try
            {
                //result.TryGetValue(this.mpzp.Key, out lista);
                var filter = new QueryFilter();
                //string idField = LayersSettingsForm.getConfig("Działki", "parcelsId");
                filter.WhereClause = $"{"OBJECTID"}="+ this.mpzp.Value.First();
                var selection = this.mpzp.Key.GetTable();
                var mpzp = selection.Search(filter);
                mpzp.MoveNext();
                var mpzpRow = mpzp.Current;
               // selection.Select(filter, SelectionType.ObjectID, SelectionOption.Normal);
                Dictionary<string, object> mpzpDictionary = new Dictionary<string, object>();
                var fields = mpzpRow.GetFields();
                int iterator = 0;
                foreach (Field field in fields)
                {
                    mpzpDictionary.Add(field.Name, mpzpRow.GetOriginalValue(iterator));
                    iterator++;
                }
                mpzpModel = new MPZPModel(mpzpDictionary);
            }
            catch(Exception e) {
                Debug.WriteLine(e.StackTrace);
            }
        }
        private void getResolution(Geometry geometry)
        {
                var selection = this.wydzielenia.Key.GetTable();
                var cursor = selection.Search();
                while (cursor.MoveNext())
                {
                    var resolutionRow = cursor.Current;
                    Dictionary<string, object> resolutionDictionary = new Dictionary<string, object>();
                    var fields = resolutionRow.GetFields();
                    int iterator = 0;
                    foreach (Field field in fields)
                    {
                        resolutionDictionary.Add(field.Name, resolutionRow.GetOriginalValue(iterator));
                        iterator++;
                    }
                    resolutionModel.Add(new ResolutionModel(resolutionDictionary));
                }
            
        }private int count;
        private void getPrecints()
        {
            //esult.TryGetValue(this.obreby.Key, out lista);
            foreach (long id in obreby.Value)
            {
                var filter = new QueryFilter();
                string idField = LayersSettingsForm.getConfig("Obręby", "areaId");
                filter.WhereClause = idField + "=" + id;
                var selection = this.obreby.Key.GetTable();
                var wydzielenia = selection.Search(filter);
                while (wydzielenia.MoveNext())
                {
                    var precintRow = wydzielenia.Current;
                    Dictionary<string, object> precintDictionary = new Dictionary<string, object>();
                    var fields = precintRow.GetFields();
                    int iterator = 0;
                    foreach (Field field in fields)
                    {
                        precintDictionary.Add(field.Name, precintRow.GetOriginalValue(iterator));
                        iterator++;
                    }
                    precints.Add(new PrecintModel(precintDictionary));
                }
            }
            
        }
        string geometryString = "";
        public List<long> getIntersectedIds(Geometry geometry, FeatureLayer layer, GeometryDimension dimension)
        {
            /*try
            {*/
                var table = layer.GetTable();
                var selection = table.Search();
                List<long> intersectedObjects = new List<long>();
                while(selection.MoveNext())
                {
                var current = selection.Current;
                    Polygon polygon = (Polygon)current.GetOriginalValue(current.FindField("Shape"));
                        IGeometryEngine engine = GeometryEngine.Instance;
                        if(!engine.Intersection(geometry, polygon, dimension).IsEmpty)
                        {
                            
                            intersectedObjects.Add(current.GetObjectID());
                        }
                //MessageBox.Show(intersectedObjects.Count.ToString());
                }
                return intersectedObjects;
                
            /*}catch(Exception e)
            {
                return null;
            }*/
            
        }

    }
}
