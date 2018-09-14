using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WypisWyrys
{
    public partial class DocumentsWindow : Form
    {
        public string doc { get; set; }
        public DocumentsWindow(string name, string document)
        {
            InitializeComponent();
            this.Text = name;
            this.doc = document;
            documentTextBox = new RichTextBox();
            documentTextBox.Rtf = openDocument(doc);
            Label label = new Label();
            label.Width = 500;
            label.Text = "Wklej szablon dokumentu";
            documentTextBox.Width = 500;
            documentTextBox.Height = 400;
            footerTextBox = new RichTextBox();
            footerTextBox.Rtf = openDocument(doc + "Footer");
            footerTextBox.Width = 500;
            footerTextBox.Height = 100;
            Label footerLabel = new Label();
            footerLabel.Text = "Wklej stopkę dokumentu";
            footerLabel.Width = 500;
            TableLayoutPanel panel = new TableLayoutPanel();
            panel.Width = 600;
            panel.Height = 600;
            panel.ColumnCount = 1;
            panel.RowCount = 4;
            panel.Controls.Add(label, 0, 0);
            panel.Controls.Add(documentTextBox, 0, 1);
            panel.Controls.Add(footerLabel, 0, 2);
            panel.Controls.Add(footerTextBox, 0, 3);
            this.Controls.Add(panel);
            this.Width = 600;
            this.Height = 600;
        }

        public RichTextBox documentTextBox { get; set; }
        public RichTextBox footerTextBox { get; set; }
        private string openDocument(string document)
        {
            try
            {
                return File.ReadAllText(document + ".rtf");
            }catch (Exception e)
            {
                return "";
            }
            

        }
    }
}
