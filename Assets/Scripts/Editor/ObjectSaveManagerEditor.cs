using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SaveSystem))]
public class ObjectSaveManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SaveSystem saveSystem = (SaveSystem)target;

        if (GUILayout.Button("Save"))
        {
            saveSystem.Save();
        }

        if (GUILayout.Button("Load"))
        {
            saveSystem.Load();
        }
    }
}
