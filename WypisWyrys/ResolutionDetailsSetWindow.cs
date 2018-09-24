using ArcGIS.Core.Data;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WypisWyrys
{
    public partial class ResolutionDetailsSetWindow : Form
    {
        string selectedName;
        public ResolutionDetailsSetWindow(string selectedName)
        {
            InitializeComponent();
            this.Text = "Edytuj opis wydzielenia";
            this.Width = 550;
            this.Height = 600;
            this.selectedName = selectedName;
            TableLayoutPanel panel = new TableLayoutPanel();
            panel.Width = 550;
            panel.Height = 600;
            panel.ColumnCount = 1;
            panel.RowCount = 2;
            Panel rtfPanel = createTextBox();
            panel.Controls.Add(rtfPanel, 0, 0);            
            Panel buttonPanel = createSaveButton();
            panel.Controls.Add(buttonPanel, 0, 1);
            this.Controls.Add(panel);
        }
        string resolutionDetails;

        public BasicFeatureLayer getLayer()
        {
            var layers = MapView.Active.Map.Layers;
            string layerName = LayersSettingsForm.getConfig("Wydzielenia", "precintLayer");
            return (BasicFeatureLayer)layers.Where((currentLayer) =>
            {
                return currentLayer.Name.Equals(layerName);
            }).First();
        }

        public RowCursor createRow()
        {
            var layer = getLayer();
            Table table = layer.GetTable();
            QueryFilter filter = new QueryFilter();
            string precintSymbolField = LayersSettingsForm.getConfig("Wydzielenia", "precintSymbol");
            filter.WhereClause = precintSymbolField + "=" + this.selectedName;
            return table.Search(filter);
        }

        RichTextBox rtfBox;
        public Panel createTextBox()
        {
            rtfBox = new RichTextBox();
            Task t = QueuedTask.Run(() =>
            {
                var results = createRow();
                results.MoveNext();
                var current = results.Current;
                string precintDetailsField = LayersSettingsForm.getConfig("Wydzielenia", "precintDetails");
                int fieldName = results.FindField(precintDetailsField);
                var value = current.GetOriginalValue(fieldName);
                this.resolutionDetails = value.ToString();
            });
            t.Wait();
            rtfBox.Rtf = resolutionDetails;
            rtfBox.Width = 500;
            rtfBox.Height = 500;
            Panel panel = new Panel();
            panel.Width = 500;
            panel.Height = 500;
            panel.Controls.Add(rtfBox);
            return panel;
        }
        public Panel createSaveButton()
        {
            Button button = new Button();
            button.Text = "Zapisz";
            button.Click += new EventHandler(saveDetails);
            Panel panel = new Panel();
            panel.Controls.Add(button);
            return panel;
        }
        public void saveDetails(object sender, EventArgs args) 
        {
            string rtfValue = this.rtfBox.Rtf;
            ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
            {
                var disLayer = getLayer();
                var oidSet = new List<long>();
                using (var rc = this.createRow())
                {
                    //Create list of oids to update
                    while (rc.MoveNext())
                    {
                        oidSet.Add(rc.Current.GetObjectID());
                    }
                }

                //Create edit operation 
                var op = new ArcGIS.Desktop.Editing.EditOperation();
                op.Name = "Update date";

                // load features into inspector and update field
                var insp = new ArcGIS.Desktop.Editing.Attributes.Inspector();
                insp.Load(disLayer, oidSet);
                insp[LayersSettingsForm.getConfig("Wydzielenia", "precintDetails")] = rtfValue;

                // modify and execute
                op.Modify(insp);
                op.Execute();
            });
        }
    }
}
