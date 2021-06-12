#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NoiseValuesUpdater))]
[RequireComponent(typeof(TerrainGenerator))]
public class CustomEditorComponents: Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if (GUILayout.Button("Update Values"))
		{
			GameObject.Find("Controller").GetComponent<TerrainGenerator>().GenerateStartingTerrain();
		}
	}
}
#endif