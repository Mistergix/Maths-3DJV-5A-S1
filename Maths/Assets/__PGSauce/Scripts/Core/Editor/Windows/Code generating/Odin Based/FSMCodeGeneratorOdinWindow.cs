using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using PGSauce.Core.PGFiniteStateMachine.ScriptableObjects;
using Sirenix.OdinInspector;
using UnityEditor;

namespace PGSauce.Core.PGEditor
{
    public class FSMCodeGeneratorOdinWindow : CodeGeneratingOdinWindowBase, IActionTemplate, IDecisionTemplate, IStateTemplate, IStateControllerTemplate, ITransitionTemplate
    {
        [MenuItem("PG/Code Generation/FSM, Action, Decision")]
        public static void OpenWindow()
        {
            ShowWindow<FSMCodeGeneratorOdinWindow>("FSM, Action, Decision");
        }
        
        protected override string InsidePgSauceSubNameSpace => "PGFiniteStateMachine";

        public string FSMName => _name;

        public string ActionName => _actionName;

        public string DecisionName => _decisionName;

        [ShowInInspector, LabelText("Name of The FSM"), PropertyOrder(-500)]
        [ValidateInput("IsNotNullOrEmpty", "Can't be empty!")]
        private string _name = "";
        
        private FSMGenerationAction _fsmGenerationAction;
        private FSMActionGenerationAction _fsmActionGenerationAction;
        private FSMDecisionGenerationAction _fsmDecisionGenerationAction;
        
        private string _baseScriptsPath = "";
        private string _defaultValue = "";
        
        [ShowInInspector, TabGroup("New Action"), LabelText("Name of The Action")]
        [ValidateInput("IsNotNullOrEmpty", "Can't be empty!")]
        private string _actionName = "";
        
        [Button("Generate Action", ButtonSizes.Large), TabGroup("New Action"), GUIColor(0,1,0)]
        private void GenerateActionButton()
        {
            _fsmActionGenerationAction.Generate();
        }
        
        [ShowInInspector, TabGroup("New Decision"), LabelText("Name of The Decision")]
        [ValidateInput("IsNotNullOrEmpty", "Can't be empty!")]
        private string _decisionName = "";
        
        [Button("Generate Decision", ButtonSizes.Large), TabGroup("New Decision"), GUIColor(0,1,0)]
        private void GenerateDecisionButton()
        {
            _fsmDecisionGenerationAction.Generate();
        }
        
        [Button("Generate New FSM", ButtonSizes.Large), TabGroup("New FSM"), GUIColor(0,1,0), PropertyOrder(-100)]
        private void GenerateFSMButton()
        {
            _fsmGenerationAction.Generate();
        }
        
        protected override void OnInspectorGUIBehaviour()
        {
            base.OnInspectorGUIBehaviour();
            _fsmGenerationAction = new FSMGenerationAction(this);
            _fsmActionGenerationAction = new FSMActionGenerationAction(this);
            _fsmDecisionGenerationAction = new FSMDecisionGenerationAction(this);
        }
        
        public bool IsNotNullOrEmpty(string val)
        {
            return !string.IsNullOrEmpty(val);
        }

        public void GenerateNewFSM()
        {
            GenerateFolders();
            
            GenerateStateController();
            GenerateState();
            GenerateTransition();
            
            GenerateFSMDecision( "True", _baseScriptsPath, "true");
            GenerateFSMDecision("False", _baseScriptsPath, "false");
            
            /*
            EditorPrefs.SetBool("ShouldCreateAsset", true);
            EditorPrefs.SetString("FalseDecisionAssetName", string.Format("{0}Decision{1}", "False", objectName));
            EditorPrefs.SetString("TrueDecisionAssetName", string.Format("{0}Decision{1}", "True", objectName));
            EditorPrefs.SetString("StateAssetName", string.Format("State{0}", objectName));
            */
        }
        
        public void GenerateNewFSMAction()
        {
            var result = Templates.fsmAction.ReplaceTemplateWithData(new ActionTemplate(this));
            var intoPath = Path.Combine(AssetsDirPath, LocalSelectedPath);
            var filename = $"{_actionName}Action{FSMName}.cs";

            AbstractGenerateOneFile(result, intoPath, filename);
        }
        
        public void GenerateNewFSMDecision()
        {
            GenerateFSMDecision(DecisionName, Path.Combine(AssetsDirPath, LocalSelectedPath), "true || false");
        }
        
