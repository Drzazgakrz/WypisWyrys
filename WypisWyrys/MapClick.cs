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
using System.Windows.Media.Imaging;
using System.Drawing;

namespace WypisWyrys
{
    class MapClick : MapTool
    {
        private Config config;
        public MapClick()
        {
            config = new Config();
            IsSketchTool = true;
            SketchType = SketchGeometryType.Multipoint;
            SketchOutputMode = SketchOutputMode.Screen;
        }

        public static bool isDockpaneActive { get; set; } = false;
        private FeatureLayer parcellayer;
        private QueryFilter parcelQuery;
        public NecessaryProperties properties;
        List<long> lista;
        private async void getData(Geometry geometry, GeometryDimension dimension)
        {
            properties = new NecessaryProperties();
            ArcGIS.Desktop.Mapping.MapView view = ArcGIS.Desktop.Mapping.MapView.Active;

                var layersTOC = ArcGIS.Desktop.Mapping.MapView.Active.Map.GetLayersAsFlattenedList();
            FeatureLayer parcelsLayer = (FeatureLayer)layersTOC.Where((layer) =>
            {
                return layer.Name.Contains(config.getConfig("Działki", "parcelsLayer"));
            }).First();
            var parcels = ArcGIS.Desktop.Mapping.MapView.Active.Map.GetLayersAsFlattenedList();
            foreach (var f in parcels)
            {
                var feat = f.Name.Equals(config.getConfig("Działki", "parcelsLayer"));
                if (feat)
                {
                    this.parcellayer = (FeatureLayer)f;
                }
            }
            this.getParcel(parcelsLayer, geometry, dimension);
            CIMLineSymbol symbol = new CIMLineSymbol();
            symbol.SetColor(ColorFactory.Instance.CreateRGBColor(255, 255, 255, 0));
            foreach (ParcelModel parcelModel in properties.parcels)
            {
                object result = null;
                var pointsFromShape = parcelModel.parcel.TryGetValue("Shape", out result);
            }
            Polygon shp = null;
            foreach (Layer layer in layersTOC)
            {
                try
                {
                    FeatureLayer fLayer = (FeatureLayer)layer; 
                    if (fLayer.Name.Contains(config.getConfig("MPZP", "MPZPlayer")))
                    {
                        this.getMPZP(fLayer, geometry, dimension);
                    }
                    else if (fLayer.Name.Contains(config.getConfig("Wydzielenia", "precintLayer")))
                    { 
                        this.getResolution(fLayer, geometry, dimension);
                    }
                    else if (fLayer.Name.Contains(config.getConfig("Obręby", "areaLayer")))
                    {
                        this.getPrecints(fLayer, geometry, dimension);
                    }
                }catch (Exception) { }
            }
        }
        protected override Task<bool> OnSketchCompleteAsync(Geometry geometry)
        {
            if (isDockpaneActive)
            {
                List<MapPoint> points = new List<MapPoint>();
                Task t = QueuedTask.Run(() =>
                {
                    foreach (MapPoint point in ((Multipoint)geometry).Points)
                    {
                        MapPoint finalPoint = ArcGIS.Desktop.Mapping.MapView.Active.ClientToMap(new System.Windows.Point(point.X, point.Y));
                        points.Add(finalPoint);
                    }

                    getData(MultipointBuilder.CreateMultipoint(points), GeometryDimension.esriGeometry0Dimension);
                });
                t.Wait();
                ((ParcelListViewModel)FrameworkApplication.DockPaneManager.Find(ParcelListViewModel._dockPaneID)).getView(properties);
                return QueuedTask.Run(() =>
                {
                    if (parcellayer != null)
                    {
                        parcellayer.Select(this.parcelQuery, SelectionCombinationMethod.New);
                    }
                    return true;
                });
            }
            return QueuedTask.Run(() =>
            {
                return false;
            });
        }
        private void getParcel(FeatureLayer layer, Geometry geometry, GeometryDimension dimension)
        {
            lista = this.getIntersectedIds(geometry, layer, dimension);
            var selection = layer.GetTable();
            var selectFilter = new QueryFilter();
            var statement = "";
            foreach (long id in lista)
            {
                if (statement.Length > 0)
                {
                    statement += " OR ";
                }
                var filter = new QueryFilter();
                statement += "OBJECTID=" + id;
                filter.WhereClause = "OBJECTID=" + id;
               
                var cursor = selection.Search(filter);
               
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
                properties.parcels.Add(new ParcelModel(parcelDictionary));
            }
            selectFilter.WhereClause = statement ;
            this.parcelQuery = selectFilter;
        }
        private List<long> getIdsFromEveryParcel(FeatureLayer layer)
        {
            List<long> allIds = new List<long>();
            foreach(ParcelModel current in properties.parcels)
            {
                object result;
                current.parcel.TryGetValue("Shape", out result);
                List<long> currentIds = getIntersectedIds((Polygon)result, layer, GeometryDimension.esriGeometry2Dimension);
                foreach(long currentId in currentIds)
                {
                    if (!allIds.Contains(currentId))
                    {
                        allIds.Add(currentId);
                    }
                }
            }
            return allIds;
        }
        private void getMPZP(FeatureLayer layer, Geometry geometry,GeometryDimension dimension)
        {
            try
            {
                var lista = getIdsFromEveryParcel(layer);
                foreach (long id in lista)
                {
                    var filter = new QueryFilter();
                    filter.WhereClause = $"{"OBJECTID"}=" + id;
                    var selection = layer.GetTable();
                    var mpzp = selection.Search(filter);
                    mpzp.MoveNext();
                    var mpzpRow = mpzp.Current;
                    Dictionary<string, object> mpzpDictionary = new Dictionary<string, object>();
                    var fields = mpzpRow.GetFields();
                    int iterator = 0;
                    var atachments = mpzpRow.GetAttachments();
                    foreach (Attachment atach in atachments)
                    {
                        var memoryStream = atach.GetData();
                        var imageByte = memoryStream.ToArray();
                        BitmapImage bitImage = new BitmapImage();
                        bitImage.BeginInit();
                        bitImage.StreamSource = memoryStream;
                        bitImage.EndInit();

                        mpzpDictionary.Add("legend", imageByte);
                        mpzpDictionary.Add("legendSize", new Size(bitImage.PixelWidth, bitImage.PixelHeight));
                        memoryStream.Close();
                    }
                    foreach (Field field in fields)
                    {
                        mpzpDictionary.Add(field.Name, mpzpRow.GetOriginalValue(iterator));
                        iterator++;
                    }
                    properties.mpzpModels.Add(new MPZPModel(mpzpDictionary));
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
        }
        private void getResolution(FeatureLayer layer, Geometry geometry, GeometryDimension dimension)
        {
            var resolutionList = this.getIdsFromEveryParcel(layer);
            foreach (long id in resolutionList)
            {
                var selection = layer.GetTable();
                QueryFilter filter = new QueryFilter();
                filter.WhereClause = "OBJECTID=" + id;
                var cursor = selection.Search(filter);
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
                    properties.resolutions.Add(new ResolutionModel(resolutionDictionary));
                }
            }
        }

        private void getPrecints(FeatureLayer layer, Geometry geometry, GeometryDimension dimension)
        {
            var precintsList = this.getIntersectedIds(geometry, layer, dimension);
            foreach (long id in precintsList)
            {
                var filter = new QueryFilter();
                string idField = config.getConfig("Obręby", "areaId");
                filter.WhereClause = idField + "=" + id;
                var selection = layer.GetTable();
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
                    properties.precints.Add(new PrecintModel(precintDictionary));
                }
            }
            
        }
        string geometryString = "";
        public List<long> getIntersectedIds(Geometry geometry, FeatureLayer layer, GeometryDimension dimension)
        {
            List<long> intersectedObjects = new List<long>();
                var table = layer.GetTable();
                var selection = table.Search();                
                while (selection.MoveNext())
                {
                    var current = selection.Current;
                    Polygon polygon = (Polygon)current.GetOriginalValue(current.FindField("Shape"));
                    IGeometryEngine engine = GeometryEngine.Instance;
                    if (!engine.Intersection(geometry, polygon, dimension).IsEmpty)
                    {
                        intersectedObjects.Add(current.GetObjectID());
                    }
                }
            return intersectedObjects;            
        }

    }
}
