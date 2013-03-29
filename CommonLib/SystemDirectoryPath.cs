using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLib
{
    /// <summary>
    /// システムフォルダパス管理クラス
    /// </summary>
    public class SystemDirectoryPath
    {
        /// <summary>
        /// ルートフォルダパス取得（\付き）
        /// </summary>
        public static string RootPath
        {
            get
            {
                var path = GetCurrentPath();
                // ルートフォルダは1つ上位
                var passArray = path.Split('\\');
                return path.Substring(0, path.Length - passArray.Last().Length);
            }
        }
        /// <summary>
        /// アセンブリフォルダパス取得（\付き）
        /// </summary>
        public static string AssemblyPath
        {
            get
            {
                return GetCurrentPath() + @"\";
            }
        }
        /// <summary>
        /// システム設定フォルダパス取得（\付き）
        /// </summary>
        public static string SystemPath
        {
            get
            {
                return RootPath + @"System\";
            }
        }
        /// <summary>
        /// 一時フォルダパス取得（\付き）
        /// </summary>
        public static string TempPath
        {
            get
            {
                return RootPath + @"Temp\";
            }
        }
        /// <summary>
        /// 項目-CH結び付けパターン保存フォルダパス取得（\付き）
        /// </summary>
        public static string TagChannelPatternPath
        {
            get
            {
                return RootPath + @"Ptn\TagChannel\";
            }
        }
        /// <summary>
        /// 測定パターン保存フォルダパス取得（\付き）
        /// </summary>
        public static string MeasurePatternPath
        {
            get
            {
                return RootPath + @"Ptn\Measure\";
            }
        }
        /// <summary>
        /// 測定データ保存フォルダパス取得（\付き）
        /// </summary>
        public static string MeasureData
        {
            get
            {
                return RootPath + @"MeasureData\";
            }
        }

        /// <summary>
        /// 実行中のアセンブリの現在パスを取得する
        /// </summary>
        /// <returns>実行中の現在パス</returns>
        private static string GetCurrentPath()
        {
            string assemblyPath = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            string localPath = (new System.Uri(assemblyPath)).LocalPath;
            return System.IO.Path.GetDirectoryName(localPath);
        }
    }
}
