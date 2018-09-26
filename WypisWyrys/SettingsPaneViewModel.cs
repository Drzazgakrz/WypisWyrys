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


namespace WypisWyrys
{
    internal class SettingsPaneViewModel : DockPane
    {
        private const string _dockPaneID = "WypisWyrys_SettingsPane";

        protected SettingsPaneViewModel() { }

        /// <summary>
        /// Show the DockPane.
        /// </summary>
        internal static void Show()
        {
            SettingsPaneViewModel pane = (SettingsPaneViewModel)FrameworkApplication.DockPaneManager.Find(_dockPaneID);
            if (pane == null)
                return;
            ((SettingsPaneView)pane.Content).getScale();
            pane.Activate();
        }

        /// <summary>
        /// Text shown near the top of the DockPane.
        /// </summary>
    }

    /// <summary>
    /// Button implementation to show the DockPane.
    /// </summary>
    internal class SettingsPane_ShowButton : Button
    {
        protected override void OnClick()
        {
            SettingsPaneViewModel.Show();
        }
    }
}
