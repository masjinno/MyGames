using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;

namespace MyGames.Model
{
    class GameInfo
    {
        // 型定義 ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        // メンバ変数定義 ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        /// <summary>
        /// ゲーム名称。
        /// コンストラクタで指定する。必須。
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// EXEパス。
        /// コンストラクタで指定する。必須。
        /// </summary>
        private string Path;

        /// <summary>
        /// EXEコマンド引数
        /// コンストラクタで指定する。必須でない。
        /// </summary>
        private string Arguments;

        /// <summary>
        /// ゲーム説明。
        /// コンストラクタで指定する。必須でない。
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// サンプルイメージのパス。
        /// コンストラクタで指定する。必須でない。
        /// </summary>
        private string SampleImagePath;

        /// <summary>
        /// サンプルイメージ。
        /// SampleImagePathを元に取得する。必須でない。
        /// </summary>
        public ImageSource SampleImage { get; private set; }


        /// <summary>
        /// プロセス情報。
        /// </summary>
        private Process process;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">ゲーム名</param>
        /// <param name="path">EXEパス(基本的に相対パス)</param>
        /// <param name="description">ゲーム説明</param>
        /// <param name="imagepath">画像パス。ファイルが存在しなければ、デフォルト画像となる。</param>
        /// <param name="arguments">コマンド引数。省略可。</param>
        public GameInfo(
            string name,
            string path,
            string description,
            string imagePath,
            string arguments = ""
            )
        {
            // ゲーム設定格納
            SetName(name);
            SetPath(path);
            SetDescription(description);
            SetSampleImagePath(imagePath);
            SetArguments(arguments);

            // 設定値をもとに使用する準備
            CreateSampleImage();
        }

        /// <summary>
        /// ゲーム名称設定
        /// </summary>
        /// <param name="name"></param>
        private void SetName(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// ゲームファイルパス設定
        /// </summary>
        /// <param name="path"></param>
        private void SetPath(string path)
        {
            if (System.IO.File.Exists(path))
            {
                this.Path = path;
            }
            else
            {
                this.Path = "";
                System.Windows.MessageBox.Show("invalid exe path.", "Error.", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// ゲーム説明設定
        /// </summary>
        /// <param name="description"></param>
        private void SetDescription(string description)
        {
            this.Description = description;
        }

        /// <summary>
        /// ゲームサンプルイメージパス設定
        /// </summary>
        /// <param name="imagePath"></param>
        private void SetSampleImagePath(string imagePath)
        {
            if (System.IO.File.Exists(imagePath))
            {
                this.SampleImagePath = imagePath;
            }
            else
            {
                // デフォルトイメージのパス(2択)
                if (File.Exists(@"..\..\Resource\NoImage.png"))
                {
                    this.SampleImagePath = @"..\..\Resource\NoImage.png";
                }
                else
                {
                    this.SampleImagePath = @".\NoImage.png";
                }
            }
        }

        /// <summary>
        /// ゲームファイル起動時のコマンド引数設定
        /// </summary>
        /// <param name="arguments"></param>
        private void SetArguments(string arguments)
        {
            this.Arguments = arguments;
        }

        /// <summary>
        /// メンバのサンプルイメージパスから画像生成
        /// </summary>
        private void CreateSampleImage()
        {
            try
            {
                // SampleImage生成
                using (FileStream fs = File.OpenRead(this.SampleImagePath))
                {
                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    bi.StreamSource = fs;
                    bi.CacheOption = BitmapCacheOption.OnLoad;
                    bi.EndInit();
                    this.SampleImage = bi;
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// EXE開始
        /// </summary>
        /// <returns>
        /// true: 成功
        /// false: 失敗
        /// </returns>
        public bool StartProgram()
        {
            // プロセス起動
            if (process != null)
            {
                if (!process.HasExited)
                {
                    // 起動済みメッセージ表示
                    System.Windows.MessageBox.Show(
                        "Game has already started.",
                        "Game start failed.",
                        System.Windows.MessageBoxButton.OK,
                        System.Windows.MessageBoxImage.Exclamation);
                    return false;
                }
            }

            // プロセスがない、もしくは終了済みのとき、新たに起動
            this.process = new Process();
            this.process.StartInfo.FileName = this.Path;
            this.process.StartInfo.Arguments = this.Arguments;

            try
            {
                return process.Start();
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.ToString(), "exception");
                return false;
            }
        }

        /// <summary>
        /// ゲーム終了
        /// </summary>
        /// <returns>
        /// true: 成功
        /// fale: 失敗
        /// </returns>
        public bool ExitProgram()
        {
            // 起動していないなら何もせずfalseを返す
            if (process != null)
            {
                try
                {

                    // 終了していないなら、プロセスを閉じる
                    if (!process.HasExited)
                    {
                        return process.CloseMainWindow();
                    }
                }
                catch (Exception e)
                {
                    System.Windows.MessageBox.Show(e.ToString(), "exception");
                    return false;
                }
            }

            // 終了するものがない場合、falseを返す
            return false;
        }
    }
}
