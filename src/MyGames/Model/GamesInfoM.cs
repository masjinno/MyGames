using MyUtility;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace MyGames.Model
{
    class GamesInfoM
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
        /// ファイル読み込みをしない場合のゲーム情報
        /// </summary>
        static readonly string[,] GamesListDefault =
        {
            {
                "TicTacToe",
                @"..\..\..\MyTicTacToe\bin\" + BUILD_CONFIG + @"\MyTicTacToe.exe",
                "TicTacToe Game.\nJapanese name is \"Sanmoku-Narabe\" or \"Maru-Batsu Game\".",
                @"..\..\..\MyTicTacToe\MyTicTacToe.png",
                ""
            },
            {
                "TicTacToe_DUMMY",
                @"..\..\..\MyTicTacToe\bin\" + BUILD_CONFIG + @"\MyTicTacToe.exe",
                "[DUMMY] TicTacToe Game.\nJapanese name is \"Sanmoku-Narabe\" or \"Maru-Batsu Game\".",
                @"..\..\..\MyGames\NoImage.png",
                ""
            }
        };

        /// <summary>
        /// ゲーム情報
        /// </summary>
        private GameInfo[] GamesList;

        public int LoadConfigFile()
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
                        // MessageBoxは使わない処理に変更する
                        System.Windows.MessageBox.Show(
                            "Invalid Path : " + values[1] + ".\n" +
                            "Loading default settings.",
                            "Error.", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
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
        public void LoadDefaultConfig()
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
        /// ゲーム名称のリストを取得する
        /// </summary>
        /// <returns></returns>
        public List<string> GetGameNames()
        {
            List<string> gameNames = new List<string>();
            foreach (GameInfo gi in GamesList)
            {
                gameNames.Add(gi.Name);
            }
            return gameNames;
        }

        /// <summary>
        /// インデックスに対応するゲームのDescriptionを取得する
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <returns></returns>
        public string GetGameDescription(int index)
        {
            return GamesList[index].Description;
        }

        /// <summary>
        /// インデックスに対応するゲームのサンプルイメージを取得する
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <returns></returns>
        public ImageSource GetGameSampleImage(int index)
        {
            return GamesList[index].SampleImage;
        }

        /// <summary>
        /// ゲーム実行
        /// </summary>
        /// <param name="index"></param>
        public void StartGame(int index)
        {
            GamesList[index].StartProgram();
        }
    }
}
