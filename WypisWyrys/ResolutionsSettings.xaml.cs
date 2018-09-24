using ArcGIS.Core.Data;
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
    public partial class ResolutionsSettingsView : UserControl
    {
        public ResolutionsSettingsView()
        {
            InitializeComponent();
        }
        List<string> symbols = new List<string>();
        RowCursor precints;
        public BasicFeatureLayer getPrecintLayer()
        {
            string layerName = LayersSettingsForm.getConfig("Wydzielenia", "precintLayer");
            return (BasicFeatureLayer)MapView.Active.Map.GetLayersAsFlattenedList().Where((layer) =>
            {
                return layer.Name.Equals(layerName);
            }).First();
        }
        public void fillList(QueryFilter filter)
        {
            symbols = new List<string>();
            string parent = "Wydzielenia";
            var precintLayer = getPrecintLayer();
            Task t = QueuedTask.Run(() =>
            {
                precints = precintLayer.GetTable().Search(filter);
                string symbolName = LayersSettingsForm.getConfig(parent, "precintSymbol");
                while (precints.MoveNext())
                {
                    var fields = precints.Current;                    
                    symbols.Add((string)fields.GetOriginalValue(fields.FindField(symbolName)));
                }
            });
            t.Wait();
            createRow();
        }

        List<string> resolutionsNames = new List<string>();

        public void addItemsToList()
        {
            var precintLayer = getPrecintLayer();
            Task t = QueuedTask.Run(() =>
            {
                precints = precintLayer.GetTable().Search();
                string symbolName = LayersSettingsForm.getConfig("Wydzielenia", "precintResolution");
                while (precints.MoveNext())
                {
                    var fields = precints.Current;
                    string resolution = (string)fields.GetOriginalValue(fields.FindField(symbolName));
                    if (!resolutionsNames.Contains(resolution))
                    {
                        this.resolutionsNames.Add(resolution);
                    }
                    
                }
            });
            t.Wait();
            foreach(string name in resolutionsNames)
            {
                resolutionNames.Items.Add(name);
            }
        }

        public void createRow()
        {
            primaryNavigator.Items.Clear();
            foreach (string symbol in symbols)
            {
                WrapPanel panel = new WrapPanel();
                System.Windows.Controls.TextBox symbolName = new System.Windows.Controls.TextBox();
                symbolName.IsReadOnly = true;
                symbolName.Name = "precintName";
                panel.Children.Add(symbolName);
                symbolName.Text = symbol;
               
                Button button = new Button();
                button.Content = "Edytuj";
                button.Click += showResolutionDetails;
                panel.Children.Add(button);
                primaryNavigator.Items.Add(panel);
            }
        }

        public void filterResultsAndDisplay()
        {
            var resolution = this.resolutionNames.SelectedItem;
            string symbol = @"'%"+this.symbolName.Text.ToUpper()+@"%'";
            string symbolName = LayersSettingsForm.getConfig("Wydzielenia", "precintSymbol");
            string precintResolution = LayersSettingsForm.getConfig("Wydzielenia", "precintResolution");
            if(symbolName == null|| precintResolution == null)
            {
                MessageBox.Show("Stwórz konfigurację dla wydzieleń");
                return;
            }
            QueryFilter filter = new QueryFilter();
            if(resolution == null || resolution.ToString()== null)
            {
                filter.WhereClause = symbolName + " LIKE " + symbol;
            }
            else if (symbolName == null)
            {
                filter.WhereClause = precintResolution + "= '" + resolution.ToString()+"'";
            }else
            {
                filter.WhereClause = precintResolution + "= '" + resolution +"'"+ " AND " + symbolName + " LIKE " + symbol;
            }
            fillList(filter);
        }

        public void showResolutionDetails(object sender, EventArgs e)
        {
            
        }

        public void selectionChanged(object sender, SelectionChangedEventArgs args)
        {
            filterResultsAndDisplay();
        }
        public void textChanged(object sender, TextChangedEventArgs args)
        {
            filterResultsAndDisplay();
        }
    }    

}
