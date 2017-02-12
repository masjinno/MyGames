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
        /// ビルド構成。
        /// パスを辿るときに使用する。
        /// </summary>
#if DEBUG
        private static readonly string BUILD_CONFIG = "Debug";
#else
        private static readonly string BUILD_CONFIG = "Release";
#endif

        /// <summary>
        /// 起動可能なゲームのリストのデフォルト値。
        /// 設定ファイルが存在しなかった場合、これを適用する。
        /// </summary>
        GamesInfoM gamesInfoModel;

        /// <summary>
        /// ファイル読み込みをしない場合のゲーム情報
        /// </summary>
        static readonly string[,] GamesListDefault =
        {
            {
                "TicTacToe",
                @"..\..\..\MyTicTacToe\bin\"+BUILD_CONFIG+@"\MyTicTacToe.exe",
                "TicTacToe Game.\nJapanese name is \"Sanmoku-Narabe\" or \"Maru-Batsu Game\".",
                @"..\..\..\MyTicTacToe\MyTicTacToe.png",
                ""
            },
            {
                "TicTacToe_DUMMY",
                @"..\..\..\MyTicTacToe\bin\"+BUILD_CONFIG+@"\MyTicTacToe.exe",
                "[DUMMY] TicTacToe Game.\nJapanese name is \"Sanmoku-Narabe\" or \"Maru-Batsu Game\".",
                @"..\..\..\MyGames\NoImage.png",
                ""
            }
        };

        /// <summary>
        /// GamesInfoコンストラクタ
        /// </summary>
        public GamesInfoVM()
        {
            // 読み込んだゲーム数を格納する
            int numGames = 0;

            // 設定ファイルロード
            numGames = LoadConfigFile();
            if (numGames <= 0)
            {
                // 設定ファイルロードに失敗したら、デフォルト値をロードする
                LoadDefaultConfig();
            }
        }
    }
}
