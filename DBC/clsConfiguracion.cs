using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace DBC
{
    public static class clsConfiguracion
    {
        public static string ObtenerCN(string pTag)
        {
            string strValorTag = "";
            try
            {
                XmlDocument documento = new XmlDocument();
                try
                {
                    documento.Load(System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\EncryptConn.xml");
                }
                catch (Exception ex)
                {
                    throw new Exception("Error leer archivo EncryptConn.xml", ex);
                }                               
                                
                XmlNodeList listaCN = documento.GetElementsByTagName("configuration");

                XmlNodeList CN = ((XmlElement)listaCN[0]).GetElementsByTagName("connectionStrings");

                foreach (XmlElement cn in listaCN)
                {
                    XmlNodeList nombre = cn.GetElementsByTagName(pTag);
                    strValorTag = nombre[0].InnerText;
                    break;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Error Archivo Configuración: {0}", ex.Message), ex);
            }

            return strValorTag;
        }
    }
}
