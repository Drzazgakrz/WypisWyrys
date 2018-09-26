using ArcGIS.Core.Data;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WypisWyrys.Models;

namespace WypisWyrys
{
    public partial class ParcelListView : System.Windows.Controls.UserControl
    {
        private Config config;
        public ParcelListView()
        {
            config = new Config();
            InitializeComponent();
        }
        private List<ParcelModel> parcels = new List<ParcelModel>();
        public void fillTextBox(List<ParcelModel> parcels)
        {
            primaryNavigator.Items.Clear();
            panel = new TableLayoutPanel();
            panel.ColumnCount = 2;
            panel.RowCount = parcels.Count;
            this.parcels = parcels;
            for(int i = 0; i < parcels.Count; i++)
            {
                createRow(parcels.ElementAt(i), i);
            }
        }

        private void clearAll(object sender, RoutedEventArgs e)
        {
            parcels = new List<ParcelModel>();
            primaryNavigator.Items.Clear();
            string layerName = config.getConfig("Działki", "parcelsLayer");
            if(layerName == null)
            {
                ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Brak konfiguracji dla warstw. Stwórz nową");
                return;
            }
            var layers = MapView.Active.Map.Layers;
            FeatureLayer parcelsLayer = (FeatureLayer)layers.Where((layer) =>
            {
                return layerName.Equals(layer.Name);
            }).First();
            if(parcelsLayer == null)
            {
                ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Brak konfiguracji dla warstw. Stwórz nową");
                return;
            }
            Task t = QueuedTask.Run(() =>
            {
                parcelsLayer.ClearSelection();
            });
        }

        string parcelIdField;
        TableLayoutPanel panel;
        public void createRow(ParcelModel parcel, int row)
        {
            WrapPanel panel = new WrapPanel();
            
            System.Windows.Controls.TextBox parcelId = new System.Windows.Controls.TextBox();
            parcelId.IsReadOnly = true;
            parcelId.Name = "parcelId";
            parcelIdField = config.getConfig("Działki", "parcelsId");
            panel.Children.Add(parcelId);
            object result = null;
            parcel.parcel.TryGetValue(parcelIdField, out result);
            parcelId.Text = result.ToString();
            System.Windows.Controls.Button removeButton = new System.Windows.Controls.Button();
            removeButton.Content = "Usuń";
            removeButton.Click +=removeParcel;
            panel.Children.Add(removeButton);
            System.Windows.Controls.Button zoomButton = new System.Windows.Controls.Button();
            zoomButton.Content = "Przybliż";
            zoomButton.Click += zoomToParcel;
            panel.Children.Add(zoomButton);
            primaryNavigator.Items.Add(panel);
        }
        public void removeParcel(object sender, EventArgs e)
        {
            System.Windows.Controls.Button source = (System.Windows.Controls.Button)sender;
            WrapPanel parent = (WrapPanel)source.Parent;
            var children = parent.Children.GetEnumerator();
            children.MoveNext();
            string parcelId = ((System.Windows.Controls.TextBox)children.Current).Text;
            ParcelModel parcelClicked = this.parcels.Where((parcel) =>
            {
                object result = null;
                parcel.parcel.TryGetValue(parcelIdField, out result);
                return parcelId.Equals(result.ToString());
            }).First();
            parcels.Remove(parcelClicked);
            primaryNavigator.Items.Clear();
            this.fillTextBox(this.parcels);
        }

        public void zoomToParcel(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button source = (System.Windows.Controls.Button)sender;
            WrapPanel parent = (WrapPanel)source.Parent;
            var children = parent.Children.GetEnumerator();
            children.MoveNext();
            string parcelId = ((System.Windows.Controls.TextBox)children.Current).Text;
            ParcelModel parcelClicked = this.parcels.Where((parcel) =>
            {
                object result = null;
                parcel.parcel.TryGetValue(parcelIdField, out result);
                return parcelId.Equals(result.ToString());
            }).First();
            object parcelPolygon = null;
            parcelClicked.parcel.TryGetValue("Shape", out parcelPolygon);
            Task t = QueuedTask.Run(() =>
            {
                MapView.Active.ZoomTo((ArcGIS.Core.Geometry.Polygon)parcelPolygon,null, false);
            });
        }

        public void checkParcelProperties(object sender, RoutedEventArgs e)
        {
            ParcelListViewModel pane = (ParcelListViewModel)FrameworkApplication.DockPaneManager.Find(ParcelListViewModel._dockPaneID);
            pane.desactivatePane();
        }
    }
}
