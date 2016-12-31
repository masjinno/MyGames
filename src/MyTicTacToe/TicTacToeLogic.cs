using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTicTacToe
{
    /// <summary>
    /// チックタックトーのロジック
    /// </summary>
    public static class TicTacToeLogic
    {
        /// <summary>
        /// マークに関する定義
        /// </summary>
        public static class TicTacToeMark
        {
            /// <summary>
            /// 盤面のマークを意味する値
            /// Circle : ○
            /// Cross : ✕
            /// </summary>
            public enum MarkNum
            {
                Circle,
                Cross,
                None
            }

            /// <summary> マーク文字列 </summary>
            private static string[] MarkString = new string[] { "○", "✕", "" };

            /// <summary>
            /// マーク番号に対応するマーク文字列を返す
            /// </summary>
            /// <param name="mn">マーク番号</param>
            /// <returns>対応するマーク文字列</returns>
            public static string GetMarkString(MarkNum mn)
            {
                string ret = MarkString[2];
                switch (mn)
                {
                    case MarkNum.Circle:
                        ret = MarkString[0];
                        break;
                    case MarkNum.Cross:
                        ret = MarkString[1];
                        break;
                }
                return ret;
            }
        }

        // 定数定義 ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        /// <summary>
        /// 縦サイズ
        /// </summary>
        public const int ROW_SIZE = 3;

        /// <summary>
        /// 横サイズ
        /// </summary>
        public const int COLUMN_SIZE = 3;


        // 静的メソッド定義 ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        
        /// <summary>
        /// 勝敗判定を行う。勝利が1パターン見つかった時点で探索を終了する。
        /// </summary>
        /// <param name="board">盤面情報</param>
        /// <param name="statement"></param>
        /// <returns>
        /// bool : true->ゲーム終了。 false->game続行。
        /// Mark : 勝利マーク。勝負中もしくは引き分けであればNoneを返す。
        /// </returns>
        public static Tuple<bool, TicTacToeMark.MarkNum> IsFinishedGame(TicTacToeMark.MarkNum[,] board)
        {
            // 各方向のゲーム終了チェック結果格納用変数
            Tuple<bool, TicTacToeMark.MarkNum> gameFinish;

            // 横方向の勝敗判定
            gameFinish = IsFinishedGameRowDirection(board);
            if (gameFinish.Item1)
            {
                return gameFinish;
            }

            // 縦方向の勝敗判定
            gameFinish = IsFinishedGameColumnDirection(board);
            if (gameFinish.Item1)
            {
                return gameFinish;
            }

            // 斜め方向の勝敗判定 (ROW_SIZE == COLUMN_SIZE の場合のみ実施する)
            if (ROW_SIZE == COLUMN_SIZE)
            {
                gameFinish = IsFinishedGameDiagonal(board);
                if (gameFinish.Item1)
                {
                    return gameFinish;
                }
            }

            // ボードが埋まっていればゲーム終了
            return Tuple.Create(IsFilledBoard(board), TicTacToeMark.MarkNum.None);
        }

        /// <summary>
        /// 行方向(横)の終了判定チェック
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        private static Tuple<bool, TicTacToeMark.MarkNum> IsFinishedGameRowDirection(TicTacToeMark.MarkNum[,] board)
        {
            for (TicTacToeMark.MarkNum m = TicTacToeMark.MarkNum.Circle; m < TicTacToeMark.MarkNum.None; m++)
            {
                for (int j = 0; j < COLUMN_SIZE; j++)
                {
                    int markCount;
                    markCount = 0;
                    for (int i = 0; i < ROW_SIZE; i++)
                    {
                        if (board[i, j] != m)
                        {
                            break;
                        }
                        markCount++;
                    }
                    if (markCount == ROW_SIZE)
                    {
                        return Tuple.Create(true, m);
                    }
                }
            }

            return Tuple.Create(false, TicTacToeMark.MarkNum.None);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        private static Tuple<bool, TicTacToeMark.MarkNum> IsFinishedGameColumnDirection(TicTacToeMark.MarkNum[,] board)
        {
            for (TicTacToeMark.MarkNum m = TicTacToeMark.MarkNum.Circle; m < TicTacToeMark.MarkNum.None; m++)
            {
                for (int i = 0; i < ROW_SIZE; i++)
                {
                    int markCount;
                    markCount = 0;
                    for (int j = 0; j < COLUMN_SIZE; j++)
                    {
                        if (board[i, j] != m)
                        {
                            break;
                        }
                        markCount++;
                    }
                    if (markCount == COLUMN_SIZE)
                    {
                        return Tuple.Create(true, m);
                    }
                }
            }

            return Tuple.Create(false, TicTacToeMark.MarkNum.None);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        private static Tuple<bool, TicTacToeMark.MarkNum> IsFinishedGameDiagonal(TicTacToeMark.MarkNum[,] board)
        {
            for (TicTacToeMark.MarkNum m = TicTacToeMark.MarkNum.Circle; m < TicTacToeMark.MarkNum.None; m++)
            {
                int markCount;

                // 左上から右下をチェック
                markCount = 0;
                for (int i = 0; i < ROW_SIZE; i++)
                {
                    if (board[i, i] != m)
                    {
                        break;
                    }
                    markCount++;
                }
                if (markCount == ROW_SIZE)
                {
                    return Tuple.Create(true, m);
                }

                // 右上から左下をチェック
                markCount = 0;
                for (int i = 0; i < ROW_SIZE; i++)
                {
                    if (board[i, ROW_SIZE - (i + 1)] != m)
                    {
                        break;
                    }
                    markCount++;
                }
                if (markCount == ROW_SIZE)
                {
                    return Tuple.Create(true, m);
                }
            }

            return Tuple.Create(false, TicTacToeMark.MarkNum.None);
        }

        /// <summary>
        /// 盤面が全て埋まっているか
        /// </summary>
        /// <param name="board">盤面情報</param>
        /// <returns>
        /// true : 全て埋まっている
        /// false : 埋まっていないマスがある
        /// </returns>
        private static bool IsFilledBoard(TicTacToeMark.MarkNum[,] board)
        {
            for (int i = 0; i < ROW_SIZE; i++)
            {
                for (int j = 0; j < COLUMN_SIZE; j++)
                {
                    // 埋まっていないマスがある
                    if (board[i, j] == TicTacToeMark.MarkNum.None)
                    {
                        return false;
                    }
                }
            }
            // 埋まっていないマスはなかった
            return true;
        }
    }
}
