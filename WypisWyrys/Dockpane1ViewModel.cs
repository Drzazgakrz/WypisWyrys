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


namespace WypisWyrys
{
    public class Dockpane1ViewModel : DockPane
    {
        private const string _dockPaneID = "WypisWyrys_Dockpane1";
        private static string name { get; set; }
        protected Dockpane1ViewModel() {
        }

        /// <summary>
        /// Show the DockPane.
        /// </summary>
        internal static void Show()
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

    internal class Dockpane1_ShowButton : Button
    {
        protected override void OnClick()
        {
            DockPane pane = FrameworkApplication.DockPaneManager.Find(BasicInformationViewModel._dockPaneID);
            pane.Hide();
            Dockpane1ViewModel.Show();
        }
    }



    /// <summary>
    /// Button implementation for the button on the menu of the burger button.
    /// </summary>
}