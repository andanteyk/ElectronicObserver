
# 七四式電子観測儀拡張版 / ElectronicObserverExtended (EOE)

現在鋭意開発中の艦これ補助ブラウザです。  



## About ElectronicObserverExtended / 拡張版について

ElectronicObserverExtended implements Plugin APIs introduced by [tsanie's fork](https://github.com/tsanie/ElectronicObserver). This allows developers to add functionalities that can be distributed in a .dll file.

To maintain compatibility with plugins developed for tsanie's fork, ElectronicObserverExtended chains a FiddlerCore proxy in front of Nekoxy. ElectronicObserverExtended with no plugins installed should behave exactly the same as the original ElectronicObserver, except:

* It by default uses UTF-8 as file encoding.
* It always respect system proxy settings, as designed by FiddlerCore.

[Download Extended / 拡張版のダウンロード](https://ci.appveyor.com/project/CNA-Bld/electronicobserverextended/build/artifacts)

[Flavors of Distribution](https://github.com/CAWAS/ElectronicObserverExtended/wiki/Flavors)


## ダウンロード

[リリースページ](https://github.com/andanteyk/ElectronicObserver/releases) もしくは [配布ブログ](http://electronicobserver.blog.fc2.com/) を参照してください。

[更新内容・履歴はこちらで確認できます。](https://github.com/andanteyk/ElectronicObserver/wiki/ChangeLog)  


## 開発者の皆様へ

[開発のための情報はこちらに掲載しています。](https://github.com/andanteyk/ElectronicObserver/wiki/ForDev)  

[Other/Information/](https://github.com/andanteyk/ElectronicObserver/tree/develop/ElectronicObserver/Other/Information) に艦これのAPIや仕様についての情報を掲載しています。  
ご自由にお持ちください。但し内容は保証しません。  

[ライセンスは MIT License です。](https://github.com/andanteyk/ElectronicObserver/blob/master/LICENSE)  


## 使用しているライブラリ

* [DynamicJson](http://dynamicjson.codeplex.com/) (JSON データの読み書き) - [Ms-PL](https://github.com/CAWAS/ElectronicObserverExtended/blob/extended/Licenses/Ms-PL.txt)
* [DynaJson](https://github.com/fujieda/DynaJson) (JSON データの読み書き) - [MIT License](https://github.com/andanteyk/ElectronicObserver/blob/master/Licenses/DynaJson.txt)
* [DockPanel Suite](http://dockpanelsuite.com/) (ウィンドウレイアウト) - [MIT License](https://github.com/andanteyk/ElectronicObserver/blob/master/Licenses/DockPanelSuite.txt)
* [Nekoxy](https://github.com/veigr/Nekoxy) (通信キャプチャ) - [MIT License](https://github.com/andanteyk/ElectronicObserver/blob/master/Licenses/Nekoxy.txt)
    * [TrotiNet](http://trotinet.sourceforge.net/) - [GNU Lesser General Public License v3.0](https://github.com/andanteyk/ElectronicObserver/blob/master/Licenses/LGPL.txt)
        * [log4net](https://logging.apache.org/log4net/) - [Apache License version 2.0](https://github.com/andanteyk/ElectronicObserver/blob/master/Licenses/Apache.txt)
* [FiddlerCore](http://www.telerik.com/fiddler/fiddlercore) - [End User License Agreement for FiddlerCore](https://github.com/CAWAS/ElectronicObserverExtended/blob/extended/Licenses/FiddlerCore.txt)
* [GeckoFX](https://bitbucket.org/geckofx/geckofx-60.0) - [Mozilla Public License](https://github.com/CAWAS/ElectronicObserverExtended/blob/extended/Licenses/MPL.txt)


## 連絡先など

* 配布サイト:[ブルネイ工廠電気実験部](http://electronicobserver.blog.fc2.com/) (バグ報告・要望等はこちらにお願いします)
* 開発:[Andante](https://twitter.com/andanteyk)
* 拡張版開発:[中国ホワイト（アルバム）学院](https://github.com/CAWAS)

