using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElectronicObserver.Data;

namespace ElectronicObserver.Utility
{
    public static class KanVoice
    {
        static int[] voiceKey = new int[] { 604825, 607300, 613847, 615318, 624009, 631856, 635451, 637218, 640529, 643036, 652687, 658008, 662481, 669598, 675545, 685034, 687703, 696444, 702593, 703894, 711191, 714166, 720579, 728970, 738675, 740918, 743009, 747240, 750347, 759846, 764051, 770064, 773457, 779858, 786843, 790526, 799973, 803260, 808441, 816028, 825381, 827516, 832463, 837868, 843091, 852548, 858315, 867580, 875771, 879698, 882759, 885564, 888837, 896168 };
        static string[] voiceName = new string[] { "获得", "母港1", "母港2", "母港3", "建成提示", "修理结束", "回港", "查看战绩", "换装", "强化", "小破入渠", "中破入渠", "编成", "出击", "开战", "攻击", "攻击2", "夜战突入", "小破", "小破2", "中破", "击沉", "MVP", "结婚", "图鉴", "远征/物资", "补给", "回港2", "放置" };
        static List<string> voiceList;
        public delegate string GetVoiceTextEvent(int shipID, int voiceID);
        public static event GetVoiceTextEvent OnGetVoiceText;

        static KanVoice()
        {
            voiceList = new List<string>();
            voiceList.AddRange(voiceName);
            for (int i = 0; i < 24; i++)
            {
                voiceList.Add(i.ToString() + "点报时");
            }
        }

        public static string GetVoiceText(int shipID, int voiceID)
        {
            if (OnGetVoiceText != null)
                return OnGetVoiceText(shipID, voiceID);
            else
                return null;
        }

        public static int GetVoiceCount()
        {
            return voiceKey.Length;
        }

        public static string GetVoiceName(int VoiceId)
        {
            return voiceList[VoiceId - 1];
        }
        public static int ConvertFilename(int ShipId, int VoiceId)
        {
            return (ShipId + 7) * 17 * (voiceKey[VoiceId] - voiceKey[VoiceId - 1]) % 99173 + 100000;
        }

        public static string GetVoicePath(int ShipId, int VoiceId)
        {
            return string.Format("kc{0}\\{1}.mp3", KCDatabase.Instance.MasterShips[ShipId].ResourceName, ConvertFilename(ShipId, VoiceId));
        }
    }
}
