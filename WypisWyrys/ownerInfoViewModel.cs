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
    internal class ownerInfoViewModel : DockPane
    {
        public UserModel getUserModel()
        {
            return ((ownerInfoView)this.Content).getUserInfo();
        }
        public static string _dockPaneID = "WypisWyrys_ownerInfo";

        protected ownerInfoViewModel() { }

        public static void Show()
        {
            DockPane pane = FrameworkApplication.DockPaneManager.Find(_dockPaneID);
            if (pane == null)
                return;

            pane.Activate();
        }
        public static void desactivatePane()
        {
            DockPane pane = FrameworkApplication.DockPaneManager.Find(_dockPaneID);
            pane.Hide();
        }
    }
    internal class ownerInfo_ShowButton : Button
    {
        protected override void OnClick()
        {
            ownerInfoViewModel.Show();
        }
    }
}
