using System.Collections.Generic;

using UnityEngine;
using UnityEditor;


public class CommonTest : MonoBehaviour
{
    public Dictionary<string, int> CommonDatas;
    private TitleData _titleData;
    
    private void Start()
    {
        CommonDatas = App.GetData<TitleData>().Common;
    }

    private void OnValidate()
    {
        if (Application.isPlaying)
        {
            //_titleData.SetCommonData(CommonDatas);
        }
    }
}

[CustomEditor(typeof(CommonTest))]
public class CommonTestEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var commonTest = (CommonTest)target;

        var commonDatas = commonTest.CommonDatas;

        if (commonDatas != null)
        {
            foreach (var kvp in commonDatas)
            {
                string key = kvp.Key;
                int value = kvp.Value;

                int newValue = EditorGUILayout.IntField(key, value);

                if (newValue != value)
                {
                    value = newValue;

                    EditorUtility.SetDirty(commonTest);
                }
            }
        }
        else
        {
            EditorGUILayout.LabelField("Status Dictionary is null or empty.");
        }
    }
}

