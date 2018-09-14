﻿using System;
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
    internal class Dockpane3ViewModel : DockPane
    {
        public static string _dockPaneID = "WypisWyrys_Dockpane3";

        protected Dockpane3ViewModel() { }

        public static void Show()
        {
            DockPane pane = FrameworkApplication.DockPaneManager.Find(_dockPaneID);
            if (pane == null)
                return;
            ((Dockpane3View)((Dockpane3ViewModel)pane).Content).getModels();
            pane.Activate();
        }
        public static void desactivatePane()
        {
            DockPane pane = FrameworkApplication.DockPaneManager.Find(_dockPaneID);
            pane.Hide();
        }
        public List<ResolutionModel> getAcceptedModels()
        {
             return ((Dockpane3View)this.Content).modelsAccepted;
        }
    }

    

    internal class Dockpane3_ShowButton : Button
    {
        protected override void OnClick()
        {
            Dockpane3ViewModel.Show();
        }
    }
}
