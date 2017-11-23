using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle
{
	public struct BattleIndex
	{
		public readonly BattleSides Side;
		public readonly int Index;


		public static readonly BattleIndex Invalid = new BattleIndex(BattleSides.FriendMain, -1);

		public static readonly BattleIndex FriendMain1 = new BattleIndex(BattleSides.FriendMain, 0);
		public static readonly BattleIndex FriendMain2 = new BattleIndex(BattleSides.FriendMain, 1);
		public static readonly BattleIndex FriendMain3 = new BattleIndex(BattleSides.FriendMain, 2);
		public static readonly BattleIndex FriendMain4 = new BattleIndex(BattleSides.FriendMain, 3);
		public static readonly BattleIndex FriendMain5 = new BattleIndex(BattleSides.FriendMain, 4);
		public static readonly BattleIndex FriendMain6 = new BattleIndex(BattleSides.FriendMain, 5);
		public static readonly BattleIndex FriendMain7 = new BattleIndex(BattleSides.FriendMain, 6);

		public static readonly BattleIndex FriendEscort1 = new BattleIndex(BattleSides.FriendEscort, 0);
		public static readonly BattleIndex FriendEscort2 = new BattleIndex(BattleSides.FriendEscort, 1);
		public static readonly BattleIndex FriendEscort3 = new BattleIndex(BattleSides.FriendEscort, 2);
		public static readonly BattleIndex FriendEscort4 = new BattleIndex(BattleSides.FriendEscort, 3);
		public static readonly BattleIndex FriendEscort5 = new BattleIndex(BattleSides.FriendEscort, 4);
		public static readonly BattleIndex FriendEscort6 = new BattleIndex(BattleSides.FriendEscort, 5);

		public static readonly BattleIndex EnemyMain1 = new BattleIndex(BattleSides.EnemyMain, 0);
		public static readonly BattleIndex EnemyMain2 = new BattleIndex(BattleSides.EnemyMain, 1);
		public static readonly BattleIndex EnemyMain3 = new BattleIndex(BattleSides.EnemyMain, 2);
		public static readonly BattleIndex EnemyMain4 = new BattleIndex(BattleSides.EnemyMain, 3);
		public static readonly BattleIndex EnemyMain5 = new BattleIndex(BattleSides.EnemyMain, 4);
		public static readonly BattleIndex EnemyMain6 = new BattleIndex(BattleSides.EnemyMain, 5);
		public static readonly BattleIndex EnemyMain7 = new BattleIndex(BattleSides.EnemyMain, 6);

		public static readonly BattleIndex EnemyEscort1 = new BattleIndex(BattleSides.EnemyEscort, 0);
		public static readonly BattleIndex EnemyEscort2 = new BattleIndex(BattleSides.EnemyEscort, 1);
		public static readonly BattleIndex EnemyEscort3 = new BattleIndex(BattleSides.EnemyEscort, 2);
		public static readonly BattleIndex EnemyEscort4 = new BattleIndex(BattleSides.EnemyEscort, 3);
		public static readonly BattleIndex EnemyEscort5 = new BattleIndex(BattleSides.EnemyEscort, 4);
		public static readonly BattleIndex EnemyEscort6 = new BattleIndex(BattleSides.EnemyEscort, 5);


		public static readonly ReadOnlyCollection<BattleIndex> Friend
			= new ReadOnlyCollection<BattleIndex>(new BattleIndex[] {
				FriendMain1,
				FriendMain2,
				FriendMain3,
				FriendMain4,
				FriendMain5,
				FriendMain6,
				FriendMain7,
			});

		public static readonly ReadOnlyCollection<BattleIndex> FriendMain
			= new ReadOnlyCollection<BattleIndex>(new BattleIndex[] {
				FriendMain1,
				FriendMain2,
				FriendMain3,
				FriendMain4,
				FriendMain5,
				FriendMain6,
			});

		public static readonly ReadOnlyCollection<BattleIndex> FriendEscort
			= new ReadOnlyCollection<BattleIndex>(new BattleIndex[] {
				FriendEscort1,
				FriendEscort2,
				FriendEscort3,
				FriendEscort4,
				FriendEscort5,
				FriendEscort6,
			});

		public static readonly ReadOnlyCollection<BattleIndex> FriendAll
			= new ReadOnlyCollection<BattleIndex>(new BattleIndex[] {
				FriendMain1,
				FriendMain2,
				FriendMain3,
				FriendMain4,
				FriendMain5,
				FriendMain6,
				FriendEscort1,
				FriendEscort2,
				FriendEscort3,
				FriendEscort4,
				FriendEscort5,
				FriendEscort6,
			});


		public static readonly ReadOnlyCollection<BattleIndex> Enemy
			= new ReadOnlyCollection<BattleIndex>(new BattleIndex[] {
				EnemyMain1,
				EnemyMain2,
				EnemyMain3,
				EnemyMain4,
				EnemyMain5,
				EnemyMain6,
				EnemyMain7,
			});

		public static readonly ReadOnlyCollection<BattleIndex> EnemyMain
			= new ReadOnlyCollection<BattleIndex>(new BattleIndex[] {
				EnemyMain1,
				EnemyMain2,
				EnemyMain3,
				EnemyMain4,
				EnemyMain5,
				EnemyMain6,
			});

		public static readonly ReadOnlyCollection<BattleIndex> EnemyEscort
			= new ReadOnlyCollection<BattleIndex>(new BattleIndex[] {
				EnemyEscort1,
				EnemyEscort2,
				EnemyEscort3,
				EnemyEscort4,
				EnemyEscort5,
				EnemyEscort6,
			});

		public static readonly ReadOnlyCollection<BattleIndex> EnemyAll
			= new ReadOnlyCollection<BattleIndex>(new BattleIndex[] {
				EnemyMain1,
				EnemyMain2,
				EnemyMain3,
				EnemyMain4,
				EnemyMain5,
				EnemyMain6,
				EnemyEscort1,
				EnemyEscort2,
				EnemyEscort3,
				EnemyEscort4,
				EnemyEscort5,
				EnemyEscort6,
		});


		public BattleIndex(BattleSides side, int index)
		{
			Side = side;
			Index = index;
		}

		public BattleIndex(int index, bool isFriendCombined, bool isEnemyCombined)
		{
			if (index < 12)
			{
				if (isFriendCombined)
				{
					Side = index < 6 ? BattleSides.FriendMain : BattleSides.FriendEscort;
					Index = index % 6;
				}
				else
				{
					Side = BattleSides.FriendMain;
					Index = index;
				}
			}
			else
			{
				if (isEnemyCombined)
				{
					Side = index < 18 ? BattleSides.EnemyMain : BattleSides.EnemyEscort;
					Index = index % 6;
				}
				else
				{
					Side = BattleSides.EnemyMain;
					Index = index - 12;
				}
			}
		}

		public bool IsMain => Side == BattleSides.FriendMain || Side == BattleSides.EnemyMain;
		public bool IsEscort => Side == BattleSides.FriendEscort || Side == BattleSides.EnemyEscort;


		// note: FriendMain7 is equal to FriendEscort1
		public static implicit operator int(BattleIndex sidedIndex) => (int)sidedIndex.Side * 6 + sidedIndex.Index;

		public static int Get(BattleSides side, int index) => new BattleIndex(side, index);

		public override string ToString() => $"{Side} #{Index + 1}";
	}

	public enum BattleSides
	{
		FriendMain,
		FriendEscort,
		EnemyMain,
		EnemyEscort,
	}

}
