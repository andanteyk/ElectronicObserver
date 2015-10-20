using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanProtector
{
    public class ProtectionVersionManager
    {
        public static ProtectionVersionManager Instance;
        static Dictionary<int, ProtectionVersion> VersionList;

        static ProtectionVersionManager()
        {
            Instance = new ProtectionVersionManager();
            AddVersions();
        }

        ProtectionVersionManager()
        {
            VersionList = new Dictionary<int, ProtectionVersion>();
        }

        public ProtectionVersion GetLatestVersion()
        {
            int ver = VersionList.Max(v => v.Key);
            return VersionList[ver];
        }

        public ProtectionVersion GetVersion(int i)
        {
            return VersionList.ContainsKey(i) ? VersionList[i] : null;
        }

        static void AddVersions()
        {
            ProtectionVersion ver = Version0();
            VersionList.Add(ver.VersionID, ver);
        }

        static ProtectionVersion Version0()
        {
            ProtectionVersion version = new ProtectionVersion();
            version.VersionID = 0;
            version.MaxIndentifiedEquipmentID = 150;

            version.IdentifiedShips = new int[]{ 1,2,6,7,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,57,58,59,60,61,62,63,64,65,66,67,68,69,70,71,72,73
                        ,74,75,76,77,78,79,80,81,82,83,84,85,86,87,88,89,90,91,92,93,94,95,96,97,98,99,100,101,102,103,104,105,106,107,108,109,110,111,112,113,114,115,116,117,118,119,120,121,122,123,124,125,126,127,128,129,130,131,132
                        ,133,134,135,136,137,138,139,140,141,142,143,144,145,146,147,148,149,150,151,152,153,154,155,156,157,158,159,160,161,163,164,165,166,167,168,169,170,171,172,173,174,175,176,177,178,179,180,181,182,183,184,185
                        ,186,187,188,189,190,191,192,193,194,195,196,197,200,201,202,203,204,205,206,207,208,209,210,211,212,213,214,215,216,217,218,219,220,221,222,223,224,225,226,227,228,229,230,231,232,233,234,235,236,237,238,239
                        ,240,241,242,243,244,245,246,247,248,249,250,251,252,253,254,255,256,257,258,259,260,261,262,263,264,265,266,267,268,269,270,271,272,273,274,275,276,277,278,279,280,281,282,283,284,285,286,287,288,289,290,291
                        ,292,293,294,295,296,297,300,301,302,303,304,305,306,307,308,309,310,311,312,313,314,316,317,318,319,320,321,322,323,324,325,326,327,328,329,330,331,332,334,343,344,345,346,347,348,349,350,351,352,398,399,400
                        ,401,402,403,404,405,406,407,408,409,410,411,412,413,414,415,416,417,419,420,421,422,424,425,426,427,428,429,430,431,434,435,436,437,441,442,443,445,446,447,450,451,453,458,459,460,461,466};

            version.IdentifiedEquipments = new int[] {8,9,22,31,36,42,43,45,47,50,52,53,56,58,62,63,67,73,76,79,80,81
                                      ,82,83,85,86,87,88,89,90,93,94,95,99,100,102,103,105,106,107,108
                                      ,109,110,111,112,113,114,116,117,118,120,121,122,123,124,126,127
                                      ,128,129,130,131,132,134,135,136,137,139,140,141,142,143,144,147
                                      , 148,149,150};
            return version;
        }
    }

    public class ProtectionVersion
    {
        public int VersionID;
        public int MaxIndentifiedEquipmentID;
        public int[] IdentifiedShips;
        public int[] IdentifiedEquipments;
    }
}
