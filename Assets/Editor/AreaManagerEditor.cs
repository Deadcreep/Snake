//using Edibles;
//using Edibles.Utilites;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine;


//#if UNITY_EDITOR

//[CustomEditor(typeof(AreaManager))]
//[CanEditMultipleObjects]
//public class AreaManagerEditor : Editor
//{
//	private AreaManager manager;
//	private List<Row> rows = new List<Row>();
//	private int testValue;

//	private void OnEnable()
//	{
//		manager = target as AreaManager;
//	}

//	public override void OnInspectorGUI()
//	{
//		serializedObject.Update();
//		DrawDefaultInspector();
//		serializedObject.ApplyModifiedProperties();
//		EditorGUILayout.BeginHorizontal();
//		testValue = EditorGUILayout.IntField(testValue);
//		EditorGUILayout.LabelField(System.Convert.ToString(testValue, 2));
//		EditorGUILayout.EndHorizontal();
//		EditorGUILayout.BeginHorizontal();
//		if (GUILayout.Button("Generate"))
//			rows.Add(manager.GenerateRow());
//		if (GUILayout.Button("Clear"))
//		{
//			rows.Clear();
//			manager.Clear();
//		}
//		EditorGUILayout.EndHorizontal();

//		var scale = 30;
//		Rect areaRect = GUILayoutUtility.GetRect(3 * scale, rows.Count * scale);
//		for (int i = 0, index = rows.Count - 1; index >= 0; index--, i++)
//		{
//			var cells = rows[index].Cells;
//			for (int j = 0; j < cells.Count; j++)
//			{
//				Rect cell = new Rect
//				{
//					width = scale,
//					height = scale,
//					x = areaRect.xMin + j * scale,
//					y = areaRect.yMin + i * scale
//				};
//				GUIStyle style = new GUIStyle();
//				Color color = cells[j].IsNegative() ? Color.red : Color.green;
//				style.normal.background = MakeTex(scale, scale, color);
//				EditorGUI.LabelField(cell, GetLabel(cells[j]), style);
//			}
//		}
//		//GUI.EndScrollView();
//	}

//	private Texture2D MakeTex(int width, int height, Color col)
//	{
//		Color[] pix = new Color[width * height];
//		for (int i = 0; i < pix.Length; ++i)
//		{
//			pix[i] = col;
//		}
//		Texture2D result = new Texture2D(width, height);
//		result.SetPixels(pix);
//		result.Apply();
//		return result;
//	}

//	private string GetLabel(CellType type)
//	{
//		switch (type)
//		{
//			case CellType.Empty:
//				return "E";

//			case CellType.Bomb:
//				return "B";

//			case CellType.Crystal:
//				return "CR";

//			case CellType.Correct:
//				return "CO";

//			case CellType.Incorrect:
//				return "IN";

//			default:
//				return "#";
//		}
//	}
//}

//#endif