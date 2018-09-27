using ArcGIS.Core.Data.Raster;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using PlanowaniePrzestrzenneExtension.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
using WypisWyrys.Models;
using WypisWyrys.Settings;

namespace WypisWyrys
{
    public partial class BasicInformationView : System.Windows.Controls.UserControl
    {
        public BasicInformationView()
        {
            this.config = new Config();
            InitializeComponent();
        }
        public void initializeComponents()
        {
            date.Text = (DateTime.Now).Date.ToShortDateString();
            receiveDate.Text = (DateTime.Now).Date.ToShortDateString();
        }
        private Config config;
        public BaseInfoModel basicInfos { get; set; }
        public NecessaryProperties acceptedProperties;
        public UserModel user { get; set; } = new UserModel();
        public void generateCertificate(object sender, RoutedEventArgs e)
        {
             try
             {
                if (!checkData())
                    return;
                Stream myStream;
                    if ((myStream = showWindow("Zapisz zaświadczenie", "Rich text document format (*.rtf)|*.rtf")) != null)
                    {
                        
                        string buffer = File.ReadAllText("certificate.rtf");
                        string editedFile = editFile(buffer);
                        
                        string footer = File.ReadAllText("certificateFooter.rtf");
                        footer = this.createFooter(footer);
                        editedFile = appendFooter(footer, editedFile);
                        if (editedFile!= null)
                        {
                            myStream.Write(Encoding.ASCII.GetBytes(editedFile), 0, editedFile.Length);
                            myStream.Close();
                            goBackToMPZP();
                        }
                        
                    }                
            }catch(FileNotFoundException)
            {
                ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Wystąpił błąd. Brakuje pliku z wzorem zaświadczenia. Wygeneruj taki zgodnie z dokumentacją i podaj ścieżkę do niego w ustawieniach.");
            }
            catch (PathTooLongException)
            {
                ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Zbyt długa ścieżka. Popraw ją i spróbuj ponownie.");
            }
            catch (Exception)
            {
                ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Wystąpił niezidentyfikowany błąd. Sprawdź czy masz odpowiednią ilość miejsca na dysku i czy oznaczenia zgadzają się z tymi w dokumentacji.");
            }
        }
        public bool getLists()
        {
            ResolutionListViewModel dockpane = (ResolutionListViewModel)FrameworkApplication.DockPaneManager.Find(ResolutionListViewModel._dockPaneID);
            OwnerInfoViewModel pane = ((OwnerInfoViewModel)FrameworkApplication.DockPaneManager.Find(OwnerInfoViewModel._dockPaneID));
            user = pane.getUserModel();
            this.acceptedProperties = dockpane.getAcceptedModels();
            return collectData();
        }
        public string editFile(string file)
        {
                file = editHeader(file);
                file = getParcelsData(file);
                file = getResolutionData(file);
                file = createAssume(file);
                file = removeStrings(file);
                return file;
        }
        private string editHeader(string file)
        {
            object result = null;
            string resolutionNumberField = config.getConfig("MPZP","MPZPResolution");
            this.acceptedProperties.mpzpModels.First().mpzp.TryGetValue(resolutionNumberField, out result);
            file = file.Replace("[[data_podania]]", StringUtil.ToRtfString(this.basicInfos.receiveDate.ToShortDateString()));
            file = file.Replace("[[nr_uchwaly]]", StringUtil.ToRtfString(result.ToString()));
            file = file.Replace("[[symbol]]", StringUtil.ToRtfString(this.basicInfos.caseSign));
            file = file.Replace("[[znak_sprawy", StringUtil.ToRtfString(this.basicInfos.caseSign));
            file = file.Replace("[[miejscowosc]]", StringUtil.ToRtfString(this.basicInfos.location));
            file = file.Replace("[[data]]", (basicInfos.date.ToShortDateString()));
            file = file.Replace("[[gmina]]", StringUtil.ToRtfString(this.basicInfos.location));
            string mpzpNameField = config.getConfig("MPZP", "MPZPName");
            this.acceptedProperties.mpzpModels.First().mpzp.TryGetValue(mpzpNameField, out result);
            file = file.Replace("[[nazwa_mpzp]]", StringUtil.ToRtfString(result.ToString()));
            return file;
        }
        private string removeStrings(string file)
        {
            while(file.IndexOf("ejscowosc]]") != -1)
                file = file.Remove(file.IndexOf("ejscowosc]]"), 11);
            while(file.IndexOf("obreb") != -1)
                file = file.Remove(file.IndexOf("obreb"), 5);
            while(file.IndexOf("symbol_przeznaczenia") != -1)
                file = file.Remove(file.IndexOf("symbol_przeznaczenia"), 20);
            while(file.IndexOf("stalenia_szczegolowe")!=-1)
                file = file.Remove(file.IndexOf("stalenia_szczegolowe"), 20);
            while(file.IndexOf("[[endeach")!=-1)
                file = file.Remove(file.IndexOf("[[endeach"), 9);
            while (file.IndexOf("par rch") != -1)
                file = file.Remove(file.IndexOf("par rch")+4, 3);
            
            while (file.IndexOf("[[legenda_rastrowa") != -1)
            {
                file = file.Remove(file.IndexOf("[[legenda_rastrowa]]"),20);
            }
            while (file.IndexOf("]]") != -1)
            {
                file = file.Remove(file.IndexOf("]]"), 2);
            }

            int place = file.LastIndexOf("phpar0");

            if (place == -1)
                return file;

            file = file.Remove(place, 6).Insert(place, "\\phpar0");
            return file;
        }
        public void getDistinctPrecints()
        {
            foreach(ParcelModel parcel in acceptedProperties.parcels)
            {
                foreach(PrecintModel precint in parcel.precints)
                {
                    if (!acceptedProperties.precints.Contains(precint))
                    {
                        acceptedProperties.precints.Add(precint);
                    }
                }
            }
        }


