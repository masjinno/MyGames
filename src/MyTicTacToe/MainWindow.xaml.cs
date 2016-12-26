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

namespace MyTicTacToe
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        /* チックタックトーのプログラム
         * 
         * ・ひとまず、サイズは3x3固定で実装。
         *   チックタックトーの4x4のゲーム性の関係上、将来的にも変える気はない。
         * 
         * ・先行は○、後攻は✕。
         * 
         * ・後のルールは、通常のチックタックトーと同様。
         * 
         * */
        
        /// <summary>
        /// チックタックトーのロジックを受け持つ
        /// </summary>
        TicTacToeLogic ticTacToeLogic;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            ticTacToeLogic = new TicTacToeLogic();
        }

        /// <summary>
        /// ボタンクリックイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void reset_Button_Click(object sender, RoutedEventArgs e)
        {
            if (ticTacToeLogic == null) return;
            ticTacToeLogic.ResetGame();

            MessageBox.Show("!");
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // ゲーム中でない場合は何もしない
            if (ticTacToeLogic.GetTurn() == TicTacToeMark.None)
            {
                return;
            }

            // 座標設定
            Point mousePoint = e.GetPosition(board_Grid);
            Size boardSize = new Size(board_Grid.ActualWidth, board_Grid.ActualHeight);

            // マス数設定
            int row = TicTacToeLogic.ROW_SIZE;
            int column = TicTacToeLogic.COLUMN_SIZE;

            // 打たれたマス特定
            int x, y;
            x = (int)(mousePoint.X / (boardSize.Width / column));
            y = (int)(mousePoint.Y / (boardSize.Height / row));
            
            // ボードへの入力処理
            bool bSetBoardSuccess = ticTacToeLogic.SetBoardXY(x, y);

            // ゲーム終了判定
            if (bSetBoardSuccess)
            {
                // TODO: 盤面描画更新処理
                //
                //

                var gameFinish = ticTacToeLogic.IsFinishedGame();
                bool fin = gameFinish.Item1;

                if (fin)
                {
                    // ゲーム終了ならばメッセージ表示
                    MessageBox.Show(
                        ticTacToeLogic.statement,
                        "Game Finished!",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                else
                {
                    // ゲーム続行
                    ticTacToeLogic.ContinueGame();
                }
            }
        }
    }
}
