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

namespace TicTacToe.View
{
    /// <summary>
    /// PlayerInfo.xaml の相互作用ロジック
    /// </summary>
    public partial class PlayerInfo : UserControl
    {
        #region クラス独自のプロパティ
        /// <summary>
        /// 盤面に表示するマーク文字列
        /// </summary>
        public static readonly DependencyProperty MarkProperty =
            DependencyProperty.Register(
                "Mark",
                typeof(string),
                typeof(PlayerInfo),
                new PropertyMetadata(
                    "",
                    new PropertyChangedCallback((sender, e) =>
                    {
                        (sender as PlayerInfo).Mark = (string)e.NewValue;
                    })));
        private string _mark;
        public string Mark
        {
            get
            {
                return _mark;
            }
            set
            {
                _mark = value;
                PlayerMarkTextBlock.Text = Mark;
            }
        }
        #endregion

        public PlayerInfo()
        {
            InitializeComponent();
        }
    }
}
