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
        private Config config;
        public const string _dockPaneID = "WypisWyrys_Dockpane2" ;
        public static string getDocpane() {
            return _dockPaneID;
        }
        protected ParcelListViewModel() {
            properties = new NecessaryProperties();
            config = new Config();
        }

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
        public NecessaryProperties properties { get; set; }
        public void getView(NecessaryProperties properties)
        {
            this.properties = properties;
            ((ParcelListView)this.Content).fillTextBox(this.properties.parcels);
        }
        public void desactivatePane()
        {
            if(checkProperties())
            {
                MapClick.isDockpaneActive = false;
                MPZPListViewModel.Show();
                DockPane pane = FrameworkApplication.DockPaneManager.Find(_dockPaneID);
                pane.Hide();
                return;
            }
            ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Wybierz jakąś działkę");
            
        }
        public bool checkProperties()
        {
            try
            {
                if (this.properties.parcels.Count > 0 && this.properties.mpzpModels.Count > 0 
                    && this.properties.resolutions.Count > 0 && this.properties.precints.Count > 0)
                    return true;
            }
            catch (Exception) { }
            return false;


        }
        public void resetPane()
        {
            ((ParcelListView)this.Content).primaryNavigator.Items.Clear();
            this.properties.mpzpModels = new List<MPZPModel>();
            this.properties.parcels = new List<ParcelModel>();
            this.properties.precints = new List<PrecintModel>();
            this.properties.resolutions = new List<ResolutionModel>();

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
