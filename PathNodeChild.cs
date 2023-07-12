using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace OcclusionGenerator
{
    class PathNodeChild
    {
        public uint PathNodeKey { get; set; }
        public uint PortalInfoIdx { get; set; }

        public XmlElement GetXml(XmlDocument doc)
        {
            XmlElement item = doc.CreateElement("Item");

            XmlElement pathNodeKeyElem = doc.CreateElement("PathNodeKey");
            pathNodeKeyElem.SetAttribute("value", PathNodeKey.ToString());

            XmlElement portalInfoIdxElem = doc.CreateElement("PortalInfoIdx");
            portalInfoIdxElem.SetAttribute("value", PortalInfoIdx.ToString());

            item.AppendChild(pathNodeKeyElem);
            item.AppendChild(portalInfoIdxElem);

            return item;
        }
    }
}
