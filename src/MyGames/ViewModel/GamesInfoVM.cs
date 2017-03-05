using Prism.Mvvm;
using MyGames.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGames.ViewModel
{
    class GamesInfoVM : BindableBase
    {
        /// <summary>
        /// 起動可能なゲームのリストのデフォルト値。
        /// 設定ファイルが存在しなかった場合、これを適用する。
        /// </summary>
        GamesInfoM gamesInfoModel;
        
        /// <summary>
        /// GamesInfoコンストラクタ
        /// </summary>
        public GamesInfoVM()
        {
            gamesInfoModel = new GamesInfoM();

            // 読み込んだゲーム数を格納する
            int numGames = 0;

            // 設定ファイルロード
            numGames = gamesInfoModel.LoadConfigFile();
            if (numGames <= 0)
            {
                // 設定ファイルロードに失敗したら、デフォルト値をロードする
                gamesInfoModel.LoadDefaultConfig();
            }
        }
    }
}
