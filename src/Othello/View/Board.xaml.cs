using Othello.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Othello.View
{
    /// <summary>
    /// Board.xaml の相互作用ロジック
    /// </summary>
    public partial class Board : UserControl
    {
        public Board()
        {
            InitializeComponent();

            InitializeDesign();
        }

        /// <summary>
        /// デザインを初期化する。
        /// 多数の定義が必要なものはxaml上で定義せずに、コードビハインド上で定義する。
        /// </summary>
        private void InitializeDesign()
        {
            InitializeCell();
        }

        /// <summary>
        /// 盤面のマスを初期化する。
        /// 具体的には以下について初期化する。
        ///     ・縦のマス数
        ///     ・横のマス数
        ///     ・マスの境界線
        /// </summary>
        private void InitializeCell()
        {
            InitializeRow();
            InitializeColumn();
            InitializeLine();
        }

        /// <summary>
        /// 縦のマス数初期化。
        /// 数は
        /// </summary>
        private void InitializeRow()
        {
            for (int y = 0; y < SettingData.ROW_SIZE; y++)
            {
                BoardGrid.RowDefinitions.Add(new RowDefinition());
            }
        }

        /// <summary>
        /// 横のマス数初期化
        /// </summary>
        private void InitializeColumn()
        {
            for (int x = 0; x < SettingData.COLUMN_SIZE; x++)
            {
                BoardGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
        }

        /// <summary>
        /// 境界線初期化
        /// </summary>
        private void InitializeLine()
        {
            for (int y = 0; y < SettingData.ROW_SIZE; y++)
            {
                for (int x = 0; x < SettingData.COLUMN_SIZE; x++)
                {
                    Border b = new Border();
                    b.BorderThickness = new Thickness(1);
                    b.BorderBrush = Brushes.Black;
                    Grid.SetRow(b, y);
                    Grid.SetColumn(b, x);
                    BoardGrid.Children.Add(b);
                }
            }
        }
    }
}
