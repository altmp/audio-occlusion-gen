﻿using System;
using System.Collections.Generic;
using System.Text;
using CodeWalker.GameFiles;

namespace OcclusionGenerator
{
    class AudioMixGenerator
    {
        public static string Generate()
        {
            string xml = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<Dat15>
 <Version value=""987894568"" />
 <Items>
  <Item type=""Scene"" ntOffset=""0"">
   <Name>custom_audio_scene</Name>
   <Flags value=""0xAAAA0001"" />
   <Unk01 />
   <Items>
    <Item>
     <Patch>custom_audio_patch</Patch>
     <Group />
    </Item>
   </Items>
  </Item>
  <Item type=""Patch"" ntOffset=""0"">
   <Name>custom_audio_patch</Name>
   <Flags value=""0xAAAAAAA1"" />
   <Unk01 value=""500"" />
   <Unk02 value=""500"" />
   <Unk03 value=""0"" />
   <Unk04 value=""0"" />
   <Unk05>hash_0D0E6F19</Unk05>
   <Unk06>hash_E865CDE8</Unk06>
   <Unk07 value=""0"" />
   <Items>
    <Item>
     <Unk01>vehicles_train</Unk01>
     <Unk02 value=""-400"" />
     <Unk03 value=""1"" />
     <Unk04 value=""92"" />
     <Unk05 value=""93"" />
     <Unk06 value=""250"" />
     <Unk07 value=""0"" />
     <Unk08 value=""0"" />
     <Unk09 value=""1"" />
     <Unk10 value=""1"" />
     <Unk11 value=""1"" />
    </Item>
    <Item>
     <Unk01>vehicles_horns_loud</Unk01>
     <Unk02 value=""-300"" />
     <Unk03 value=""1"" />
     <Unk04 value=""92"" />
     <Unk05 value=""93"" />
     <Unk06 value=""0"" />
     <Unk07 value=""0"" />
     <Unk08 value=""0"" />
     <Unk09 value=""1"" />
     <Unk10 value=""1"" />
     <Unk11 value=""0.5"" />
    </Item>
    <Item>
     <Unk01>ambience</Unk01>
     <Unk02 value=""600"" />
     <Unk03 value=""1"" />
     <Unk04 value=""92"" />
     <Unk05 value=""93"" />
     <Unk06 value=""0"" />
     <Unk07 value=""0"" />
     <Unk08 value=""0"" />
     <Unk09 value=""1"" />
     <Unk10 value=""1"" />
     <Unk11 value=""1"" />
    </Item>
    <Item>
     <Unk01>weather</Unk01>
     <Unk02 value=""-10400"" />
     <Unk03 value=""1"" />
     <Unk04 value=""92"" />
     <Unk05 value=""93"" />
     <Unk06 value=""0"" />
     <Unk07 value=""0"" />
     <Unk08 value=""0"" />
     <Unk09 value=""1"" />
     <Unk10 value=""1"" />
     <Unk11 value=""1"" />
    </Item>
   </Items>
  </Item>
 </Items>
</Dat15>";
            return xml;
        }
    }
}