        private void GenerateFolders()
        {
            var folderPath = Path.Combine(AssetsDirPath, LocalSelectedPath, $"State Machine {FSMName}");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            Directory.CreateDirectory(_baseScriptsPath = Path.Combine(folderPath, "__BASE SCRIPTS"));

            Directory.CreateDirectory(Path.Combine(folderPath, "User Scripts"));

            Directory.CreateDirectory(Path.Combine(folderPath, "User Scripts", "Actions"));
            Directory.CreateDirectory(Path.Combine(folderPath, "User Scripts", "Decisions"));

            Directory.CreateDirectory(Path.Combine(folderPath, "Scriptable Objects"));

            Directory.CreateDirectory(Path.Combine(folderPath, "Scriptable Objects", "Actions"));
            Directory.CreateDirectory(Path.Combine(folderPath, "Scriptable Objects", "Decisions"));
            Directory.CreateDirectory(Path.Combine(folderPath, "Scriptable Objects", "States"));
        }
        
        private void GenerateStateController()
        {
            var result = Templates.fsmStateController.ReplaceTemplateWithData( new StateControllerTemplate(this));
            var intoPath = Path.Combine(_baseScriptsPath);
            var filename = $"{StateControllerName()}.cs";
            
            AbstractGenerateOneFile(result, intoPath, filename);
        }
        private void GenerateState()
        {
            var result = Templates.fsmState.ReplaceTemplateWithData(new StateTemplate(this));
            var intoPath = Path.Combine(_baseScriptsPath);
            var filename = $"State{FSMName}.cs";
            
            AbstractGenerateOneFile(result, intoPath, filename);
        }
        
        private void GenerateTransition()
        {
            var result = Templates.fsmTransition.ReplaceTemplateWithData(new TransitionTemplate(this));
            var intoPath = Path.Combine(_baseScriptsPath);
            var filename = $"Transition{FSMName}.cs";
            
            AbstractGenerateOneFile(result, intoPath, filename);
        }
        
        private void GenerateFSMDecision(string decisionName, string pathToScript, string defaultValue)
        {
            _decisionName = decisionName;
            _defaultValue = defaultValue;

            var result = Templates.fsmDecision.ReplaceTemplateWithData(new DecisionTemplate(this));
            var intoPath = Path.Combine(pathToScript);
            var filename = $"{_decisionName}Decision{FSMName}.cs";

            AbstractGenerateOneFile(result, intoPath, filename);
        }
        
        private string StateControllerName()
        {
            return "StateController" + char.ToUpper(FSMName[0]) + FSMName.Substring(1);
        }
        
        public string SUBNAMESPACE()
        {
            return GetSubNamespace();
        }

        public string STATECONTROLLERNAME()
        {
            return StateControllerName();
        }

        public string NAME()
        {
            return FSMName;
        }

        public string DECISIONNAME()
        {
            return _decisionName;
        }

        public string DEFAULTVALUE()
        {
            return _defaultValue;
        }
        
        public string ACTIONNAME()
        {
            return _actionName;
        }
        
        /*
        [UnityEditor.Callbacks.DidReloadScripts]
        private static void CreateAssetWhenReady()
        {
            if (EditorApplication.isCompiling || EditorApplication.isUpdating)
            {
                EditorApplication.delayCall += CreateAssetWhenReady;
                return;
            }

            EditorApplication.delayCall += CreateAssetNow;
        }

        private static void CreateAssetNow()
        {
            bool generate = EditorPrefs.GetBool("ShouldCreateAsset", false);

            if (generate)
            {
                string fDecisionName = EditorPrefs.GetString("FalseDecisionAssetName");
                string tDecisionName = EditorPrefs.GetString("TrueDecisionAssetName");
                string stateName = EditorPrefs.GetString("StateAssetName");

                Type fDecisionType = Type.GetType(fDecisionName);
                Type tDecisionType = Type.GetType(tDecisionName);
                Type stateType = Type.GetType(stateName);

                Debug.Log(fDecisionName + " " + tDecisionName + " " + stateName);
                Debug.Log(fDecisionType +" " + tDecisionType + " " + stateType);
                Debug.Log(fDecisionType.FullName + " " + tDecisionType.Name + " " + stateType.Name);
            }


            EditorPrefs.SetBool("ShouldCreateAsset", false);
            EditorPrefs.SetString("FalseDecisionAssetName", string.Empty);
            EditorPrefs.SetString("TrueDecisionAssetName", string.Empty);
            EditorPrefs.SetString("StateAssetName", string.Empty);
        }
        */

        
    }
}
