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
    /// PlayerInfo.xaml の相互作用ロジック
    /// </summary>
    public partial class PlayerInfo : UserControl
    {
        #region PlayerInfoクラス独自のプロパティ
        public static readonly DependencyProperty MarkProperty =
            DependencyProperty.Register(
                "Mark",
                typeof(SettingData.OthelloMark),
                typeof(PlayerInfo),
                new PropertyMetadata(
                    SettingData.OthelloMark.None,
                    new PropertyChangedCallback((sender, e) =>
                    {
                        (sender as PlayerInfo).Mark = (SettingData.OthelloMark)e.NewValue;
                    })));
        private SettingData.OthelloMark _mark;
        public SettingData.OthelloMark Mark
        {
            get
            {
                return this._mark;
            }
            set
            {
                this._mark = value;
                if (Mark == SettingData.OthelloMark.Black)
                {
                    MarkEllipse.Fill = Brushes.Black;
                }
                else if (Mark == SettingData.OthelloMark.White)
                {
                    MarkEllipse.Fill = Brushes.White;
                }
            }
        }
        #endregion


        public PlayerInfo()
        {
            InitializeComponent();
        }
    }
}
