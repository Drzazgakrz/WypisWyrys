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
    internal class Step2ViewModel : DockPane
    {
        private const string _dockPaneID = "WypisWyrys_Step2";

        protected Step2ViewModel() { }

        public static void Show()
        {
            Step2ViewModel pane = (Step2ViewModel)FrameworkApplication.DockPaneManager.Find(_dockPaneID);
            if (pane == null)
                return;

            // parcel = pane2.getParcel();    
            pane.fillTextBoxes();
            pane.Activate();
        }
        private static ParcelModel parcel;

        public void fillTextBoxes()
        {
            Dockpane2ViewModel pane2 = (Dockpane2ViewModel)FrameworkApplication.DockPaneManager.
                Find(Dockpane2ViewModel._dockPaneID);
            ((Step2View)this.Content).fillTextViews(pane2.model);
        }
        public static void desactivatePane()
        {
            Step2ViewModel pane = (Step2ViewModel)FrameworkApplication.DockPaneManager.Find(_dockPaneID);
            pane.Hide();
        }
    }

    internal class Step2_ShowButton : Button
    {
        protected override void OnClick()
        {
            Step2ViewModel.Show();
        }
    }
}
