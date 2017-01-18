using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.IO;

using MyUtility;

namespace MyGames.Model
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
        /// 起動可能なゲームのリストのデフォルト値。
        /// 設定ファイルが存在しなかった場合、これを適用する。
        /// </summary>
        GameInfo[] GamesList;

        string[,] GamesListDefault =
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
        public GamesInfo()
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

        /// <summary>
        /// 設定ファイルから設定をロードする。
        /// </summary>
        /// <returns>
        /// 負数: ロード失敗。
        /// 0以上: 読み込んだゲーム数。もちろん、0であればゲーム登録なし。
        /// </returns>
        private int LoadConfigFile()
        {
            const string CONF_FILENAME = "MyGames.conf";
            // 設定ファイル有無をチェック
            if (!File.Exists(CONF_FILENAME))
            {
                // ファイルがなければ即-1を返す。
                return -1;
            }

            // 全てのゲーム名(⇔セクション名) を取得
            int numGames;  // ゲーム個数
            string[] gameSections = IniFile.GetPrivateProfileSectionAsStringArray("GameRegistration", Directory.GetCurrentDirectory() + @"\" + CONF_FILENAME);
            if (gameSections == null)
            {
                return -1;
            }

            numGames = gameSections.Count();
            if (numGames <= 0)
            {
                return 0;
            }
            GamesList = new GameInfo[numGames];

            const int CONFPARAM_LENGTH_MAX = 1024;
            int gameIndex = 0;
            StringBuilder gameSectionName = new StringBuilder(CONFPARAM_LENGTH_MAX, CONFPARAM_LENGTH_MAX);
            uint gameSectionNameLength;
            do
            {
                // ゲームセクション名を取得する
                gameSectionNameLength = IniFile.GetPrivateProfileString(
                    "GameRegistration",
                    "Game" + gameIndex.ToString(),
                    "",
                    gameSectionName,
                    Convert.ToUInt32(gameSectionName.Capacity),
                    Directory.GetCurrentDirectory() + @"\" + CONF_FILENAME
                    );

                // ゲームセクション名取得成功したら、パラメータを読み込む
                if (gameSectionNameLength > 0)
                {
                    string[] keys = { "Name", "Path", "Description", "ImagePath", "Arguments" };
                    string[] values = new string[5];
                    StringBuilder gameParams = new StringBuilder(CONFPARAM_LENGTH_MAX, CONFPARAM_LENGTH_MAX);
                    uint gameParamsLength;

                    int i = 0;
                    foreach (string key in keys)
                    {
                        gameParamsLength = IniFile.GetPrivateProfileString(
                            gameSectionName.ToString(),
                            key,
                            "",
                            gameParams,
                            Convert.ToUInt32(gameParams.Capacity),
                            Directory.GetCurrentDirectory() + @"\" + CONF_FILENAME
                            );
                        values[i] = gameParams.ToString();
                        i++;
                    }

                    if (!IsValidPath(values[1]))
                    {
                        MessageBox.Show(
                            "Invalid Path : " + values[1] + ".\n" +
                            "Loading default settings.",
                            "Error.", MessageBoxButton.OK, MessageBoxImage.Error);
                        return -1;
                    }
                    GamesList[gameIndex] = new GameInfo(values[0], values[1], values[2], values[3], values[4]);
                    
                    gameIndex++;
                }
            } while (gameSectionNameLength != 0);

            return numGames;
        }
        
        /// <summary>
        /// ファイルパスが妥当かチェックする。
        /// 現状、パスのファイルが存在すれば妥当であると判断している。
        /// </summary>
        /// <param name="path">ファイルパス</param>
        /// <returns></returns>
        private bool IsValidPath(string path)
        {
            return System.IO.File.Exists(path);
        }
        
        /// <summary>
        /// デフォルト設定をロードする。
        /// </summary>
        private void LoadDefaultConfig()
        {
            // GamesListを空にする
            if (GamesList != null)
            {
                GamesList.Initialize();
            }

            // デフォルトゲーム設定値からゲーム数を取得する (⇔1次元目の要素数を取得する)
            GamesList = new GameInfo[GamesListDefault.GetLength(0)];

            // ゲーム単位でループ
            for (int i = 0; i < GamesListDefault.GetLength(0); i++)
            {
                GamesList[i] = new GameInfo(
                    GamesListDefault[i, 0],
                    GamesListDefault[i, 1],
                    GamesListDefault[i, 2],
                    GamesListDefault[i, 3],
                    GamesListDefault[i, 4]);
            }

            return;
        }

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
            if (index < 0 || GamesList.Count() <= index)
            {
                return "";
            }
            return GamesList[index].Description;
        }

        /// <summary>
        /// ゲームイメージゲッタ
        /// </summary>
        /// <param name="index">リストで何番目か</param>
        /// <returns>ImageSource</returns>
        public ImageSource GetGameImageSource(int index)
        {
            if (index < 0 || GamesList.Count() <= index)
            {
                return null;
            }
            return GamesList[index].SampleImage.Source;
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
            if (index < 0 || GamesList.Count() <= index)
            {
                return false;
            }

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
