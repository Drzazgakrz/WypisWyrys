using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WypisWyrys.Models;

namespace WypisWyrys
{
    public partial class MPZPListView : UserControl
    {
        public MPZPListView()
        {
            InitializeComponent();
        }

        public List<MPZPModel> models;
        public static MPZPModel model { get; set; }
        public static List<ParcelModel> parcels { get; set; } = new List<ParcelModel>();
        public static List<ParcelModel> parcelsInMPZP { get; set; } = new List<ParcelModel>();

        public void fillTextViews(List<MPZPModel> model)
        {
            parcels = MapClick.parcel;
            try
            {
                this.models = model;
                var i = 0;
                foreach(MPZPModel currentModel in model)
                {
                    createRow(currentModel, i);
                    i++;
                }

            }catch(Exception e)
            {
                MessageBox.Show("Wybierz działkę");
            }
        }

        private TextBox createTextBox(MPZPModel mpzp, string parent, string child, string name)
        {
            System.Windows.Controls.TextBox MPZPTextBox = new System.Windows.Controls.TextBox();
            MPZPTextBox.IsReadOnly = true;
            MPZPTextBox.Width = 150;
            MPZPTextBox.Name = name;
            object result = null;
            string MPZPIdField = LayersSettingsForm.getConfig(parent, child);
            mpzp.mpzp.TryGetValue(MPZPIdField, out result);
            MPZPTextBox.Text = result.ToString();
            return MPZPTextBox;
        }

        public void createRow(MPZPModel mpzp, int row)
        {
            WrapPanel panel = new WrapPanel();

            TextBox MPZPName = createTextBox(mpzp, "MPZP", "MPZPId", "MPZPName");
            panel.Children.Add(MPZPName);

            TextBox resolutionName = createTextBox(mpzp, "MPZP", "MPZPResolution", "MPZPResolution");
            panel.Children.Add(resolutionName);

            System.Windows.Controls.Button button = new System.Windows.Controls.Button();
            button.Content = "Wybierz";
            button.Click += chooseMPZP;
            panel.Children.Add(button);
            primaryNavigator.Items.Add(panel);
        }

        public void chooseMPZP(object sender, RoutedEventArgs args) 
        {
            Button source = (Button)sender;
            WrapPanel panel = (WrapPanel)source.Parent;
            string text = "";
            foreach(UIElement element in panel.Children)
            {
                TextBox box = (TextBox)element;
                if (box.Name.Equals("MPZPName"))
                {
                    text = box.Text;
                    break;
                }
            }
            model = models.Where((mpzp) =>
            {
                object result = null;
                mpzp.mpzp.TryGetValue(LayersSettingsForm.getConfig("MPZP", "MPZPId"), out result);
                return (text.Equals(result.ToString()));
            }).First();
            getIntersectedData();
            nextPane();
            
        }

        public void clearList()
        {
            this.primaryNavigator.Items.Clear();
        }

        public void nextPane()
        {
            ResolutionListViewModel.Show();
            clearList();
            MPZPListViewModel.desactivatePane();
        }
        public void goBack(object sender, RoutedEventArgs e)
        {
            ParcelListViewModel.Show();
            MPZPListViewModel.desactivatePane();
        }
        public void getIntersectedData()
        {
            parcelsInMPZP = new List<ParcelModel>();
            object result = null;
            model.mpzp.TryGetValue("Shape", out result);
            Task t = QueuedTask.Run(() =>
            {
                parcelsInMPZP = parcels.Where((parcel) =>
                {
                    object parcelsShape = null;
                    parcel.parcel.TryGetValue("Shape", out parcelsShape);
                    return (!GeometryEngine.Instance.Intersection((ArcGIS.Core.Geometry.Polygon)result,
                        (ArcGIS.Core.Geometry.Polygon)parcelsShape, GeometryDimension.esriGeometry2Dimension).IsEmpty);
                }).ToList();
            });            
            t.Wait();
        }
    }
}
