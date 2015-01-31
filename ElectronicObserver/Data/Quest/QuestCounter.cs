using ElectronicObserver.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Quest {
	
	/// <summary>
	/// 任務の進捗カウンタを処理するクラスの基底です。
	/// </summary>
	public abstract class QuestCounter {

		public int Counter { get; set; }
		public int CounterMax { get; protected set; }


		public QuestCounter() {
			Counter = 0;
			CounterMax = 0;
		}

		public QuestCounter( int max )
			: this() {
			CounterMax = max;
		}


		/// <summary>
		/// イベントを登録します。
		/// </summary>
		public abstract void Register();

		/// <summary>
		/// イベントの登録を解除します。＊絶対に＊忘れないでください。
		/// </summary>
		public abstract void Unregister();

		/// <summary>
		/// 登録したAPIが呼ばれたときに処理されます。
		/// </summary>
		public virtual void Received( string apiname, dynamic data ) {
			Counter++;
		}


		public override string ToString() {
			return Counter + "/" + CounterMax;
		}


		/*
		 * memo:
		 * daily:
		 * 敵艦隊を10回邀撃せよ！
		 * 敵補給艦を3隻撃沈せよ！
		 * 南西諸島海域の制海権を握れ！
		 * 敵潜水艦を制圧せよ！
		 * 「演習」で練度向上！
		 * 「演習」で他提督を圧倒せよ！
		 * 「遠征」を3回成功させよう！
		 * 「遠征」を10回成功させよう！
		 * 艦隊大整備！
		 * 艦隊酒保祭り！
		 * 	装備「開発」集中強化！
		 * 艦娘「建造」艦隊強化！
		 * 軍縮条約対応！
		 * 艦の「近代化改修」を実施せよ！
		 * 敵空母を3隻撃沈せよ！
		 * 敵輸送船団を叩け！
		 * 
		 * weekly:
		 * あ号作戦
		 * い号作戦
		 * 海上通商破壊作戦
		 * ろ号作戦
		 * 海上護衛戦
		 * 敵東方艦隊を撃滅せよ！
		 * 敵北方艦隊主力を撃滅せよ！
		 * 南方海域珊瑚諸島沖の制空権を握れ！
		 * 	海上輸送路の安全確保に努めよ！
		 * 大規模演習
		 * 大規模遠征作戦、発令！
		 * 南方への鼠輸送を継続実施せよ!
		 * 「近代化改修」を進め、戦備を整えよ！
		 * 資源の再利用
		 * 
		 * monthly:
		 * 「潜水艦隊」出撃せよ！
		 * 
		 */
	}

}
