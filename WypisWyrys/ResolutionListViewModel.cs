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
using WypisWyrys.Models;

namespace WypisWyrys
{
    internal class ResolutionListViewModel : DockPane
    {
        public static string _dockPaneID = "WypisWyrys_Dockpane3";

        protected ResolutionListViewModel() { }

        public static void Show(NecessaryProperties properties)
        {
            ResolutionListViewModel pane = (ResolutionListViewModel)FrameworkApplication.DockPaneManager.Find(_dockPaneID);
            if (pane == null)
                return;
            ((ResolutionListView)pane.Content).emptyListView();
            ((ResolutionListView)pane.Content).getModels(properties);            
            pane.Activate();
        }
        public static void desactivatePane()
        {
            DockPane pane = FrameworkApplication.DockPaneManager.Find(_dockPaneID);
            pane.Hide();
        }
        public NecessaryProperties getAcceptedModels()
        {
             return ((ResolutionListView)this.Content).propertiesAccepted;
        }
    }
}
