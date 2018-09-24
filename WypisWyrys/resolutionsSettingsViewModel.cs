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
    public class ResolutionsSettingsViewModel : DockPane
    {
        public static string _dockPaneID = "WypisWyrys_resolutionsSettings";

        protected ResolutionsSettingsViewModel() { }

        public static void Show()
        {
            ResolutionsSettingsViewModel pane = (ResolutionsSettingsViewModel)FrameworkApplication.DockPaneManager.Find(_dockPaneID);
            if (pane == null)
                return;

            ResolutionsSettingsView view = ((ResolutionsSettingsView)pane.Content);
            view.fillList(null);
            view.addItemsToList();
            pane.Activate();
        }

        private string _heading = "Zmień ustalenia wydzieleń";
        public string Heading
        {
            get { return _heading; }
            set
            {
                SetProperty(ref _heading, value, () => Heading);
            }
        }
    }

    internal class resolutionsSettings_ShowButton : Button
    {
        protected override void OnClick()
        {
            ResolutionsSettingsViewModel.Show();
        }
    }
}
