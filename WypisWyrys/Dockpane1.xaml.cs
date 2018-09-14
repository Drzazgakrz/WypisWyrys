using ArcGIS.Desktop.Framework;
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
    public partial class Dockpane1View : UserControl
    {
        public Dockpane1View()
        {            
            InitializeComponent();
        }

        public void Button_Click(object sender, RoutedEventArgs e)
        {
            MapClick.isDockpaneActive = true;
            Dockpane2ViewModel pane = (Dockpane2ViewModel)FrameworkApplication.DockPaneManager.Find(Dockpane2ViewModel._dockPaneID);
            pane.resetPane();
            Dockpane2ViewModel.Show();
            Dockpane1ViewModel.desactivatePane();
        }
    }
}
