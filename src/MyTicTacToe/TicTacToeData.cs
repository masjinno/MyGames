using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTicTacToe
{
    class TicTacToeData
    {
        // メンバ変数定義 ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        /// <summary>
        /// 盤面。
        /// +-----+-----+-----+
        /// |[0,0]|[1,0]|[2,0]|  ROW 0
        /// +-----+-----+-----+
        /// |[0,1]|[1,1]|[2,1]|  ROW 1
        /// +-----+-----+-----+
        /// |[0,2]|[1,2]|[2,2]|  ROW 2
        /// +-----+-----+-----+
        ///  COL 0 COL 1 COL 2
        /// </summary>
        private TicTacToeLogic.TicTacToeMark.MarkNum[,] Board;

        /// <summary>
        /// 手番。
        /// Mark.Noneはありえない。
        /// </summary>
        private TicTacToeLogic.TicTacToeMark.MarkNum Turn;

        /// <summary>
        /// 外部へのメッセージ。(MS-DOSのERRORLEVELを文字列にしたイメージ)
        /// ステータスバーなどがあれば、そこへの表示を想定する。
        /// クラス外へ処理が戻る前に、何らかの値を代入すること。
        /// </summary>
        public string statement { get; private set; }


        // メソッド定義 ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        // コンストラクタ
        public TicTacToeData(int row_size, int column_size)
        {
            Board = new TicTacToeLogic.TicTacToeMark.MarkNum[column_size, row_size];

            ResetGame();
        }

        /// <summary>
        /// Board X,Y Getter.
        /// </summary>
        /// <param name="x">横位置(0～COLUMN_SIZE-1)</param>
        /// <param name="y">縦位置(0～ROW_SIZE-1)</param>
        /// <returns>Board[y,x]</returns>
        public TicTacToeLogic.TicTacToeMark.MarkNum GetBoardXY(int x, int y)
        {
            if ((0 <= x && x < TicTacToeLogic.ROW_SIZE) || (0 <= y && y < TicTacToeLogic.COLUMN_SIZE))
            {
                statement = "GetBoardXY(): ERROR. Arg x or y is WRONG. (0-x-" + TicTacToeLogic.ROW_SIZE + ", 0-y-" + TicTacToeLogic.COLUMN_SIZE + ")";
                return TicTacToeLogic.TicTacToeMark.MarkNum.None;
            }

            statement = "";
            return Board[x, y];
        }

        /// <summary>
        /// 盤面データゲッタ
        /// </summary>
        /// <returns>盤面データ</returns>
        public TicTacToeLogic.TicTacToeMark.MarkNum[,] GetBoard()
        {
            return Board;
        }

        /// <summary>
        /// Board Setter.
        /// </summary>
        /// <param name="x">横x番目(0～ROW_SIZE-1)</param>
        /// <param name="y">縦y番目(0～COLUMN_SIZE-1)</param>
        /// <returns>
        /// true: 入力成功
        /// false: 入力失敗
        /// </returns>
        public bool SetBoardXY(int x, int y)
        {
            if (Board[x, y] == TicTacToeLogic.TicTacToeMark.MarkNum.None)
            {
                Board[x, y] = Turn;
                statement = "";
                return true;
            }
            else
            {
                statement = "There is " + Board[x, y].ToString();
                return false;
            }
        }

        /// <summary>
        /// Turn Getter.
        /// </summary>
        /// <returns>Turn</returns>
        public TicTacToeLogic.TicTacToeMark.MarkNum GetTurn()
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
            if (Turn != TicTacToeLogic.TicTacToeMark.MarkNum.None - 1)
            {
                Turn = Turn + 1;
            }
            else
            {
                Turn = TicTacToeLogic.TicTacToeMark.MarkNum.Circle;
            }

            statement = "Turn is " + Turn.ToString();
        }

        /// <summary>
        /// ゲームをリセットする
        /// </summary>
        public void ResetGame()
        {
            for (int i = 0; i < TicTacToeLogic.COLUMN_SIZE; i++)
            {
                for (int j = 0; j < TicTacToeLogic.ROW_SIZE; j++)
                {
                    Board[i, j] = TicTacToeLogic.TicTacToeMark.MarkNum.None;
                }
            }

            Turn = TicTacToeLogic.TicTacToeMark.MarkNum.Circle;

            statement = "Game Reset.";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Tuple<bool, TicTacToeLogic.TicTacToeMark.MarkNum> IsFinishedGame()
        {
            var result = TicTacToeLogic.IsFinishedGame(Board);

            if (!result.Item1)
            {
                statement = "Game continues";
            }
            else
            {
                Turn = TicTacToeLogic.TicTacToeMark.MarkNum.None;
                if (result.Item2 == TicTacToeLogic.TicTacToeMark.MarkNum.None)
                {
                    statement = "Draw Game.";
                }
                else
                {
                    statement = TicTacToeLogic.TicTacToeMark.GetMarkString(result.Item2) + " WON!";
                }
            }

            return result;
        }
    }
}
