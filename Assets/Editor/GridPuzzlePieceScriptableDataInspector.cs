using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GridPuzzlePieceScriptableData))]
public class GridPuzzlePieceScriptableDataInspector : Editor
{
    private GridPuzzlePieceScriptableData data;
    private SerializedProperty sprite;
    private SerializedProperty occupyList;
    private SerializedProperty columnCount;
    private SerializedProperty rowCount;

    public void OnEnable()
    {
        data = target as GridPuzzlePieceScriptableData;
        sprite = this.serializedObject.FindProperty("Sprite");
        occupyList = this.serializedObject.FindProperty("OccupyList");
        columnCount = this.serializedObject.FindProperty("ColumnCount");
        rowCount = this.serializedObject.FindProperty("RowCount");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        Bool2DArrayEditorUtility.BoolGrid(occupyList, columnCount, rowCount);

        EditorGUILayout.PropertyField(sprite);

        GUILayout.Space(3);

        var texture = AssetPreview.GetAssetPreview(data.Sprite);
        if (texture != null)
        {
            var width = data.ColumnCount * 40f;
            var height = data.RowCount * 40f;
            GUILayout.Label("", GUILayout.Height(height), GUILayout.Width(width));
            GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
