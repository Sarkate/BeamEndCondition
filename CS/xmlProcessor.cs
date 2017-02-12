using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace EndRelease
{
    public static class XmlProcessor
    {

        public static void xmlReader(string fileName, out Dictionary<string, string> typeDict)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(fileName);

            string momentType = "";
            string canType = "";

            typeDict = new Dictionary<string, string>();

            XmlNodeList typeNodeList = doc.SelectNodes("/Types");
            foreach (XmlNode typeNode in typeNodeList)
            {
                momentType = typeNode["MOMENT"].InnerText;
                canType = typeNode["CANTILEVER"].InnerText;
            }

            typeDict.Add("MOMENT", momentType);
            typeDict.Add("CANTILEVER", canType);
        }

        public static void xmlWriter(string fileName, Dictionary<string,string> connectionTypeDict)
        {
            XmlDocument xml = new XmlDocument();
           
            XmlWriterSettings setting = new XmlWriterSettings();
            setting.Indent = true;

            using(XmlWriter writer = XmlWriter.Create(fileName,setting))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Types");
                foreach(var v in connectionTypeDict)
                {
                    writer.WriteElementString(v.Key,v.Value);
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }

        }
    }
}
