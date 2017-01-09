using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTicTacToe
{
    class AIManager
    {
        /// <summary>
        /// AI名
        /// </summary>
        private string[] aiNames = { "AI:Random", "AI:NextMove" };

        /// <summary>
        /// AI実態
        /// </summary>
        private ITicTacToeAI[] TicTacToeAI;

        /// <summary>
        /// 現ターン時にAIが有効か否か
        /// </summary>
        public bool IsEnabled {
            get;
            set;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AIManager()
        {
            TicTacToeAI = new ITicTacToeAI[]
            {
                new TicTacToeAIRandom(),
                new TicTacToeAINextMove()
            };
            IsEnabled = false;
        }

        /// <summary>
        /// AI名ゲッタ
        /// </summary>
        /// <returns>AI名</returns>
        public string[] GetAiNames()
        {
            return aiNames;
        }

        /// <summary>
        /// AIの打った場所を取得する。
        /// </summary>
        /// <param name="aiIndex">AIインデックス</param>
        /// <param name="gameData">盤面データとターンを保持するゲームデータ</param>
        /// <returns>指定された場所</returns>
        public Tuple<int, int> GetAiPutPlace(int aiIndex, TicTacToeData gameData)
        {
            if (aiIndex < 0 || aiNames.Count() <= aiIndex)
            {
                return Tuple.Create<int, int>(-1, -1);
            }

            Tuple<int, int> ret = TicTacToeAI[aiIndex].PutMark(gameData.GetBoard(), gameData.GetTurn());
            return ret;
        }
    }
}
