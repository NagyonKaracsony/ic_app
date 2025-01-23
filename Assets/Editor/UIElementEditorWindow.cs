using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class UIElementEditorWindow : EditorWindow
{
    [SerializeField]
    private StyleSheet m_StyleSheet = default;

    [MenuItem("Window/UI Toolkit/UIElementEditorWindow")]
    public static void ShowExample()
    {
        UIElementEditorWindow wnd = GetWindow<UIElementEditorWindow>();
        wnd.titleContent = new GUIContent("UIElementEditorWindow");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // VisualElements objects can contain other VisualElement following a tree hierarchy.
        VisualElement label = new Label("Hello World! From C#");
        VisualElement label2 = new Label("Hello World! From C#");
        root.Add(label);
        root.Add(label2);

        // Add label
        VisualElement labelWithStyle = new Label("Hello World! With Style");
        labelWithStyle.AddToClassList("custom-label");
        labelWithStyle.styleSheets.Add(m_StyleSheet);
        root.Add(labelWithStyle);
    }
}
