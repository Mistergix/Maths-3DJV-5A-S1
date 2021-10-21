using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using PGSauce.Core.PGEditor;
using PGSauce.Core.PGEvents;
using PGSauce.Core.PGFiniteStateMachine.ScriptableObjects;
using Sirenix.OdinInspector;
using UnityEditor;

namespace PGSauce.Core.PGEditor
{
    public class EventCodeGeneratorOdinWindow : CodeGeneratingOdinWindowBase, IUnityEventTemplate, IPGEventGenericTemplate, IPGEventListenerGenericTemplate, IPGEventListenerTemplate, IPGEventTemplate
    {
        [MenuItem("PG/Code Generation/New Event Type")]
        public static void OpenWindow()
        {
            ShowWindow<EventCodeGeneratorOdinWindow>("Event code generator");
        }

        protected override string InsidePgSauceSubNameSpace => "PGEvents";

        [ShowInInspector]
        [ValidateInput("LessThan5Args", "Can't have more than 4 args !")]
        private List<string> _types = new List<string>();

        [ShowInInspector, FoldoutGroup("Results")]
        private string Types => string.Join(",", _types.Select(type => type.Trim()).Where(type => !string.IsNullOrEmpty(type)));
        
        [ShowInInspector, FoldoutGroup("Inside Pg Sauce", expanded: false), PropertyOrder(-1000)]
        private const string LocalPathEvents = "__PGSauce/Events";
        
        private EventGenerationAction _eventGenerationAction;
        private int _numberArgs;
        
        private string _formattedTypes;
        private string _formattedSpacedTypes;
        
        [Button("Generate Event")]
        private void GenerateEventButton()
        {
            _eventGenerationAction.Generate();
        }
        
        protected override void OnInspectorGUIBehaviour()
        {
            base.OnInspectorGUIBehaviour();
            _eventGenerationAction = new EventGenerationAction(this);
        }

        private bool LessThan5Args(List<string> list)
        {
            return list.Count < 5;
        }

        public (bool isOK, string errorMessage) VerifyCriticalData()
        {
            if (Types.Length <= 0)
            {
                return (false, "Types must be not empty");
            }

            if (!LessThan5Args(_types))
            {
                return (false, "Can't have more than 4 args !");
            }

            return CodeGenerationAction.OkData;
        }

        public void GenerateEvent()
        {
            var typesList = Types.Trim().Split(',').ToList();

            for (var i = 0; i < typesList.Count; i++)
            {
                typesList[i] = typesList[i].Trim();
            }
            
            _numberArgs = typesList.Count;
            _formattedTypes = FormatTypes(typesList);
            _formattedSpacedTypes = FormatSpaceTypes(typesList);

            GenerateUnityEventCode();
            //GeneratePGEventCodeGeneric();
            //GeneratePGEventListenerCodeGeneric();
            GeneratePGEventCode();
            GeneratePGEventListenerCode();

            if (IsInsidePGSauce)
            {
                var eventsPath = Path.Combine(AssetsDirPath, LocalPathEvents, _formattedSpacedTypes);

                if (!Directory.Exists(eventsPath))
                {
                    Directory.CreateDirectory(eventsPath);
                }
            }
        }

        private void GenerateUnityEventCode()
        {
            var result = Templates.unityEvent.ReplaceTemplateWithData(new UnityEventTemplate(this));
            var intoPath = Path.Combine(AssetsDirPath, LocalSelectedPath);
            var filename = $"UnityEvent{_formattedTypes}.cs";
            
            AbstractGenerateOneFile(result, intoPath, filename);
        }
        
        private void GeneratePGEventCodeGeneric()
        {
            var result = Templates.pgEventGeneric.ReplaceTemplateWithData(new PGEventGenericTemplate(this));
            var intoPath = Path.Combine(AssetsDirPath, LocalSelectedPath, $"{_numberArgs} args");
            var filename = $"PGEvent{_numberArgs}Args.cs";

            AbstractGenerateOneFile(result, intoPath, filename);
        }
        
        private void GeneratePGEventListenerCodeGeneric()
        {
            var result = Templates.pgEventListenerGeneric.ReplaceTemplateWithData(new PGEventListenerGenericTemplate(this));
            var intoPath = Path.Combine(AssetsDirPath, LocalSelectedPath, $"{_numberArgs} args");
            var filename = $"PGEventListener{_numberArgs}Args.cs";

            AbstractGenerateOneFile(result, intoPath, filename);
        }
        
        private void GeneratePGEventCode()
        {
            var result = Templates.pgEvent.ReplaceTemplateWithData(new PGEventTemplate(this));
            var intoPath = Path.Combine(AssetsDirPath, LocalSelectedPath, $"{_numberArgs} args", _formattedSpacedTypes);
            var filename = $"PGEvent{_formattedTypes}.cs";

            AbstractGenerateOneFile(result, intoPath, filename);
        }
        
        private void GeneratePGEventListenerCode()
        {
            var result = Templates.pgEventListener.ReplaceTemplateWithData(new PGEventListenerTemplate(this));
            var intoPath = Path.Combine(AssetsDirPath, LocalSelectedPath, $"{_numberArgs} args", _formattedSpacedTypes);
            var filename = $"PGEventListener{_formattedTypes}.cs";

            AbstractGenerateOneFile(result, intoPath, filename);
        }

        private static string FormatTypes(IReadOnlyList<string> typesList)
        {
            return AbstractFormatting(typesList, (value, index) => char.ToUpper(value[0]) + value.Substring(1).ToLower());
        }

        private static string FormatSpaceTypes(IReadOnlyList<string> typesList)
        {
            return AbstractFormatting(typesList, (value, index) =>
            {
                if (index == typesList.Count - 1)
                {
                    return char.ToUpper(value[0]) + value.Substring(1).ToLower();
                }

                return char.ToUpper(value[0]) + value.Substring(1).ToLower() + " ";
            });
        }
        
        private static string GetGenericTypes(int count)
        {
            return AbstractFormatting(EmptyStringList(count),
                (value, index) => index == count - 1 ? $"T{index}" : $"T{index}, ");
        }

        private static string GetGenericArguments(int count)
        {
            return AbstractFormatting(EmptyStringList(count),
                (value, index) => index == count - 1 ? $"T{index} value{index}" : $"T{index} value{index}, ");
        }
        
        private static string GetGenericValues(int count)
        {
            return AbstractFormatting(EmptyStringList(count),
                (value, index) => index == count - 1 ? $"value{index}" : $"value{index}, ");
        }
        
        private static List<string> EmptyStringList(int count)
        {
            return Enumerable.Repeat("", count).ToList();
        }

        public string SUBNAMESPACE()
        {
            return GetSubNamespace();
        }

        public string FORMATTEDSPACEDTYPES()
        {
            return _formattedSpacedTypes;
        }

        public string NUMBERARG()
        {
            return _numberArgs.ToString();
        }

        public string GENERICTYPES()
        {
            return GetGenericTypes(_numberArgs);
        }

        public string GENERICARGUMENTS()
        {
            return GetGenericArguments(_numberArgs);
        }

        public string GENERICVALUES()
        {
            return GetGenericValues(_numberArgs);
        }

        public string FORMATTEDTYPES()
        {
            return _formattedTypes;
        }

        public string TYPES()
        {
            return Types;
        }
    }
}
