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
    internal class MPZPListViewModel : DockPane
    {
        public static string _dockPaneID = "WypisWyrys_Step2";

        protected MPZPListViewModel() { }

        public NecessaryProperties getAcceptedProperties()
        {
            return ((MPZPListView)this.Content).acceptedProperties;
        }

        public static void Show()
        {
            MPZPListViewModel pane = (MPZPListViewModel)FrameworkApplication.DockPaneManager.Find(_dockPaneID);
            if (pane == null)
                return;
            ((MPZPListView)pane.Content).clearList();
            pane.fillTextBoxes();
            pane.Activate();
        }
        private static ParcelModel parcel;

        public void fillTextBoxes()
        {
            ParcelListViewModel pane2 = (ParcelListViewModel)FrameworkApplication.DockPaneManager.
                Find(ParcelListViewModel._dockPaneID);
            ((MPZPListView)this.Content).fillTextViews(pane2.properties);
        }
        public static void desactivatePane()
        {
            MPZPListViewModel pane = (MPZPListViewModel)FrameworkApplication.DockPaneManager.Find(_dockPaneID);
            pane.Hide();
        }
    }

    internal class Step2_ShowButton : Button
    {
        protected override void OnClick()
        {
            MPZPListViewModel.Show();
        }
    }
}
