using System;
using System.Text;
using System.Runtime.InteropServices;

namespace MyUtility
{
    /// <summary>
    /// 参考にしたサイト
    ///   URL: https://gist.github.com/katabamisan/5231237
    /// </summary>
    public static class IniFile
    {
        /// <summary>
        /// Iniファイルの指定されたセクション内にある指定されたキーに関連づけられている文字列を取得する
        /// </summary>
        /// <param name="lpAppName">セクション名</param>
        /// <param name="lpKeyName">キー名</param>
        /// <param name="lpDefault">見つからなかった場合のデフォルト値</param>
        /// <param name="lpReturnedString">読みとったキーの値</param>
        /// <param name="nSize">バッファサイズ</param>
        /// <param name="lpFileName">iniファイルパス。フルパスを指定すること。</param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        public static extern uint GetPrivateProfileString(
            string lpAppName,
            string lpKeyName,
            string lpDefault,
            StringBuilder lpReturnedString,
            uint nSize,
            string lpFileName);

        /// <summary>
        /// Iniファイルの指定されたセクション内にある指定されたキーに関連づけられている整数を取得する
        /// </summary>
        /// <param name="lpAppName">セクション名</param>
        /// <param name="lpKeyName">キー名</param>
        /// <param name="nDefault">見つからなかった場合のデフォルト値</param>
        /// <param name="lpFilename">iniファイルパス。フルパスを指定すること。</param>
        /// <returns>読み取ったキーの値。キーの値が負であれば0を返す。</returns>
        [DllImport("kernel32.dll")]
        public static extern uint GetPrivateProfileInt(
            string lpAppName,
            string lpKeyName,
            int nDefault,
            string lpFilename);

        /// <summary>
        /// Iniファイルから、指定されたセクション内の全てのキーと値を取得する
        /// </summary>
        /// <param name="lpAppName">セクション名</param>
        /// <param name="lpReturnedString">全てのキーと結果の格納バッファ。"key=value\0key=value\0...\0key=value\0\0"の形式</param>
        /// <param name="nSize">バッファサイズ</param>
        /// <param name="lpFileName">iniファイルパス。フルパスを指定すること。</param>
        /// <returns>バッファに格納された文字列</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi)]
        public static extern uint GetPrivateProfileSection(
            string lpAppName,
            IntPtr lpReturnedString,
            uint nSize,
            string lpFileName);


        // 以下、ラッパー的な関数


        /// <summary>
        /// 指定のセクションが持つキーを配列として返す
        /// ※特定のキー名に限定する
        /// </summary>
        /// <param name="lpAppName">セクション名</param>
        /// <param name="lpFileName">ファイルパス。フルパスを指定すること。</param>
        /// <returns></returns>
        public static string[] GetPrivateProfileSectionAsStringArray(
            string lpAppName,
            string lpFileName
            )
        {
            const int BUFFER_EXPANDING_SIZE = 256;
            IntPtr buf = IntPtr.Zero;
            try
            {
                int length = 0;
                int copied = 0;
                do
                {
                    length += BUFFER_EXPANDING_SIZE;
                    buf = Marshal.ReAllocCoTaskMem(buf, length);
                    copied = (int)GetPrivateProfileSection(lpAppName, buf, (uint)length, lpFileName);
                } while (copied + 2 == length);
                return Marshal.PtrToStringAuto(buf, copied - 1).Split('\0');
            }
            catch (ArgumentException e)
            {
                // 処理中に引数例外が発生した場合は、指定セクション内にキーなしとみなしてnullを返す
                return null;
            }
            finally
            {
                Marshal.FreeCoTaskMem(buf);
            }
        }
    }
}
