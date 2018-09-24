using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
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
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using WypisWyrys.Models;

namespace WypisWyrys
{
    public partial class ResolutionsWindow : Form
    {
        public List<ResolutionModel> models { get; set; }
        public ResolutionsWindow(List<ResolutionModel> models)
        {
            this.models = models;
            InitializeComponent();
            CreateForm();
        }

        private void CreateForm()
        {
            this.Controls.Clear();
            System.Windows.Forms.DataGridView grid = new System.Windows.Forms.DataGridView();
            grid.Dock = DockStyle.Fill;
            grid.ColumnCount = 3;
            grid.Columns[0].Name = "symbol działki";
            List<DataGridViewRow> rows = new List<DataGridViewRow>();
            for (int i = 0;i<models.Count;i++)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.Cells.Add(new DataGridViewTextBoxCell());
                var button = new ButtonCell(this);
                button.Value = "Usuń";
                ResolutionModel model = models.ElementAt(i);
                string name = LayersSettingsForm.getConfig("Wydzielenia", "precintSymbol");
                object result = null;
                model.resolution.TryGetValue(name, out result);
                row.Cells[0].Value = result.ToString();
                row.Cells[0].ReadOnly = true;
                row.Cells.Add(button);
                ZoomButton zoom = new ZoomButton(this);
                zoom.Value = "Zbliż";
                row.Cells.Add(zoom);
                grid.Rows.Add(row);
            }
            grid.Size = new Size(300, 300);
            grid.Visible = true;
            this.Controls.Add(grid);
        }
        private class ButtonCell: DataGridViewButtonCell
        {
            ResolutionsWindow outer;
            public ButtonCell(ResolutionsWindow outer)
            {
                this.outer = outer;
            }
            protected override void OnClick(DataGridViewCellEventArgs e)
            {
                outer.models.RemoveAt(e.RowIndex);
                outer.CreateForm();
            }
        }
        private class ZoomButton : DataGridViewButtonCell
        {
            ResolutionsWindow outer;
            public ZoomButton(ResolutionsWindow outer)
            {
                this.outer = outer;
            }
            protected override void OnClick(DataGridViewCellEventArgs e)
            {
                ResolutionModel model = outer.models.ElementAt(e.RowIndex);
                object shape = null;
                model.resolution.TryGetValue("Shape", out shape);
                Task t = QueuedTask.Run(() =>
                {
                    MapView.Active.ZoomTo((Polygon)shape);
                });
                t.Wait();
            }
        }
    }
}
