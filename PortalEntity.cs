using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace OcclusionGenerator
{
    class PortalEntity
    {
        public int LinkType { get; set; }
        public float MaxOcclusion { get; set; }
        public uint LinkedEntityHash { get; set; }
        public bool IsDoor { get; set; }
        public bool IsGlass { get; set; }

        public XmlElement GetXml(XmlDocument doc)
        {
            XmlElement item = doc.CreateElement("Item");

            XmlElement linkTypeElem = doc.CreateElement("LinkType");
            linkTypeElem.SetAttribute("value", LinkType.ToString());

            XmlElement maxOcclusionElem = doc.CreateElement("MaxOcclusion");
            maxOcclusionElem.SetAttribute("value", MaxOcclusion.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture));

            XmlElement linkedEntityHashElem = doc.CreateElement("hash_E3674005");
            linkedEntityHashElem.SetAttribute("value", ((int)LinkedEntityHash).ToString());

            XmlElement isDoorElem = doc.CreateElement("IsDoor");
            isDoorElem.SetAttribute("value", IsDoor.ToString().ToLower());

            XmlElement isGlassElem = doc.CreateElement("IsGlass");
            isGlassElem.SetAttribute("value", IsGlass.ToString().ToLower());

            item.AppendChild(linkTypeElem);
            item.AppendChild(maxOcclusionElem);
            item.AppendChild(linkedEntityHashElem);
            item.AppendChild(isDoorElem);
            item.AppendChild(isGlassElem);

            return item;
        }
    }
}
