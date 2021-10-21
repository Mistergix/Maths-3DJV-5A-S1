using System.Collections.Generic;
using System.IO;
using System.Text;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

namespace PGSauce.Core
{
    public static class PGAssets
    {
        public static string GetPathUnderMouse(bool sanitized = true)
        {
#if UNITY_EDITOR
            var folderPath = AssetDatabase.GetAssetPath(Selection.activeInstanceID);
            if(sanitized) {folderPath = SanitizePath(folderPath);}
            return folderPath;
#endif
            return "";
        }

        private static string SanitizePath(string folderPath)
        {
#if UNITY_EDITOR
            if (folderPath.Contains("."))
                folderPath = folderPath.Remove(folderPath.LastIndexOf('/'));
            folderPath = folderPath.Replace("Assets/", "");
            folderPath = folderPath.Replace("Assets", "");
            return folderPath;
#endif
            return "";
        }

        public static string GetAssetPath(Object obj, bool sanitized = true)
        {
#if UNITY_EDITOR
            var folderPath = AssetDatabase.GetAssetPath(obj);
            if(sanitized) {folderPath = SanitizePath(folderPath);}
            return folderPath;
#endif
            return "";
        }
        
        public static void AbstractGenerateOneFile(string result, string intoPath, string fileName)
        {
#if UNITY_EDITOR
            if (!Directory.Exists(intoPath))
            {
                Directory.CreateDirectory(intoPath);
            }
            
            intoPath = Path.Combine(intoPath, fileName);
            
            File.WriteAllText(intoPath, result);
#endif
        }
        
        public delegate string Format(string value, int index);
        public static string AbstractFormatting(IReadOnlyList<string> values, Format formatter)
        {
#if UNITY_EDITOR
            var sb = new StringBuilder();
            
            for (var i = 0; i < values.Count; i++)
            {
                sb.Append(formatter(values[i], i)) ;
            }
            
            return sb.ToString();
#endif
            return "";
        }

        public static void SaveAssets()
        {
#if UNITY_EDITOR
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
#endif
            return;
        }
    }
}