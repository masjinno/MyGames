using MyGames.Model;
using MyGames.Resource;
using MyUtility;
using Prism.Mvvm;
using Prism.Commands;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Media;

namespace MyGames.ViewModel
{
    class GamesInfoVM : BindableBase
    {
        #region Binding用のプロパティ

        /// <summary>
        /// ゲーム説明
        /// </summary>
        private string _description;
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                SetProperty(ref _description, value);
            }
        }

        /// <summary>
        /// ゲーム名称一覧
        /// </summary>
        private List<string> _gameNameList;
        public List<string> GameNameList
        {
            get
            {
                return _gameNameList;
            }
            set
            {
                SetProperty(ref _gameNameList, value);
            }
        }

        /// <summary>
        /// ゲームインデックス
        /// </summary>
        private int _selectedGameIndex;
        public int SelectedGameIndex
        {
            get
            {
                return _selectedGameIndex;
            }
            set
            {
                SetProperty(ref _selectedGameIndex, value);
                SelectedGameIndexChanged(SelectedGameIndex);
            }
        }
        #endregion

        #region Binding用のコマンド

        /// <summary>
        /// ゲーム実行ボタンが押されたときのコマンド
        /// </summary>
        public ICommand ExecuteCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    System.Windows.MessageBox.Show("ExecuteCommand");
                });
            }
        }

        /// <summary>
        /// ゲーム実行ボタンが押されたときのコマンド
        /// </summary>
        public ICommand QuitCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    System.Windows.MessageBox.Show("QuitCommand");
                });
            }
        }

        #endregion

        #region MV内部処理用プロパティ
        /// <summary>
        /// 起動可能なゲームのリストのデフォルト値。
        /// 設定ファイルが存在しなかった場合、これを適用する。
        /// </summary>
        private GamesInfoM gamesInfoModel { get; set; }
        #endregion

        /// <summary>
        /// GamesInfoコンストラクタ
        /// </summary>
        public GamesInfoVM()
        {
            Initialize();
        }

        /// <summary>
        /// ゲーム設定の初期化
        /// </summary>
        private void Initialize()
        {
            gamesInfoModel = new GamesInfoM();

            // 読み込んだゲーム数を格納する
            int numGames;

            // 設定ファイルロード
            numGames = gamesInfoModel.LoadConfigFile();
            if (numGames <= 0)
            {
                // 設定ファイルロードに失敗したら、デフォルト値をロードする
                gamesInfoModel.LoadDefaultConfig();
            }

            // ロードした設定をプロパティに反映
            GameNameList = gamesInfoModel.GetGameNames();
            SelectedGameIndex = 0;

            Description = gamesInfoModel.GetGameDescription(SelectedGameIndex);
        }

        /// <summary>
        /// ゲームインデックス変更時の処理
        /// </summary>
        /// <param name="newGameIndex">変更後のゲームインデックス</param>
        private void SelectedGameIndexChanged(int newGameIndex)
        {
            Description = gamesInfoModel.GetGameDescription(newGameIndex);
        }
    }
}
