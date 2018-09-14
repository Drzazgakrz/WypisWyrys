﻿using ArcGIS.Core.Data.Raster;
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
            InitializeComponent();
        }
        public BaseInfoModel basicInfos { get; set; }
        public List<ParcelModel> parcels { get; set; }
        public List<PrecintModel> precints { get; set; }
        public MPZPModel mpzp { get; set; } 
        public List<ResolutionModel> resolutions { get; set; }
        public UserModel user { get; set; } = new UserModel();
        public void generateCertificate(object sender, RoutedEventArgs e)
        {
             try
             {
                if (!getLists())
                {
                    return;
                }
                if (mpzp == null || resolutions.Count == 0 || parcels.Count == 0)
                {
                    ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Brak danych na temat działki");
                    return;
                }
                if (user == null)
                {
                    ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Wypełnij wszystkie pola dotyczące petenta");
                    return;
                }
                Stream myStream;
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();

                saveFileDialog1.Title = "Zapisz zaświadczenie";
                saveFileDialog1.Filter = "Rich text document format (*.rtf)|*.rtf";
                saveFileDialog1.FilterIndex = 0;
                saveFileDialog1.RestoreDirectory = true;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    if ((myStream = saveFileDialog1.OpenFile()) != null)
                    {
                        
                        string buffer = File.ReadAllText("certificate.rtf");
                        string editedFile = editFile(buffer);
                        
                        string footer = File.ReadAllText("certificateFooter.rtf");
                        footer = this.createFooter(footer);
                        editedFile = appendFooter(footer, editedFile);
                        if (editedFile!= null)
                        {
                            File.WriteAllText(saveFileDialog1.FileName, editedFile);
                            //myStream.Write(Encoding.ASCII.GetBytes(editedFile), 0, editedFile.Length);
                            myStream.Close();
                        }
                        
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
            parcels = new List<ParcelModel>();
            precints = new List<PrecintModel>();

            Dockpane2ViewModel dockpane = (Dockpane2ViewModel)FrameworkApplication.DockPaneManager.Find(Dockpane2ViewModel._dockPaneID);
            ownerInfoViewModel pane = ((ownerInfoViewModel)FrameworkApplication.DockPaneManager.Find(ownerInfoViewModel._dockPaneID));
            user = pane.getUserModel();
            precints = dockpane.precints;
            parcels = dockpane.parcels;
            mpzp = dockpane.model;
            resolutions = ((Dockpane3ViewModel)FrameworkApplication.DockPaneManager.Find(Dockpane3ViewModel._dockPaneID)).getAcceptedModels();
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
            string resolutionNumberField = LayersSettingsForm.getConfig("Wydzielenia","precintResolution");
            this.resolutions.First().resolution.TryGetValue(resolutionNumberField, out result);
            file = file.Replace("[[data_podania]]", StringUtil.ToRtfString(this.basicInfos.receiveDate.ToShortDateString()));
            file = file.Replace("[[nr_uchwaly]]", StringUtil.ToRtfString(result.ToString()));
            file = file.Replace("[[symbol]]", StringUtil.ToRtfString(this.basicInfos.caseSign));
            file = file.Replace("[[znak_sprawy", StringUtil.ToRtfString(this.basicInfos.caseSign));
            file = file.Replace("[[miejscowosc]]", StringUtil.ToRtfString(this.basicInfos.location));
            file = file.Replace("[[data]]", (basicInfos.date.ToShortDateString()));
            file = file.Replace("[[gmina]]", StringUtil.ToRtfString(this.basicInfos.location));
            string mpzpNameField = LayersSettingsForm.getConfig("MPZP", "MPZPName");
            this.mpzp.mpzp.TryGetValue(mpzpNameField, out result);
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
            while (file.IndexOf("]]") != -1)
            {
                file = file.Remove(file.IndexOf("]]"), 2);
            }
            
            return file;
        }
        public string getParcelsData(string file)
        {
            int index1 = file.IndexOf("[[each obreb]]") + 14;
            int index2 = file.IndexOf("[[endeach");
            string model = file.Substring(index1, index2 - index1);
            string parcelsString = "";
            object result = null;
            foreach (PrecintModel precint in precints)
            {
                string name = LayersSettingsForm.getConfig("Obręby", "areaName");
                precint.precint.TryGetValue(name, out result);
                string precintName = result.ToString();
                string parcelsIdField = LayersSettingsForm.getConfig("Działki", "parcelsId");
                List<ParcelModel> parcelsInPrecint = (parcels.Where((parcel) => {
                    parcel.parcel.TryGetValue(parcelsIdField, out result);
                    return result.ToString().Contains(precintName); })).ToList();
                string parcelsNumbers = "";
                foreach (ParcelModel parcel in parcelsInPrecint)
                {
                    parcel.parcel.TryGetValue(parcelsIdField, out result);
                    parcelsNumbers += result.ToString() + ",";
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
             foreach (ResolutionModel resolution in resolutions)
             {
                 var parcelsInResolution = parcels.Where((parcel) =>
                 {
                        return parcel.resolutions.Contains(resolution);
                 }).ToList();
                if (parcelsInResolution.Count == 0)
                    continue;
                 string symbolName = LayersSettingsForm.getConfig("Wydzielenia", "precintSymbol");
                 resolution.resolution.TryGetValue(symbolName, out result);
                 string resolutionText = model.Replace("[[symbol_przeznaczenia]]", result.ToString());
                 string parcelsNumber = "";
                 foreach (ParcelModel parcel in parcelsInResolution)
                 {
                    string parcelsIdFieldName = LayersSettingsForm.getConfig("Działki", "parcelsId");
                    parcel.parcel.TryGetValue(parcelsIdFieldName, out result);
                    parcelsNumber += StringUtil.ToRtfString(result.ToString()) + " ";
                 }
                string destiantionFiledName = LayersSettingsForm.getConfig("Wydzielenia", "precintDestination");
                resolution.resolution.TryGetValue(destiantionFiledName, out result);
                object detailsResult = null;
                string detailsFiledName = LayersSettingsForm.getConfig("Wydzielenia", "precintDetails");
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
            foreach (ParcelModel parcel in parcels)
            {
                string rowBg = (i % 2 == 0) ? "\\clcbpat2" : "\\clcbpat3";
                string parcelsIdField = LayersSettingsForm.getConfig("Działki", "parcelsId");
                object result = null;
                parcel.parcel.TryGetValue(parcelsIdField, out result);
                var array = result.ToString().Split('.');
                podsumowanieTable += "\\trowd\\trrh360 " +
                rowBg + "\\clvertalc\\cellx4000 " +
                rowBg + "\\clvertalc\\cellx5000 " +
                rowBg + "\\clvertalc\\cellx7000 " +
                result.ToString() + " \\intbl\\cell " +
                array[1].TrimStart(new Char[] { '0' }) + " \\intbl\\cell " +
                array[2] + " \\intbl\\cell " +
                "\\row ";
                i++;
            }
            podsumowanieTable += "}";

            file = file.Replace("[[podsumowanie_tabela]]", podsumowanieTable);
            string legendFieldName = LayersSettingsForm.getConfig("MPZP", "MPZPLegend");
            object legend = null;
            mpzp.mpzp.TryGetValue(legendFieldName, out legend);
            //file = file.Replace("[[legenda_rastrowa]]", ((Raster)legend).ToString());
            //ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show((legend.GetType()).ToString());
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
            }catch(Exception e)
            {
                ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Wypełnij odpowiednie pola dotyczące sprawy");
                return false;
            }            
        }
        public void generateDocument(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!getLists())
                {
                    return;
                }
                if(mpzp == null || resolutions == null || parcels == null || resolutions.Count==0|| parcels.Count==0)
                {
                    ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Brak danych na temat działki");
                    return;
                }
                if (user == null)
                {
                    ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Wypełnij wszystkie pola dotyczące petenta");
                    return;
                }
                this.basicInfos.receiveDate = DateTime.Parse(this.receiveDate.Text);
                if(this.basicInfos.receiveDate > this.basicInfos.date)
                {
                    ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Data otrzymania powinna być mniejsza od daty utworzenia");
                    return;
                }
                if (this.basicInfos.receiveDate > DateTime.Now)
                {
                    ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Data otrzymania powinna być mniejsza od dzisiejszej daty");
                    return;
                }
                if (this.basicInfos.date > DateTime.Now)
                {
                    ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Data utworzenia powinna być mniejsza od dzisiejszej daty");
                    return;
                }
                Stream myStream;
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Title = "Zapisz wypis i wyrys";
                saveFileDialog1.Filter = "Rich text document format (*.rtf)|*.rtf";
                saveFileDialog1.FilterIndex = 0;
                saveFileDialog1.RestoreDirectory = true;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    if ((myStream = saveFileDialog1.OpenFile()) != null)
                    {
                        List<MapPoint> points = new List<MapPoint>();
                        foreach (ParcelModel parcel in parcels)
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
                            Thread t1 = new Thread(() =>
                            {
                                ArcGIS.Core.Geometry.Polygon polygon = PolygonBuilder.CreatePolygon(points);
                                MapView.Active.ZoomTo(polygon);
                            });
                            t1.Start();
                            if (!t1.Join(TimeSpan.FromSeconds(5)))
                            {
                                t1.Abort();

                            }
                        });
                        t.Wait();       
                        string buffer = File.ReadAllText("document.rtf");
                        string editedFile = editDocument(buffer);
                        string footer = File.ReadAllText("documentFooter.rtf");
                        footer = this.createFooter(footer);
                        editedFile = appendFooter(footer, editedFile);

                        if (editedFile!= null && editedFile.Length>0)
                        {
                            System.Windows.Forms.RichTextBox rtf = new System.Windows.Forms.RichTextBox();
                            myStream.Write(Encoding.ASCII.GetBytes(editedFile), 0, editedFile.Length);
                            myStream.Close();
                        }                        
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
            /*catch (Exception)
            {
                ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Wystąpił niezidentyfikowany błąd. Sprawdź czy masz odpowiednią ilość miejsca na dysku i czy oznaczenia zgadzają się z tymi w dokumentacji.");
            }*/
        }
        private string createFooter(string file)
        {
            file = file.Remove(file.IndexOf("{\\rtf1"), 6);
            file = file.Remove(file.LastIndexOf('}'));
            file = file.Replace("[[oplata_kwota]]", this.basicInfos.price.ToString());
            return file.Replace("[[oplata_slownie]]", this.basicInfos.price2);
        }        
        private string appendFooter(string footer, string document)
        {            
            int documentEndIndex = document.IndexOf("{\\rtf1")+6;
            return document.Substring(0, documentEndIndex)+ "{\\footer\\pard " +footer+ " \\par}"+ document.Substring(documentEndIndex, document.Length - documentEndIndex);
            // document.Substring(documentEndIndex)
        }
        private void goBack(object sender, RoutedEventArgs e)
        {
            ownerInfoViewModel.Show();
            BasicInformationViewModel.desactivatePane();
        }
    }
}