using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGames
{
    /// <summary>
    /// ランチャープログラムが呼び出す実行ファイル情報など
    /// </summary>
    class GamesInfo
    {
        /// <summary>
        /// ビルド構成。
        /// パスを辿るときに使用する。
        /// </summary>
#if DEBUG
        const string BUILD_CONFIG = "Debug";
#else
        const string BUILD_CONFIG = "Release";
#endif
        /// <summary>
        /// 起動可能なゲームのリスト
        /// </summary>
        GameInfo[] GamesList = new GameInfo[]
        {
            new GameInfo(
                "TicTacToe",
                @"..\..\..\MyTicTacToe\bin\"+BUILD_CONFIG+@"\MyTicTacToe.exe",
                "TicTacToe Game.\nJapanese name is \"Sanmoku-Narabe\" or \"Maru-Batsu Game\".",
                ""),
            new GameInfo(
                "TicTacToe_DUMMY",
                @"..\..\..\MyTicTacToe\bin\"+BUILD_CONFIG+@"\MyTicTacToe.exe",
                "[DUMMY] TicTacToe Game.\nJapanese name is \"Sanmoku-Narabe\" or \"Maru-Batsu Game\".",
                ""),
        };
        
        /// <summary>
        /// 全ゲーム名称取得
        /// </summary>
        /// <returns>ゲーム名</returns>
        public string[] GetAllGameNames()
        {
            int i = 0;
            string[] gameNames = new string[GamesList.Count()];
            foreach (GameInfo gi in GamesList)
            {
                gameNames[i] = gi.Name;
                i++;
            }
            return gameNames;
        }

        /// <summary>
        /// ゲーム説明ゲッタ
        /// </summary>
        /// <param name="index">リストで何番目か</param>
        /// <returns>ゲーム説明</returns>
        public string GetGameDescription(int index)
        {
            return GamesList[index].Description;
        }

        /// <summary>
        /// ゲームスタート
        /// </summary>
        /// <param name="index">リストで何番目か</param>
        /// <returns>
        /// true: 起動成功
        /// false: 起動失敗
        /// </returns>
        public bool StartGame(int index)
        {
            return GamesList[index].StartProgram();
        }

        /// <summary>
        /// 全ゲームを終了する
        /// </summary>
        /// <returns>終了したゲームの数</returns>
        public int CloseAllGame()
        {
            int closedPrograms = 0;
            foreach (GameInfo gi in GamesList)
            {
                if (gi.ExitProgram())
                {
                    closedPrograms++;
                }
            }
            return closedPrograms;
        }
    }
}
