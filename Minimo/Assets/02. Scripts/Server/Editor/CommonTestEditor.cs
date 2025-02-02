using UnityEditor;

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