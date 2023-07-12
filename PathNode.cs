using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace OcclusionGenerator
{
    class PathNode
    {
        public int Key { get; set; }
        public List<PathNodeChild> PathNodeChildList = new List<PathNodeChild>();

        public XmlElement GetXml(XmlDocument doc)
        {
            XmlElement item = doc.CreateElement("Item");

            XmlElement keyElem = doc.CreateElement("Key");
            keyElem.SetAttribute("value", Key.ToString());

            XmlElement pathNodeChildListElem = doc.CreateElement("PathNodeChildList");
            pathNodeChildListElem.SetAttribute("itemType", "hash_892CF74F");

            foreach (PathNodeChild pathNodeChild in PathNodeChildList)
            {
                pathNodeChildListElem.AppendChild(pathNodeChild.GetXml(doc));
            }

            item.AppendChild(keyElem);
            item.AppendChild(pathNodeChildListElem);

            return item;
        }
    }
}
