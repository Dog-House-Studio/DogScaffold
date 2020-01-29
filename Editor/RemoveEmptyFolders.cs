#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using UnityEditor;

using UnityEngine;


namespace DogScaffold
{
    /// <summary>
    /// Removes empty folders automatically when assets are saved.
    /// 
    /// Source: https://gist.github.com/mob-sakai/b98a62c1f2c94c1fef9021101635a5cd
    /// </summary>
    public static partial class EditorHelper
    {
        private const string kMenuText = "DogHouse/(Editor) Remove Empty Folders";
        private static readonly StringBuilder s_Log = new StringBuilder();
        private static readonly List<DirectoryInfo> s_Results = new List<DirectoryInfo>();


        /// <summary>
        /// Raises the will save assets event.
        /// </summary>
        private static string[] OnWillSaveAssets(string[] paths)
        {
            // Get empty directories in Assets directory
            s_Results.Clear();
            var assetsDir = Application.dataPath + Path.DirectorySeparatorChar;
            GetEmptyDirectories(new DirectoryInfo(assetsDir), s_Results);

            // When empty directories has detected, remove the directory.
            if (0 < s_Results.Count)
            {
                s_Log.Length = 0;
                s_Log.AppendFormat("Remove {0} empty directories as following:\n", s_Results.Count);
                foreach (var d in s_Results)
                {
                    s_Log.AppendFormat("- {0}\n", d.FullName.Replace(assetsDir, ""));
                    FileUtil.DeleteFileOrDirectory(d.FullName);
                }

                // UNITY BUG: Debug.Log can not set about more than 15000 characters.
                s_Log.Length = Mathf.Min(s_Log.Length, 15000);
                Debug.Log(s_Log.ToString());
                s_Log.Length = 0;

                AssetDatabase.Refresh();
            }

            s_Results.Clear();
            return paths;
        }


        [MenuItem(kMenuText)]
        private static void OnClickMenu()
        {
            OnWillSaveAssets(null);
        }


        /// <summary>
        /// Get empty directories.
        /// </summary>
        private static bool GetEmptyDirectories(DirectoryInfo dir, List<DirectoryInfo> results)
        {
            bool isEmpty = true;
            try
            {
                isEmpty =
                    dir.GetDirectories().Count(x => !GetEmptyDirectories(x, results)) == 0  // Are sub directories empty?
                    && dir.GetFiles("*.*").All(x => x.Extension == ".meta");                // No file exist?
            }
            catch
            {
            }

            // Store empty directory to results.
            if (isEmpty)
            {
                results.Add(dir);
            }

            return isEmpty;
        }
    }
}

#endif