        public void getDistinctResolutions()
        {
            foreach (ParcelModel parcel in acceptedProperties.parcels)
            {
                foreach (ResolutionModel resolution in parcel.resolutions)
                {
                    if (!acceptedProperties.resolutions.Contains(resolution))
                    {
                        acceptedProperties.resolutions.Add(resolution);
                    }
                }
            }
        }
        public string getParcelsData(string file)
        {
            int index1 = file.IndexOf("[[each obreb]]") + 14;
            int index2 = file.IndexOf("[[endeach");
            string model = file.Substring(index1, index2 - index1);
            string parcelsString = "";
            object result = null;
            getDistinctPrecints();
            foreach (PrecintModel precint in acceptedProperties.precints)
            {
                string name = config.getConfig("Obręby", "areaName");
                precint.precint.TryGetValue(name, out result);
                string precintName = result.ToString();
                string parcelsIdField = config.getConfig("Działki", "parcelsId");
                List<ParcelModel> parcelsInPrecint = (acceptedProperties.parcels.Where((parcel) => {
                    parcel.parcel.TryGetValue(parcelsIdField, out result);
                    return result.ToString().Contains(precintName); })).ToList();
                string parcelsNumbers = "";
                foreach (ParcelModel parcel in parcelsInPrecint)
                {
                    parcel.parcel.TryGetValue(parcelsIdField, out result);
                    string parcelId = result.ToString();
                    int index = parcelId.LastIndexOf('.');
                    parcelsNumbers += parcelId.Substring(index, parcelId.Length - index) + ",";
                }

                parcelsString += model.Replace("[[nry_dzialek]],", StringUtil.ToRtfString(parcelsNumbers)).
                    Replace("[[obreb]]", StringUtil.ToRtfString(precintName));
            }
            model = file.Substring(index1 - 14, index2 - index1 + 17 + 14);
            return file.Replace(model, parcelsString);
        }
        public string getResolutionData(string file)
        {
             string assume = "";
             int index1 = file.IndexOf("[[each symbol_przeznaczenia]]") + 29;
             int index2 = file.IndexOf("[[endeach");
             string model = file.Substring(index1, index2 - index1);
             string assumeSymbol = "";
             object result;
            getDistinctResolutions();
             foreach (ResolutionModel resolution in acceptedProperties.resolutions)
             {
                 var parcelsInResolution = acceptedProperties.parcels.Where((parcel) =>
                 {
                        return parcel.resolutions.Contains(resolution);
                 }).ToList();
                if (parcelsInResolution.Count == 0)
                    continue;
                 string symbolName = config.getConfig("Wydzielenia", "precintSymbol");
                 resolution.resolution.TryGetValue(symbolName, out result);
                 string resolutionText = model.Replace("[[symbol_przeznaczenia]]", result.ToString());
                 string parcelsNumber = "";
                 foreach (ParcelModel parcel in parcelsInResolution)
                 {
                    string parcelsIdFieldName = config.getConfig("Działki", "parcelsId");
                    parcel.parcel.TryGetValue(parcelsIdFieldName, out result);
                    string parcelId = result.ToString();
                    int index = parcelId.LastIndexOf('.')+1;
                    parcelsNumber += parcelId.Substring(index, parcelId.Length - index) + ",";
                }
                string destiantionFiledName = config.getConfig("Wydzielenia", "precintDestination");
                resolution.resolution.TryGetValue(destiantionFiledName, out result);
                object detailsResult = null;
                string detailsFiledName = config.getConfig("Wydzielenia", "precintDetails");
                resolution.resolution.TryGetValue(detailsFiledName, out detailsResult);
                assume += resolutionText.Replace("[[przeznaczenie]]", StringUtil.ToRtfString(result.ToString())).
                    Replace("[[nry_dzialek_w_symbolu]]", parcelsNumber).
                    Replace("[[u", detailsResult.ToString());
                resolution.resolution.TryGetValue("nazwa",out result);
                if(!assumeSymbol.Contains(result.ToString()))
                    assumeSymbol += result.ToString();
             }
             model = file.Substring(index1 - 29, index2 - index1 + 29 + 32);
             file = file.Replace(model, assume);
             return file.Replace("[[symbole_przeznaczenia]]", StringUtil.ToRtfString(assumeSymbol));
        }
        private string createAssume(string file)
        {
            file = file.Replace("[[wnioskodawca_imie]]", StringUtil.ToRtfString(user.firstName));
            file = file.Replace("[[wnioskodawca_nazwisko]]", StringUtil.ToRtfString(user.lastName));
            file = file.Replace("[[wnioskodawca_adres]]", user.zipCode + " " + StringUtil.ToRtfString(user.location) + 
                ", ul. " + StringUtil.ToRtfString(user.streetName));
            return file.Replace("[[mi", StringUtil.ToRtfString(this.location.Text));
        }
        private string convertMapToString(string file)
        {
            
            Task task = QueuedTask.Run(() =>
            {                
                PNGFormat png = new PNGFormat();
                png.Height = 700;
                png.Width = 700;
                png.OutputFileName = System.IO.Path.GetTempPath() + "\\map.png";
                MapView.Active.Export(png);
            });
            task.Wait();
            System.Drawing.Image img = System.Drawing.Image.FromFile(System.IO.Path.GetTempPath() + "\\map.png");
            MemoryStream stream = new MemoryStream();
            img.Save(stream, img.RawFormat);
            byte[] bytes = stream.ToArray();
            string str = BitConverter.ToString(bytes, 0).Replace("-", string.Empty);
            string rtfImageStr = "{\\pict\\pngblip\\picw2947\\pich2579\\picwgoal9070\\pichgoal7937 " + str + "}";
            
            int scale = (int)MapView.Active.Camera.Scale;
            file = file.Replace("[[skala]]", "1:" + scale);
            return file.Replace("[[mapa]]", rtfImageStr);
        }
        public string editDocument(string file)
        {
            
            file = editHeader(file);
            file = getParcelsData(file);
            file = getResolutionData(file);
            file = createAssume(file);
                           
            string HEADER_BG = "\\red212\\green234\\blue255";
            string BG_LIGHT_GRAY = "\\red232\\green232\\blue232";
            string BG_LIGHTER_GRAY = "\\red240\\green240\\blue240";
            string paperSizeProps = "\\paperw11909\\paperh16834";
            string colorTableString = "{\\colortbl;" + HEADER_BG + ";" + BG_LIGHT_GRAY + ";" + BG_LIGHTER_GRAY + "}";
            string marginsProps = "\\margl1400\\margr1400\\margt1400\\margb1400";
            string podsumowanieTable = "{";
            podsumowanieTable += "\\trowd\\trrh400 " +
                    "\\clvertalc\\clcbpat1\\trbrdrb\\cellx4000 " +
                    "\\clvertalc\\clcbpat1\\trbrdrb\\cellx5000 " +
                    "\\clvertalc\\clcbpat1\\trbrdrb\\cellx7000 " +
                    "\\b " + StringUtil.ToRtfString("Nazwa obrębu") + " \\b0\\intbl\\cell " +
                    "\\b " + StringUtil.ToRtfString("Nr obrębu") + " \\b0\\intbl\\cell " +
                    "\\b " + StringUtil.ToRtfString("Nr działki") + " \\b0\\intbl\\cell " +
                    "\\row";
            var i = 0;
            foreach (ParcelModel parcel in acceptedProperties.parcels)
            {
                string rowBg = (i % 2 == 0) ? "\\clcbpat2" : "\\clcbpat3";
                string parcelsIdField = config.getConfig("Działki", "parcelsId");
                string areaNameField = config.getConfig("Obręby", "areaName");
                object areaName = null;
                var area = parcel.precints.Where((precint) =>
                {
                    object resultShape = null;
                    precint.precint.TryGetValue("Shape", out resultShape);
                    ArcGIS.Core.Geometry.Polygon polygon = (ArcGIS.Core.Geometry.Polygon)resultShape;
                    parcel.parcel.TryGetValue("Shape", out resultShape);
                    ArcGIS.Core.Geometry.Polygon parcelPolygon = (ArcGIS.Core.Geometry.Polygon)resultShape;
                    return !GeometryEngine.Instance.Intersection(polygon, parcelPolygon).IsEmpty;
                }).First();
                area.precint.TryGetValue(areaNameField, out areaName);          
                object result = null;
                parcel.parcel.TryGetValue(parcelsIdField, out result);
                var array = result.ToString().Split('.');
                podsumowanieTable += "\\trowd\\trrh360 " +
                rowBg + "\\clvertalc\\cellx4000 " +
                rowBg + "\\clvertalc\\cellx5000 " +
                rowBg + "\\clvertalc\\cellx7000 " +
                StringUtil.ToRtfString(areaName.ToString()) + " \\intbl\\cell " +
                array[1].TrimStart(new Char[] { '0' }) + " \\intbl\\cell " +
                array[2] + " \\intbl\\cell " +
                "\\row ";
                i++;
            }
            podsumowanieTable += "}";

            file = file.Replace("[[podsumowanie_tabela]]", podsumowanieTable);

            string legendFieldName = config.getConfig("MPZP", "MPZPLegend");
            MPZPModel mpzp = acceptedProperties.mpzpModels.First();
            Object legend = null;
            mpzp.mpzp.TryGetValue("legend", out legend);
            if (legend != null) {
                Object legendSize = null;
                mpzp.mpzp.TryGetValue("legendSize", out legendSize);

                string str = BitConverter.ToString((byte[])legend, 0).Replace("-", string.Empty);

                string rtfImageStr = @"{\pict\pngblip\picw" +
        ((System.Drawing.Size)legendSize).Width + @"\pich" + (((System.Drawing.Size)legendSize).Height * 10) +
        @"\picwgoal" + (((System.Drawing.Size)legendSize).Width * 10) + @"\pichgoal" + (((System.Drawing.Size)legendSize).Height * 10) +
        str + "}";
            
            while(file.IndexOf("[[legenda_rastrowa]]")!= file.LastIndexOf("[[legenda_rastrowa]]")){
                file = file.Remove(file.LastIndexOf("[[legenda_rastrowa]]"), 20);
            }
            
            file = file.Replace("[[legenda_rastrowa]]", rtfImageStr);
            }
            file = convertMapToString(file);
            file = removeStrings(file);
            return file;            
        }
        public bool collectData()
        {
            try
            {
                string caseSign = this.caseSign.Text;
                string location = this.location.Text;
                string community = this.community.Text;
                DateTime date = DateTime.Parse(this.date.Text);            
                double price = Convert.ToDouble(this.price.Text);
                string price2 = this.price2.Text;
                this.basicInfos = new BaseInfoModel(caseSign, location,community, date,null, price, price2);
                return true;
            }catch(Exception)
            {
                ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Wypełnij odpowiednie pola dotyczące sprawy");
                return false;
            }            
        }

