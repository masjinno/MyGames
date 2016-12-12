using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTicTacToe
{
    /// <summary>
    /// 盤面のマーク
    /// Circle : ○
    /// Cross : ✕
    /// </summary>
    public enum TicTacToeMark
    {
        Circle,
        Cross,
        None
    }

    class TicTacToeLogic
    {
        // 定数定義 ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        /// <summary>
        /// 縦サイズ
        /// </summary>
        public const int ROW_SIZE = 3;

        /// <summary>
        /// 横サイズ
        /// </summary>
        public const int COLUMN_SIZE = 3;


        // メンバ変数定義 ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        /// <summary>
        /// 盤面。
        /// +-----+-----+-----+
        /// |[0,0]|[0,1]|[0,2]|  ROW 0
        /// +-----+-----+-----+
        /// |[1,0]|[1,1]|[1,2]|  ROW 1
        /// +-----+-----+-----+
        /// |[2,0]|[2,1]|[2,2]|  ROW 2
        /// +-----+-----+-----+
        ///  COL 0 COL 1 COL 2
        /// </summary>
        private TicTacToeMark[,] Board;

        /// <summary>
        /// 手番。
        /// Mark.Noneはありえない。
        /// </summary>
        private TicTacToeMark Turn;

        /// <summary>
        /// 外部へのメッセージ。(MS-DOSのERRORLEVELを文字列にしたイメージ)
        /// ステータスバーなどがあれば、そこへの表示を想定する。
        /// クラス外へ処理が戻る前に、何らかの値を代入すること。
        /// </summary>
        public string statement;


        // メソッド定義 ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        // コンストラクタ
        public TicTacToeLogic()
        {
            Board = new TicTacToeMark[ROW_SIZE, COLUMN_SIZE];

            ResetGame();
        }

        /// <summary>
        /// Board X,Y Getter.
        /// </summary>
        /// <param name="x">横位置(0～COLUMN_SIZE-1)</param>
        /// <param name="y">縦位置(0～ROW_SIZE-1)</param>
        /// <returns>Board[y,x]</returns>
        public TicTacToeMark GetBoardXY(int x, int y)
        {
            if ((0 <= x && x < COLUMN_SIZE) || (0 <= y && y < ROW_SIZE))
            {
                statement = "GetBoardXY(): ERROR. Arg x or y is WRONG. (0-x-" + COLUMN_SIZE + ", 0-y-" + ROW_SIZE + ")";
                return TicTacToeMark.None;
            }

            statement = "";
            return Board[y, x];
        }

        /// <summary>
        /// Board Setter.
        /// </summary>
        /// <param name="x">横x番目(0～COLUMN_SIZE-1)</param>
        /// <param name="y">縦y番目(0～ROW_SIZE-1)</param>
        /// <returns></returns>
        public bool SetBoardXY(int x, int y)
        {
            if (Board[y, x] == TicTacToeMark.None)
            {
                Board[y, x] = Turn;
                statement = "";
                return true;
            }
            else
            {
                statement = "There is " + Board[y, x].ToString();
                return false;
            }
        }

        /// <summary>
        /// Turn Getter.
        /// </summary>
        /// <returns>Turn</returns>
        public TicTacToeMark GetTurn()
        {
            statement = "";
            return Turn;
        }

        /// <summary>
        /// ゲーム続行時の処理
        /// </summary>
        public void ContinueGame()
        {
            // 手番更新
            if (Turn != TicTacToeMark.None - 1)
            {
                Turn = Turn + 1;
            }
            else
            {
                Turn = TicTacToeMark.Circle;
            }

            statement = "Turn is " + Turn.ToString();
        }

        /// <summary>
        /// ゲームをリセットする
        /// </summary>
        public void ResetGame()
        {
            for (int i = 0; i < ROW_SIZE; i++)
            {
                for (int j = 0; j < COLUMN_SIZE; j++)
                {
                    Board[i, j] = TicTacToeMark.None;
                }
            }

            Turn = TicTacToeMark.Circle;

            statement = "Game Reset.";
        }

        /// <summary>
        /// 勝敗判定を行う。勝利が1パターン見つかった時点で探索を終了する。
        /// </summary>
        /// <returns>
        /// bool : true->ゲーム終了。 false->game続行。
        /// Mark : 勝利マーク。勝負中もしくは引き分けであればNoneを返す。
        /// </returns>
        public Tuple<bool, TicTacToeMark> IsFinishedGame()
        {
            // 横方向の勝敗判定
            for (TicTacToeMark m = TicTacToeMark.Circle; m < TicTacToeMark.None; m++)
            {
                for (int j = 0; j < COLUMN_SIZE; j++)
                {
                    int markCount;
                    markCount = 0;
                    for (int i = 0; i < ROW_SIZE; i++)
                    {
                        if (Board[i, j] != m)
                        {
                            break;
                        }
                        markCount++;
                    }
                    if (markCount == ROW_SIZE)
                    {
                        Turn = TicTacToeMark.None;
                        statement = m.ToString() + " WON!";
                        return Tuple.Create(true, m);
                    }
                }
            }

            // 縦方向の勝敗判定
            for (TicTacToeMark m = TicTacToeMark.Circle; m < TicTacToeMark.None; m++)
            {
                for (int i = 0; i < ROW_SIZE; i++)
                {
                    int markCount;
                    markCount = 0;
                    for (int j = 0; j < COLUMN_SIZE; j++)
                    {
                        if (Board[i, j] != m)
                        {
                            break;
                        }
                        markCount++;
                    }
                    if (markCount == COLUMN_SIZE)
                    {
                        Turn = TicTacToeMark.None;
                        statement = m.ToString() + " WON!";
                        return Tuple.Create(true, m);
                    }
                }
            }

            // 斜め方向の勝敗判定 (ROW_SIZE == COLUMN_SIZE の場合のみ実施する)
            if (ROW_SIZE == COLUMN_SIZE)
            {
                for (TicTacToeMark m = TicTacToeMark.Circle; m < TicTacToeMark.None; m++)
                {
                    int markCount;

                    // 左上から右下をチェック
                    markCount = 0;
                    for (int i = 0; i < ROW_SIZE; i++)
                    {
                        if (Board[i, i] != m)
                        {
                            break;
                        }
                        markCount++;
                    }
                    if (markCount == ROW_SIZE)
                    {
                        Turn = TicTacToeMark.None;
                        statement = m.ToString() + " WON!";
                        return Tuple.Create(true, m);
                    }

                    // 右上から左下をチェック
                    markCount = 0;
                    for (int i = 0; i < ROW_SIZE; i++)
                    {
                        if (Board[i, ROW_SIZE - (i + 1)] != m)
                        {
                            break;
                        }
                        markCount++;
                    }
                    if (markCount == ROW_SIZE)
                    {
                        Turn = TicTacToeMark.None;
                        statement = m.ToString() + " WON!";
                        return Tuple.Create(true, m);
                    }
                }
            }

            // 全部埋まっているか判定
            for (int i = 0; i < ROW_SIZE; i++)
            {
                for (int j = 0; j < COLUMN_SIZE; j++)
                {
                    // 埋まっていないマスがある
                    if (Board[i, j] == TicTacToeMark.None)
                    {
                        statement = "Game continues";
                        return Tuple.Create(false, TicTacToeMark.None);
                    }
                }
            }

            // 全部埋まっているので、ゲームは引き分け
            statement = "Draw Game.";
            return Tuple.Create(true, TicTacToeMark.None);
        }
    }
}
