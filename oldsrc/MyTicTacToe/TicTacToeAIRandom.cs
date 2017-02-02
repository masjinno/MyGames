using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTicTacToe
{
    class TicTacToeAIRandom : ITicTacToeAI
    {
        /// <summary>
        /// boardDataに対して、ランダムでmarkを打つ場所を指定する
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

            int cntEmptyCells;
            cntEmptyCells = 0;
            for (int y = 0; y < TicTacToeLogic.ROW_SIZE; y++)
            {
                for (int x = 0; x < TicTacToeLogic.COLUMN_SIZE; x++)
                {
                    if (boardData[x, y] == TicTacToeLogic.TicTacToeMark.MarkNum.None)
                    {
                        emptyCellArray[cntEmptyCells] = new Tuple<int, int>(x, y);
                        cntEmptyCells++;
                    }
                }
            }
            
            Random r = new Random();
            return emptyCellArray[r.Next(cntEmptyCells)];
        }
    }
}
