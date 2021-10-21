using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using PGSauce.Core.PGEditor;
using Sirenix.OdinInspector;
using UnityEditor;

namespace PGSauce.Core.PGEditor
{
    public class GlobalVariableGeneratorOdinWindow : CodeGeneratingOdinWindowBase, IGlobalVariableTemplate
    {
        [MenuItem("PG/Code Generation/Global Variable")]
        private static void OpenWindow()
        {
            ShowWindow<GlobalVariableGeneratorOdinWindow>("Global Variable");
        }

        [ShowInInspector, FoldoutGroup("Inside Pg Sauce", expanded: false), PropertyOrder(-1000)]
        private const string LocalPathGv = "__PGSauce/Global Variables";

        [ShowInInspector]
        [ValidateInput("IsNotNullOrEmpty", "Can't be empty !")]
        private string _variableType = "";
        
        [ShowInInspector, FoldoutGroup("Results")]
        private string FormattedType => FormatType(_variableType);
        [ShowInInspector, FoldoutGroup("Results")]
        private string FullPath => Path.Combine(AssetsDirPath, LocalSelectedPath, $"Global{FormattedType}.cs");

        private GlobalVariableGenerationAction _globalVariableGenerationAction;
        
        [Button("Generate Global Variable")]
        private void GenerateGV()
        {
            _globalVariableGenerationAction.Generate();
        }

        protected override void OnInspectorGUIBehaviour()
        {
            base.OnInspectorGUIBehaviour();
            _globalVariableGenerationAction = new GlobalVariableGenerationAction(this);
        }

        public void GenerateGVCode()
        {
            var result = Templates.globalVariable.ReplaceTemplateWithData(new GlobalVariableTemplate(this));

            var intoPath = FullPath;
            
            File.WriteAllText(intoPath, result);
            
            if (IsInsidePGSauce)
            {
                var gvPath = Path.Combine(AssetsDirPath, LocalPathGv, FormattedType);

                if (!Directory.Exists(gvPath))
                {
                    Directory.CreateDirectory(gvPath);
                }
            }
        }
        
        public (bool isOK, string errorMessage) VerifyCriticalData()
        {
            if (! IsNotNullOrEmpty(_variableType))
            {
                return (false, "Type must be not empty");
            }

            return CodeGenerationAction.OkData;
        }

        private bool IsNotNullOrEmpty(string val)
        {
            return !string.IsNullOrEmpty(val);
        }
        
        private static string FormatType(string variableType)
        {
            if (variableType.Length <= 0)
            {
                return "";
            }
            
            var result = variableType.Trim().Replace("<", "")
                .Replace(">", "");


            return char.ToUpper(result[0]) + result.Substring(1);
        }


        public string SUBNAMESPACE()
        {
            return GetSubNamespace();
        }

        public string FORMATTEDTYPE()
        {
            return FormattedType;
        }

        public string TYPE()
        {
            return _variableType;
        }

        
    }
}
