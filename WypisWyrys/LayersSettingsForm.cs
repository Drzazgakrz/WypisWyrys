using ArcGIS.Core.Data;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Xml.Linq;

namespace WypisWyrys
{

    public partial class LayersSettingsForm : Form
    {
        private int currentTab = 0;
        private Config config;
        public LayersSettingsForm()
        {
            InitializeComponent();
            config = new Config();
            this.Text = "Ustawienia warstw";
            getLayers();
        }
        private string[] layersNames;
        private ReadOnlyObservableCollection<Layer> layers;
        private System.Windows.Forms.TabControl control = new System.Windows.Forms.TabControl();
        private void getLayers()
        {
            control.TabPages.Clear();
            layers = MapView.Active.Map.Layers;

            layersNames = new string[layers.Count()];
            int i = 0;
            foreach (Layer layer in layers)
            {
                layersNames[i] = layer.Name;
                i++;
            }
            if (config.getConfig("Działki", "parcelsLayer") == null)
            {
                control.TabPages.Add(createParcels(null));
            }
            if (config.getConfig("MPZP", "MPZPlayer") == null)
            {
                control.TabPages.Add(createMPZP(null));
            }
            if (config.getConfig("Wydzielenia", "precintLayer") == null)
            {
                control.TabPages.Add(createResolution(null));
            }
            if (config.getConfig("Obręby", "areaLayer") == null)
            {
                control.TabPages.Add(createArea(null));
            }
            foreach (Layer layer in layers)
            {

                if (layer.Name.Equals(config.getConfig("Działki", "parcelsLayer")))
                {
                    control.TabPages.Add(createParcels((FeatureLayer)layer));
                }

                if (layer.Name.Equals(config.getConfig("MPZP", "MPZPlayer")))
                {
                    control.TabPages.Add(createMPZP((FeatureLayer)layer));
                }
                if (layer.Name.Equals(config.getConfig("Wydzielenia", "precintLayer")))
                {
                    control.TabPages.Add(createResolution((FeatureLayer)layer));
                }
                if (layer.Name.Equals(config.getConfig("Obręby", "areaLayer")))
                {
                    control.TabPages.Add(createArea((FeatureLayer)layer));
                }
            }
            this.control.SelectedIndex = currentTab;
            control.Width = 300;
            control.Height = 300;
            this.Controls.Add(control);
        }

        private System.Windows.Forms.ComboBox createLayerComboBox(string[] layersNames, string name, string parent)
        {
            System.Windows.Forms.ComboBox list = createRow(layersNames, name, parent);
            list.SelectedValueChanged += getLayerComboBox;
            return list;
        }

        private System.Windows.Forms.ComboBox createComboBox(string[] layersNames, string name, string parent)
        {
            System.Windows.Forms.ComboBox list = createRow(layersNames, name, parent);
            list.SelectedValueChanged += getValue;
            return list;
        }

        private TabPage createParcels(FeatureLayer layer)
        {
            string parent = "Działki";
            TabPage page = new TabPage();
            page.Width = 300;
            TableLayoutPanel panel = new TableLayoutPanel();
            panel.Width = 300;
            panel.Height = 300;
            panel.RowCount = 2;
            panel.ColumnCount = 2;
            string[] columns = prepareData(layer);
            panel.Controls.Add(new System.Windows.Forms.Label { Text = "Wybierz warstwę" }, 0, 0);
            panel.Controls.Add(createLayerComboBox(layersNames, "parcelsLayer", parent), 1, 0);
            panel.Controls.Add(new System.Windows.Forms.Label { Text = "Wybierz identyfikator" }, 0, 1);            
            panel.Controls.Add(createComboBox(columns, "parcelsId", parent), 1, 1);
            page.Controls.Add(panel);
            page.Text = parent;
            return page;
        }
        private TabPage createMPZP(FeatureLayer layer)
        {
            TabPage page = new TabPage();
            page.Width = 300;
            string parent = "MPZP";
            string[] columns = prepareData(layer);
            TableLayoutPanel panel = new TableLayoutPanel();
            panel.Width = 300;
            panel.Height = 300;
            panel.RowCount = 5;
            panel.ColumnCount = 2;
            panel.Controls.Add(new System.Windows.Forms.Label { Text = "MPZP" }, 0, 0);            
            panel.Controls.Add(createLayerComboBox(layersNames, "MPZPlayer", parent), 1, 0);
            panel.Controls.Add(new System.Windows.Forms.Label { Text = "Nazwa" }, 0, 1);
            panel.Controls.Add(createComboBox(columns, "MPZPId", parent), 1, 1);
            panel.Controls.Add(new System.Windows.Forms.Label { Text = "Nr uchwały" }, 0, 2);            
            panel.Controls.Add(createComboBox(columns, "MPZPResolution", parent), 1, 2);
            panel.Controls.Add(new System.Windows.Forms.Label { Text = "Nazwa dziennika" }, 0, 3);            
            panel.Controls.Add(createComboBox(columns, "MPZPDiaryName", parent), 1, 3);
            panel.Controls.Add(new System.Windows.Forms.Label { Text = "Legenda rastrowa" }, 0, 4);            
            panel.Controls.Add(createComboBox(columns, "MPZPLegend", parent), 1, 4);
            page.Controls.Add(panel);
            page.Text = parent;
            return page;
        }

        

