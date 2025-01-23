using UnityEngine;
using UnityEditor;
using Assets;
[CustomEditor(typeof(Star))]
public class StarEditor : Editor
{
    Star star;
    public override void OnInspectorGUI()
    {
        using (var check = new EditorGUI.ChangeCheckScope())
        {
            base.OnInspectorGUI();
            if (check.changed) star.GenerateStar();
        }

        if (GUILayout.Button("Generate")) star.GenerateStar();
        if (GUILayout.Button("Save")) star.Save();
    }
    private void OnEnable()
    {
        star = (Star)target;
    }
}