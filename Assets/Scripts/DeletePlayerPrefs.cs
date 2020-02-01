#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class DeletePlayerPrefsWindow : EditorWindow
{
    [MenuItem("Tools/Delete PlayerPrefs (All)")]
    static void DeleteAllPlayerPrefs() => PlayerPrefs.DeleteAll();
}
#endif