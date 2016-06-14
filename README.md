
## 七四式電子観測儀
---

現在鋭意開発中の艦これ補助ブラウザです。  

[![Build status](https://ci.appveyor.com/api/projects/status/7jciq1cqj6qto3ij/branch/makai)](https://ci.appveyor.com/project/tsanie/electronicobserver/branch/makai/artifacts)

### 実装されている機能
---

![](https://github.com/andanteyk/ElectronicObserver/wiki/media/mainimage2.png)

各機能はそれぞれウィンドウとして独立しており、自由にドッキング・タブ化するなどしてレイアウト可能です。  
以下では概略を紹介します。**詳しくは[Wikiを参照](https://github.com/andanteyk/ElectronicObserver/wiki)してください。**  

* 内蔵ブラウザ(スクリーンショット, ズーム, ミュートなど)
* 艦隊(状態(遠征中, 未補給など), 制空戦力, 索敵能力)
    * 個艦(Lv, HP, コンディション, 補給, 装備スロット)
    * 艦隊一覧(全艦隊の状態を一目で確認できます)
    * グループ(フィルタリングで艦娘情報を表示)
* 入渠(入渠艦, 残り時間)
* 工廠(建造中の艦名, 残り時間)
* 司令部(提督情報, 資源情報)
* 羅針盤(次の進路, 敵編成・獲得資源等のイベント予測)
* 戦闘(戦闘予測・結果表示)
* 情報(中破絵未回収艦一覧, 海域ゲージ残量など)
* 任務(達成回数/最大値表示)
* 図鑑(艦船/装備図鑑)
* 装備一覧
* 通知(遠征・入渠完了, 大破進撃警告など)
* レコード(開発・建造・ドロップ艦の記録など)
* ウィンドウキャプチャ(他プログラムのウィンドウを取り込む)

なお、全ての機能において艦これ本体の送受信する情報に干渉する操作は行っていません。

### ダウンロード
---

*このリンクの更新は遅れる可能性があります。最新版は[こちら](http://electronicobserver.blog.fc2.com/)で確認してください。*  

[ver. 2.3.1 (2016/06/10)](http://bit.ly/24FrlWO)  

[更新内容・履歴はこちらで確認できます。](https://github.com/andanteyk/ElectronicObserver/wiki/ChangeLog)  

### 開発者の皆様へ
---

[Other/Information/](https://github.com/andanteyk/ElectronicObserver/tree/develop/ElectronicObserver/Other/Information) に艦これのAPIや仕様についての情報を掲載しています。  
ご自由にお持ちください。但し内容は保証しません。  

また、実行する際は実行フォルダに気を付けてください。  
Assets.zip をプログラムと同じ場所にコピーするか、実行フォルダの設定を変更してください。

[ライセンスは MIT License です。](https://github.com/andanteyk/ElectronicObserver/blob/master/LICENSE)  

### 使用しているライブラリ
---

* [DynamicJson](http://dynamicjson.codeplex.com/) (JSON データの読み書き) - [Ms-PL](https://github.com/andanteyk/ElectronicObserver/blob/master/Licenses/Ms-PL.txt)
* [DockPanel Suite](http://dockpanelsuite.com/) (ウィンドウレイアウト) - [MIT License](https://github.com/andanteyk/ElectronicObserver/blob/master/Licenses/DockPanelSuite.txt)
* [Nekoxy](https://github.com/veigr/Nekoxy) (通信キャプチャ) - [MIT License](https://github.com/andanteyk/ElectronicObserver/blob/master/Licenses/Nekoxy.txt)
    * [TrotiNet](http://trotinet.sourceforge.net/) - [GNU Lesser General Public License v3.0](https://github.com/andanteyk/ElectronicObserver/blob/master/Licenses/LGPL.txt)
        * [log4net](https://logging.apache.org/log4net/) - [Apache License version 2.0](https://github.com/andanteyk/ElectronicObserver/blob/master/Licenses/Apache.txt)

### 連絡先など
---

* 配布サイト:[ブルネイ工廠電気実験部](http://electronicobserver.blog.fc2.com/) (バグ報告・要望等はこちらにお願いします)
* 開発:[Andante](https://twitter.com/andanteyk)
