using ArcGIS.Desktop.Framework;
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
    public partial class Dockpane2View : System.Windows.Controls.UserControl
    {
        public Dockpane2View()
        {
            InitializeComponent();
        }
        private List<ParcelModel> parcels;
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
            //primaryNavigator.ItemsSource = panel;
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
            //button. = result.ToString();
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
            Dockpane2ViewModel pane = (Dockpane2ViewModel)FrameworkApplication.DockPaneManager.Find(Dockpane2ViewModel._dockPaneID);
            pane.desactivatePane();
        }
    }
}
