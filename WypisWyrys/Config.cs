using ArcGIS.Desktop.Framework.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WypisWyrys
{
    public class Config
    {
        public void setConfig(string parent, string node, string value)
        {
            try
            {
                XDocument document = XDocument.Load("config.xml");
                if (document.Root.Element(parent).Element(node) == null)
                {
                    if (document.Root.Element(parent) == null)
                        document.Root.Add(new XElement(parent, ""));
                    document.Root.Element(parent).Add(new XElement(node, value));
                }else if(node == null)
                {
                    document.Root.Element(parent).ReplaceWith(new XElement(parent, value));
                }
                else
                    document.Root.Element(parent).Element(node).ReplaceWith(new XElement(node, value));
                document.Save("config.xml");
            }
            catch (System.IO.FileNotFoundException e)
            {
                if (saveEmptyConfigFile())
                {
                    setConfig(parent, node, value);
                }
                else
                {
                    MessageBox.Show("Nie udało się zapisać konfiguracji.");
                }
                
            }
        }

        public bool saveEmptyConfigFile()
        {
            try
            {
                new XDocument(
                        new XElement("root",
                            new XElement("MPZP", ""),
                            new XElement("Działki", ""),
                            new XElement("Wydzielenia", ""),
                            new XElement("Obręby", ""),
                            new XElement("paths", ""),
                            new XElement("scale", "")
                        )
                    ).Save("config.xml");
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public string getConfig(string parent, string field)
        {
            try
            {
                XDocument document = XDocument.Load("config.xml");
                if (field != null)
                    return document.Root.Element(parent).Element(field).Value;
                else
                    return document.Root.Element(parent).Value;
            }
            catch (Exception e)
            {
                return null;
            }

        }

    }
}
