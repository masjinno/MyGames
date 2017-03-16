using Othello.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello.Model
{
    class OthelloBoardData
    {
        /// <summary>
        /// 盤面データ
        /// </summary>
        private SettingData.OthelloMark[,] _boardMark;
        public SettingData.OthelloMark[,] BoardMark
        {
            get { return _boardMark; }
            set { _boardMark = value; }
        }
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public OthelloBoardData()
        {
            InitializeData();
        }

        /// <summary>
        /// データ初期化
        /// </summary>
        private void InitializeData()
        {
            BoardMark = new SettingData.OthelloMark[SettingData.COLUMN_SIZE, SettingData.ROW_SIZE];
        }

        /// <summary>
        /// 盤面をゲーム開始時の状態に初期化する
        /// </summary>
        public void ResetData()
        {
            // 全て空で埋める
            for (int y = 0; y < SettingData.ROW_SIZE; y++)
            {
                for (int x = 0; x < SettingData.COLUMN_SIZE; x++)
                {
                    BoardMark[x, y] = SettingData.OthelloMark.None;
                }
            }

            // 縦マス数・横マス数は偶数の前提で、初期配置マスを決定
            int CenterXLeft = SettingData.COLUMN_SIZE / 2 - 1;
            int CenterXRight = SettingData.COLUMN_SIZE / 2;
            int CenterYUpper = SettingData.ROW_SIZE / 2 - 1;
            int CenterYLower = SettingData.ROW_SIZE / 2;

            // 初期配置を入力
            BoardMark[CenterXLeft, CenterYLower] = SettingData.OthelloMark.Black;
            BoardMark[CenterXRight, CenterYUpper] = SettingData.OthelloMark.Black;
            BoardMark[CenterXLeft, CenterYUpper] = SettingData.OthelloMark.White;
            BoardMark[CenterXRight, CenterYLower] = SettingData.OthelloMark.White;
        }
    }
}
