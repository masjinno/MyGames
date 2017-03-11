using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUtility
{
    /// <summary>
    /// 画像入出力に関する静的クラス
    /// </summary>
    public static class ImageIO
    {
        /// <summary>
        /// 画像ファイルをロードする
        /// </summary>
        /// <param name="path">画像ファイルパス</param>
        /// <returns>画像ソース</returns>
        public static ImageSource LoadImageFile(string path)
        {
            ImageSource imageSource;
            using (FileStream fs = File.OpenRead(path))
            {
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.StreamSource = fs;
                bi.CacheOption = BitmapCacheOption.OnLoad;
                bi.EndInit();
                imageSource = bi;
            }
            return imageSource;
        }
    }
}
