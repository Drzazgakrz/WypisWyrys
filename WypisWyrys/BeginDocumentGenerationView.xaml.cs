using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace WypisWyrys
{
    /// <summary>
    /// Interaction logic for Dockpane1View.xaml
    /// </summary>
    public partial class BeginDocumentGenerationView : UserControl
    {
        private MapInteraction interaction;
        public BeginDocumentGenerationView()
        {
            interaction = new MapInteraction();            
            InitializeComponent();
        }

        public void Button_Click(object sender, RoutedEventArgs e)
        {
            MapClick.isDockpaneActive = true;
            ParcelListViewModel pane = (ParcelListViewModel)FrameworkApplication.DockPaneManager.Find(ParcelListViewModel._dockPaneID);
            pane.resetPane();
            if (!interaction.clearSelection())
            {
                return;
            }
            ParcelListViewModel.Show();
            BeginDocumentGenerationViewModel.desactivatePane();
        }
        
    }    
}
