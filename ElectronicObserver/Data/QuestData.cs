﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data
{

	/// <summary>
	/// 任務のデータを保持します。
	/// </summary>
	public class QuestData : ResponseWrapper, IIdentifiable
	{

		/// <summary>
		/// 任務ID
		/// </summary>
		public int QuestID => (int)RawData.api_no;

		/// <summary>
		/// 任務カテゴリ
		/// </summary>
		public int Category => (int)RawData.api_category;

		/// <summary>
		/// 任務出現タイプ
		/// 1=デイリー, 2=ウィークリー, 3=マンスリー, 4=単発, 5=他
		/// </summary>
		public int Type => (int)RawData.api_type;

		/// <summary>
		/// 遂行状態
		/// 1=未受領, 2=遂行中, 3=達成
		/// </summary>
		public int State
		{
			get { return (int)RawData.api_state; }
			set { RawData.api_state = value; }
		}

		/// <summary>
		/// 任務名
		/// </summary>
		public string Name => (string)RawData.api_title;

		/// <summary>
		/// 説明
		/// </summary>
		public string Description => ((string)RawData.api_detail).Replace("<br>", "\r\n");

		//undone:api_bonus_flag

		/// <summary>
		/// 進捗
		/// </summary>
		public int Progress => (int)RawData.api_progress_flag;

		/// <summary>
		/// The position of the quest in quest list
		/// Start by 1 to prevent display issue
		/// Set by QuestManager when load response
		/// </summary>
		public int DisplayPos;

		public string DisplayPage => DisplayPos > 0 ? ((DisplayPos - 1) / 5 + 1).ToString() : "";

		public int ID => QuestID;
		public override string ToString() => $"[{QuestID}] {Name}";
	}


}
