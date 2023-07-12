using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace OcclusionGenerator
{
    class OcclusionPSO
    {
        public List<PortalInfo> portalInfoList = new List<PortalInfo>();
        public List<PathNode> pathNodeList = new List<PathNode>();

        public XmlDocument GetXml()
        {
            XmlDocument doc = new XmlDocument();
            doc.AppendChild(doc.CreateXmlDeclaration("1.0", "UTF-8", null));

            XmlElement root = (XmlElement)doc.AppendChild(doc.CreateElement("hash_DE5DB4C2"));

            XmlElement portalInfoListElem = doc.CreateElement("PortalInfoList");
            portalInfoListElem.SetAttribute("itemType", "hash_811C03CF");

            foreach (PortalInfo portalInfo in portalInfoList)
            {
                portalInfoListElem.AppendChild(portalInfo.GetXml(doc));
            }

            XmlElement pathNodeListElem = doc.CreateElement("PathNodeList");
            pathNodeListElem.SetAttribute("itemType", "hash_771E3577");

            foreach (PathNode pathNode in pathNodeList)
            {
                pathNodeListElem.AppendChild(pathNode.GetXml(doc));
            }

            root.AppendChild(portalInfoListElem);
            root.AppendChild(pathNodeListElem);
            return doc;
        }

        public void AddPortalInfo(PortalInfo info)
        {
            portalInfoList.Add(info);
        }

        public void AddPathNodeInfo(PathNode info)
        {
            pathNodeList.Add(info);
        }
    }
}
