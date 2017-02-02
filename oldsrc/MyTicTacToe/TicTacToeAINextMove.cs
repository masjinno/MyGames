using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTicTacToe
{
    class TicTacToeAINextMove : ITicTacToeAI
    {
        /// <summary>
        /// boardDataに対して、次の手を読んでmarkを打つ場所を指定する
        /// </summary>
        /// <param name="boardData">盤面データ</param>
        /// <param name="mark">打つマーク</param>
        /// <returns>
        /// Item1 : 横位置 (boarData[Item1, Item2])
        /// Item2 : 縦位置 (boarData[Item1, Item2])
        /// </returns>
        public Tuple<int, int> PutMark(TicTacToeLogic.TicTacToeMark.MarkNum[,] boardData, TicTacToeLogic.TicTacToeMark.MarkNum mark)
        {
            if (mark == TicTacToeLogic.TicTacToeMark.MarkNum.None)
            {
                return Tuple.Create<int, int>(-1, -1);
            }

            Tuple<int, int>[] emptyCellArray = new Tuple<int, int>[TicTacToeLogic.ROW_SIZE * TicTacToeLogic.COLUMN_SIZE];
            Tuple<int, int> retCell = Tuple.Create<int, int>(-1, -1);   // 初期値は負数(⇔盤外を示すマス)
            
            TicTacToeLogic.TicTacToeMark.MarkNum[,] tmpBoardData;
            TicTacToeLogic.TicTacToeMark.MarkNum enemyMark;
            tmpBoardData = CopyBoard(boardData);
            if (mark == TicTacToeLogic.TicTacToeMark.MarkNum.Circle)
            {
                enemyMark = TicTacToeLogic.TicTacToeMark.MarkNum.Cross;
            }
            else
            {
                enemyMark = TicTacToeLogic.TicTacToeMark.MarkNum.Circle;
            }

            int cntEmptyCells;
            cntEmptyCells = 0;
            for (int y = 0; y < TicTacToeLogic.ROW_SIZE; y++)
            {
                for (int x = 0; x < TicTacToeLogic.COLUMN_SIZE; x++)
                {
                    if (tmpBoardData[x, y] == TicTacToeLogic.TicTacToeMark.MarkNum.None)
                    {
                        Tuple<bool, TicTacToeLogic.TicTacToeMark.MarkNum> gameResult;
                        
                        // 自分が打って勝てるか判定
                        tmpBoardData[x, y] = mark;
                        gameResult = TicTacToeLogic.IsFinishedGame(tmpBoardData);
                        if (gameResult.Item1)
                        {
                            return Tuple.Create<int, int>(x, y);
                        }
                        
                        // 相手が打ったら負けるか判定
                        // 既に見つけていれば判定不要
                        if (retCell.Item1 < 0)
                        {
                            tmpBoardData[x, y] = enemyMark;
                            gameResult = TicTacToeLogic.IsFinishedGame(tmpBoardData);
                            if (gameResult.Item1)
                            {
                                retCell = Tuple.Create<int, int>(x, y);
                            }
                        }
                        tmpBoardData[x, y] = TicTacToeLogic.TicTacToeMark.MarkNum.None;

                        emptyCellArray[cntEmptyCells] = new Tuple<int, int>(x, y);
                        cntEmptyCells++;
                    }
                }
            }

            // 打つマスを見つけられていればそれを返す
            if (retCell.Item1 >= 0)
            {
                return retCell;
            }

            // 打つマスを見つけられていなければ、ランダムで場所を返す。
            Random r = new Random();
            return emptyCellArray[r.Next(cntEmptyCells)];
        }

        /// <summary>
        /// 盤面コピー
        /// </summary>
        /// <param name="b">コピーする盤面データ</param>
        /// <returns>コピーした盤面</returns>
        private TicTacToeLogic.TicTacToeMark.MarkNum[,] CopyBoard(TicTacToeLogic.TicTacToeMark.MarkNum[,] b)
        {
            if (b == null)
            {
                return null;
            }

            TicTacToeLogic.TicTacToeMark.MarkNum[,] tmp = new TicTacToeLogic.TicTacToeMark.MarkNum[TicTacToeLogic.COLUMN_SIZE, TicTacToeLogic.ROW_SIZE];

            for (int x = 0; x < TicTacToeLogic.COLUMN_SIZE; x++)
            {
                for (int y = 0; y < TicTacToeLogic.ROW_SIZE; y++)
                {
                    tmp[x, y] = b[x, y];
                }
            }

            return tmp;
        }
        
    }
}
