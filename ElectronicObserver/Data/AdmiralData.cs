﻿using ElectronicObserver.Utility.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data
{

	/// <summary>
	/// 提督および司令部の情報を保持します。
	/// </summary>
	public class AdmiralData : APIWrapper
	{

		/// <summary>
		/// 提督ID
		/// </summary>
		public int AdmiralID
		{
			get
			{
				if (RawData.api_member_id is string)
					return int.Parse(RawData.api_member_id);
				else
					return (int)RawData.api_member_id;
			}
		}

		/// <summary>
		/// 提督名
		/// </summary>
		public string AdmiralName => RawData.api_nickname;

		/// <summary>
		/// 起動日時
		/// </summary>
		public DateTime StartTime => DateTimeHelper.FromAPITime((long)RawData.api_starttime);

		/// <summary>
		/// 艦隊司令部Level.
		/// </summary>
		public int Level => (int)RawData.api_level;

		/// <summary>
		/// 階級
		/// </summary>
		public int Rank => (int)RawData.api_rank;

		/// <summary>
		/// 提督経験値
		/// </summary>
		public int Exp => (int)RawData.api_experience;

		/// <summary>
		/// 提督コメント
		/// </summary>
		public string Comment => RawData.api_comment;

		/// <summary>
		/// 最大保有可能艦娘数
		/// </summary>
		public int MaxShipCount => (int)RawData.api_max_chara;

		/// <summary>
		/// 最大保有可能装備数
		/// </summary>
		public int MaxEquipmentCount => (int)RawData.api_max_slotitem;

		/// <summary>
		/// 最大保有可能艦隊数
		/// </summary>
		public int FleetCount => (int)RawData.api_count_deck;

		/// <summary>
		/// 工廠ドック数
		/// </summary>
		public int ArsenalCount => (int)RawData.api_count_kdock;

		/// <summary>
		/// 入渠ドック数
		/// </summary>
		public int DockCount => (int)RawData.api_count_ndock;


		/// <summary>
		/// 家具コイン
		/// </summary>
		public int FurnitureCoin => (int)RawData.api_fcoin;

		/// <summary>
		/// 出撃の勝数
		/// </summary>
		public int SortieWin => (int)RawData.api_st_win;

		/// <summary>
		/// 出撃の敗数
		/// </summary>
		public int SortieLose => (int)RawData.api_st_lose;

		/// <summary>
		/// 遠征の回数
		/// </summary>
		public int MissionCount => (int)RawData.api_ms_count;

		/// <summary>
		/// 遠征の成功数
		/// </summary>
		public int MissionSuccess => (int)RawData.api_ms_success;

		/// <summary>
		/// 演習の勝数
		/// </summary>
		public int PracticeWin => (int)RawData.api_pt_win;

		/// <summary>
		/// 演習の敗数
		/// </summary>
		public int PracticeLose => (int)RawData.api_pt_lose;

		/// <summary>
		/// 甲種勲章保有数
		/// </summary>
		public int Medals
		{
			get { return (int)RawData.api_medals; }
		}


		/// <summary>
		/// 資源の自然回復上限
		/// </summary>
		public int MaxResourceRegenerationAmount => Level * 250 + 750;


		public override void LoadFromRequest(string apiname, Dictionary<string, string> data)
		{
			base.LoadFromRequest(apiname, data);

			if (apiname == "api_req_member/updatecomment")
			{
				if (RawData != null)
					RawData.api_comment = data["api_cmt"];
			}
		}
	}


}
