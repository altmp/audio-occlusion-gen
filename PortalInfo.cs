using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace OcclusionGenerator
{
    class PortalInfo
    {
        public int InteriorProxyHash { get; set; }
        public uint PortalIdx { get; set; }
        public uint RoomIdx { get; set; }
        public int DestInteriorHash { get; set; }
        public uint DestRoomIdx { get; set; }
        public List<PortalEntity> PortalEntityList = new List<PortalEntity>();

        public XmlElement GetXml(XmlDocument doc)
        {
            XmlElement item = doc.CreateElement("Item");

            XmlElement interiorProxyHashElem = doc.CreateElement("InteriorProxyHash");
            interiorProxyHashElem.SetAttribute("value", InteriorProxyHash.ToString());

            XmlElement portalIdxElem = doc.CreateElement("PortalIdx");
            portalIdxElem.SetAttribute("value", PortalIdx.ToString());

            XmlElement roomIdxElem = doc.CreateElement("RoomIdx");
            roomIdxElem.SetAttribute("value", RoomIdx.ToString());

            XmlElement destInteriorHashElem = doc.CreateElement("DestInteriorHash");
            destInteriorHashElem.SetAttribute("value", DestInteriorHash.ToString());

            XmlElement destRoomIdxElem = doc.CreateElement("DestRoomIdx");
            destRoomIdxElem.SetAttribute("value", DestRoomIdx.ToString());

            XmlElement portalEntityListElem = doc.CreateElement("PortalEntityList");
            portalEntityListElem.SetAttribute("itemType", "hash_F6624EF9");

            foreach (PortalEntity portalEntity in PortalEntityList)
            {
                portalEntityListElem.AppendChild(portalEntity.GetXml(doc));
            }

            item.AppendChild(interiorProxyHashElem);
            item.AppendChild(portalIdxElem);
            item.AppendChild(roomIdxElem);
            item.AppendChild(destInteriorHashElem);
            item.AppendChild(destRoomIdxElem);
            item.AppendChild(portalEntityListElem);

            return item;
        }
    }
}
