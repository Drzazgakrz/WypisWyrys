using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Linq;

namespace WypisWyrys
{
    /// <summary>
    /// Interaction logic for SettingsPaneView.xaml
    /// </summary>
    
    public partial class SettingsPaneView : System.Windows.Controls.UserControl
    {
        private ReadOnlyObservableCollection<Layer> layers;
        public SettingsPaneView()
        {
            InitializeComponent();
            System.Windows.Forms.TabControl tab = new System.Windows.Forms.TabControl();
            Task task = QueuedTask.Run(() =>
            {
                this.layers = MapView.Active.Map.Layers;
                foreach(Layer layer in layers)
                {
                    TabPage page = new TabPage();
                    tab.TabPages.Add(page);
                }
            });
            task.Wait();
            //dockPanel1.Add
        }
        public void setLayersSettings(object sender, RoutedEventArgs e)
        {
            LayersSettingsForm form = new LayersSettingsForm();
            form.Show();
        }
        public void setCertificatePath(object sender, RoutedEventArgs e)
        {
            DocumentsWindow window = new DocumentsWindow("Zapisz szablon zaświadczenia", "certificate");
            window.FormClosed += new FormClosedEventHandler(this.Form1_Closing);
            window.Show();
        }
        private void Form1_Closing(object sender, FormClosedEventArgs e)
        {
            DocumentsWindow form = (DocumentsWindow)sender;
            FileStream stream = File.Open(form.doc + ".rtf", FileMode.OpenOrCreate);
            stream.Write(Encoding.ASCII.GetBytes(form.documentTextBox.Rtf), 0, form.documentTextBox.Rtf.Length);
            stream.Close();
            FileStream footerStream = File.Open(form.doc + "Footer.rtf", FileMode.OpenOrCreate);
            footerStream.Write(Encoding.ASCII.GetBytes(form.footerTextBox.Rtf), 0, form.footerTextBox.Rtf.Length);
            footerStream.Close();
        }
        int iterator;
        public void setDocumentPath(object sender, RoutedEventArgs e)
        {
            DocumentsWindow window = new DocumentsWindow("Zapisz szablon wyrysu i wypisu", "document");
            window.FormClosed += new FormClosedEventHandler(this.Form1_Closing);
            window.Show();
        }
        public static string findPath(string tagName)
        {
            try
            {
                XDocument document = XDocument.Load("config.xml");
                return document.Root.Element("root").Element("paths").Element(tagName).Value.ToString();
            }
            catch(Exception e)
            {
                ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Brak konfiguracji dla tego dokumentu utwórz nowy. dodaj plik do konfiguracji");
                return null;
            }
            
        }

        private void changeResolutionsSettings(object sender, RoutedEventArgs args)
        {
            ResolutionsSettingsViewModel.Show();
        }

        public void saveScale(object sender, RoutedEventArgs args)
        {
            try
            {
                int scaleValue = Convert.ToInt32(scale.Text);
                string node = "scale";
                XDocument document = XDocument.Load("config.xml");
                if (document.Root.Element(node) == null)
                {                    
                    document.Root.Add(new XElement(node, scaleValue));
                }                   
                else
                    document.Root.Element(node).ReplaceWith(new XElement(node, scaleValue));
                document.Save("config.xml");
            }
            catch (System.IO.FileNotFoundException)
            {
                LayersSettingsForm.saveEmptyConfigFile();
            }
        }
      
    }
}