        private TabPage createArea(FeatureLayer layer)
        {
            TabPage page = new TabPage();
            string parent = "Obręby";
            string[] columns = prepareData(layer);
            TableLayoutPanel panel = new TableLayoutPanel();
            panel.Width = 300;
            panel.Height = 300;
            panel.RowCount = 3;
            panel.ColumnCount = 2;
            panel.Controls.Add(new System.Windows.Forms.Label { Text = "Obręby" }, 0, 0);            
            panel.Controls.Add(createLayerComboBox(layersNames, "areaLayer", parent), 1, 0);
            panel.Controls.Add(new System.Windows.Forms.Label { Text = "Identyfikator obrębu" }, 0, 1);            
            panel.Controls.Add(createComboBox(columns, "areaId", parent), 1, 1);
            panel.Controls.Add(new System.Windows.Forms.Label { Text = "Nazwa obrębu" }, 0, 2);            
            panel.Controls.Add(createComboBox(columns, "areaName", parent), 1, 2);
            page.Controls.Add(panel);
            page.Text = parent;
            return page;
        }
        private TabPage createResolution(FeatureLayer layer)
        {
            TabPage page = new TabPage();
            string parent = "Wydzielenia";
            string[] columns = prepareData(layer);
            TableLayoutPanel panel = new TableLayoutPanel();
            panel.Width = 300;
            panel.Height = 300;
            panel.RowCount = 5;
            panel.ColumnCount = 2;
            panel.Controls.Add(new System.Windows.Forms.Label { Text = "Wydzielenia" }, 0, 0);            
            panel.Controls.Add(createLayerComboBox(layersNames, "precintLayer", parent), 1, 0);
            panel.Controls.Add(new System.Windows.Forms.Label { Text = "Symbol przeznaczenia" }, 0, 1);            
            panel.Controls.Add(createComboBox(columns, "precintSymbol", parent), 1, 1);
            panel.Controls.Add(new System.Windows.Forms.Label { Text = "Przeznaczenie" }, 0, 2);            
            panel.Controls.Add(createComboBox(columns, "precintDestination", parent), 1, 2);
            panel.Controls.Add(new System.Windows.Forms.Label { Text = "Nr uchwały" }, 0, 3);            
            panel.Controls.Add(createComboBox(columns, "precintResolution", parent), 1, 3);
            panel.Controls.Add(new System.Windows.Forms.Label { Text = "Ustalenia szczegółowe" }, 0, 4);            
            panel.Controls.Add(createComboBox(columns, "precintDetails", parent), 1, 4);
            page.Controls.Add(panel);
            page.Text = parent;
            return page;
        }
        private System.Windows.Forms.ComboBox createRow(string[] source, string cellName, string parent)
        {

            System.Windows.Forms.ComboBox list = new System.Windows.Forms.ComboBox();
            list.Name = cellName;
            if (source != null)
            {
                foreach (string element in source)
                {
                    list.Items.Add(element);
                }
                string selected = config.getConfig(parent, cellName);
                if (selected != null)
                {
                    list.SelectedItem = config.getConfig(parent, cellName);
                }                
                return list;
            }
            return new System.Windows.Forms.ComboBox();

        }
        public string[] prepareData(FeatureLayer data)
        {
            try
            {
                string[] columns = null;
                if (data != null)
                {
                    Task t = QueuedTask.Run(() =>
                    {

                        Table table = data.GetTable();
                        RowCursor cursor = table.Search();
                        cursor.MoveNext();
                        var fields = cursor.GetFields();
                        columns = new string[fields.Count];
                        int i = 0;
                        foreach (Field field in fields)
                        {
                            columns[i] = field.Name;
                            i++;
                        }
                    });
                    t.Wait();
                }
                return columns;
            }
            catch (Exception e)
            {
                return null;
            }

        }
        public void getLayerComboBox(object sender, EventArgs e)
        {
            System.Windows.Forms.ComboBox comboBox = (System.Windows.Forms.ComboBox)sender;
            saveConfig(comboBox);
            getLayers();
        }
        public void saveConfig(System.Windows.Forms.ComboBox comboBox)
        {            
            string currentField = comboBox.SelectedItem.ToString();
            string parent = ((comboBox.Parent).Parent).Text;
            string comboboxName = comboBox.Name;
            currentTab = this.control.SelectedIndex;
            config.setConfig(parent, comboboxName, currentField);
        }
        public void getValue(object sender, EventArgs e)
        {
            System.Windows.Forms.ComboBox comboBox = (System.Windows.Forms.ComboBox)sender;
            saveConfig(comboBox);
        }  
    }
}
