using Othello.Resource;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello.ViewModel
{
    class ViewModel : BindableBase
    {
        #region Binding用のプロパティ

        private SettingData.OthelloMark[,] _boardMark;
        public SettingData.OthelloMark[,] BoardMark
        {
            get
            {
                return _boardMark;
            }
            set
            {
                SetProperty(ref _boardMark, value);
            }
        }

        private int _blackCount;
        public int BlackCount
        {
            get
            {
                return _blackCount;
            }
            set
            {
                SetProperty(ref _blackCount, value);
            }
        }

        private int _whiteCount;
        public int WhiteCount
        {
            get
            {
                return _whiteCount;
            }
            set
            {
                SetProperty(ref _whiteCount, value);
            }
        }
        #endregion

        public ViewModel()
        {
            
        }
    }
}
