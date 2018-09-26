using ArcGIS.Core.Geometry;
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
    public partial class ResolutionListView : System.Windows.Controls.UserControl
    {
        public NecessaryProperties propertiesAccepted { get; set; }
        private Config config;
        public ResolutionListView()
        {
            config = new Config();
            InitializeComponent();
           
        }
        public void getModels(NecessaryProperties acceptedProperties)
        {
            this.propertiesAccepted = acceptedProperties;
            fillTextBox();
        }
        public void getOwnerInfo(object sender, RoutedEventArgs e)
        {
            OwnerInfoViewModel.Show();
            ResolutionListViewModel.desactivatePane();
        }
        public void goBack(object sender, RoutedEventArgs e)
        {
            MPZPListViewModel.Show();
            ResolutionListViewModel.desactivatePane();
        }
        void Form1_Closing(object sender, FormClosedEventArgs e)
        {            
             current.resolutions = ((ResolutionsWindow)sender).models;
        }

        public void emptyListView()
        {
            this.primaryNavigator.Items.Clear();
        }

        private ParcelModel current;
        public void addResolutionsToParcel(ParcelModel currentParcel)
        {
            Task t = QueuedTask.Run(() =>
            {
                var models = propertiesAccepted.resolutions.Where((resolution) =>
                {
                    object resolutionShape;
                    propertiesAccepted.resolutions.First().resolution.TryGetValue("Shape", out resolutionShape);
                    object parcelShape;
                    currentParcel.parcel.TryGetValue("Shape", out parcelShape);
                    ArcGIS.Core.Geometry.Polygon resolutionPolygon = (ArcGIS.Core.Geometry.Polygon)resolutionShape;
                    ArcGIS.Core.Geometry.Polygon parcelPolygon = (ArcGIS.Core.Geometry.Polygon)parcelShape;
                    ArcGIS.Core.Geometry.Geometry geometry = GeometryEngine.Instance.Intersection(resolutionPolygon, parcelPolygon, GeometryDimension.esriGeometry2Dimension);
                    return (!geometry.IsEmpty);
                }).ToList();
                currentParcel.resolutions = models;
            });
        }
        public void fillTextBox()
        {
            panel = new TableLayoutPanel();
            panel.ColumnCount = 2;
            panel.RowCount = propertiesAccepted.parcels.Count;
            for (int i = 0; i < propertiesAccepted.parcels.Count; i++)
            {
                createRow(propertiesAccepted.parcels.ElementAt(i), i);
                addResolutionsToParcel(propertiesAccepted.parcels.ElementAt(i));
            }
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
            System.Windows.Controls.Button button = new System.Windows.Controls.Button();
            button.Content = "Usuń";
            button.Click += removeParcel;
            panel.Children.Add(button);
            System.Windows.Controls.Button infoButton = new System.Windows.Controls.Button();
            infoButton.Content = "Pokaż wydzielenia";
            infoButton.Click += getParcelsResolution;
            panel.Children.Add(infoButton);
            primaryNavigator.Items.Add(panel);
        }
        public void removeParcel(object sender, EventArgs e)
        {
            System.Windows.Controls.Button source = (System.Windows.Controls.Button)sender;
            WrapPanel parent = (WrapPanel)source.Parent;
            var children = parent.Children.GetEnumerator();
            children.MoveNext();
            string parcelId = ((System.Windows.Controls.TextBox)children.Current).Text;
            ParcelModel parcelClicked = propertiesAccepted.parcels.Where((parcel) =>
            {
                object result = null;
                parcel.parcel.TryGetValue(parcelIdField, out result);
                return parcelId.Equals(result.ToString());
            }).First();
            propertiesAccepted.parcels.Remove(parcelClicked);
            primaryNavigator.Items.Clear();
            this.fillTextBox();
        }

        public void getParcelsResolution(object sender, EventArgs e)
        {
            System.Windows.Controls.Button source = (System.Windows.Controls.Button)sender;
            WrapPanel parent = (WrapPanel)source.Parent;
            var children = parent.Children.GetEnumerator();
            children.MoveNext();
            string parcelId = ((System.Windows.Controls.TextBox)children.Current).Text;
            ParcelModel parcel = propertiesAccepted.parcels.Where((singleParcel) =>
            {
                object id = null;
                string identifier = config.getConfig("Działki", "parcelsId");
                singleParcel.parcel.TryGetValue(identifier, out id);
                return parcelId.Equals(id.ToString());
            }).First();
            current = parcel;
            ResolutionsWindow form = new ResolutionsWindow(parcel.resolutions);
            form.FormClosed += new FormClosedEventHandler(Form1_Closing);
            form.Show();
        }
    }

}
