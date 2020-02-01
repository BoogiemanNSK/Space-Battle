#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UICoreECS;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace UICoreECS.Editor
{
    [CustomEditor(typeof(ECSUIScreen), true)]
    public class UIScreenEditor : UnityEditor.Editor
    {
        public const string SwitchersParentName = "Switchers";
        
        private ECSUIScreen _screen => (ECSUIScreen) target;
        private ReorderableList _showChainList;
        private ReorderableList _hideChainList;
        private int _switchType = 0;
        private bool _reverseHide;
        
        private void OnEnable()
        {
            
            _showChainList = EditorHelper.DrawReorderableList(serializedObject, "_showSwitchersChain", "ShowChain");
            _showChainList.onRemoveCallback = list => RemoveSwithcer(_screen.ShowSwitchersChain, list.index);
            _showChainList.onChangedCallback = list => UpdateHideChainReverse();
            _showChainList.onReorderCallback = list => UpdateHideChainReverse();
            _showChainList.onAddDropdownCallback = (rect, list) => DrawGenericMenu(_screen.ShowSwitchersChain);
            
            _hideChainList = EditorHelper.DrawReorderableList(serializedObject, "_hideSwitchersChain", "HideChain");
            _hideChainList.onAddCallback = list => {AddSwitchChainElement(_screen.HideSwitchersChain);};
            _hideChainList.onRemoveCallback = list => RemoveSwithcer(_screen.HideSwitchersChain, list.index);
            _hideChainList.onAddDropdownCallback = (rect, list) => DrawGenericMenu(_screen.HideSwitchersChain);
            
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            ECSUIScreen screen = (ECSUIScreen) target;
            if (GUILayout.Button("Collect extensions"))
            {
                SerializedProperty prop = serializedObject.FindProperty("_extensions");
                AUIEntity[] ext = CollectExtensions(screen);
                prop.ClearArray();
                for (int i = 0; i < ext.Length; i++)
                {
                    prop.InsertArrayElementAtIndex(i);
                    prop.GetArrayElementAtIndex(i).objectReferenceValue = ext[i];
                }
            }

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Screen switch chain setup");
            _showChainList.DoLayoutList();
            if (GUILayout.Button(_reverseHide ? "Custom hide chain" : "Reverse show chain at hide"))
            {
                _reverseHide = !_reverseHide;
                if (_reverseHide == false)
                {
                    _screen.HideSwitchersChain = new List<AScreenSwitcher>();
                }
                UpdateHideChainReverse();

            }
            if (!_reverseHide)
            {
                EditorGUILayout.Space();
                _hideChainList.DoLayoutList();
            }
            
            serializedObject.ApplyModifiedProperties();
            
            if (EditorApplication.isPlaying && GUILayout.Button("ShowScreen"))
            {
                screen.ShowSelf();
            }

        }
        
        private AUIEntity[] CollectExtensions(ECSUIScreen screen)
        {
            List<AUIEntity> extensions = CollectRecursive(screen.transform, new List<AUIEntity>());
            return extensions.ToArray();
        }

        private List<AUIEntity> CollectRecursive(Transform current, List<AUIEntity> extensions)
        {
            AUIEntity extension = current.GetComponent<AUIEntity>();
            if (extension != null )
            {
                extensions.Add(extension);  
            }
            for (int i = 0; i < current.childCount; i++)
            {
                if (current.GetChild(i).gameObject.GetComponent<ECSUIScreen>() == null)
                {
                    CollectRecursive(current.GetChild(i), extensions);
                }
            }
            return extensions;
        }

        private void UpdateHideChainReverse()
        {
            if (_reverseHide)
            {
                
                _screen.HideSwitchersChain = new List<AScreenSwitcher>(_screen.ShowSwitchersChain);
                _screen.HideSwitchersChain.Reverse();
            }
        }
        
        private void AddSwitchChainElement(List<AScreenSwitcher> chain)
        {
            Transform parent = _screen.transform.Find(SwitchersParentName);
            if (parent == null)
            {
                parent = new GameObject().transform;
                parent.name = SwitchersParentName;
                parent.parent = _screen.transform;
                parent.SetAsFirstSibling();
            }

            List<Type> switchers = EditorHelper.GetChildTypes(typeof(AScreenSwitcher));
            
            if (_switchType < switchers.Count)
            {
                Transform swithcer = new GameObject().transform;
                swithcer.parent = parent;
                swithcer.name = $"{switchers[_switchType].Name}({parent.childCount})";
                chain.Add(swithcer.gameObject
                    .AddComponent(switchers[_switchType])
                    .GetComponent<AScreenSwitcher>());
            }
            else
            {
                chain.Add(null);
            }

            UpdateHideChainReverse();
        }

        private void RemoveSwithcer(List<AScreenSwitcher> chain, int index)
        {
            if(chain[index] != null)
                DestroyImmediate(chain[index].gameObject);
            chain.RemoveAt(index);
        }

        private void DrawGenericMenu(List<AScreenSwitcher> chain)
        {
            var menu = new GenericMenu();
            string[] options = EditorHelper.GetChildChildTypeNames(typeof(AScreenSwitcher));
            for (int i =0; i < options.Length; i++)
            {
                menu.AddItem(new GUIContent(options[i]), false, data => { _switchType = (int) data; AddSwitchChainElement(chain); }, i);
                
            }
            menu.AddItem(new GUIContent("custom"), false, data => { _switchType = options.Length; AddSwitchChainElement(chain); }, options.Length);
            menu.ShowAsContext();
        }
        
    }
}
#endif