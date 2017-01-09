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
        /// 盤面とボタンの関連付け
        /// </summary>
        Button[,] boardCellButtons;

        /// <summary>
        /// AI管理
        /// </summary>
        AIManager aiManager;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            // ゲーム根幹設定初期化
            ticTacToeData = new TicTacToeData(TicTacToeLogic.ROW_SIZE, TicTacToeLogic.COLUMN_SIZE);
            ResetGame();
            boardCellButtons = new Button[,]
            {
                { board00_Button, board01_Button, board02_Button},
                { board10_Button, board11_Button, board12_Button},
                { board20_Button, board21_Button, board22_Button}
            };

            // 打ち手設定リセット
            aiManager = new AIManager();
            foreach (string s in aiManager.GetAiNames())
            {
                playerCircle_ComboBox.Items.Add(s);
                playerCross_ComboBox.Items.Add(s);
            }
            aiManager.IsEnabled = false;    // ゲーム開始時は人間プレイの設定のためAI無効。AIプレイ設定に変わるのはコンボボックス操作時。
            playCircle_Button.IsEnabled = false;
            playCross_Button.IsEnabled = false;
            playerCircle_ComboBox.SelectedIndex = 0;
            playerCross_ComboBox.SelectedIndex = 0;
            autoPlayCircle_CheckBox.IsChecked = false;
            autoPlayCross_CheckBox.IsChecked = false;
            autoPlayCircle_CheckBox.IsEnabled = false;
            autoPlayCross_CheckBox.IsEnabled = false;
        }

        /// <summary>
        /// ゲーム初期化処理
        /// </summary>
        private void ResetGame()
        {
            if (ticTacToeData == null) return;

            // ゲーム根幹設定リセット
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
        /// <param name="x">x位置</param>
        /// <param name="y">y位置</param>
        private void BoardUpdate(int x, int y)
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
                boardCellButtons[x,y].Content = TicTacToeLogic.TicTacToeMark.GetMarkString(ticTacToeData.GetTurn());

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
                    SetPlayers();
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

        /// <summary>
        /// 手番に対応した先手後手の打ち手情報を設定する。
        /// </summary>
        private void SetPlayers()
        {
            SetPlayerCircle();
            SetPlayerCross();
        }

        /// <summary>
        /// 手番に対応した先手の打ち手情報を設定する。
        /// </summary>
        private void SetPlayerCircle()
        {
            if (ticTacToeData.GetTurn() == TicTacToeLogic.TicTacToeMark.MarkNum.Circle)
            {
                playCircle_Button.IsEnabled = (playerCircle_ComboBox.SelectedIndex >= 1);
                aiManager.IsEnabled = (playerCircle_ComboBox.SelectedIndex == 0);
                autoPlayCircle_CheckBox.IsEnabled = (playerCircle_ComboBox.SelectedIndex >= 1);
                if (autoPlayCircle_CheckBox.IsChecked == true)
                {
                    PutCircleAI();
                }
            }
            else
            {
                playCircle_Button.IsEnabled = false;
            }
        }

        /// <summary>
        /// 手番に対応した後手の打ち手情報を設定する。
        /// </summary>
        private void SetPlayerCross()
        {
            if (ticTacToeData.GetTurn() == TicTacToeLogic.TicTacToeMark.MarkNum.Cross)
            {
                playCross_Button.IsEnabled = (playerCross_ComboBox.SelectedIndex >= 1);
                aiManager.IsEnabled = (playerCross_ComboBox.SelectedIndex == 0);
                autoPlayCross_CheckBox.IsEnabled = (playerCross_ComboBox.SelectedIndex >= 1);
                if (autoPlayCross_CheckBox.IsChecked == true)
                {
                    PutCrossAI();
                }
            }
            else
            {
                playCross_Button.IsEnabled = false;
            }
        }

        /// <summary>
        /// AI先手
        /// </summary>
        private void PutCircleAI()
        {
            Tuple<int, int> result = aiManager.GetAiPutPlace(playerCircle_ComboBox.SelectedIndex - 1, ticTacToeData);
            BoardUpdate(result.Item1, result.Item2);
        }

        /// <summary>
        /// AI後手
        /// </summary>
        private void PutCrossAI()
        {
            Tuple<int, int> result = aiManager.GetAiPutPlace(playerCross_ComboBox.SelectedIndex - 1, ticTacToeData);
            BoardUpdate(result.Item1, result.Item2);
        }


        // 以下、イベントハンドラ

        /// <summary>
        /// リセットボタンクリックイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void reset_Button_Click(object sender, RoutedEventArgs e)
        {
            if (ticTacToeData == null) return;
            ResetGame();
        }

        private void board00_Button_Click(object sender, RoutedEventArgs e)
        {
            if (aiManager.IsEnabled)
            {
                BoardUpdate(0, 0);
            }
        }

        private void board01_Button_Click(object sender, RoutedEventArgs e)
        {
            if (aiManager.IsEnabled)
            {
                BoardUpdate(0, 1);
            }
        }

        private void board02_Button_Click(object sender, RoutedEventArgs e)
        {
            if (aiManager.IsEnabled)
            {
                BoardUpdate(0, 2);
            }
        }

        private void board10_Button_Click(object sender, RoutedEventArgs e)
        {
            if (aiManager.IsEnabled)
            {
                BoardUpdate(1, 0);
            }
        }

        private void board11_Button_Click(object sender, RoutedEventArgs e)
        {
            if (aiManager.IsEnabled)
            {
                BoardUpdate(1, 1);
            }
        }

        private void board12_Button_Click(object sender, RoutedEventArgs e)
        {
            if (aiManager.IsEnabled)
            {
                BoardUpdate(1, 2);
            }
        }

        private void board20_Button_Click(object sender, RoutedEventArgs e)
        {
            if (aiManager.IsEnabled)
            {
                BoardUpdate(2, 0);
            }
        }

        private void board21_Button_Click(object sender, RoutedEventArgs e)
        {
            if (aiManager.IsEnabled)
            {
                BoardUpdate(2, 1);
            }
        }

        private void board22_Button_Click(object sender, RoutedEventArgs e)
        {
            if (aiManager.IsEnabled)
            {
                BoardUpdate(2, 2);
            }
        }

        private void playerCircle_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetPlayerCircle();
        }

        private void playerCross_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetPlayerCross();
        }

        private void playCircle_Button_Click(object sender, RoutedEventArgs e)
        {
            PutCircleAI();
        }

        private void playCross_Button_Click(object sender, RoutedEventArgs e)
        {
            PutCrossAI();
        }
    }
}
