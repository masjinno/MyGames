using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTicTacToe
{
    /// <summary>
    /// TicTacToeのAIインターフェース。
    /// staticなクラスとして実装されることを想定する。
    /// </summary>
    interface ITicTacToeAIInterface
    {
        /// <summary>
        /// boardDataに対して、markを打つ場所を返す静的メソッドのインターフェース
        /// </summary>
        /// <param name="boardData">盤面データ</param>
        /// <param name="mark">打つマーク</param>
        /// <returns>
        /// Item1 : 横位置 (boarData[Item1, Item2])
        /// Item2 : 縦位置 (boarData[Item1, Item2])
        /// </returns>
        Tuple<int, int> PutMark(TicTacToeLogic.TicTacToeMark.MarkNum[,] boardData, TicTacToeLogic.TicTacToeMark.MarkNum mark);
    }
}
