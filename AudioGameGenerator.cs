using CodeWalker.GameFiles;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Text;

namespace OcclusionGenerator
{
    public class InteriorData
    {
        public string mloName;
        public List<string> rooms = new List<string>();
        public SharpDX.Vector3 center;
        public SharpDX.Vector3 outerSize;
        public SharpDX.Vector3 innerSize;

        public void AddInteriorRoom(string roomName)
        {
            rooms.Add(roomName);
        }
        public void SetAmbientZoneData(Vector3 bsCenter, Vector3 boxSize)
        {
            center = bsCenter;
            innerSize = (boxSize * 3) + 1.0f;
            outerSize = (boxSize * 3) + 2.0f;
        }
    }
    public class AudioGameGenerator
    {
        List<InteriorData> interiors = new List<InteriorData>();

        public void AddInterior(InteriorData interior)
        {
            interiors.Add(interior);
        }

        public string GetResult()
        {
            string result = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><Dat151><Version value=\"999999999\" /><Items>";
            if (interiors.Count > 0)
            {
                result += "<Item type=\"AmbientZoneList\" ntOffset=\"0\"><Name>ambient_zone_list</Name><Zones>";
                foreach (InteriorData interior in interiors)
                {
                    result += string.Format("<Item>{0}_ambient_zone</Item>", interior.mloName);
                }
                result += "</Zones></Item>";

                foreach (InteriorData interior in interiors)
                {
                    result += string.Format("<Item type=\"Interior\" ntOffset=\"0\"><Name>{0}</Name><Unk0 value=\"0xAAAAA044\" /><Unk1 value=\"0xD4855127\" /><Unk2 value=\"0x00000000\" /><Rooms>", interior.mloName);
                    foreach (string roomName in interior.rooms)
                    {
                        result += string.Format("<Item>{0}_{1}_audio_room</Item>", interior.mloName, roomName);
                    }
                    result += "</Rooms></Item>";

                    result += string.Format("<Item type=\"AmbientZone\" ntOffset=\"0\"><Name>{0}_ambient_zone</Name><Flags0 value=\"0xAA800420\" /><Shape>Box</Shape><Flags1 value=\"0x00000000\" /><OuterPos x=\"{1}\" y=\"{2}\" z=\"{3}\" /><OuterSize x=\"{4}\" y=\"{5}\" z=\"{6}\" /><OuterVec1 x=\"0\" y=\"0\" z=\"0\" w=\"0\" /><OuterVec2 x=\"0\" y=\"0\" z=\"0\" w=\"0\" /><OuterAngle value=\"0\" /><OuterVec3 x=\"0\" y=\"0\" z=\"0\" /><InnerPos x=\"{1}\" y=\"{2}\" z=\"{3}\" /><InnerSize x=\"{7}\" y=\"{8}\" z=\"{9}\" /><InnerVec1 x=\"0\" y=\"0\" z=\"0\" w=\"0\" /><InnerVec2 x=\"1\" y=\"1\" z=\"1\" w=\"0\" /><InnerAngle value=\"0\" /><InnerVec3 x=\"0\" y=\"0\" z=\"0\" /><UnkVec1 x=\"0\" y=\"0\" z=\"0\" w=\"0\" /><UnkVec2 x=\"0\" y=\"0\" z=\"0\" w=\"0\" /><UnkHash0 /><UnkHash1>custom_audio_scene</UnkHash1><UnkVec3 x=\"-1\" y=\"0\" /><Flags2 value=\"0x00000000\" /><Unk14 /><Unk15 value=\"1\" /><Unk16 value=\"0\" /><Hashes><Item>hash_4ACAAADD</Item><Item>hash_ED816221</Item></Hashes><ExtParams /></Item>",
                        interior.mloName,
                        interior.center.X.ToString("0.0000", System.Globalization.CultureInfo.InvariantCulture),
                        interior.center.Y.ToString("0.0000", System.Globalization.CultureInfo.InvariantCulture),
                        interior.center.Z.ToString("0.0000", System.Globalization.CultureInfo.InvariantCulture),
                        interior.outerSize.X.ToString("0.0000", System.Globalization.CultureInfo.InvariantCulture),
                        interior.outerSize.Y.ToString("0.0000", System.Globalization.CultureInfo.InvariantCulture),
                        interior.outerSize.Z.ToString("0.0000", System.Globalization.CultureInfo.InvariantCulture),
                        interior.innerSize.X.ToString("0.0000", System.Globalization.CultureInfo.InvariantCulture),
                        interior.innerSize.Y.ToString("0.0000", System.Globalization.CultureInfo.InvariantCulture),
                        interior.innerSize.Z.ToString("0.0000", System.Globalization.CultureInfo.InvariantCulture));

                    foreach (string roomName in interior.rooms)
                    {
                        result += string.Format("<Item type=\"InteriorRoom\" ntOffset=\"0\"><Name>{0}_{1}_audio_room</Name><Flags0 value=\"0xAAAAAAAA\" /><MloRoom>{1}</MloRoom><Hash1>{0}_ambient_zone</Hash1><Unk02 value=\"0\" /><Unk03 value=\"0.5\" /><Unk04 value=\"0\" /><Unk05 value=\"0\" /><Unk06>null_sound</Unk06><Unk07 value=\"0\" /><Unk08 value=\"0\" /><Unk09 value=\"0\" /><Unk10 value=\"0.9\" /><Unk11 value=\"0\" /><Unk12 value=\"20\" /><Unk13 /><Unk14>hash_D4855127</Unk14></Item>",
                            interior.mloName,
                            roomName);
                    }
                }
            }
            result += "</Items></Dat151>";
            return result;
        }
    }
}
