#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using System;
using System.Linq;

namespace UICoreECS.Editor
{
    public static class EditorHelper
    {

        public const int X_OFFSET = 7;
        public const int Y_OFFSET = 1;
        public const int HEIHGHT_SPACE = 10;
        
        public static ReorderableList DrawReorderableList(SerializedObject serializedObject, string serializedProperty, string header)
        {
            ReorderableList rList = new ReorderableList(serializedObject, serializedObject.FindProperty(serializedProperty), true,true,true,true);
            rList.drawHeaderCallback = rect =>
            {
                EditorGUI.LabelField(rect, header);
            };
            rList.drawElementCallback = (rect, index, active, focused) =>
            {
                var go = rList.serializedProperty.GetArrayElementAtIndex(index);
                rect.y += 2;
                EditorGUI.PropertyField(new Rect(rect.x + X_OFFSET, rect.y+Y_OFFSET, rect.width-X_OFFSET, EditorGUIUtility.singleLineHeight ),go, GUIContent.none);
                
            };
            rList.elementHeight = EditorGUIUtility.singleLineHeight;
            rList.elementHeightCallback = index => EditorGUI.GetPropertyHeight(rList.serializedProperty.GetArrayElementAtIndex(index)) + HEIHGHT_SPACE;
            
            return rList;
        }
        
        public static List<Type> GetChildTypes(Type type)
        {
            System.Reflection.Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            List<Type> result = new List<Type>();
            foreach (var assembly in assemblies)
            {
                result.AddRange(assembly.GetTypes().Where(t => t.IsSubclassOf(type) && !t.IsAbstract));
            }

            return result;
        }

        public static string[] GetChildChildTypeNames(Type type)
        {
            List<Type> ts = GetChildTypes(type);

            String[] names = new string[ts.Count];
            for (int i = 0; i < ts.Count; i++)
            {
                names[i] = ts[i].Name;
            }

            return names;
        }
    }
    
    

}

#endif