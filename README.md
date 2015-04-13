
## 七四式電子観測儀
---

現在鋭意開発中の艦これ補助ブラウザです。  

### 実装されている機能
---

![](https://github.com/andanteyk/ElectronicObserver/wiki/media/mainimage2.png)

各機能はそれぞれウィンドウとして独立しており、自由にドッキング・タブ化するなどしてレイアウト可能です。  
以下では概略を紹介します。**詳しくは[Wikiを参照](https://github.com/andanteyk/ElectronicObserver/wiki)してください。**  

* 内蔵ブラウザ(スクリーンショット, ズーム, ミュートなど)
* 艦隊(状態(遠征中, 未補給など), 制空戦力, 索敵能力)
    * 個艦(Lv, HP, コンディション, 補給, 装備スロット)
    * 艦隊一覧(全艦隊の状態を一目で確認できます)
    * グループ(任意の艦娘の状態をグループ化して表示できます)
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

なお、全ての機能において艦これ本体の送受信する情報に干渉する操作は行っていません。

### ダウンロード
---

*このリンクの更新は遅れる可能性があります。最新版は[こちら](http://electronicobserver.blog.fc2.com/)で確認してください。*  

[ver. 1.1.1 (2015/04/11)](http://bit.ly/1DSY4QD)  

更新内容・履歴は[こちら](https://github.com/andanteyk/ElectronicObserver/wiki/ChangeLog)で確認できます。  

### 開発者の皆様へ
---

[Other/Information/](https://github.com/andanteyk/ElectronicObserver/tree/develop/ElectronicObserver/Other/Information) に艦これのAPIや仕様についての情報を掲載しています。  
ご自由にお持ちください。但し内容は保証しません。  

また、実行する際は実行フォルダに気を付けてください。  
Assets.zip をプログラムと同じ場所にコピーするか、実行フォルダの設定を変更してください。

[ライセンスはこちらを参照してください。](https://github.com/andanteyk/ElectronicObserver/blob/master/LICENSE)  

### 使用しているライブラリ
---

* [DynamicJson](http://dynamicjson.codeplex.com/)
* [DockPanel Suite](http://dockpanelsuite.com/)
* [FiddlerCore](http://www.telerik.com/fiddler/fiddlercore)

### 連絡先など
---

* 配布サイト:[ブルネイ工廠電気実験部](http://electronicobserver.blog.fc2.com/) (バグ報告・要望等はこちらにお願いします)
* 開発:[Andante](https://twitter.com/andanteyk)
