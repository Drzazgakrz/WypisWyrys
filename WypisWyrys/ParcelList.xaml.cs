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
        public ParcelListView()
        {
            InitializeComponent();
        }
        private List<ParcelModel> parcels = new List<ParcelModel>();
        public void fillTextBox(List<ParcelModel> parcels)
        {
            panel = new TableLayoutPanel();
            panel.ColumnCount = 2;
            panel.RowCount = parcels.Count;
            this.parcels = parcels;
            for(int i = 0; i < parcels.Count; i++)
            {
                createRow(parcels.ElementAt(i), i);
            }
        }

        private void findParcel(object sender, RoutedEventArgs e)
        {
            string parcelIdentifier = this.identifier.Text;
            string id = LayersSettingsForm.getConfig("Działki", "parcelsId");
            string parcelLayerName = LayersSettingsForm.getConfig("Działki", "parcelsLayer");
            if (id == null)
            {
                ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Brak danych konfiguracyjnych. Podaj nowe");
                return;
            }
            Task t = QueuedTask.Run(() =>
            {
                var layers = MapView.Active.GetSelectedLayers();
                object result = null;
                Layer parcelLayer = layers.Where((layer) =>
                {
                    return layer.Name.Equals(parcelLayerName);
                }).First();

                ArcGIS.Core.Data.Table table = ((FeatureLayer)parcelLayer).GetTable();
                QueryFilter filter = new QueryFilter();
                filter.WhereClause = id + "=" + parcelIdentifier;
                var cursor = table.Search(filter);
                cursor.MoveNext();
                var parcel = cursor.Current;
                Dictionary<string, object> parcelModel = new Dictionary<string, object>();
                int i = 0;
                foreach(Field field in parcel.GetFields())
                {
                    parcelModel.Add(field.Name, parcel.GetOriginalValue(i));
                    i++;
                }
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
            parcelIdField = LayersSettingsForm.getConfig("Działki", "parcelsId");
            panel.Children.Add(parcelId);
            object result = null;
            parcel.parcel.TryGetValue(parcelIdField, out result);
            parcelId.Text = result.ToString();
            System.Windows.Controls.Button button = new System.Windows.Controls.Button();
            button.Content = "Usuń";
            button.Click +=removeParcel;
            panel.Children.Add(button);
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
        public void checkParcelProperties(object sender, RoutedEventArgs e)
        {
            ParcelListViewModel pane = (ParcelListViewModel)FrameworkApplication.DockPaneManager.Find(ParcelListViewModel._dockPaneID);
            pane.desactivatePane();
        }
    }
}
