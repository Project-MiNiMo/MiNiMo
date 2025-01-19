using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

public class ProduceTest : MonoBehaviour
{
    public Dictionary<string, ProduceData> ProduceDatas;
    private TitleData _titleData;

    private void Start()
    {
        ProduceDatas = App.GetData<TitleData>().Produce;
    }
    
    private void OnValidate()
    {
        if (Application.isPlaying)
        {
            _titleData.SetProduceData(ProduceDatas);
        }
    }
}

[CustomEditor(typeof(ProduceTest))]
public class ProduceTestEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var produceTest = (ProduceTest)target;

        var produceDatas = produceTest.ProduceDatas;

        if (produceDatas != null)
        {
            foreach (var kvp in produceDatas)
            {
                string key = kvp.Key;
                
                var produceOptions = kvp.Value.ProduceOptions;

                if (produceOptions != null && produceOptions.Length > 0)
                {
                    GUIStyle labelStyle = new GUIStyle(EditorStyles.boldLabel)
                    {
                        fontSize = 20, 
                    };

                    GUILayout.Label(key, labelStyle);

                    // 각 ProduceOption 표시
                    for (int i = 0; i < produceOptions.Length; i++)
                    {
                        var option = produceOptions[i];

                        GUIStyle smallLabelStyle = new GUIStyle(EditorStyles.label)
                        {
                            fontSize = 14,
                            padding = new RectOffset(20, 0, 0, 0) 
                        };
                        GUILayout.Label($"- {option.Results[0].Code}", smallLabelStyle);
     
                        EditorGUILayout.BeginVertical("box");
                        option.Results[0].Amount = EditorGUILayout.IntField("Amount", option.Results[0].Amount);
                        option.Time = EditorGUILayout.IntField("Time", option.Time);
                        EditorGUILayout.EndVertical();
                    }
                }
                else
                {
                    EditorGUILayout.LabelField($"Key: {key} has no options.");
                }
            }
            
            if (GUI.changed)
            {
                EditorUtility.SetDirty(produceTest);
            }
        }
        else
        {
            EditorGUILayout.LabelField("Status Dictionary is null or empty.");
        }
    }
}
