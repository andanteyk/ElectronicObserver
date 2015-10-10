﻿using ElectronicObserver.Data;
using ElectronicObserver.Data.Battle;
using ElectronicObserver.Data.Battle.Phase;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.Window.Control;
using ElectronicObserver.Window.Support;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace ElectronicObserver.Window {

	public partial class FormBattle : DockContent {

		private List<ShipStatusHP> HPBars;
		private List<ImageLabel> DamageLabels;

		public Font MainFont { get; set; }
		public Font SubFont { get; set; }

		private Pen LinePen = Pens.Silver;

		public FormBattle( FormMain parent ) {
			this.SuspendLayoutForDpiScale();
			InitializeComponent();

			ControlHelper.SetDoubleBuffered( TableTop );
			ControlHelper.SetDoubleBuffered( TableBottom );

			ConfigurationChanged();


			HPBars = new List<ShipStatusHP>( 18 );
			DamageLabels = new List<ImageLabel>( 12 );


			TableBottom.SuspendLayout();
			for ( int i = 0; i < 12; i++ ) {
				var lbl = new ImageLabel();
				DamageLabels.Add( lbl );
				{
					lbl.ImageList = ResourceManager.Instance.Icons;
					lbl.ImageIndex = (int)ResourceManager.IconContent.ConditionNormal;
					lbl.ImageAlign = ContentAlignment.MiddleLeft;
					lbl.AutoSize = false;
					lbl.ShowText = !Utility.Configuration.Config.FormBattle.IsShortDamage;
					lbl.Size = new Size( 56, 20 );
					lbl.Margin = new Padding( 2, 0, 2, 0 );
					lbl.Anchor = AnchorStyles.Left | AnchorStyles.Right;
					lbl.Font = MainFont;
				}
				if ( i < 6 ) {
					TableBottom.Controls.Add( lbl, 1, i + 1 );
				} else {
					TableBottom.Controls.Add( lbl, 3, i - 5 );
				}
			}

			for ( int i = 0; i < 18; i++ ) {
				HPBars.Add( new ShipStatusHP() );
				HPBars[i].Size = new Size( 80, 20 );
				HPBars[i].Margin = new Padding( 2, 0, 2, 0 );
				HPBars[i].Anchor = AnchorStyles.Left | AnchorStyles.Right;
				HPBars[i].MainFont = MainFont;
				HPBars[i].SubFont = SubFont;
				HPBars[i].MainFontColor = Utility.Configuration.Config.UI.ForeColor;
				HPBars[i].SubFontColor = Utility.Configuration.Config.UI.SubForeColor;
				HPBars[i].UsePrevValue = true;
				HPBars[i].ShowDifference = true;
				HPBars[i].MaximumDigit = 9999;

				if ( i < 6 ) {
					TableBottom.Controls.Add( HPBars[i], 0, i + 1 );
				} else if ( i < 12 ) {
					TableBottom.Controls.Add( HPBars[i], 4, i - 5 );
				} else {
					TableBottom.Controls.Add( HPBars[i], 2, i - 11 );
				}
			}
			TableBottom.ResumeLayout();


			SearchingFriend.ImageList =
			SearchingEnemy.ImageList =
			AACutin.ImageList =
			AirStage1Friend.ImageList =
			AirStage1Enemy.ImageList =
			AirStage2Friend.ImageList =
			AirStage2Enemy.ImageList =
				ResourceManager.Instance.Equipments;

			BaseLayoutPanel.Visible = false;


			Icon = ResourceManager.ImageToIcon( ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormBattle] );
			this.ResumeLayoutForDpiScale();

			DamageWidth = TableBottom.ColumnStyles[1].Width;
		}

		float DamageWidth = 60;

		private void FormBattle_Load( object sender, EventArgs e ) {

			APIObserver o = APIObserver.Instance;

			o.APIList["api_port/port"].ResponseReceived += Updated;
			o.APIList["api_req_map/start"].ResponseReceived += Updated;
			o.APIList["api_req_map/next"].ResponseReceived += Updated;
			o.APIList["api_req_sortie/battle"].ResponseReceived += Updated;
			o.APIList["api_req_sortie/battleresult"].ResponseReceived += Updated;
			o.APIList["api_req_battle_midnight/battle"].ResponseReceived += Updated;
			o.APIList["api_req_battle_midnight/sp_midnight"].ResponseReceived += Updated;
			o.APIList["api_req_sortie/airbattle"].ResponseReceived += Updated;
			o.APIList["api_req_combined_battle/battle"].ResponseReceived += Updated;
			o.APIList["api_req_combined_battle/midnight_battle"].ResponseReceived += Updated;
			o.APIList["api_req_combined_battle/sp_midnight"].ResponseReceived += Updated;
			o.APIList["api_req_combined_battle/airbattle"].ResponseReceived += Updated;
			o.APIList["api_req_combined_battle/battle_water"].ResponseReceived += Updated;
			o.APIList["api_req_combined_battle/battleresult"].ResponseReceived += Updated;
			o.APIList["api_req_practice/battle"].ResponseReceived += Updated;
			o.APIList["api_req_practice/midnight_battle"].ResponseReceived += Updated;
			o.APIList["api_req_practice/battle_result"].ResponseReceived += Updated;

			Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;

		}

		private void ContextMenuBattle_ExportReport_Click( object sender, EventArgs e ) {

			BattleManager bm = KCDatabase.Instance.Battle;
			if ( bm == null
				|| bm.BattleMode == BattleManager.BattleModes.Undefined	// 无战斗
				|| bm.BattleMode == BattleManager.BattleModes.NightDay )	// TODO: 夜转昼
				//|| bm.BattleMode > BattleManager.BattleModes.BattlePhaseMask )	// TODO：联合舰队
				return;

			bool isWater = ( ( bm.BattleMode & BattleManager.BattleModes.CombinedSurface ) > 0 );
			bool isCombined = isWater || ( bm.BattleMode > BattleManager.BattleModes.BattlePhaseMask );

			StringBuilder builder = new StringBuilder();
			builder.AppendLine( @"<html>
<head>
<style type=""text/css"">
body {font-family:""Microsoft JhengHei UI"";}
table {border:none;}
th {background:#eee;}
td,th,tr {text-align:left; padding:2px 4px;}
.changed {color:red;}
.damage {background:#fcfcfc;color:#822;}
</style>
</head>
<body>
" );

			var day = bm.BattleDay;
			PhaseInitial init;
			string[] enemys;

			if ( bm.BattleDay == null ) {
				init = bm.BattleNight.Initial;
				enemys = ( (int[])bm.BattleNight.RawData.api_ship_ke ).Skip( 1 ).Select( id => id <= 0 ? null : KCDatabase.Instance.MasterShips[id].NameWithClass ).ToArray();
			} else {
				init = bm.BattleDay.Initial;
				enemys = ( (int[])bm.BattleDay.RawData.api_ship_ke ).Skip( 1 ).Select( id => id <= 0 ? null : KCDatabase.Instance.MasterShips[id].NameWithClass ).ToArray();
			}

			// 基本信息
			// 阵形
			builder.AppendFormat( @"<h2>战斗阵形</h2>
<hr />
<table cellspacing=""2"" cellpadding=""0"">
<tbody>
<tr>
<th width=""90"">我方</th><td>{0}</td>
</tr>
<tr>
<th width=""90"">敌方</th><td>{1}</td>
</tr>
<tr>
<th width=""90"">航向</th><td>{2}</td>
</tr>
</tbody>
</table>
", FormationFriend.Text, FormationEnemy.Text, Formation.Text );
			// 索敌
			if ( day != null && day.IsAvailable )
			{
				int searchFriend = day.Searching.SearchingFriend;
				int searchEnemy = day.Searching.SearchingEnemy;
				builder.AppendFormat( @"<hr/>
<table cellspacing=""2"" cellpadding=""0"">
<tbody>
<tr>
<th width=""90"">我方索敌</th><td>{0}</td><td>{1}</td>
</tr>
<tr>
<th width=""90"">敌方索敌</th><td>{2}</td><td>{3}</td>
</tr>
</tbody>
</table>
", ( searchFriend < 4 ? "水上机" : "雷达" ), Constants.GetSearchingResult( searchFriend ),
 ( searchEnemy < 4 ? "水上机" : "雷达" ), Constants.GetSearchingResult( searchEnemy ) );
			}


			// 战斗过程

			string[] friends = init.FriendFleet.MembersInstance.Select( s => s == null ? null : s.NameWithLevel ).ToArray();
			string[] accompany = init.AccompanyFleet.MembersInstance.Select( s => s == null ? null : s.NameWithLevel ).ToArray();
			int[] hps = (int[])init.InitialHPs.Clone();
			int[] maxHps = init.MaxHPs;

			// day
			{
				if ( day != null && day.IsAvailable )
				{
					try
					{
						var pd1 = day.AirBattle;
						var pd2 = ( day is BattleAirBattle ? ( (BattleAirBattle)day ).AirBattle2 : null );

						bool[] s1available = { pd1.IsStage1Available, ( pd2 != null && pd2.IsStage1Available ) };
						bool[] s2available = { pd1.IsStage2Available, ( pd2 != null && pd2.IsStage2Available ) };
						int[] touches =
						{
							pd1.TouchAircraftFriend, s1available[1] ? pd2.TouchAircraftFriend : -1,
							pd1.TouchAircraftEnemy, s1available[1] ? pd2.TouchAircraftEnemy : -1
						};
						EquipmentDataMaster[] planes =
						{
							KCDatabase.Instance.MasterEquipments[touches[0]],
							KCDatabase.Instance.MasterEquipments[touches[1]],
							KCDatabase.Instance.MasterEquipments[touches[2]],
							KCDatabase.Instance.MasterEquipments[touches[3]]
						};
						bool[] fire = new bool[] { s2available[0] && pd1.IsAACutinAvailable, s2available[1] && pd2.IsAACutinAvailable };
						int[] cutinID = new int[]
						{
							fire[0] ? pd1.AACutInKind : -1,
							fire[1] ? pd2.AACutInKind : -1,
						};

						// 接触信息
						builder.AppendFormat( @"<h2>航空战</h2>
<hr/>
<table cellspacing=""2"" cellpadding=""0"">
<tbody>
<tr>
<th width=""90""></th><th width=""110"">我方</th><th width=""110""></th><th width=""110"">敌方</th><th width=""110""></th>
</tr>
<tr>
<th width=""90"">制空</th><td>{0}</td><td>{1}</td><td colspan=""2""></td>
</tr>
<tr>
<th width=""90"">接触信息</th><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td>
</tr>
<tr>
<th width=""90"">stage1</th><td>{6}</td><td>{7}</td><td>{8}</td><td>{9}</td>
</tr>
<tr>
<th width=""90"">stage2</th><td>{10}</td><td>{11}</td><td>{12}</td><td>{13}</td>
</tr>
<tr>
<th width=""90"">对空</th><td>{14}</td><td>{15}</td><td colspan=""2""></td>
</tr>
</tbody>
</table>
", Constants.GetAirSuperiority( pd1.AirSuperiority ),
	( pd2 == null ? null : Constants.GetAirSuperiority( pd2.AirSuperiority ) ),

	( !s1available[0] || planes[0] == null ? "-" : planes[0].Name ),
	( !s1available[1] || planes[1] == null ? null : planes[1].Name ),
	( !s1available[0] || planes[2] == null ? "-" : planes[2].Name ),
	( !s1available[1] || planes[3] == null ? null : planes[3].Name ),

	( !s1available[0] ? "-" : string.Format( "-{0}/{1}", pd1.AircraftLostStage1Friend, pd1.AircraftTotalStage1Friend ) ),
	( !s1available[1] ? null : string.Format( "-{0}/{1}", pd2.AircraftLostStage1Friend, pd2.AircraftTotalStage1Friend ) ),
	( !s1available[0] ? null : string.Format( "-{0}/{1}", pd1.AircraftLostStage1Enemy, pd1.AircraftTotalStage1Enemy ) ),
	( !s1available[1] ? null : string.Format( "-{0}/{1}", pd2.AircraftLostStage1Enemy, pd2.AircraftTotalStage1Enemy ) ),

	( !s2available[0] ? "-" : string.Format( "-{0}/{1}", pd1.AircraftLostStage2Friend, pd1.AircraftTotalStage2Friend ) ),
	( !s2available[1] ? null : string.Format( "-{0}/{1}", pd2.AircraftLostStage2Friend, pd2.AircraftTotalStage2Friend ) ),
	( !s2available[0] ? null : string.Format( "-{0}/{1}", pd1.AircraftLostStage2Enemy, pd1.AircraftTotalStage2Enemy ) ),
	( !s2available[1] ? null : string.Format( "-{0}/{1}", pd2.AircraftLostStage2Enemy, pd2.AircraftTotalStage2Enemy ) ),

	( !fire[0] && !fire[1] ? "对空炮火" : ( !fire[0] ? "-" : string.Format( "{0}<br/>{1} (#{2})", pd1.AACutInShip.NameWithLevel, Constants.GetAACutinKind( cutinID[0] ), cutinID[0] ) ) ),
	( !fire[1] || !s1available[1] ? "-" : string.Format( "{0}<br/>{1} (#{2})", pd2.AACutInShip.NameWithLevel, Constants.GetAACutinKind( cutinID[1] ), cutinID[1] ) )
	);


						// 航空战血量变化
						if ( day.AirBattle.IsAvailable && day.AirBattle.IsStage3Available )
						{
							var stage3 = day.RawData.api_kouku.api_stage3;
							int[] flagsfriend = ( (int[])stage3.api_frai_flag ).Skip( 1 ).Concat( ( (int[])stage3.api_fbak_flag ).Skip( 1 ) ).ToArray();
							int[] flagsenemy = ( (int[])stage3.api_erai_flag ).Skip( 1 ).Concat( ( (int[])stage3.api_ebak_flag ).Skip( 1 ) ).ToArray();
							FillAirDamage( builder, flagsfriend, flagsenemy, day.AirBattle.Damages, friends, isCombined ? accompany : null, enemys, hps, maxHps );
						}

						// 支援
						if ( day.Support != null && day.Support.SupportFlag > 0 )
						{

							string[] supportnames = day.Support.SupportFleet.MembersInstance.Select( m => m == null ? null : m.NameWithLevel ).ToArray();

							switch ( day.Support.SupportFlag )
							{
								case 1:
									FillSupportDamage( "航空支援", builder, day.Support.AirRaidDamages, supportnames, enemys, hps, maxHps );
									break;

								case 2:
								case 3:
									FillSupportDamage( "炮雷支援", builder, day.Support.ShellingTorpedoDamages, supportnames, enemys, hps, maxHps );
									break;
							}

						}
					}
					catch ( Exception ex )
					{

						Utility.ErrorReporter.SendErrorReport( ex, "航空/支援解析出错。", "battle day", day.RawData.ToString() );

					}

					// 开幕雷击
					if ( day.OpeningTorpedo != null && day.OpeningTorpedo.IsAvailable )
					{
						FillTorpedoDamage( "开幕雷击", builder, day.OpeningTorpedo.TorpedoData, isCombined ? accompany : friends, enemys, hps, maxHps );
					}


					// 炮击战
					if ( day.Shelling1 != null && day.Shelling1.IsAvailable )
					{
						FillShellingDamage( "炮击战1回合", builder, day.Shelling1.ShellingData, ( isCombined && !isWater ) ? accompany : friends, enemys, hps, maxHps );
					}

					if ( isCombined && !isWater )
					{
						// 机动部队闭幕雷击
						if ( day.Torpedo != null && day.Torpedo.IsAvailable )
						{
							FillTorpedoDamage( "闭幕雷击", builder, day.Torpedo.TorpedoData, accompany, enemys, hps, maxHps );
						}
					}

					if ( day.Shelling2 != null && day.Shelling2.IsAvailable )
					{
						FillShellingDamage( "炮击战2回合", builder, day.Shelling2.ShellingData, friends, enemys, hps, maxHps );
					}

					if ( day.Shelling3 != null && day.Shelling3.IsAvailable )
					{
						FillShellingDamage( "炮击战3回合", builder, day.Shelling3.ShellingData, isWater ? accompany : friends, enemys, hps, maxHps );
					}

					// 闭幕雷击
					if ( ( !isCombined || isWater ) && day.Torpedo != null && day.Torpedo.IsAvailable )
					{
						FillTorpedoDamage( "闭幕雷击", builder, day.Torpedo.TorpedoData, isCombined ? accompany : friends, enemys, hps, maxHps );
					}
				}
			}

			// night
			{
				var night = bm.BattleNight;
				if ( night != null && night.IsAvailable ) {

					var nightbattle = night.NightBattle;

					// 夜战buff判定
					var ship = ( nightbattle.SearchlightIndexFriend < 0 ? null : nightbattle.FriendFleet.MembersInstance[nightbattle.SearchlightIndexFriend] );
					var enemy = nightbattle.SearchlightEnemyInstance;
					EquipmentDataMaster[] touches =
					{
						KCDatabase.Instance.MasterEquipments[nightbattle.TouchAircraftFriend],
						KCDatabase.Instance.MasterEquipments[nightbattle.TouchAircraftEnemy]
					};
					var flareFriend = ( nightbattle.FlareIndexFriend < 0 ? null : nightbattle.FriendFleet.MembersInstance[nightbattle.FlareIndexFriend] );
					var flareEnemy = nightbattle.FlareEnemyInstance;

					builder.AppendFormat( @"<h2>夜战</h2>
<hr/>
<table cellspacing=""2"" cellpadding=""0"">
<tbody>
<tr>
<th width=""90""></th><th width=""110"">我方</th><th width=""110"">敌方</th>
</tr>
<tr>
<th width=""90"">探照灯</th><td>{0}</td><td>{1}</td>
</tr>
<tr>
<th width=""90"">夜间接触</th><td>{2}</td><td>{3}</td>
</tr>
<tr>
<th width=""90"">照明弹</th><td>{4}</td><td>{5}</td>
</tr>
</tbody>
</table>
", ( ship == null ? "-" : ship.NameWithLevel ),
	( enemy == null ? "-" : enemy.NameWithClass ),
	( touches[0] == null ? "-" : touches[0].Name ),
	( touches[1] == null ? "-" : touches[1].Name ),
	( flareFriend == null ? "-" : flareFriend.NameWithLevel ),
	( flareEnemy == null ? "-" : flareEnemy.NameWithClass )
	);

					// 战况
					if ( nightbattle.ShellingData != null ) {
						FillShellingDamage( null, builder, nightbattle.ShellingData, isCombined ? accompany : friends, enemys, hps, maxHps );
					}

				}
			}

			builder.AppendLine( "</body>\r\n</html>" );

			new Dialog.DialogBattleReport( builder.ToString() ).Show();
		}

		private void FillAirDamage( StringBuilder builder, int[] flagsfriend, int[] flagsenemy, int[] damages, string[] friends, string[] accompany, string[] enemys, int[] hps, int[] maxHps )
		{
			builder.AppendLine( @"<table cellspacing=""2"" cellpadding=""0"">
<thead>
<th width=""160"">我方</th>
<th width=""90"">所受伤害</th>
<th width=""90"">血量</th>" );
			if ( accompany != null )
			{
				builder.AppendLine( @"<th width=""160"">伴随</th>
<th width=""90"">所受伤害</th>
<th width=""90"">血量</th>" );
			}
			builder.AppendLine( @"<th width=""160"">敌方</th>
<th width=""90"">所受伤害</th>
<th width=""90"">血量</th>
</tr>
</thead>
<tbody>" );

			for ( int i = 0; i < 6; i++ ) {
				builder.AppendLine( "<tr>" );

				// 航空开幕
				if ( friends[i] != null ) {
					int before = hps[i];
					hps[i] = Math.Max( hps[i] - damages[i], 0 );
					builder.AppendFormat( "<td>{5}.{0}</td><td>{6}</td><td{4}>{1}→{2}/{3}</td>\r\n",
						friends[i], before, hps[i], maxHps[i],
						( before == hps[i] ? null : @" class=""changed""" ),
						( i + 1 ),
						( damages[i] > 0 ? damages[i].ToString() : ( ( flagsfriend[i] > 0 || flagsfriend[i + 6] > 0 ) ? "miss" : null ) ) );

				} else {
					builder.AppendLine( "<td>&nbsp;</td><td>&nbsp;</td>" );
				}

				if ( accompany != null )
				{
					if ( accompany[i] != null )
					{
						int before = hps[i + 12];
						hps[i + 12] = Math.Max( hps[i + 12] - damages[i + 12], 0 );
						builder.AppendFormat( "<td>{5}.{0}</td><td>{6}</td><td{4}>{1}→{2}/{3}</td>\r\n",
							accompany[i], before, hps[i + 12], maxHps[i + 12],
							( before == hps[i + 12] ? null : @" class=""changed""" ),
							( i + 1 ),
							( damages[i + 12] > 0 ? damages[i + 12].ToString() : null ) );
					}
					else
					{
						builder.AppendLine( "<td>&nbsp;</td><td>&nbsp;</td>" );
					}
				}

				if ( enemys[i] != null ) {
					int before = hps[i + 6];
					hps[i + 6] = Math.Max( hps[i + 6] - damages[i + 6], 0 );
					builder.AppendFormat( "<td>{5}.{0}</td><td>{6}</td><td{4}>{1}→{2}/{3}</td>\r\n",
						enemys[i], before, hps[i + 6], maxHps[i + 6],
						( before == hps[i + 6] ? null : @" class=""changed""" ),
						( i + 1 ),
						( damages[i + 6] > 0 ? damages[i + 6].ToString() : ( ( flagsenemy[i] > 0 || flagsenemy[i + 6] > 0 ) ? "miss" : null ) ) );

				} else {
					builder.AppendLine( "<td>&nbsp;</td><td>&nbsp;</td>" );
				}

				builder.AppendLine( "</tr>" );
			}

			builder.AppendLine( "</tbody>\r\n</table>" );

		}

		private void FillSupportDamage( string name, StringBuilder builder, int[] damages, string[] friends, string[] enemys, int[] hps, int[] maxHps )
		{

			builder.AppendFormat( @"<h2>{0} <small>（伤害无对应关系）</small></h2>
<hr />
<table cellspacing=""2"" cellpadding=""0"">
<thead>
<tr>
<th width=""160"">我方</th>
<th width=""40"">&nbsp;</th>
<th width=""160"">敌方</th>
<th width=""90"">伤害</th>
<th width=""90"">血量</th>
</tr>
</thead>
<tbody>
", name );

			for ( int i = 0; i < 6; i++ ) {
				builder.AppendLine( "<tr>" );

				// 支援
				if ( friends[i] != null ) {
					builder.AppendFormat( "<td>{0}.{1}</td><td></td>\r\n", ( i + 1 ), friends[i] );
				} else {
					builder.AppendLine( "<td>&nbsp;</td><td>&nbsp;</td>" );
				}

				if ( enemys[i] != null ) {
					int before = hps[i + 6];
					hps[i + 6] = Math.Max( hps[i + 6] - damages[i], 0 );
					builder.AppendFormat( "<td>{5}.{0}</td><td>{6}</td><td{4}>{1}→{2}/{3}</td>\r\n",
						enemys[i], before, hps[i + 6], maxHps[i + 6],
						( before == hps[i + 6] ? null : @" class=""changed""" ),
						( i + 1 ),
						( damages[i] > 0 ? damages[i].ToString() : "miss" ) );

				} else {
					builder.AppendLine( "<td>&nbsp;</td><td>&nbsp;</td>" );
				}

				builder.AppendLine( "</tr>" );
			}

			builder.AppendLine( "</tbody>\r\n</table>" );

		}

		private void FillTorpedoDamage( string name, StringBuilder builder, dynamic data, string[] friends, string[] enemys, int[] hps, int[] maxHps ) {

			try {
				builder.AppendFormat( @"<h2>{0}</h2>
<hr />
<table cellspacing=""2"" cellpadding=""0"">
<thead>
<tr>
<th width=""160"">舰</th>
<th width=""40"">&nbsp;</th>
<th width=""160"">舰</th>
<th width=""90"">伤害</th>
<th width=""90"">暴击</th>
</tr>
</thead>
<tbody>
", name );

				int[] friendTarget = ( (int[])data.api_frai ).Skip( 1 ).ToArray();
				int[] friendDamages = ( (int[])data.api_fydam ).Skip( 1 ).ToArray();
				int[] friendFlags = ( (int[])data.api_fcl ).Skip( 1 ).ToArray();
				builder.AppendLine( @"<tr><th colspan=""5"">我方攻击</td></tr>" );
				for ( int i = 0; i < 6; i++ ) {

					if ( friendTarget[i] > 0 ) {
						builder.AppendLine( "<tr>" );

						builder.AppendFormat( "<td>{4}.{0}</td><td>→</td><td>{5}.{1}</td><td>{2}</td><td>{3}</td>\r\n",
							friends[i], enemys[friendTarget[i] - 1],
							( friendDamages[i] == 0 ? ( friendFlags[i] == 0 ? "miss" : "0" ) : friendDamages[i].ToString() ),
							( friendFlags[i] == 2 ? "√" : "" ),
							( i + 1 ), ( friendTarget[i] ) );

						builder.AppendLine( "</tr>" );
					}
				}

				int[] enemyTarget = ( (int[])data.api_erai ).Skip( 1 ).ToArray();
				int[] enemyDamages = ( (int[])data.api_eydam ).Skip( 1 ).ToArray();
				int[] enemyFlags = ( (int[])data.api_ecl ).Skip( 1 ).ToArray();
				builder.AppendLine( @"<tr><th colspan=""5"">敌方攻击</td></tr>" );
				for ( int i = 0; i < 6; i++ ) {

					if ( enemyTarget[i] > 0 ) {
						builder.AppendLine( "<tr>" );

						builder.AppendFormat( "<td>{4}.{0}</td><td>→</td><td>{5}.{1}</td><td>{2}</td><td>{3}</td>\r\n",
							enemys[i], friends[enemyTarget[i] - 1],
							( enemyDamages[i] == 0 ? ( enemyFlags[i] == 0 ? "miss" : "0" ) : enemyDamages[i].ToString() ),
							( enemyFlags[i] == 2 ? "√" : "" ),
							( i + 1 ), ( enemyTarget[i] ) );

						builder.AppendLine( "</tr>" );
					}
				}

				builder.AppendLine( "</tbody>\r\n</table>" );

				// 计算血量变化
				int[] fdam = ( (int[])data.api_fdam ).Skip( 1 ).ToArray();
				int[] edam = ( (int[])data.api_edam ).Skip( 1 ).ToArray();
				for ( int i = 0; i < 6; i++ ) {
					hps[i] -= fdam[i];
					hps[i + 6] -= edam[i];
				}

			} catch ( Exception ex ) {

				Utility.ErrorReporter.SendErrorReport( ex, name + "解析出错。", "torpedo", data.ToString() );
			}

		}

		private void FillShellingDamage( string name, StringBuilder builder, dynamic data, string[] friends, string[] enemys, int[] hps, int[] maxHps ) {

			try {

				if ( !string.IsNullOrEmpty( name ) )
				{
					builder.AppendFormat( @"<h2>{0}</h2>
<hr />
", name );
				}

				builder.AppendLine( @"<table cellspacing=""2"" cellpadding=""0"">
<thead>
<tr>
<th width=""24"">&nbsp;</th>
<th width=""160"">舰</th>
<th width=""40"">&nbsp;</th>
<th width=""24"">&nbsp;</th>
<th width=""160"">舰</th>
<th width=""90"">伤害</th>
<th width=""90"">血量</th>
<th width=""90"">暴击</th>
<th width=""120"">装备</th>
</tr>
</thead>
<tbody>" );

				int[] at_list = (int[])data.api_at_list;

				for ( int i = 1; i < at_list.Length; i++ ) {

					int from = at_list[i] - 1;
					int[] enemy_list = (int[])data.api_df_list[i];
					int[] equips = (int[])data.api_si_list[i];
					int[] flags = (int[])data.api_cl_list[i];
					int[] damages = (int[])data.api_damage[i];

					for ( int j = 0; j < enemy_list.Length; j++ ) {
						int to = enemy_list[j] - 1;

						if ( to < 0 ) {
							// 3炮ci？
							continue;
						}

						if ( to < 6 ) {
							// 我方受到攻击
							builder.AppendLine( @"<tr class=""damage"">" );
						} else {
							builder.AppendLine( "<tr>" );
						}

						if ( j > 0 ) {
							builder.Append( @"<td colspan=""4"">&nbsp;</td>" );
						} else {
							builder.AppendFormat( "<td>{0}</td><td>{1}.{2}</td><td>→</td><td>{3}</td>",
								( from < 6 ? "我" : "敌" ),
								( from % 6 + 1 ),
								( from < 6 ? friends[from] : enemys[from - 6] ),
								( to < 6 ? "我" : "敌" ) );
						}

						int before = hps[to];
						hps[to] -= damages[j];

						builder.AppendFormat( "<td>{0}.{1}</td><td>{2}</td><td>{3}→{4}/{5}</td><td>{6}</td>",
							( to % 6 + 1 ),
							( to < 6 ? friends[to] : enemys[to - 6] ),
							( damages[j] == 0 ? ( flags[j] == 0 ? "miss" : "0" ) : damages[j].ToString() ),
							Math.Max( before, 0 ),
							Math.Max( hps[to], 0 ), maxHps[to],
							( flags[j] == 2 ? "√" : "" ) );

						if ( ( enemy_list.Length == 1 && equips.Length > 1 ) || enemy_list.Any( el => el < 0 ) ) {	// 3炮ci貌似list后两个都是-1

							var eqs = equips.Select( ei =>
							{
								var eq = KCDatabase.Instance.MasterEquipments[ei];
								return eq == null ? "&lt;" + ei + "&gt;" : eq.Name;
							} );
							builder.AppendFormat( "<td>{0}</td>", string.Join( ", ", equips.Select( ei => KCDatabase.Instance.MasterEquipments[ei].Name ) ) );

						} else {
							int equipid = equips[j];
							var eq = KCDatabase.Instance.MasterEquipments[equipid];
							builder.AppendFormat( "<td>{0}</td>", ( eq == null ? "" : eq.Name ) );
						}

						builder.AppendLine( "</tr>" );
					}

				}

				builder.AppendLine( "</tbody>\r\n</table>" );
			} catch ( Exception ex ) {

				Utility.ErrorReporter.SendErrorReport( ex, name + "解析出错。", "shelling", data.ToString() );
			}
		}


		private void Updated( string apiname, dynamic data ) {

			KCDatabase db = KCDatabase.Instance;
			BattleManager bm = db.Battle;

			BaseLayoutPanel.SuspendLayout();
			TableTop.SuspendLayout();
			TableBottom.SuspendLayout();
			switch ( apiname ) {

				case "api_req_map/start":
				case "api_req_map/next":
				case "api_port/port":
					BaseLayoutPanel.Visible = false;
					ToolTipInfo.RemoveAll();
					break;


				case "api_req_sortie/battle":
				case "api_req_practice/battle": {

						SetFormation( bm.BattleDay );
						SetSearchingResult( bm.BattleDay );
						SetAerialWarfare( bm.BattleDay.AirBattle );
						SetHPNormal( bm.BattleDay );
						SetDamageRateNormal( bm.BattleDay, bm.BattleDay.Initial.InitialHPs );

						BaseLayoutPanel.Visible = true;
					} break;

				case "api_req_battle_midnight/battle":
				case "api_req_practice/midnight_battle": {

						SetNightBattleEvent( bm.BattleNight.NightBattle );
						SetHPNormal( bm.BattleNight );
						SetDamageRateNormal( bm.BattleNight, bm.BattleDay.Initial.InitialHPs );

						BaseLayoutPanel.Visible = true;
					} break;

				case "api_req_battle_midnight/sp_midnight": {

						SetFormation( bm.BattleNight );
						ClearAerialWarfare();
						ClearSearchingResult();
						SetNightBattleEvent( bm.BattleNight.NightBattle );
						SetHPNormal( bm.BattleNight );
						SetDamageRateNormal( bm.BattleNight, bm.BattleNight.Initial.InitialHPs );

						BaseLayoutPanel.Visible = true;
					} break;

				case "api_req_sortie/airbattle": {

						SetFormation( bm.BattleDay );
						SetSearchingResult( bm.BattleDay );
						SetAerialWarfareAirBattle( bm.BattleDay.AirBattle, ( (BattleAirBattle)bm.BattleDay ).AirBattle2 );
						SetHPNormal( bm.BattleDay );
						SetDamageRateNormal( bm.BattleDay, bm.BattleDay.Initial.InitialHPs );

						BaseLayoutPanel.Visible = true;
					} break;

				case "api_req_combined_battle/battle":
				case "api_req_combined_battle/battle_water": {

						SetFormation( bm.BattleDay );
						SetSearchingResult( bm.BattleDay );
						SetAerialWarfare( bm.BattleDay.AirBattle );
						SetHPCombined( bm.BattleDay );
						SetDamageRateCombined( bm.BattleDay, bm.BattleDay.Initial.InitialHPs );

						BaseLayoutPanel.Visible = true;
					} break;

				case "api_req_combined_battle/airbattle": {

						SetFormation( bm.BattleDay );
						SetSearchingResult( bm.BattleDay );
						SetAerialWarfareAirBattle( bm.BattleDay.AirBattle, ( (BattleCombinedAirBattle)bm.BattleDay ).AirBattle2 );
						SetHPCombined( bm.BattleDay );
						SetDamageRateCombined( bm.BattleDay, bm.BattleDay.Initial.InitialHPs );

						BaseLayoutPanel.Visible = true;
					} break;

				case "api_req_combined_battle/midnight_battle": {

						SetNightBattleEvent( bm.BattleNight.NightBattle );
						SetHPCombined( bm.BattleNight );
						SetDamageRateCombined( bm.BattleNight, bm.BattleDay.Initial.InitialHPs );

						BaseLayoutPanel.Visible = true;
					} break;

				case "api_req_combined_battle/sp_midnight": {

						SetFormation( bm.BattleNight );
						ClearAerialWarfare();
						ClearSearchingResult();
						SetNightBattleEvent( bm.BattleNight.NightBattle );
						SetHPCombined( bm.BattleNight );
						SetDamageRateCombined( bm.BattleNight, bm.BattleNight.Initial.InitialHPs );

						BaseLayoutPanel.Visible = true;
					} break;


			}
			TableTop.ResumeLayout();
			TableBottom.ResumeLayout();
			BaseLayoutPanel.ResumeLayout();

		}


		/// <summary>
		/// 陣形・交戦形態を設定します。
		/// </summary>
		private void SetFormation( BattleData bd ) {

			FormationFriend.Text = Constants.GetFormationShort( bd.Searching.FormationFriend );
			FormationEnemy.Text = Constants.GetFormationShort( bd.Searching.FormationEnemy );
			Formation.Text = Constants.GetEngagementForm( bd.Searching.EngagementForm );

		}

		/// <summary>
		/// 索敵結果を設定します。
		/// </summary>
		private void SetSearchingResult( BattleData bd ) {

			int searchFriend = bd.Searching.SearchingFriend;
			SearchingFriend.Text = Constants.GetSearchingResultShort( searchFriend );
			SearchingFriend.ImageAlign = ContentAlignment.MiddleLeft;
			SearchingFriend.ImageIndex = (int)( searchFriend < 4 ? ResourceManager.EquipmentContent.Seaplane : ResourceManager.EquipmentContent.Radar );
			ToolTipInfo.SetToolTip( SearchingFriend, null );

			int searchEnemy = bd.Searching.SearchingEnemy;
			SearchingEnemy.Text = Constants.GetSearchingResultShort( searchEnemy );
			SearchingEnemy.ImageAlign = ContentAlignment.MiddleLeft;
			SearchingEnemy.ImageIndex = (int)( searchEnemy < 4 ? ResourceManager.EquipmentContent.Seaplane : ResourceManager.EquipmentContent.Radar );
			ToolTipInfo.SetToolTip( SearchingEnemy, null );

		}

		/// <summary>
		/// 索敵結果をクリアします。
		/// 索敵フェーズが発生しなかった場合にこれを設定します。
		/// </summary>
		private void ClearSearchingResult() {

			SearchingFriend.Text = "-";
			SearchingFriend.ImageAlign = ContentAlignment.MiddleCenter;
			SearchingFriend.ImageIndex = -1;
			ToolTipInfo.SetToolTip( SearchingFriend, null );

			SearchingEnemy.Text = "-";
			SearchingEnemy.ImageAlign = ContentAlignment.MiddleCenter;
			SearchingEnemy.ImageIndex = -1;
			ToolTipInfo.SetToolTip( SearchingEnemy, null );

		}

		/// <summary>
		/// 航空戦情報を設定します。
		/// </summary>
		private void SetAerialWarfare( PhaseAirBattle pd ) {

			//空対空戦闘
			if ( pd.IsStage1Available ) {

				AirSuperiority.Text = Constants.GetAirSuperiority( pd.AirSuperiority );
				ToolTipInfo.SetToolTip( AirSuperiority, string.Format( "航空伤害: {0}", pd.TotalDamage ) );

				int[] planeFriend = { pd.AircraftLostStage1Friend, pd.AircraftTotalStage1Friend };
				AirStage1Friend.Text = string.Format( "-{0}/{1}", planeFriend[0], planeFriend[1] );

				if ( planeFriend[1] > 0 && planeFriend[0] == planeFriend[1] )
					AirStage1Friend.ForeColor = Utility.Configuration.Config.UI.FailedColor;
				else
					AirStage1Friend.ForeColor = Utility.Configuration.Config.UI.ForeColor;

				int[] planeEnemy = { pd.AircraftLostStage1Enemy, pd.AircraftTotalStage1Enemy };
				AirStage1Enemy.Text = string.Format( "-{0}/{1}", planeEnemy[0], planeEnemy[1] );

				if ( planeEnemy[1] > 0 && planeEnemy[0] == planeEnemy[1] )
					AirStage1Enemy.ForeColor = Utility.Configuration.Config.UI.FailedColor;
				else
					AirStage1Enemy.ForeColor = Utility.Configuration.Config.UI.ForeColor;


				//触接
				int touchFriend = pd.TouchAircraftFriend;
				if ( touchFriend != -1 ) {
					AirStage1Friend.ImageAlign = ContentAlignment.MiddleLeft;
					AirStage1Friend.ImageIndex = (int)ResourceManager.EquipmentContent.Seaplane;
					ToolTipInfo.SetToolTip( AirStage1Friend, "触接中: " + KCDatabase.Instance.MasterEquipments[touchFriend].Name );
				} else {
					AirStage1Friend.ImageAlign = ContentAlignment.MiddleCenter;
					AirStage1Friend.ImageIndex = -1;
					ToolTipInfo.SetToolTip( AirStage1Friend, null );
				}

				int touchEnemy = pd.TouchAircraftEnemy;
				if ( touchEnemy != -1 ) {
					AirStage1Enemy.ImageAlign = ContentAlignment.MiddleLeft;
					AirStage1Enemy.ImageIndex = (int)ResourceManager.EquipmentContent.Seaplane;
					ToolTipInfo.SetToolTip( AirStage1Enemy, "触接中: " + KCDatabase.Instance.MasterEquipments[touchEnemy].Name );
				} else {
					AirStage1Enemy.ImageAlign = ContentAlignment.MiddleCenter;
					AirStage1Enemy.ImageIndex = -1;
					ToolTipInfo.SetToolTip( AirStage1Enemy, null );
				}

			} else {		//空対空戦闘発生せず

				AirSuperiority.Text = Constants.GetAirSuperiority( -1 );
				ToolTipInfo.SetToolTip( AirSuperiority, null );

				AirStage1Friend.Text = "-";
				AirStage1Friend.ForeColor = Utility.Configuration.Config.UI.ForeColor;
				AirStage1Friend.ImageAlign = ContentAlignment.MiddleCenter;
				AirStage1Friend.ImageIndex = -1;
				ToolTipInfo.SetToolTip( AirStage1Friend, null );

				AirStage1Enemy.Text = "-";
				AirStage1Enemy.ForeColor = Utility.Configuration.Config.UI.ForeColor;
				AirStage1Enemy.ImageAlign = ContentAlignment.MiddleCenter;
				AirStage1Enemy.ImageIndex = -1;
				ToolTipInfo.SetToolTip( AirStage1Enemy, null );
			}

			//艦対空戦闘
			if ( pd.IsStage2Available ) {

				int[] planeFriend = { pd.AircraftLostStage2Friend, pd.AircraftTotalStage2Friend };
				AirStage2Friend.Text = string.Format( "-{0}/{1}", planeFriend[0], planeFriend[1] );

				if ( planeFriend[1] > 0 && planeFriend[0] == planeFriend[1] )
					AirStage2Friend.ForeColor = Utility.Configuration.Config.UI.FailedColor;
				else
					AirStage2Friend.ForeColor = Utility.Configuration.Config.UI.ForeColor;

				int[] planeEnemy = { pd.AircraftLostStage2Enemy, pd.AircraftTotalStage2Enemy };
				AirStage2Enemy.Text = string.Format( "-{0}/{1}", planeEnemy[0], planeEnemy[1] );

				if ( planeEnemy[1] > 0 && planeEnemy[0] == planeEnemy[1] )
					AirStage2Enemy.ForeColor = Utility.Configuration.Config.UI.FailedColor;
				else
					AirStage2Enemy.ForeColor = Utility.Configuration.Config.UI.ForeColor;


				//対空カットイン
				if ( pd.IsAACutinAvailable ) {
					int cutinID = pd.AACutInKind;
					int cutinIndex = pd.AACutInIndex;

					AACutin.Text = "#" + ( cutinIndex + 1 );
					AACutin.ImageAlign = ContentAlignment.MiddleLeft;
					AACutin.ImageIndex = (int)ResourceManager.EquipmentContent.HighAngleGun;
					ToolTipInfo.SetToolTip( AACutin, string.Format(
						"対空カットイン: {0}\r\nカットイン種別: {1} ({2})",
						pd.AACutInShip.NameWithLevel,
						cutinID,
						Constants.GetAACutinKind( cutinID ) ) );

				} else {
					AACutin.Text = "対空砲火";
					AACutin.ImageAlign = ContentAlignment.MiddleCenter;
					AACutin.ImageIndex = -1;
					ToolTipInfo.SetToolTip( AACutin, null );
				}

			} else {	//艦対空戦闘発生せず
				AirStage2Friend.Text = "-";
				AirStage2Friend.ForeColor = Utility.Configuration.Config.UI.ForeColor;
				AirStage2Enemy.Text = "-";
				AirStage2Enemy.ForeColor = Utility.Configuration.Config.UI.ForeColor;
				AACutin.Text = "対空砲火";
				AACutin.ImageAlign = ContentAlignment.MiddleCenter;
				AACutin.ImageIndex = -1;
				ToolTipInfo.SetToolTip( AACutin, null );
			}


			AirStage2Friend.ImageAlign = ContentAlignment.MiddleCenter;
			AirStage2Friend.ImageIndex = -1;
			ToolTipInfo.SetToolTip( AirStage2Friend, null );
			AirStage2Enemy.ImageAlign = ContentAlignment.MiddleCenter;
			AirStage2Enemy.ImageIndex = -1;
			ToolTipInfo.SetToolTip( AirStage2Enemy, null );

		}


		/// <summary>
		/// 航空戦情報(航空戦)を設定します。
		/// 通常艦隊・連合艦隊両用です。
		/// </summary>
		private void SetAerialWarfareAirBattle( PhaseAirBattle pd1, PhaseAirBattle pd2 ) {

			//空対空戦闘
			if ( pd1.IsStage1Available ) {

				//二回目の空戦が存在するか
				bool isBattle2Enabled = pd2.IsStage1Available;

				AirSuperiority.Text = Constants.GetAirSuperiority( pd1.AirSuperiority );
				if ( isBattle2Enabled ) {
					ToolTipInfo.SetToolTip( AirSuperiority, string.Format( "第2次: {0}\r\n航空伤害: {1}",
						Constants.GetAirSuperiority( pd2.AirSuperiority ),
						pd1.TotalDamage + pd2.TotalDamage ) );
				} else {
					ToolTipInfo.SetToolTip( AirSuperiority, string.Format( "航空伤害: {0}", pd1.TotalDamage ) );
				}


				int[] planeFriend = {
					pd1.AircraftLostStage1Friend,
					pd1.AircraftTotalStage1Friend,
					( isBattle2Enabled ? pd2.AircraftLostStage1Friend : 0 ),
					( isBattle2Enabled ? pd2.AircraftTotalStage1Friend : 0 ),
				};
				AirStage1Friend.Text = string.Format( "-{0}/{1}", planeFriend[0] + planeFriend[2], planeFriend[1] );
				ToolTipInfo.SetToolTip( AirStage1Friend, string.Format( "第1次: -{0}/{1}\r\n第2次: -{2}/{3}\r\n",
					planeFriend[0], planeFriend[1], planeFriend[2], planeFriend[3] ) );

				if ( ( planeFriend[1] > 0 && planeFriend[0] == planeFriend[1] ) ||
					 ( planeFriend[3] > 0 && planeFriend[2] == planeFriend[3] ) )
					AirStage1Friend.ForeColor = Utility.Configuration.Config.UI.FailedColor;
				else
					AirStage1Friend.ForeColor = Utility.Configuration.Config.UI.ForeColor;


				int[] planeEnemy = { 
					pd1.AircraftLostStage1Enemy,
					pd1.AircraftTotalStage1Enemy,
					( isBattle2Enabled ? pd2.AircraftLostStage1Enemy : 0 ),
					( isBattle2Enabled ? pd2.AircraftTotalStage1Enemy : 0 ),
				};
				AirStage1Enemy.Text = string.Format( "-{0}/{1}", planeEnemy[0] + planeEnemy[2], planeEnemy[1] );
				ToolTipInfo.SetToolTip( AirStage1Enemy, string.Format( "第1次: -{0}/{1}\r\n第2次: -{2}/{3}\r\n",
					planeEnemy[0], planeEnemy[1], planeEnemy[2], planeEnemy[3] ) );

				if ( ( planeEnemy[1] > 0 && planeEnemy[0] == planeEnemy[1] ) ||
					 ( planeEnemy[3] > 0 && planeEnemy[2] == planeEnemy[3] ) )
					AirStage1Enemy.ForeColor = Utility.Configuration.Config.UI.FailedColor;
				else
					AirStage1Enemy.ForeColor = Utility.Configuration.Config.UI.ForeColor;


				//触接
				int[] touchFriend = { 
					pd1.TouchAircraftFriend,
					isBattle2Enabled ? pd2.TouchAircraftFriend : -1
					};
				if ( touchFriend[0] != -1 || touchFriend[1] != -1 ) {
					AirStage1Friend.ImageAlign = ContentAlignment.MiddleLeft;
					AirStage1Friend.ImageIndex = (int)ResourceManager.EquipmentContent.Seaplane;

					EquipmentDataMaster[] planes = { KCDatabase.Instance.MasterEquipments[touchFriend[0]], KCDatabase.Instance.MasterEquipments[touchFriend[1]] };
					ToolTipInfo.SetToolTip( AirStage1Friend, string.Format(
						"{0}触接中\r\n第1次: {1}\r\n第2次: {2}",
						ToolTipInfo.GetToolTip( AirStage1Friend ) ?? "",
						planes[0] != null ? planes[0].Name : "(なし)",
						planes[1] != null ? planes[1].Name : "(なし)"
						) );
				} else {
					AirStage1Friend.ImageAlign = ContentAlignment.MiddleCenter;
					AirStage1Friend.ImageIndex = -1;
					//ToolTipInfo.SetToolTip( AirStage1Friend, null );
				}

				int[] touchEnemy = {
					pd1.TouchAircraftEnemy,
					isBattle2Enabled ? pd2.TouchAircraftEnemy : -1
					};
				if ( touchEnemy[0] != -1 || touchEnemy[1] != -1 ) {
					AirStage1Enemy.ImageAlign = ContentAlignment.MiddleLeft;
					AirStage1Enemy.ImageIndex = (int)ResourceManager.EquipmentContent.Seaplane;

					EquipmentDataMaster[] planes = { KCDatabase.Instance.MasterEquipments[touchEnemy[0]], KCDatabase.Instance.MasterEquipments[touchEnemy[1]] };
					ToolTipInfo.SetToolTip( AirStage1Enemy, string.Format(
						"{0}触接中\r\n第1次: {1}\r\n第2次: {2}",
						ToolTipInfo.GetToolTip( AirStage1Enemy ) ?? "",
						planes[0] != null ? planes[0].Name : "(なし)",
						planes[1] != null ? planes[1].Name : "(なし)"
						) );
				} else {
					AirStage1Enemy.ImageAlign = ContentAlignment.MiddleCenter;
					AirStage1Enemy.ImageIndex = -1;
					//ToolTipInfo.SetToolTip( AirStage1Enemy, null );
				}

			} else {	//空対空戦闘発生せず(!?)
				AirSuperiority.Text = Constants.GetAirSuperiority( -1 );
				ToolTipInfo.SetToolTip( AirSuperiority, null );
				AirStage1Friend.Text = "-";
				AirStage1Friend.ForeColor = Utility.Configuration.Config.UI.ForeColor;
				ToolTipInfo.SetToolTip( AirStage1Friend, null );
				AirStage1Enemy.Text = "-";
				AirStage1Enemy.ForeColor = Utility.Configuration.Config.UI.ForeColor;
				ToolTipInfo.SetToolTip( AirStage1Enemy, null );
			}

			//艦対空戦闘
			if ( pd1.IsStage2Available ) {

				//二回目の空戦が存在するか
				bool isBattle2Enabled = pd2.IsStage2Available;


				int[] planeFriend = { 
					pd1.AircraftLostStage2Friend,
					pd1.AircraftTotalStage2Friend,
					( isBattle2Enabled ? pd2.AircraftLostStage2Friend : 0 ),
					( isBattle2Enabled ? pd2.AircraftTotalStage2Friend : 0 ),
				};
				AirStage2Friend.Text = string.Format( "-{0}/{1}", planeFriend[0] + planeFriend[2], planeFriend[1] );
				ToolTipInfo.SetToolTip( AirStage2Friend, string.Format( "第1次: -{0}/{1}\r\n第2次: -{2}/{3}\r\n",
					planeFriend[0], planeFriend[1], planeFriend[2], planeFriend[3] ) );

				if ( ( planeFriend[1] > 0 && planeFriend[0] == planeFriend[1] ) ||
					 ( planeFriend[3] > 0 && planeFriend[2] == planeFriend[3] ) )
					AirStage2Friend.ForeColor = Utility.Configuration.Config.UI.FailedColor;
				else
					AirStage2Friend.ForeColor = Utility.Configuration.Config.UI.ForeColor;


				int[] planeEnemy = { 
					pd1.AircraftLostStage2Enemy,
					pd1.AircraftTotalStage2Enemy,
					( isBattle2Enabled ? pd2.AircraftLostStage2Enemy : 0 ),
					( isBattle2Enabled ? pd2.AircraftTotalStage2Enemy : 0 ),
				};
				AirStage2Enemy.Text = string.Format( "-{0}/{1}", planeEnemy[0] + planeEnemy[2], planeEnemy[1] );
				ToolTipInfo.SetToolTip( AirStage2Enemy, string.Format( "第1次: -{0}/{1}\r\n第2次: -{2}/{3}\r\n{4}",
					planeEnemy[0], planeEnemy[1], planeEnemy[2], planeEnemy[3],
					isBattle2Enabled ? "" : "(第二次戦発生せず)" ) );			//DEBUG

				if ( ( planeEnemy[1] > 0 && planeEnemy[0] == planeEnemy[1] ) ||
					 ( planeEnemy[3] > 0 && planeEnemy[2] == planeEnemy[3] ) )
					AirStage2Enemy.ForeColor = Utility.Configuration.Config.UI.FailedColor;
				else
					AirStage2Enemy.ForeColor = Utility.Configuration.Config.UI.ForeColor;


				//対空カットイン
				{
					bool[] fire = new bool[] { pd1.IsAACutinAvailable, isBattle2Enabled && pd2.IsAACutinAvailable };
					int[] cutinID = new int[] {
						fire[0] ? pd1.AACutInKind : -1,
						fire[1] ? pd2.AACutInKind : -1,
					};
					int[] cutinIndex = new int[] {
						fire[0] ? pd1.AACutInIndex : -1,
						fire[1] ? pd2.AACutInIndex : -1,
					};

					if ( fire[0] || fire[1] ) {

						AACutin.Text = string.Format( "#{0}/{1}", fire[0] ? ( cutinIndex[0] + 1 ).ToString() : "-", fire[1] ? ( cutinIndex[1] + 1 ).ToString() : "-" );
						AACutin.ImageAlign = ContentAlignment.MiddleLeft;
						AACutin.ImageIndex = (int)ResourceManager.EquipmentContent.HighAngleGun;

						StringBuilder sb = new StringBuilder();
						sb.AppendLine( "対空カットイン" );
						for ( int i = 0; i < 2; i++ ) {
							if ( fire[i] ) {
								sb.AppendFormat( "第{0}次: {1}\r\nカットイン種別: {2} ({3})\r\n",
									i + 1,
									( i == 0 ? pd1 : pd2 ).AACutInShip.NameWithLevel,
									cutinID[i],
									Constants.GetAACutinKind( cutinID[i] ) );
							} else {
								sb.AppendFormat( "第{0}次: (発動せず)\r\n",
									i + 1 );
							}
						}
						ToolTipInfo.SetToolTip( AACutin, sb.ToString() );

					} else {
						AACutin.Text = "対空砲火";
						AACutin.ImageAlign = ContentAlignment.MiddleCenter;
						AACutin.ImageIndex = -1;
						ToolTipInfo.SetToolTip( AACutin, null );
					}
				}

			} else {	//艦対空戦闘発生せず
				AirStage2Friend.Text = "-";
				AirStage2Friend.ForeColor = Utility.Configuration.Config.UI.ForeColor;
				ToolTipInfo.SetToolTip( AirStage2Friend, null );
				AirStage2Enemy.Text = "-";
				AirStage2Enemy.ForeColor = Utility.Configuration.Config.UI.ForeColor;
				ToolTipInfo.SetToolTip( AirStage2Enemy, null );
				AACutin.Text = "対空砲火";
				AACutin.ImageAlign = ContentAlignment.MiddleCenter;
				AACutin.ImageIndex = -1;
				ToolTipInfo.SetToolTip( AACutin, null );
			}

			AirStage2Friend.ImageAlign = ContentAlignment.MiddleCenter;
			AirStage2Friend.ImageIndex = -1;
			AirStage2Enemy.ImageAlign = ContentAlignment.MiddleCenter;
			AirStage2Enemy.ImageIndex = -1;

		}


		/// <summary>
		/// 航空戦情報をクリアします。
		/// </summary>
		private void ClearAerialWarfare() {
			AirSuperiority.Text = "-";
			ToolTipInfo.SetToolTip( AirSuperiority, null );

			AirStage1Friend.Text = "-";
			AirStage1Friend.ForeColor = Utility.Configuration.Config.UI.ForeColor;
			AirStage1Friend.ImageAlign = ContentAlignment.MiddleCenter;
			AirStage1Friend.ImageIndex = -1;
			ToolTipInfo.SetToolTip( AirStage1Friend, null );

			AirStage1Enemy.Text = "-";
			AirStage1Enemy.ForeColor = Utility.Configuration.Config.UI.ForeColor;
			AirStage1Enemy.ImageAlign = ContentAlignment.MiddleCenter;
			AirStage1Enemy.ImageIndex = -1;
			ToolTipInfo.SetToolTip( AirStage1Enemy, null );

			AirStage2Friend.Text = "-";
			AirStage2Friend.ForeColor = Utility.Configuration.Config.UI.ForeColor;
			AirStage2Friend.ImageAlign = ContentAlignment.MiddleCenter;
			AirStage2Friend.ImageIndex = -1;
			ToolTipInfo.SetToolTip( AirStage2Friend, null );

			AirStage2Enemy.Text = "-";
			AirStage2Enemy.ImageAlign = ContentAlignment.MiddleCenter;
			AirStage2Enemy.ForeColor = Utility.Configuration.Config.UI.ForeColor;
			AirStage2Enemy.ImageIndex = -1;
			ToolTipInfo.SetToolTip( AirStage2Enemy, null );

			AACutin.Text = "-";
			AACutin.ImageAlign = ContentAlignment.MiddleCenter;
			AACutin.ImageIndex = -1;
			ToolTipInfo.SetToolTip( AACutin, null );
		}


		/// <summary>
		/// 両軍のHPゲージを設定します。
		/// </summary>
		private void SetHPNormal( BattleData bd ) {

			TableTop.SuspendLayout();
			TableBottom.SuspendLayout();

			float damageWidth = ( Utility.Configuration.Config.FormBattle.IsShortDamage ? DamageWidth / 2 : DamageWidth );
			float singleWidth = TableBottom.ColumnStyles[0].Width;

			TableTop.ColumnStyles[1].Width = singleWidth;
			TableTop.ClientSize = new System.Drawing.Size( (int)( 3 * singleWidth ), TableTop.ClientSize.Height );

			TableBottom.ColumnStyles[1].Width = damageWidth;
			TableBottom.ColumnStyles[3].Width = 0;
			TableBottom.ColumnStyles[2].Width = singleWidth - damageWidth;
			TableBottom.ClientSize = new System.Drawing.Size( (int)( 3 * singleWidth ), TableBottom.ClientSize.Height );


			KCDatabase db = KCDatabase.Instance;
			bool isPractice = ( bd.BattleType & BattleData.BattleTypeFlag.Practice ) != 0;

			var initialHPs = bd.Initial.InitialHPs;
			var maxHPs = bd.Initial.MaxHPs;
			var resultHPs = bd.ResultHPs;
			var attackDamages = bd.AttackDamages;
			var attackAirDamages = bd.AttackAirDamages;

			for ( int i = 0; i < 12; i++ ) {
				if ( initialHPs[i] != -1 ) {
					HPBars[i].Value = resultHPs[i];
					HPBars[i].PrevValue = initialHPs[i];
					HPBars[i].MaximumValue = maxHPs[i];
					HPBars[i].BackColor = Utility.Configuration.Config.UI.BackColor;
					HPBars[i].Visible = true;
					DamageLabels[i].ImageIndex = (int)ResourceManager.IconContent.ConditionNormal;
					DamageLabels[i].Visible = true;
				} else {
					HPBars[i].Visible = false;
					DamageLabels[i].Visible = false;
				}
			}


			for ( int i = 0; i < 6; i++ ) {
				if ( initialHPs[i] != -1 ) {
					ShipData ship = bd.Initial.FriendFleet.MembersInstance[i];

					DamageLabels[i].Text = ( attackAirDamages[i] + attackDamages[i] ) > 0 ? ( attackAirDamages[i] + attackDamages[i] ).ToString() : string.Empty;

					ToolTipInfo.SetToolTip( HPBars[i],
						string.Format( "{0} {1} Lv. {2}\r\nHP: ({3} → {4})/{5} ({6}) [{7}]\r\n造成伤害: {8}",
							ship.MasterShip.ShipTypeName,
							ship.MasterShip.NameWithClass,
							ship.Level,
							Math.Max( HPBars[i].PrevValue, 0 ),
							Math.Max( HPBars[i].Value, 0 ),
							HPBars[i].MaximumValue,
							HPBars[i].Value - HPBars[i].PrevValue,
							Constants.GetDamageState( (double)HPBars[i].Value / HPBars[i].MaximumValue, isPractice, ship.MasterShip.IsLandBase ),
							( attackAirDamages[i] > 0 ) ? string.Format( "{0} (+{1})", attackDamages[i], attackAirDamages[i] ) : attackDamages[i].ToString()
							)
						);
				}
			}

			for ( int i = 0; i < 6; i++ ) {
				if ( initialHPs[i + 6] != -1 ) {
					ShipDataMaster ship = bd.Initial.EnemyMembersInstance[i];

					ToolTipInfo.SetToolTip( HPBars[i + 6],
						string.Format( "{0} {1} Lv. {2}\r\nHP: ({3} → {4})/{5} ({6}) [{7}]",
							ship.ShipTypeName,
							ship.NameWithClass,
							bd.Initial.EnemyLevels[i],
							Math.Max( HPBars[i + 6].PrevValue, 0 ),
							Math.Max( HPBars[i + 6].Value, 0 ),
							HPBars[i + 6].MaximumValue,
							HPBars[i + 6].Value - HPBars[i + 6].PrevValue,
							Constants.GetDamageState( (double)HPBars[i + 6].Value / HPBars[i + 6].MaximumValue, isPractice, ship.IsLandBase )
							)
						);
				}
			}

			//HPBars[bd.MVPShipIndex].BackColor = Color.Moccasin;
			DamageLabels[bd.MVPShipIndex].ImageIndex = (int)ResourceManager.IconContent.ConditionSparkle;

			FleetCombined.Visible = false;
			for ( int i = 12; i < 18; i++ ) {
				HPBars[i].Visible = false;
			}

			TableTop.ResumeLayout();
			TableBottom.ResumeLayout();

		}

		/// <summary>
		/// 両軍のHPゲージを設定します。(連合艦隊用)
		/// </summary>
		private void SetHPCombined( BattleData bd ) {

			TableTop.SuspendLayout();
			TableBottom.SuspendLayout();

			float damageWidth = ( Utility.Configuration.Config.FormBattle.IsShortDamage ? DamageWidth / 2 : DamageWidth );
			float singleWidth = TableBottom.ColumnStyles[0].Width;

			TableTop.ColumnStyles[1].Width = singleWidth + 2 * damageWidth;
			TableTop.ClientSize = new System.Drawing.Size( (int)( 3 * singleWidth + 2 * damageWidth ), TableTop.ClientSize.Height );

			TableBottom.ColumnStyles[1].Width =
			TableBottom.ColumnStyles[3].Width = damageWidth;
			TableBottom.ColumnStyles[2].Width = singleWidth;
			TableBottom.ClientSize = new System.Drawing.Size( (int)( 3 * singleWidth + 2 * damageWidth ), TableBottom.ClientSize.Height );


			KCDatabase db = KCDatabase.Instance;
			bool isPractice = ( bd.BattleType & BattleData.BattleTypeFlag.Practice ) != 0;

			var initialHPs = bd.Initial.InitialHPs;
			var maxHPs = bd.Initial.MaxHPs;
			var resultHPs = bd.ResultHPs;
			var attackDamages = bd.AttackDamages;
			var attackAirDamages = bd.AttackAirDamages;


			FleetCombined.Visible = true;
			for ( int i = 0; i < 18; i++ ) {
				if ( initialHPs[i] != -1 ) {
					HPBars[i].Value = resultHPs[i];
					HPBars[i].PrevValue = initialHPs[i];
					HPBars[i].MaximumValue = maxHPs[i];
					HPBars[i].Visible = true;
					if ( i < 6 )
					{
						DamageLabels[i].ImageIndex = (int)ResourceManager.IconContent.ConditionNormal;
						DamageLabels[i].Visible = true;
					}
					else if ( i >= 12 )
					{
						DamageLabels[i - 6].ImageIndex = (int)ResourceManager.IconContent.ConditionNormal;
						DamageLabels[i - 6].Visible = true;
					}
				} else {
					HPBars[i].Visible = false;
					if ( i < 6 )
						DamageLabels[i].Visible = false;
					else if ( i >= 12 )
						DamageLabels[i - 6].Visible = false;
				}
			}



			for ( int i = 0; i < 6; i++ ) {
				if ( initialHPs[i] != -1 ) {
					ShipData ship = bd.Initial.FriendFleet.MembersInstance[i];
					bool isEscaped =  bd.Initial.FriendFleet.EscapedShipList.Contains( ship.MasterID );

					DamageLabels[i].Text = ( attackAirDamages[i] + attackDamages[i] ) > 0 ? ( attackAirDamages[i] + attackDamages[i] ).ToString() : string.Empty;

					ToolTipInfo.SetToolTip( HPBars[i],
						string.Format( "{0} Lv. {1}\r\nHP: ({2} → {3})/{4} ({5}) [{6}]\r\n\r\n造成伤害: {7}",
							ship.MasterShip.NameWithClass,
							ship.Level,
							Math.Max( HPBars[i].PrevValue, 0 ),
							Math.Max( HPBars[i].Value, 0 ),
							HPBars[i].MaximumValue,
							HPBars[i].Value - HPBars[i].PrevValue,
							Constants.GetDamageState( (double)HPBars[i].Value / HPBars[i].MaximumValue, isPractice, ship.MasterShip.IsLandBase, isEscaped ),
							( attackAirDamages[i] > 0 ) ? string.Format( "{0} (+{1})", attackDamages[i], attackAirDamages[i] ) : attackDamages[i].ToString()
							)
						);

					if ( isEscaped ) HPBars[i].BackColor = Color.Silver;
					else HPBars[i].BackColor = Utility.Configuration.Config.UI.BackColor;
				}
			}

			for ( int i = 0; i < 6; i++ ) {
				if ( initialHPs[i + 6] != -1 ) {
					ShipDataMaster ship = bd.Initial.EnemyMembersInstance[i];

					ToolTipInfo.SetToolTip( HPBars[i + 6],
						string.Format( "{0} Lv. {1}\r\nHP: ({2} → {3})/{4} ({5}) [{6}]",
							ship.NameWithClass,
							bd.Initial.EnemyLevels[i],
							Math.Max( HPBars[i + 6].PrevValue, 0 ),
							Math.Max( HPBars[i + 6].Value, 0 ),
							HPBars[i + 6].MaximumValue,
							HPBars[i + 6].Value - HPBars[i + 6].PrevValue,
							Constants.GetDamageState( (double)HPBars[i + 6].Value / HPBars[i + 6].MaximumValue, isPractice, ship.IsLandBase )
							)
						);
				}
			}

			for ( int i = 0; i < 6; i++ ) {
				if ( initialHPs[i + 12] != -1 ) {
					ShipData ship = db.Fleet[2].MembersInstance[i];
					bool isEscaped = db.Fleet[2].EscapedShipList.Contains( ship.MasterID );

					DamageLabels[i + 6].Text = ( attackAirDamages[i + 12] + attackDamages[i + 12] ) > 0 ? ( attackAirDamages[i + 12] + attackDamages[i + 12] ).ToString() : string.Empty;

					ToolTipInfo.SetToolTip( HPBars[i + 12],
						string.Format( "{0} Lv. {1}\r\nHP: ({2} → {3})/{4} ({5}) [{6}]\r\n\r\n造成伤害: {7}",
							ship.MasterShip.NameWithClass,
							ship.Level,
							Math.Max( HPBars[i + 12].PrevValue, 0 ),
							Math.Max( HPBars[i + 12].Value, 0 ),
							HPBars[i + 12].MaximumValue,
							HPBars[i + 12].Value - HPBars[i + 12].PrevValue,
							Constants.GetDamageState( (double)HPBars[i + 12].Value / HPBars[i + 12].MaximumValue, ship.MasterShip.IsLandBase, isEscaped ),
							( attackAirDamages[i + 12] > 0 ) ? string.Format( "{0} (+{1})", attackDamages[i + 12], attackAirDamages[i + 12] ) : attackDamages[i + 12].ToString()
							)
						);

					if ( isEscaped ) HPBars[i + 12].BackColor = Color.Silver;
					else HPBars[i + 12].BackColor = Utility.Configuration.Config.UI.BackColor;
				}
			}


			//HPBars[bd.MVPShipIndex].BackColor = Color.Moccasin;
			//HPBars[12 + bd.MVPShipCombinedIndex].BackColor = Color.Moccasin;
			DamageLabels[bd.MVPShipIndex].ImageIndex = (int)ResourceManager.IconContent.ConditionSparkle;
			DamageLabels[6 + bd.MVPShipCombinedIndex].ImageIndex = (int)ResourceManager.IconContent.ConditionSparkle;

			TableTop.ResumeLayout();
			TableBottom.ResumeLayout();
		}


		/// <summary>
		/// 勝利ランクを計算します。連合艦隊は情報が少ないので正確ではありません。
		/// </summary>
		/// <param name="countFriend">戦闘に参加した自軍艦数。</param>
		/// <param name="countEnemy">戦闘に参加した敵軍艦数。</param>
		/// <param name="sunkFriend">撃沈された自軍艦数。</param>
		/// <param name="sunkEnemy">撃沈した敵軍艦数。</param>
		/// <param name="friendrate">自軍損害率。</param>
		/// <param name="enemyrate">敵軍損害率。</param>
		/// <param name="defeatFlagship">敵旗艦を撃沈しているか。</param>
		/// <remarks>thanks: nekopanda</remarks>
		private static int GetWinRank(
			int countFriend, int countEnemy,
			int sunkFriend, int sunkEnemy,
			double friendrate, double enemyrate,
			bool defeatFlagship ) {

			int rifriend = (int)( friendrate * 100 );
			int rienemy = (int)( enemyrate * 100 );

			bool borderC = rienemy > ( 0.9 * rifriend );
			bool borderB = rienemy > ( 2.5 * rifriend );

			if ( sunkFriend == 0 ) {	// 味方轟沈数ゼロ
				if ( enemyrate >= 1.0 ) {	// 敵を殲滅した
					if ( friendrate <= 0.0 ) {	// 味方ダメージゼロ
						return 7;	// SS
					}
					return 6;	// S

				} else if ( sunkEnemy >= (int)Math.Round( countEnemy * 0.6 ) ) {	// 半数以上撃破
					return 5;	// A

				} else if ( defeatFlagship || borderB ) {	// 敵旗艦を撃沈 or 戦果ゲージが2.5倍以上
					return 4;	// B
				}

			} else {
				if ( enemyrate >= 1.0 ) {	// 敵を殲滅した
					return 4;	// B
				}
				// 敵旗艦を撃沈 and 味方轟沈数 < 敵撃沈数
				if ( defeatFlagship && ( sunkFriend < sunkEnemy ) ) {
					return 4;	// B
				}
				// 戦果ゲージが2.5倍以上
				if ( borderB ) {
					return 4;	// B
				}
				// 敵旗艦を撃沈
				// TODO: 味方の轟沈艦が２隻以上ある場合、敵旗艦を撃沈してもDになる場合がある
				if ( defeatFlagship ) {
					return 3;	// C
				}
			}

			// 戦果ゲージが0.9倍以上
			if ( borderC ) {
				return 3;	// C
			}
			// 轟沈艦があり かつ 残った艦が１隻のみ
			if ( ( sunkFriend > 0 ) && ( ( countFriend - sunkFriend ) == 1 ) ) {
				return 1;	// E
			}

			// 残りはD
			return 2;	// D
		}


		/// <summary>
		/// 損害率と戦績予測を設定します。
		/// </summary>
		private void SetDamageRateNormal( BattleData bd, int[] initialHPs ) {

			int friendbefore = 0;
			int friendafter = 0;
			double friendrate;
			int enemybefore = 0;
			int enemyafter = 0;
			double enemyrate;

			var resultHPs = bd.ResultHPs;

			for ( int i = 0; i < 6; i++ ) {
				friendbefore += Math.Max( initialHPs[i], 0 );
				friendafter += Math.Max( resultHPs[i], 0 );
				enemybefore += Math.Max( initialHPs[i + 6], 0 );
				enemyafter += Math.Max( resultHPs[i + 6], 0 );
			}

			friendrate = ( (double)( friendbefore - friendafter ) / friendbefore );
			DamageFriend.Text = string.Format( "{0:p1}", friendrate );
			enemyrate = ( (double)( enemybefore - enemyafter ) / enemybefore );
			DamageEnemy.Text = string.Format( "{0:p1}", enemyrate );


			//戦績判定
			{
				int countFriend = bd.Initial.FriendFleet.Members.Count( v => v != -1 );
				int countEnemy = ( bd.Initial.EnemyMembers.Count( v => v != -1 ) );
				int sunkFriend = resultHPs.Take( countFriend ).Count( v => v <= 0 );
				int sunkEnemy = resultHPs.Skip( 6 ).Take( countEnemy ).Count( v => v <= 0 );

				int rank = GetWinRank( countFriend, countEnemy, sunkFriend, sunkEnemy, friendrate, enemyrate, resultHPs[6] <= 0 );


				WinRank.Text = Constants.GetWinRank( rank );
				WinRank.ForeColor = rank >= 4 ? Utility.Configuration.Config.UI.ForeColor : Utility.Configuration.Config.UI.FailedColor;
			}
		}


		/// <summary>
		/// 損害率と戦績予測を設定します(連合艦隊用)。
		/// </summary>
		private void SetDamageRateCombined( BattleData bd, int[] initialHPs ) {

			int friendbefore = 0;
			int friendafter = 0;
			double friendrate;
			int enemybefore = 0;
			int enemyafter = 0;
			double enemyrate;

			var resultHPs = bd.ResultHPs;

			for ( int i = 0; i < 6; i++ ) {
				friendbefore += Math.Max( initialHPs[i], 0 );
				friendafter += Math.Max( resultHPs[i], 0 );
				friendbefore += Math.Max( initialHPs[i + 12], 0 );
				friendafter += Math.Max( resultHPs[i + 12], 0 );
				enemybefore += Math.Max( initialHPs[i + 6], 0 );
				enemyafter += Math.Max( resultHPs[i + 6], 0 );
			}

			friendrate = ( (double)( friendbefore - friendafter ) / friendbefore );
			DamageFriend.Text = string.Format( "{0:p1}", friendrate );
			enemyrate = ( (double)( enemybefore - enemyafter ) / enemybefore );
			DamageEnemy.Text = string.Format( "{0:p1}", enemyrate );


			//戦績判定
			{
				int countFriend = bd.Initial.FriendFleet.Members.Count( v => v != -1 );
				int countFriendCombined = KCDatabase.Instance.Fleet[2].Members.Count( v => v != -1 );
				int countEnemy = ( bd.Initial.EnemyMembers.Count( v => v != -1 ) );
				int sunkFriend = resultHPs.Take( countFriend ).Count( v => v <= 0 ) + resultHPs.Skip( 12 ).Take( countFriendCombined ).Count( v => v <= 0 );
				int sunkEnemy = resultHPs.Skip( 6 ).Take( countEnemy ).Count( v => v <= 0 );

				int rank = GetWinRank( countFriend + countFriendCombined, countEnemy, sunkFriend, sunkEnemy, friendrate, enemyrate, resultHPs[6] <= 0 );


				WinRank.Text = Constants.GetWinRank( rank );
				WinRank.ForeColor = rank >= 4 ? Utility.Configuration.Config.UI.ForeColor : Utility.Configuration.Config.UI.FailedColor;
			}
		}


		/// <summary>
		/// 夜戦における各種表示を設定します。
		/// </summary>
		/// <param name="hp">戦闘開始前のHP。</param>
		/// <param name="isCombined">連合艦隊かどうか。</param>
		/// <param name="bd">戦闘データ。</param>
		private void SetNightBattleEvent( PhaseNightBattle pd ) {

			FleetData fleet = pd.FriendFleet;

			//味方探照灯判定
			{
				int index = pd.SearchlightIndexFriend;

				if ( index != -1 ) {
					ShipData ship = fleet.MembersInstance[index];

					AirStage1Friend.Text = "#" + ( index + 1 );
					AirStage1Friend.ForeColor = Utility.Configuration.Config.UI.ForeColor;
					AirStage1Friend.ImageAlign = ContentAlignment.MiddleLeft;
					AirStage1Friend.ImageIndex = (int)ResourceManager.EquipmentContent.Searchlight;
					ToolTipInfo.SetToolTip( AirStage1Friend, "探照灯照射: " + ship.NameWithLevel );
				} else {
					ToolTipInfo.SetToolTip( AirStage1Friend, null );
				}
			}

			//敵探照灯判定
			{
				int index = pd.SearchlightIndexEnemy;
				if ( index != -1 ) {
					AirStage1Enemy.Text = "#" + ( index + 1 );
					AirStage1Enemy.ForeColor = Utility.Configuration.Config.UI.ForeColor;	
					AirStage1Enemy.ImageAlign = ContentAlignment.MiddleLeft;
					AirStage1Enemy.ImageIndex = (int)ResourceManager.EquipmentContent.Searchlight;
					ToolTipInfo.SetToolTip( AirStage1Enemy, "探照灯照射: " + pd.SearchlightEnemyInstance.NameWithClass );
				} else {
					ToolTipInfo.SetToolTip( AirStage1Enemy, null );
				}
			}


			//夜間触接判定
			if ( pd.TouchAircraftFriend != -1 ) {
				SearchingFriend.Text = "夜間触接";
				SearchingFriend.ImageIndex = (int)ResourceManager.EquipmentContent.Seaplane;
				SearchingFriend.ImageAlign = ContentAlignment.MiddleLeft;
				ToolTipInfo.SetToolTip( SearchingFriend, "夜間触接中: " + KCDatabase.Instance.MasterEquipments[pd.TouchAircraftFriend].Name );
			} else {
				ToolTipInfo.SetToolTip( SearchingFriend, null );
			}

			if ( pd.TouchAircraftEnemy != -1 ) {
				SearchingEnemy.Text = "夜間触接";
				SearchingEnemy.ImageIndex = (int)ResourceManager.EquipmentContent.Seaplane;
				SearchingFriend.ImageAlign = ContentAlignment.MiddleLeft;
				ToolTipInfo.SetToolTip( SearchingEnemy, "夜間触接中: " + KCDatabase.Instance.MasterEquipments[pd.TouchAircraftEnemy].Name );
			} else {
				ToolTipInfo.SetToolTip( SearchingEnemy, null );
			}

			//照明弾投射判定
			{
				int index = pd.FlareIndexFriend;

				if ( index != -1 ) {
					AirStage2Friend.Text = "#" + ( index + 1 );
					AirStage2Friend.ForeColor = Utility.Configuration.Config.UI.ForeColor;
					AirStage2Friend.ImageAlign = ContentAlignment.MiddleLeft;
					AirStage2Friend.ImageIndex = (int)ResourceManager.EquipmentContent.Flare;
					ToolTipInfo.SetToolTip( AirStage2Friend, "照明弾投射: " + fleet.MembersInstance[index].NameWithLevel );

				} else {
					ToolTipInfo.SetToolTip( AirStage2Friend, null );
				}
			}

			{
				int index = pd.FlareIndexEnemy;

				if ( index != -1 ) {
					AirStage2Enemy.Text = "#" + ( index + 1 );
					AirStage2Enemy.ForeColor = Utility.Configuration.Config.UI.ForeColor;
					AirStage2Enemy.ImageAlign = ContentAlignment.MiddleLeft;
					AirStage2Enemy.ImageIndex = (int)ResourceManager.EquipmentContent.Flare;
					ToolTipInfo.SetToolTip( AirStage2Enemy, "照明弾投射: " + pd.FlareEnemyInstance.NameWithClass );
				} else {
					ToolTipInfo.SetToolTip( AirStage2Enemy, null );
				}
			}
		}



		void ConfigurationChanged() {

			TableTop.SuspendLayout();
			TableBottom.SuspendLayout();

			MainFont = TableTop.Font = TableBottom.Font = Font = Utility.Configuration.Config.UI.MainFont;
			SubFont = Utility.Configuration.Config.UI.SubFont;

			LinePen = new Pen( Utility.Configuration.Config.UI.LineColor.ColorData );

			bool shorten = Utility.Configuration.Config.FormBattle.IsShortDamage;
			bool isCombined = ( KCDatabase.Instance.Battle.BattleMode & BattleManager.BattleModes.CombinedMask ) != 0;

			float damageWidth = ( shorten ? DamageWidth / 2 : DamageWidth );
			float singleWidth = TableBottom.ColumnStyles[0].Width;

			if ( isCombined ) {

				TableTop.ColumnStyles[1].Width = singleWidth + 2 * damageWidth;
				TableTop.ClientSize = new System.Drawing.Size( (int)( 3 * singleWidth + 2 * damageWidth ), TableTop.ClientSize.Height );

				TableBottom.ColumnStyles[1].Width =
				TableBottom.ColumnStyles[3].Width = damageWidth;
				TableBottom.ColumnStyles[2].Width = singleWidth;
				TableBottom.ClientSize = new System.Drawing.Size( (int)( 3 * singleWidth + 2 * damageWidth ), TableBottom.ClientSize.Height );
			} else {

				TableTop.ColumnStyles[1].Width = singleWidth;
				TableTop.ClientSize = new System.Drawing.Size( (int)( 3 * singleWidth ), TableTop.ClientSize.Height );

				TableBottom.ColumnStyles[1].Width = damageWidth;
				TableBottom.ColumnStyles[3].Width = 0;
				TableBottom.ColumnStyles[2].Width = singleWidth - damageWidth;
				TableBottom.ClientSize = new System.Drawing.Size( (int)( 3 * singleWidth ), TableBottom.ClientSize.Height );
			}

			TableTop.ResumeLayout();
			TableBottom.ResumeLayout();

			if ( DamageLabels == null )
				return;


			for ( int i = 0; i < 12; i++ ) {

				DamageLabels[i].ShowText = !shorten;
			}

		}



		private void TableTop_CellPaint( object sender, TableLayoutCellPaintEventArgs e ) {
			if ( e.Row == 1 || e.Row == 3 )
				e.Graphics.DrawLine( LinePen, e.CellBounds.X, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1 );
		}

		private void TableBottom_CellPaint( object sender, TableLayoutCellPaintEventArgs e ) {
			if ( e.Row == 7 )
				e.Graphics.DrawLine( LinePen, e.CellBounds.X, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1 );
		}


		public override string GetPersistString()
		{
			return "Battle";
		}


	}

}
