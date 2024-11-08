using UnityEditor;
using UnityEngine;

// reference: https://imgur.com/a/a0DNhcj
public static class Bool2DArrayEditorUtility
{
    private static int ELEMENT_SIZE = 20;

    public static void BoolGrid(
        SerializedProperty boolList,
        SerializedProperty columnCount,
        SerializedProperty rowCount)
    {
        EditorGUILayout.LabelField("OccupyArray", EditorStyles.boldLabel);
        ++EditorGUI.indentLevel;
        EditorGUILayout.PropertyField(columnCount);
        EditorGUILayout.PropertyField(rowCount);

        if (columnCount.intValue <= 0)
        {
            columnCount.intValue = 1;
        }
        else if (rowCount.intValue <= 0)
        {
            rowCount.intValue = 1;
        }

        boolList.arraySize = columnCount.intValue * rowCount.intValue;

        var buttonText = new string[boolList.arraySize];

        for (var i = 0; i < buttonText.Length; i++)
        {
            buttonText[i] = boolList.GetArrayElementAtIndex(i).boolValue ? "бс" : "";
        }

        GUILayout.Space(3);

        GUILayout.BeginHorizontal();
        GUILayout.Space(15);
        var selectedIndex = GUILayout.SelectionGrid(
            -1,
            buttonText,
            columnCount.intValue,
            new GUILayoutOption[] {
                GUILayout.Width(ELEMENT_SIZE * columnCount.intValue),
                GUILayout.Height(ELEMENT_SIZE * rowCount.intValue),
            });
        GUILayout.EndHorizontal();

        GUILayout.Space(3);

        GUILayout.BeginHorizontal();
        GUILayout.Space(15);
        if (GUILayout.Button("Full", new GUILayoutOption[] { GUILayout.Width(40) }))
        {
            for (var i = 0; i < boolList.arraySize; i++)
            {
                boolList.GetArrayElementAtIndex(i).boolValue = true;
            }
        }

        if (GUILayout.Button("Clear", new GUILayoutOption[] { GUILayout.Width(40) }))
        {
            for (var i = 0; i < boolList.arraySize; i++)
            {
                boolList.GetArrayElementAtIndex(i).boolValue = false;
            }
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(15);

        if (selectedIndex != -1)
        {
            boolList.GetArrayElementAtIndex(selectedIndex).boolValue = !boolList.GetArrayElementAtIndex(selectedIndex).boolValue;
        }

        --EditorGUI.indentLevel;
    }
}