        public Stream showWindow(string title, string filter)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Title = title;
            saveFileDialog1.Filter = filter;
            saveFileDialog1.FilterIndex = 0;
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                return saveFileDialog1.OpenFile();
            }
            return null;
        }
        public bool checkData()
        {
            if (!getLists())
            {
                return false;
            }
            if (acceptedProperties.mpzpModels == null || acceptedProperties.resolutions == null || acceptedProperties.parcels == null || acceptedProperties.parcels.Count == 0)
            {
                ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Brak danych na temat działki");
                return false;
            }
            if (user == null)
            {
                ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Wypełnij wszystkie pola dotyczące petenta");
                return false;
            }           
            if (this.basicInfos.date > DateTime.Now)
            {
                ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Data utworzenia powinna być mniejsza od dzisiejszej daty");
                return false;
            }
            return true;
        }
        public void generateDocument(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!checkData())
                    return;
                this.basicInfos.receiveDate = DateTime.Parse(this.receiveDate.Text);
                if (this.basicInfos.receiveDate > this.basicInfos.date)
                {
                    ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Data otrzymania powinna być mniejsza od daty utworzenia");
                    return;
                }
                if (this.basicInfos.receiveDate > DateTime.Now)
                {
                    ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Data otrzymania powinna być mniejsza od dzisiejszej daty");
                    return;
                }
                Stream myStream;
                if ((myStream = showWindow("Zapisz wypis i wyrys", "Rich text document format (*.rtf)|*.rtf")) != null)
                {
                    List<MapPoint> points = new List<MapPoint>();
                    foreach (ParcelModel parcel in acceptedProperties.parcels)
                    {
                        object result = null;
                        parcel.parcel.TryGetValue("Shape", out result);
                        foreach (MapPoint point in ((ArcGIS.Core.Geometry.Polygon)result).Points)
                        {
                            points.Add(point);
                        }
                    }
                    Task t = QueuedTask.Run(() =>
                    {
                        string scale;
                        
                        if ((scale = config.getConfig("scale", null)) != null)
                        {
                            Camera camera = MapView.Active.Camera;
                            camera.Scale = Convert.ToDouble(scale);
                            ArcGIS.Core.Geometry.Polygon polygon = PolygonBuilder.CreatePolygon(points);
                            MapPoint center = polygon.Extent.Center;
                            camera.X = center.X;
                            camera.Y = center.Y;
                            MapView.Active.ZoomTo(camera);
                        }
                        else
                        {
                            MapView.Active.ZoomToSelected();
                        }              
                    });
                    t.Wait();

                    string buffer = File.ReadAllText("document.rtf");
                    string editedFile = editDocument(buffer);
                    string footer = File.ReadAllText("documentFooter.rtf");
                    footer = this.createFooter(footer);
                    editedFile = appendFooter(footer+"\\par{", editedFile);

                    if (editedFile != null && editedFile.Length > 0)
                    {
                        System.Windows.Forms.RichTextBox rtf = new System.Windows.Forms.RichTextBox();
                        myStream.Write(Encoding.ASCII.GetBytes(editedFile), 0, editedFile.Length);
                        myStream.Close();
                        goBackToMPZP();
                    }
                }
            }
            catch (FileNotFoundException)
            {
                ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Wystąpił błąd. Brakuje pliku z wzorem dokumentu. Wygeneruj taki zgodnie z dokumentacją i podaj ścieżkę do niego w ustawieniach.");
            }
            catch (PathTooLongException)
            {
                ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Zbyt długa ścieżka. Popraw ją i spróbuj ponownie.");
            }
            catch (Exception)
            {
                ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Wystąpił niezidentyfikowany błąd. Sprawdź czy masz odpowiednią ilość miejsca na dysku i czy oznaczenia zgadzają się z tymi w dokumentacji.");
            }
        }
        private string createFooter(string file)
        {
            file = file.Replace("[[oplata_kwota]]", this.basicInfos.price.ToString());
            return file.Replace("[[oplata_slownie]]", this.basicInfos.price2);
        }        
        private string appendFooter(string footer, string document)
        {            
            int documentEndIndex = document.IndexOf("{\\rtf1")+6;
            return document.Substring(0, documentEndIndex)+ "{\\footer\\pard " +footer + document.Substring(documentEndIndex, document.Length - documentEndIndex);
        }
        private void goBack(object sender, RoutedEventArgs e)
        {
            OwnerInfoViewModel.Show();
            BasicInformationViewModel.desactivatePane();
        }

        private void goBackToMPZP()
        {
            BasicInformationViewModel pane = (BasicInformationViewModel)FrameworkApplication.DockPaneManager.Find(BasicInformationViewModel._dockPaneID);
            pane.Hide();
            MPZPListViewModel mpzpPane = (MPZPListViewModel)FrameworkApplication.DockPaneManager.Find(MPZPListViewModel._dockPaneID);
            mpzpPane.Activate();
        }
    }
}
