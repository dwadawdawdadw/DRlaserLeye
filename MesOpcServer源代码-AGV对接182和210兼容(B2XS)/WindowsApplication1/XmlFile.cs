using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Net;
using System.IO;
using System.Xml;

namespace WindowsApplication1
{
    class XmlFile
    {
        private XElement m_xe = null;
    //    private string strFilePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase+"XmlParam";
    //    private string strFileName = "XmlParam.xml";
        private string strXmlFilepath = "";
        public XmlFile()
        {
      //      CreateXml("");
        }
        public bool OpenXml(string strFilePath)
        {
            strXmlFilepath = strFilePath;

            if (!File.Exists(strXmlFilepath))
            {
                return false;           
            }
            m_xe = XElement.Load(strXmlFilepath);
           
            return true;
        }

        public void CreateXml(string strFilePath)
        {
            string[] strTemp = strFilePath.Split(new char[]{'\\'});  //Join(string separator, string[] value, int startIndex, int count);

            strXmlFilepath = String.Join("\\", strTemp, 0, strTemp.Length - 1);

            if (!Directory.Exists(strXmlFilepath))
            {
                Directory.CreateDirectory(strXmlFilepath);
            }

            if (!File.Exists(strFilePath))
            {
                XmlDocument temDoc = new XmlDocument();
                //创建Xml文件
                XmlDeclaration declaration = temDoc.CreateXmlDeclaration("1.0", "utf-8", "yes");
                temDoc.AppendChild(declaration);
                //创建一个根节点　
                XmlElement xmlelem = temDoc.CreateElement("", "Param", "");
                temDoc.AppendChild(xmlelem);
                //保存文件
                temDoc.Save(strFilePath);
            }
            m_xe = XElement.Load(strFilePath);
        }

        public void ReadValue(string strName,ref string strValue)
        {

            IEnumerable<XElement> elements = from ele in m_xe.Elements(strName)
                                            select ele;
            if(elements.Count()<=0)
            {
                XElement temXE = new XElement(
                    new XElement(strName, "")
                    );
                m_xe.Add(temXE);
                m_xe.Save(strXmlFilepath);

                strValue = "";
            }
            else
            {
                XElement xt = elements.First();
                strValue = xt.Value.ToString();
            }
        }
        public  void ReadValue(string strName,ref double dValue)
        {

            IEnumerable<XElement> elements = from ele in m_xe.Elements(strName)
                                             select ele;
            if (elements.Count() <= 0)
            {
                XElement temXE = new XElement(
                    new XElement(strName, "")
                    );
                m_xe.Add(temXE);
                m_xe.Save(strXmlFilepath);

                dValue = 0;
            }
            else
            {
                XElement xt = elements.First();
                dValue = Convert.ToDouble(xt.Value.ToString());
            }
        }

        public void ReadValue(string strName, ref int nValue)
        {

            IEnumerable<XElement> elements = from ele in m_xe.Elements(strName)
                                             select ele;
            if (elements.Count() <= 0)
            {
                XElement temXE = new XElement(
                    new XElement(strName, "")
                    );
                m_xe.Add(temXE);
                m_xe.Save(strXmlFilepath);

                nValue = 0;
            }
            else
            {
                XElement xt = elements.First();
                nValue = Convert.ToInt32(xt.Value.ToString());
            }
        }

        public void ReadValue(string strName, ref uint nValue)
        {

            IEnumerable<XElement> elements = from ele in m_xe.Elements(strName)
                                             select ele;
            if (elements.Count() <= 0)
            {
                XElement temXE = new XElement(
                    new XElement(strName, "")
                    );
                m_xe.Add(temXE);
                m_xe.Save(strXmlFilepath);

                nValue = 0;
            }
            else
            {
                XElement xt = elements.First();
                nValue = Convert.ToUInt32(xt.Value.ToString());
            }
        }

        public void WriteValue(string strName,string strValue)
        {

            IEnumerable<XElement> element = from ele in m_xe.Elements(strName)
                                            select ele;
            if (element.Count() > 0)
            {
                XElement temXE = element.First();
                temXE.ReplaceWith
                    (
                        new XElement(strName,strValue)
                    );
                m_xe.Save(strXmlFilepath);
            }
            else
            {
                XElement temXE = new XElement(
                     new XElement(strName, strValue)
                    );
                m_xe.Add(temXE);
                m_xe.Save(strXmlFilepath);
            }
        }



    }
}
