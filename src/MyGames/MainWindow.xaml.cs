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
using System.IO;

namespace MyGames
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        GamesInfo gamesInfo;
        bool IsClosed;

        /// <summary>
        /// ゲームイメージのデフォルト
        /// </summary>
        Image DefaultGameImage;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindow()
        {
            // WPFとしての立ち上げ処理
            InitializeComponent();

            // メンバ変数初期化
            gamesInfo = new MyGames.GamesInfo();
            IsClosed = false;
            using (FileStream fs = File.OpenRead(@"..\..\NoImage.png"))
            {
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.StreamSource = fs;
                bi.CacheOption = BitmapCacheOption.OnLoad;
                bi.EndInit();
                this.DefaultGameImage = new Image();
                this.DefaultGameImage.Source = bi;
            }

            // GUI初期化
            // リストボックス初期化
            string[] gameNames = gamesInfo.GetAllGameNames();
            foreach (string s in gameNames)
            {
                ListBoxItem lbi = new ListBoxItem();
                lbi.Content = s;
                gameList_ListBox.Items.Add(lbi);
            }
            // Executeボタン初期化
            execute_Button.IsEnabled = false;
        }

        /// <summary>
        /// 起動したすべてのゲームを閉じる。
        /// ただし、閉じる前に、閉じていいか確認するメッセージを表示する。
        /// </summary>
        /// <returns>
        /// true: 閉じる
        /// false: やっぱり閉じない
        /// </returns>
        private bool CloseAllGame()
        {
            // 起動済み全ゲーム終了確認メッセージ
            MessageBoxResult mbr;
            mbr = MessageBox.Show(
                "Are you sure to close all games?",
                "Closing all games",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (mbr == MessageBoxResult.No)
            {
                return false;
            }
            
            // 全ゲーム終了処理
            gamesInfo.CloseAllGame();

            return true;
        }

        /// <summary>
        /// 終了時の処理
        /// </summary>
        private void ClosingProgram()
        {
            // 終了済みならば、判定不要
            if (IsClosed)
            {
                return;
            }

            // ゲーム終了処理。終了するなら、ランチャー終了フラグを立てる。
            if (this.CloseAllGame())
            {
                IsClosed = true;
            }
        }
        

        // 以下、イベントハンドラ定義 ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        /// <summary>
        /// Executeボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void execute_Button_Click(object sender, RoutedEventArgs e)
        {
            gamesInfo.StartGame(gameList_ListBox.SelectedIndex);
        }

        /// <summary>
        /// Quitボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void quit_Button_Click(object sender, RoutedEventArgs e)
        {
            // 終了時の判定処理
            this.ClosingProgram();

            // 終了
            if (IsClosed)
            {
                // Closeメソッドを呼び出すと、Closingイベントが発生する ⇒ 関連づけたWindow_Closingメソッドが呼び出される
                this.Close();
            }
        }

        /// <summary>
        /// 終了処理開始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!IsClosed)
            {
                this.ClosingProgram();
                if (!IsClosed)
                {
                    e.Cancel = true;
                }
            }
        }

        private void gameList_ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (gameList_ListBox.SelectedIndex != -1)
            {
                execute_Button.IsEnabled = true;
                description_TextBox.Text = gamesInfo.GetGameDescription(gameList_ListBox.SelectedIndex);
                game_Image.Source = gamesInfo.GetGameImageSource(gameList_ListBox.SelectedIndex);
            }
            else
            {
                execute_Button.IsEnabled = false;
                description_TextBox.Text = "No Description";
                game_Image.Source = DefaultGameImage.Source;
            }
        }
    }
}
