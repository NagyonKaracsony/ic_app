using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(Planet))]
public class PlanetEditor : Editor
{
    Planet planet;
    Editor shapeEditor;
    Editor colorEditor;
    public override void OnInspectorGUI()
    {
        using (var check = new EditorGUI.ChangeCheckScope())
        {
            base.OnInspectorGUI();
            if (check.changed) planet.GeneratePlanet();
        }

        if (GUILayout.Button("Generate Planet")) planet.GeneratePlanet();
        if (GUILayout.Button("Save Planet Settings")) planet.SaveTo($"C:\\Asztali gép\\test/{this.planet.saveTemplateAs}.json");
        // if (GUILayout.Button("Log Planet Settings")) planet.LogData();

        DrawSettingsEditor(planet.shapeSettings, planet.OnShapeSettingsUpdated, ref planet.shapeSettingsFoldout, ref shapeEditor);
        DrawSettingsEditor(planet.colorSettings, planet.OnColorSettingsUpdated, ref planet.colorSettingsFoldout, ref colorEditor);
    }

    void DrawSettingsEditor(Object settings, System.Action onSettingsUpdated, ref bool foldout, ref Editor editor)
    {
        if (settings != null)
        {
            foldout = EditorGUILayout.InspectorTitlebar(foldout, settings);
            using var check = new EditorGUI.ChangeCheckScope();
            if (foldout)
            {
                CreateCachedEditor(settings, null, ref editor);
                editor.OnInspectorGUI();

                if (check.changed && onSettingsUpdated != null) onSettingsUpdated();
            }
        }
    }
    private void OnEnable()
    {
        planet = (Planet)target;
    }
}