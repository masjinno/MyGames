using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows;

namespace MyGames
{
    class GameInfo
    {
        // 型定義 ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        // メンバ変数定義 ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        /// <summary>
        /// ゲーム名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// ゲーム説明
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// EXEパス
        /// </summary>
        private string Path;

        /// <summary>
        /// EXEコマンド引数
        /// </summary>
        private string Arguments;

        /// <summary>
        /// プロセス情報
        /// </summary>
        private Process process;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">ゲーム名</param>
        /// <param name="path">EXEパス(基本的に相対パス)</param>
        /// <param name="description">ゲーム説明</param>
        /// <param name="arguments">コマンド引数</param>
        public GameInfo(string name, string path, string description ="NoDescription", string arguments="")
        {
            // ゲーム情報セット
            this.Name = name;
            this.Description = description;
            this.Path = path;
            this.Arguments = arguments;
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
                // 終了していないなら、プロセスを閉じる
                if (!process.HasExited)
                {
                    try
                    {
                        return process.CloseMainWindow();
                    }
                    catch (Exception e)
                    {
                        return false;
                    }
                }
            }
            
            // 終了するものがない場合、falseを返す
            return false;
        }
    }
}
