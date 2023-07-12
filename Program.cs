using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using CodeWalker;
using CodeWalker.Core;
using CodeWalker.GameFiles;
using SharpDX;

namespace OcclusionGenerator
{
    class Program
    {
        static Dictionary<uint, LinkableEntityData> linkableEntitiesData = new Dictionary<uint, LinkableEntityData>();
        static void Main(string[] args)
        {
            string[] ytypFiles = Directory.GetFiles("./", "*.ytyp", SearchOption.AllDirectories);
            string[] miloFiles = Directory.GetFiles("./", "*.ymap", SearchOption.AllDirectories);

            if (ytypFiles.Length == 0)
            {
                Console.WriteLine("Ytyp files not found in current folder");
                Console.ReadKey();
                return;
            }

            if (miloFiles.Length == 0)
            {
                Console.WriteLine("Ymap files not found in current folder");
                Console.ReadKey();
                return;
            }

            AudioGameGenerator audioGen = new AudioGameGenerator();

            foreach (string currentYtypFile in ytypFiles)
            {
                string ytypFile = currentYtypFile;
                YmapEntityDef miloEntity = null;

                string[] parts = ytypFile.Split('/', '\\');
                string lastPart = parts[parts.Length - 1];
                int lastIndex = lastPart.LastIndexOf('.');
                string ytypName = lastPart.Substring(0, lastIndex);
                uint ytypHash = JenkHash.GenHash(ytypName);

                bool found = false;
                foreach (string currentYmapFile in miloFiles)
                {
                    YmapFile milo = new YmapFile();
                    byte[] miloFilesBytes = File.ReadAllBytes(currentYmapFile);
                    milo.Load(miloFilesBytes);
                    foreach (YmapEntityDef entityDef in milo.AllEntities)
                    {
                        if (entityDef._CEntityDef.archetypeName.Hash == ytypHash)
                        {
                            miloEntity = entityDef;
                            found = true;
                            break;
                        }
                    }
                    if (found)
                        break;
                }

                if (!found)
                {
                    Console.WriteLine(string.Format("Milo file not found for {0}", ytypFile));
                    continue;
                }


                string ytypPath = ytypFile;

                uint xhash = (uint)(miloEntity.Position.X * 100);
                uint yhash = (uint)(miloEntity.Position.Y * 100);
                uint zhash = (uint)(miloEntity.Position.Z * 100);
                uint hash = miloEntity._CEntityDef.archetypeName.Hash ^ xhash ^ yhash ^ zhash & 0xffffffff;

                byte[] fileBytes = File.ReadAllBytes(ytypPath);

                YtypFile input = new YtypFile();
                input.Load(fileBytes);

                Dictionary<uint, List<uint>> roomPortals = new Dictionary<uint, List<uint>>();

                MloArchetype mlo = null;

                foreach (Archetype archetype in input.AllArchetypes)
                {
                    if (archetype.Type == MetaName.CMloArchetypeDef)
                    {
                        mlo = (MloArchetype)archetype;
                        break;
                    }
                }

                if (mlo == null)
                {
                    Console.WriteLine("MLO not found in ytyp");
                    Console.ReadKey();
                    return;
                }

                if (mlo.rooms == null || mlo.portals == null)
                    continue;

                foreach (MCMloRoomDef room in mlo.rooms)
                {
                    List<uint> portals = new List<uint>();
                    roomPortals.Add((uint)room.Index, portals);
                }

                foreach (MCMloPortalDef portal in mlo.portals)
                {
                    uint roomFrom = portal._Data.roomFrom;
                    uint roomTo = portal._Data.roomTo;
                    roomPortals[roomFrom].Add((uint)portal.Index);
                    roomPortals[roomTo].Add((uint)portal.Index);
                }

                OcclusionPSO pso = new OcclusionPSO();

                foreach (MCMloPortalDef portal in mlo.portals)
                {
                    uint roomFrom = portal._Data.roomFrom;
                    uint roomTo = portal._Data.roomTo;

                    PortalInfo portal1 = new PortalInfo();
                    portal1.DestInteriorHash = (int)hash;
                    portal1.InteriorProxyHash = (int)hash;
                    portal1.RoomIdx = roomFrom;
                    portal1.DestRoomIdx = roomTo;
                    int portalIdxInRoom1 = roomPortals[portal1.RoomIdx].IndexOf((uint)portal.Index);
                    if (portalIdxInRoom1 != -1)
                    {
                        portal1.PortalIdx = (uint)portalIdxInRoom1;
                        if (portal.AttachedObjects != null)
                        {
                            foreach (uint objIdx in portal.AttachedObjects)
                            {
                                MCEntityDef entityDef = mlo.entities[objIdx];
                                PortalEntity portalEntity = new PortalEntity();

                                portalEntity.LinkType = 1;
                                portalEntity.LinkedEntityHash = entityDef.Data.archetypeName.Hash;

                                portalEntity.MaxOcclusion = 0.7f;
                                portalEntity.IsDoor = true;
                                portalEntity.IsGlass = false;

                                portal1.PortalEntityList.Add(portalEntity);
                            }
                        }
                        pso.AddPortalInfo(portal1);
                    }

                    PortalInfo portal2 = new PortalInfo();
                    portal2.DestInteriorHash = (int)hash;
                    portal2.InteriorProxyHash = (int)hash;
                    portal2.RoomIdx = roomTo;
                    portal2.DestRoomIdx = roomFrom;
                    int portalIdxInRoom2 = roomPortals[portal2.RoomIdx].IndexOf((uint)portal.Index);
                    if (portalIdxInRoom2 != -1)
                    {
                        portal2.PortalIdx = (uint)portalIdxInRoom2;
                        if (portal.AttachedObjects != null)
                        {
                            foreach (uint objIdx in portal.AttachedObjects)
                            {
                                MCEntityDef entityDef = mlo.entities[objIdx];
                                PortalEntity portalEntity = new PortalEntity();

                                portalEntity.LinkType = 1;
                                portalEntity.LinkedEntityHash = entityDef.Data.archetypeName.Hash;

                                portalEntity.MaxOcclusion = 0.7f;
                                portalEntity.IsDoor = true;
                                portalEntity.IsGlass = false;

                                portal2.PortalEntityList.Add(portalEntity);
                            }
                        }
                        pso.AddPortalInfo(portal2);
                    }
                }

                HashSet<uint> filteredRooms = new HashSet<uint>();
                for (int i = 0; i < pso.portalInfoList.Count; ++i)
                {
                    PortalInfo portalInfo = pso.portalInfoList[i];

                    uint key = portalInfo.RoomIdx * 100 + portalInfo.DestRoomIdx;
                    if (filteredRooms.Contains(key))
                        continue;

                    filteredRooms.Add(key);

                    for (int k = 0; k < 3; ++k)
                    {
                        PathNode pathNode = new PathNode();
                        uint room1Hash = mlo.rooms[portalInfo.RoomIdx].Name == "limbo" ? JenkHash.GenHash("outside") : (hash ^ JenkHash.GenHash(mlo.rooms[portalInfo.RoomIdx].Name));
                        uint room2Hash = mlo.rooms[portalInfo.DestRoomIdx].Name == "limbo" ? JenkHash.GenHash("outside") : (hash ^ JenkHash.GenHash(mlo.rooms[portalInfo.DestRoomIdx].Name));
                        pathNode.Key = ((int)room1Hash - (int)room2Hash + k + 1);

                        for (int j = 0; j < pso.portalInfoList.Count; ++j)
                        {
                            PortalInfo portalInfo2 = pso.portalInfoList[j];

                            uint key2 = portalInfo2.RoomIdx * 100 + portalInfo2.DestRoomIdx;
                            if (key == key2)
                            {
                                PathNodeChild pathNodeChild = new PathNodeChild();
                                pathNodeChild.PathNodeKey = 0;
                                pathNodeChild.PortalInfoIdx = (uint)j;
                                pathNode.PathNodeChildList.Add(pathNodeChild);
                            }
                        }

                        pso.AddPathNodeInfo(pathNode);
                    }
                }

                {
                    XmlDocument doc = pso.GetXml();
                    var psoData = XmlPso.GetPso(doc);
                    if ((psoData.DataSection == null) || (psoData.DataMapSection == null) || (psoData.SchemaSection == null))
                    {
                        Console.WriteLine("PSO bake error");
                        Console.ReadKey();
                        return;
                    }
                    byte[] data = psoData.Save();
                    File.WriteAllBytes(string.Format("{0}.ymt", hash), data);
                    Console.WriteLine(string.Format("{0}.ymt saved", hash));
                }

                InteriorData intData = new InteriorData();
                intData.mloName = ytypName;
                foreach (var room in mlo.rooms)
                {
                    if (room.Name != "limbo")
                    {
                        intData.AddInteriorRoom(room.Name);
                    }
                }
                Vector3 boxSize = input.AllArchetypes[0].BBMax - input.AllArchetypes[0].BBMin;
                Vector3 bsCenter = mlo.entities[0].Data.position + input.AllArchetypes[0].BSCenter;
                Quaternion angle = new Quaternion(mlo.entities[0].Data.rotation);
                bsCenter += miloEntity.Position;
                intData.SetAmbientZoneData(bsCenter, boxSize);

                audioGen.AddInterior(intData);
            }



            {
                string mixXml = AudioMixGenerator.Generate();
                XmlDocument mixDoc = new XmlDocument();
                mixDoc.LoadXml(mixXml);
                var rel = XmlRel.GetRel(mixDoc);
                if ((rel.RelDatasSorted == null) || (rel.RelDatas == null))
                {
                    Console.WriteLine("Mix rel file bake error");
                    Console.ReadKey();
                    return;
                }
                byte[] data = rel.Save();
                File.WriteAllBytes("custom_mix.dat15.rel", data);
                Console.WriteLine("custom_mix.dat15.rel saved");
            }

            {
                string gameXml = audioGen.GetResult();
                XmlDocument gameDoc = new XmlDocument();
                gameDoc.LoadXml(gameXml);
                var rel = XmlRel.GetRel(gameDoc);
                if ((rel.RelDatasSorted == null) || (rel.RelDatas == null))
                {
                    Console.WriteLine("Mix rel file bake error");
                    Console.ReadKey();
                    return;
                }
                byte[] data = rel.Save();
                File.WriteAllBytes("custom_game.dat151.rel", data);
                Console.WriteLine("custom_game.dat151.rel saved");
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
            return;
        }
    }
}
