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
using WypisWyrys.Models;

namespace WypisWyrys
{
    public partial class MPZPListView : UserControl
    {
        public MPZPListView()
        {
            InitializeComponent();
        }
        public void fillTextViews(MPZPModel model)
        {
            try
            {
                object result = null;
                model.mpzp.TryGetValue(LayersSettingsForm.getConfig("MPZP", "MPZPId"), out result);
                parcelName.Text = result.ToString();
                model.mpzp.TryGetValue(LayersSettingsForm.getConfig("MPZP", "MPZPResolution"), out result);
                resolutionNumber.Text = result.ToString();
            }catch(Exception e)
            {
                MessageBox.Show("Wybierz działkę");
            }
        }
        public void nextPane(object sender, RoutedEventArgs e)
        {
            ResolutionListViewModel.Show();
            MPZPListViewModel.desactivatePane();
        }
        public void goBack(object sender, RoutedEventArgs e)
        {
            ParcelListViewModel.Show();
            MPZPListViewModel.desactivatePane();
        }
    }
}
