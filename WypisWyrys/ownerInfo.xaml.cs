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
    public partial class OwnerInfoView : UserControl
    {
        public OwnerInfoView()
        {
            InitializeComponent();
        }
        public void getBasicInfo(object sender, RoutedEventArgs e)
        {
            BasicInformationViewModel.Show();
            OwnerInfoViewModel.desactivatePane();
        }
        public void goBack(object sender, RoutedEventArgs e)
        {
            MPZPListViewModel.Show();
            OwnerInfoViewModel.desactivatePane();
        }

        public UserModel getUserInfo()
        {
            string name = this.firstName.Text;
            string lastName = this.lastName.Text;
            string zipCode = this.zipCode.Text;
            string streetName = this.streetName.Text;
            string cityName = this.localization.Text;
            if (name == "" || lastName == "" || zipCode == "" || streetName == "" || cityName == "")
                return null;
            return new UserModel(name, lastName, cityName, zipCode, streetName);
        }
    }
}
