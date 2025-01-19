using UnityEditor;
using UnityEngine;

public class TimeViewer : EditorWindow
{
    [MenuItem("Tools/Time Viewer")]
    public static void ShowWindow()
    {
        GetWindow<TimeViewer>("Time Viewer");
    }

    private void Update()
    {
        Repaint();
    }

    private void OnGUI()
    {
        if (App.Services == null)
        {
            EditorGUILayout.HelpBox("Service container is not initialized.", MessageType.Error);
            return;
        }
        
        GUILayout.Label("Time(Server): " + App.GetManager<TimeManager>().Time);
        GUILayout.Label("Time(Local): " + App.GetManager<TimeManager>().LocalTime);
    }
}
