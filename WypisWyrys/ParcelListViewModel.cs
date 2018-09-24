using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArcGIS.Core.CIM;
using ArcGIS.Core.Data;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Catalog;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Editing;
using ArcGIS.Desktop.Extensions;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework.Dialogs;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using System.Windows.Forms;
using WypisWyrys.Models;

namespace WypisWyrys
{
    public class ParcelListViewModel : DockPane
    {
        public const string _dockPaneID = "WypisWyrys_Dockpane2" ;
        public static string getDocpane() {
            return _dockPaneID;
        }
        protected ParcelListViewModel() { }

        /// <summary>
        /// Show the DockPane.
        /// </summary>
        public static void Show()
        {
            ParcelListViewModel pane = (ParcelListViewModel)FrameworkApplication.DockPaneManager.Find(_dockPaneID);
            if (pane == null)
                return;            
            pane.Activate();
        }
        private static System.Collections.Generic.IReadOnlyList<ArcGIS.Desktop.Mapping.Layer> layers;
        public static MapView map;
        public List<ParcelModel> parcels { get; set; }


        public MPZPModel model { get; set; }
        public List<ResolutionModel> resolution { get; set; }
        public List<PrecintModel> precints { get; set; }
        public void getView()
        {
            this.model = MapClick.mpzpModel ;
            parcels = MapClick.parcel;
            this.resolution = MapClick.resolutionModel;
            this.precints = MapClick.precints;
            //string id = LayersSettingsForm.getConfig("Działki", "parcelsId");
            ((ParcelListView)this.Content).fillTextBox(parcels);
        }
        public void desactivatePane()
        {
            if(parcels != null)
            {
                MapClick.isDockpaneActive = false;
                MPZPListViewModel.Show();
                DockPane pane = FrameworkApplication.DockPaneManager.Find(_dockPaneID);
                pane.Hide();
                return;
            }
            ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Wybierz jakąś działkę");
            
        }
        public void resetPane()
        {
            ((ParcelListView)this.Content).primaryNavigator.Items.Clear();
            this.model = null;
            this.parcels = new List<ParcelModel>();
            this.precints = new List<PrecintModel>();
            this.resolution = new List<ResolutionModel>();

        }
    }
    
    /// <summary>
    /// Button implementation to show the DockPane.
    /// </summary>
    internal class Dockpane2_ShowButton : ArcGIS.Desktop.Framework.Contracts.Button
    {
        protected override void OnClick()
        {            
            ParcelListViewModel.Show();
        }
        
    }
    
}
