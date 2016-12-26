using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.IO;

namespace MyGames
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
        public Image SampleImage { get; private set; }

        
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
            // 引数格納
            this.Name = name;

            if (System.IO.File.Exists(path))
            {
                this.Path = path;
            }
            else
            {
                this.Path = "";
                MessageBox.Show("invalid exe path.", "Error.", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            this.Description = description;

            if (System.IO.File.Exists(imagePath))
            {
                this.SampleImagePath = imagePath;
            }
            else
            {
                // デフォルトイメージのパス(2択)
                if (File.Exists(@"..\..\NoImage.png"))
                {
                    this.SampleImagePath = @"..\..\NoImage.png";
                }
                else
                {
                    this.SampleImagePath = @".\Games\NoImage.png";
                }
            }

            this.Arguments = arguments;

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
                    this.SampleImage = new Image();
                    this.SampleImage.Source = bi;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
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
                    MessageBox.Show(
                        "Game has already started.",
                        "Game start failed.",
                        MessageBoxButton.OK,
                        MessageBoxImage.Exclamation);
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
                MessageBox.Show(e.ToString(), "exception");
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
                    MessageBox.Show(e.ToString(), "exception");
                    return false;
                }
            }
            
            // 終了するものがない場合、falseを返す
            return false;
        }
        

        ///// <summary>
        ///// SampleImageを、Name.pngとして保存する
        ///// </summary>
        //public void saveSampleImage()
        //{
        //    BitmapEncoder encoder = new PngBitmapEncoder();
        //    BitmapSource bmpSrc = SampleImage.Source as BitmapSource;
        //    encoder.Frames.Add(BitmapFrame.Create(bmpSrc));
        //    using (FileStream fs =
        //        new FileStream(this.Name + ".png", System.IO.FileMode.Create))
        //    {
        //        encoder.Save(fs);
        //    }
        //}
    }
}
