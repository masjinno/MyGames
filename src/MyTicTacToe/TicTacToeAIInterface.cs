﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTicTacToe
{
    interface ITicTacToeAIInterface
    {
        Tuple<int, int> PutMark(TicTacToeLogic.TicTacToeMark.MarkNum[,] boardData, TicTacToeLogic.TicTacToeMark.MarkNum mark);
    }
}
