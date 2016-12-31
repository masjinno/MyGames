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
        TicTacToeData ticTacToeData;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            ticTacToeData = new TicTacToeData(TicTacToeLogic.ROW_SIZE, TicTacToeLogic.COLUMN_SIZE);
            ResetGame();
        }

        private void ResetGame()
        {
            if (ticTacToeData == null) return;
            ticTacToeData.ResetGame();
            board00_Button.Content = TicTacToeLogic.TicTacToeMark.GetMarkString(TicTacToeLogic.TicTacToeMark.MarkNum.None);
            board01_Button.Content = TicTacToeLogic.TicTacToeMark.GetMarkString(TicTacToeLogic.TicTacToeMark.MarkNum.None);
            board02_Button.Content = TicTacToeLogic.TicTacToeMark.GetMarkString(TicTacToeLogic.TicTacToeMark.MarkNum.None);
            board10_Button.Content = TicTacToeLogic.TicTacToeMark.GetMarkString(TicTacToeLogic.TicTacToeMark.MarkNum.None);
            board11_Button.Content = TicTacToeLogic.TicTacToeMark.GetMarkString(TicTacToeLogic.TicTacToeMark.MarkNum.None);
            board12_Button.Content = TicTacToeLogic.TicTacToeMark.GetMarkString(TicTacToeLogic.TicTacToeMark.MarkNum.None);
            board20_Button.Content = TicTacToeLogic.TicTacToeMark.GetMarkString(TicTacToeLogic.TicTacToeMark.MarkNum.None);
            board21_Button.Content = TicTacToeLogic.TicTacToeMark.GetMarkString(TicTacToeLogic.TicTacToeMark.MarkNum.None);
            board22_Button.Content = TicTacToeLogic.TicTacToeMark.GetMarkString(TicTacToeLogic.TicTacToeMark.MarkNum.None);
            SetTurnVisiblity();
        }

        /// <summary>
        /// ボタン押下に伴う盤面更新処理
        /// </summary>
        /// <param name="b">押されたボタン</param>
        private void BoardUpdate(Button b, int x, int y)
        {
            // ゲーム中でない場合は何もしない
            if (ticTacToeData.GetTurn() == TicTacToeLogic.TicTacToeMark.MarkNum.None)
            {
                return;
            }
            // xyの妥当性チェック
            if (x < 0 || TicTacToeLogic.COLUMN_SIZE <= x || y < 0 || TicTacToeLogic.ROW_SIZE <= y)
            {
                return;
            }

            // ボードへの入力処理
            bool bSetBoardSuccess = ticTacToeData.SetBoardXY(x, y);

            // ゲーム終了判定
            if (bSetBoardSuccess)
            {
                // 入力に応じて盤面更新
                b.Content = TicTacToeLogic.TicTacToeMark.GetMarkString(ticTacToeData.GetTurn());

                var gameFinish = ticTacToeData.IsFinishedGame();

                if (gameFinish.Item1)
                {
                    // ゲーム終了ならばメッセージ表示
                    MessageBox.Show(
                        ticTacToeData.statement,
                        "Game Finished!",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                else
                {
                    // ゲーム続行
                    ticTacToeData.ContinueGame();
                    SetTurnVisiblity();
                }
            }
        }

        /// <summary>
        /// 画面のターンマーク表示切替え
        /// </summary>
        private void SetTurnVisiblity()
        {
            if (ticTacToeData.GetTurn() == TicTacToeLogic.TicTacToeMark.MarkNum.Circle)
            {
                circleTurnMark_Rectangle.Visibility = Visibility.Visible;
                crossTurnMark_Rectangle.Visibility = Visibility.Hidden;
            }
            else
            {
                circleTurnMark_Rectangle.Visibility = Visibility.Hidden;
                crossTurnMark_Rectangle.Visibility = Visibility.Visible;
            }
        }


        // 以下、イベントハンドラ

        /// <summary>
        /// ボタンクリックイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void reset_Button_Click(object sender, RoutedEventArgs e)
        {
            if (ticTacToeData == null) return;
            ticTacToeData.ResetGame();
            board00_Button.Content = TicTacToeLogic.TicTacToeMark.GetMarkString(TicTacToeLogic.TicTacToeMark.MarkNum.None);
            board01_Button.Content = TicTacToeLogic.TicTacToeMark.GetMarkString(TicTacToeLogic.TicTacToeMark.MarkNum.None);
            board02_Button.Content = TicTacToeLogic.TicTacToeMark.GetMarkString(TicTacToeLogic.TicTacToeMark.MarkNum.None);
            board10_Button.Content = TicTacToeLogic.TicTacToeMark.GetMarkString(TicTacToeLogic.TicTacToeMark.MarkNum.None);
            board11_Button.Content = TicTacToeLogic.TicTacToeMark.GetMarkString(TicTacToeLogic.TicTacToeMark.MarkNum.None);
            board12_Button.Content = TicTacToeLogic.TicTacToeMark.GetMarkString(TicTacToeLogic.TicTacToeMark.MarkNum.None);
            board20_Button.Content = TicTacToeLogic.TicTacToeMark.GetMarkString(TicTacToeLogic.TicTacToeMark.MarkNum.None);
            board21_Button.Content = TicTacToeLogic.TicTacToeMark.GetMarkString(TicTacToeLogic.TicTacToeMark.MarkNum.None);
            board22_Button.Content = TicTacToeLogic.TicTacToeMark.GetMarkString(TicTacToeLogic.TicTacToeMark.MarkNum.None);
            SetTurnVisiblity();
        }

        private void board00_Button_Click(object sender, RoutedEventArgs e)
        {
            BoardUpdate(board00_Button, 0, 0);
        }

        private void board01_Button_Click(object sender, RoutedEventArgs e)
        {
            BoardUpdate(board01_Button, 0, 1);
        }

        private void board02_Button_Click(object sender, RoutedEventArgs e)
        {
            BoardUpdate(board02_Button, 0, 2);
        }

        private void board10_Button_Click(object sender, RoutedEventArgs e)
        {
            BoardUpdate(board10_Button, 1, 0);
        }

        private void board11_Button_Click(object sender, RoutedEventArgs e)
        {
            BoardUpdate(board11_Button, 1, 1);
        }

        private void board12_Button_Click(object sender, RoutedEventArgs e)
        {
            BoardUpdate(board12_Button, 1, 2);
        }

        private void board20_Button_Click(object sender, RoutedEventArgs e)
        {
            BoardUpdate(board20_Button, 2, 0);
        }

        private void board21_Button_Click(object sender, RoutedEventArgs e)
        {
            BoardUpdate(board21_Button, 2, 1);
        }

        private void board22_Button_Click(object sender, RoutedEventArgs e)
        {
            BoardUpdate(board22_Button, 2, 2);
        }
    }
}
