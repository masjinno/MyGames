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

namespace TicTacToe
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        //static bool IsDesignInitialized = false;

        public MainWindow()
        {
            InitializeComponent();

            //InitializeDesign();
        }

        //private void InitializeDesign()
        //{
        //    if (IsDesignInitialized)
        //    {
        //        return;
        //    }

        //    PlaceButtonsOnGrid();

        //    IsDesignInitialized = true;
        //}

        //private void PlaceButtonsOnGrid()
        //{
        //    int rows = BoardGrid.RowDefinitions.Count;
        //    int columns = BoardGrid.ColumnDefinitions.Count;

        //    Binding commandBinding;
        //    Button button;

        //    for (int x = 0; x < columns; x++)
        //    {
        //        for (int y = 0; y < rows; y++)
        //        {
        //            // 配置する
        //            button = new Button();
        //            button.Background = new ImageBrush();
        //            Grid.SetRow(button, y);
        //            Grid.SetColumn(button, x);
        //            BoardGrid.Children.Add(button);
        //        }
        //    }
        //}
    }
}
