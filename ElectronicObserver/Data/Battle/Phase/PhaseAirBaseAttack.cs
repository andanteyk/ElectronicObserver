using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codeplex.Data;

namespace ElectronicObserver.Data.Battle.Phase
{

    /// <summary>
    /// 航空戦フェーズの処理を行います。
    /// </summary>
    public class PhaseAirBaseAttack : PhaseBase
    {
        public class SquadronPlane
        {
            public int Plane { get; set; }
            public int Count { get; set; }

            public SquadronPlane(int plane,int count)
            {
                Plane = plane;
                Count = count;
            }
        }

        public class AirBaseAttack
        {
            public dynamic RawData { get; set; }

            public int Index { get; set; }

            public AirBaseAttack(dynamic PhaseAirBaseAttackData, int index)
            {
                Index = index;
                RawData = PhaseAirBaseAttackData;
                int count = ((DynamicJson)PhaseAirBaseAttackData.api_squadron_plane).GetCount();
                SquadronPlane = new PhaseAirBaseAttack.SquadronPlane[count];
                for (int i = 0; i < count; i++)
                {
                    SquadronPlane[i] = new PhaseAirBaseAttack.SquadronPlane((int)PhaseAirBaseAttackData.api_squadron_plane[i].api_mst_id, (int)PhaseAirBaseAttackData.api_squadron_plane[i].api_count);
                }
            }

            public void EmulateBattle(int[] hps, int[] damages)
            {

                if (!IsStage3Available) return;

                {
                    int[] dmg = Damages;

                    for (int i = 0; i < 6; i++)
                    {
                        AddDamage(hps, i + 6, dmg[i + 6]);
                        damages[i + 6] += dmg[i + 6];
                    }
                }

            }

            /// <summary>
            /// 被ダメージ処理を行います。
            /// </summary>
            /// <param name="hps">各艦のHPリスト。</param>
            /// <param name="index">ダメージを受ける艦のインデックス。</param>
            /// <param name="damage">ダメージ。</param>
            protected void AddDamage(int[] hps, int index, int damage)
            {

                hps[index] -= Math.Max(damage, 0);
            }

            /// <summary>
            /// 各Stageが存在するか
            /// </summary>
            public int[] StageFlag
            {
                get
                {
                    return RawData.IsDefined("api_stage_flag") ? (int[])RawData["api_stage_flag"] : null;
                }
            }

            public int BaseId
            {
                get
                {
                    return (int)RawData.api_base_id;
                }
            }

            public SquadronPlane[] SquadronPlane
            {
                get;
                set;
            }

            //stage 1

            /// <summary>
            /// Stage1(空対空戦闘)が存在するか
            /// </summary>
            public bool IsStage1Available { get { return StageFlag != null && StageFlag[0] != 0 && RawData.api_stage1() && RawData.api_stage1 != null; } }

            /// <summary>
            /// 自軍Stage1参加機数
            /// </summary>
            public int AircraftTotalStage1Friend { get { return (int)RawData.api_stage1.api_f_count; } }

            /// <summary>
            /// 敵軍Stage1参加機数
            /// </summary>
            public int AircraftTotalStage1Enemy { get { return (int)RawData.api_stage1.api_e_count; } }

            /// <summary>
            /// 自軍Stage1撃墜機数
            /// </summary>
            public int AircraftLostStage1Friend { get { return (int)RawData.api_stage1.api_f_lostcount; } }

            /// <summary>
            /// 敵軍Stage1撃墜機数
            /// </summary>
            public int AircraftLostStage1Enemy { get { return (int)RawData.api_stage1.api_e_lostcount; } }

            /// <summary>
            /// 制空権
            /// </summary>
            public int AirSuperiority { get { return (int)RawData.api_stage1.api_disp_seiku; } }

            /// <summary>
            /// 自軍触接機ID
            /// </summary>
            public int TouchAircraftFriend { get { return (int)RawData.api_stage1.api_touch_plane[0]; } }

            /// <summary>
            /// 敵軍触接機ID
            /// </summary>
            public int TouchAircraftEnemy { get { return (int)RawData.api_stage1.api_touch_plane[1]; } }


            //stage 2

            /// <summary>
            /// Stage2(艦対空戦闘)が存在するか
            /// </summary>
            public bool IsStage2Available { get { return StageFlag != null && StageFlag[1] != 0 && RawData.api_stage2() && RawData.api_stage2 != null; } }

            /// <summary>
            /// 自軍Stage2参加機数
            /// </summary>
            public int AircraftTotalStage2Friend { get { return (int)RawData.api_stage2.api_f_count; } }

            /// <summary>
            /// 敵軍Stage2参加機数
            /// </summary>
            public int AircraftTotalStage2Enemy { get { return (int)RawData.api_stage2.api_e_count; } }

            /// <summary>
            /// 自軍Stage2撃墜機数
            /// </summary>
            public int AircraftLostStage2Friend { get { return (int)RawData.api_stage2.api_f_lostcount; } }

            /// <summary>
            /// 敵軍Stage2撃墜機数
            /// </summary>
            public int AircraftLostStage2Enemy { get { return (int)RawData.api_stage2.api_e_lostcount; } }

            //stage 3

            /// <summary>
            /// Stage3(航空攻撃)が存在するか
            /// </summary>
            public bool IsStage3Available { get { return StageFlag != null && StageFlag[2] != 0 && RawData.api_stage3() && RawData.api_stage3 != null; } }

            /// <summary>
            /// 各艦の被ダメージ
            /// </summary>
            public int[] Damages
            {
                get
                {

                    int[] ret = new int[18];

                    int[] enemy = (int[])RawData.api_stage3.api_edam;

                    for (int i = 0; i < 6; i++)
                    {
                        ret[i + 6] = Math.Max(enemy[i + 1], 0);
                    }

                    return ret;
                }

            }

            /// <summary>
            /// 总伤害
            /// </summary>
            public int TotalDamage
            {
                get
                {
                    if (IsStage3Available)
                    {
                        return Damages.Skip(6).Take(6).Sum();
                    }

                    return 0;
                }
            }
        }

        public AirBaseAttack[] AirBaseAttacks;

        public PhaseAirBaseAttack(BattleData data)
            : base(data)
        {
            if (IsAvailable)
            {
                int count = ((DynamicJson)_battleData.RawData.api_air_base_attack).GetCount();
                AirBaseAttacks = new AirBaseAttack[count];
                for (int i = 0; i < count; i++)
                {
                    AirBaseAttacks[i] = new AirBaseAttack(_battleData.RawData.api_air_base_attack[i], i);
                }
            }
        }

        public override void EmulateBattle(int[] hps, int[] damages)
        {
            if (IsAvailable)
            {
                for (int i = 0; i < AirBaseAttacks.Length; i++)
                {
                    AirBaseAttacks[i].EmulateBattle(hps, damages);
                }
            }
        }

        public override bool IsAvailable
        {
            get
            {
                return _battleData.RawData.IsDefined("api_air_base_attack");
            }
        }



    }
}
