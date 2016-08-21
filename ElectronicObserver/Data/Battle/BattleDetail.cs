using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle
{
    /// <summary>
    /// Save the battle detail information
    /// </summary>
    public abstract class BattleDetail
    {
        // Save id rather than ship name here
        protected int attacker;
        protected int defender;
        public int[] damage;
        public CriticalType[] critical;

        /// <summary>
        /// The first id in battle data's id list is always -1
        /// Skip it automatically
        /// so return attacker - 1
        /// The same as defender
        /// </summary>
        public int Attacker { get { return attacker - 1; } set { attacker = value + 1; } }

        /// <summary>
        /// Though there mutiple defenders in API, but they are the same so it
        /// uses one defender
        /// </summary>
        public int Defender { get { return defender - 1; } set { defender = value + 1; } }
        public enum CriticalType
        {
            Miss = 0,
            Hit = 1,
            Critical = 2, 
            Unknown = -1
        }

        public BattleDetail(int attackerId, int defenderId, int[] d, int[] c)
        {
            attacker = attackerId;
            defender = defenderId;
            damage = d;
            List<CriticalType> ct = new List<CriticalType>();
            foreach (int i in c)
            {
                ct.Add((CriticalType)i);
            }
            critical = ct.ToArray();
        }

        /// <summary>
        /// Because BattleDetail doesn't save ship's name
        /// It can't return the final string of the description
        /// So name it BattleDescription rether than ToString()
        /// </summary>
        /// <returns>Description with parameters</returns>
        public virtual string BattleDescription()
        {
            return "Unsupported";
        }
    }

    /// <summary>
    /// Cutin types are different in day battle and night battle
    /// </summary>
    public class BattleDayDetail : BattleDetail
    {
        public enum DayCutinType
        {
            Normal = 0,
            Laser = 1,
            Double = 2,
            Cutin_Main_Sub = 3,
            Cutin_Main_Radar = 4,
            Cutin_Main_Bomb = 5,
            Cutin_Main_Main = 6,

            Torpedo = 10
        }
        public DayCutinType dayCutin;

        public BattleDayDetail(int attackerId, int defenderId, int[] d, int[] c, int cutin)
            : base(attackerId, defenderId, d, c)
        {
            dayCutin = (DayCutinType)cutin;
        }

        public override string BattleDescription()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("{0} → {1}");
            builder.Append("Attack Type : ");
            switch (dayCutin)
            {
                case DayCutinType.Normal: builder.AppendLine("通常"); break;
                case DayCutinType.Laser: builder.AppendLine("レーザー"); break;
                case DayCutinType.Double: builder.AppendLine("連撃"); break;
                case DayCutinType.Cutin_Main_Sub: builder.AppendLine("カットイン(主砲/副砲)"); break;
                case DayCutinType.Cutin_Main_Radar: builder.AppendLine("カットイン(主砲/電探)"); break;
                case DayCutinType.Cutin_Main_Bomb: builder.AppendLine("カットイン(主砲/徹甲)"); break;
                case DayCutinType.Cutin_Main_Main: builder.AppendLine("カットイン(主砲/主砲)"); break;
                case DayCutinType.Torpedo: builder.AppendLine("雷撃"); break;
            }
            builder.Append("Damage:    ");
            for (int i = 0; i < damage.Length; i++)
            {
                if (i > 0) builder.AppendLine("").Append("                 ");
                builder.Append(damage[i]);
                builder.Append("  ");
                switch (critical[i])
                {
                    case CriticalType.Miss: builder.Append("Miss"); break;
                    case CriticalType.Hit: builder.Append("Hit"); break;
                    case CriticalType.Critical: builder.Append("Critical"); break;
                }
            }
            builder.AppendLine("");
            return builder.ToString();

        }
    }

    public class BattleNightDetail : BattleDetail
    {
        public enum NightCutin
        {
            Normal = 0,
            Double = 1,
            Cutin_Main_Tor = 2,
            Cutin_Tor = 3,
            Cutin_Main_Sub = 4,
            Cutin_Main_Main = 5
        }

        public NightCutin nightCutin;

        public BattleNightDetail(int attackerId, int defenderId, int[] d, int[] c, int cutin)
            : base(attackerId, defenderId, d, c)
        {
            nightCutin = (NightCutin)cutin;
        }

        public override string BattleDescription()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("{0} → {1}");
            builder.Append("Attack Type : ");
            switch (nightCutin)
            {
                case NightCutin.Normal: builder.AppendLine("通常"); break;
                case NightCutin.Double: builder.AppendLine("連撃"); break;
                case NightCutin.Cutin_Main_Tor: builder.AppendLine("カットイン(主砲/魚雷)"); break;
                case NightCutin.Cutin_Tor: builder.AppendLine("カットイン(魚雷/魚雷)"); break;
                case NightCutin.Cutin_Main_Sub: builder.AppendLine("カットイン(主砲/副砲)"); break;
                case NightCutin.Cutin_Main_Main: builder.AppendLine("カットイン(主砲/主砲)"); break;
            }
            builder.Append("Damage:    ");
            for (int i = 0; i < damage.Length; i++)
            {
                if (i > 0) builder.AppendLine("").Append("                 ");
                builder.Append(damage[i]);
                builder.Append("  ");
                switch (critical[i])
                {
                    case CriticalType.Miss: builder.Append("Miss"); break;
                    case CriticalType.Hit: builder.Append("Hit"); break;
                    case CriticalType.Critical: builder.Append("Critical"); break;
                }
            }
            return builder.ToString();

        }
    }
}
