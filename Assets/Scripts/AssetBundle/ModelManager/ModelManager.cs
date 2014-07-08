using UnityEngine;
using System.Collections;
using UnityEditor;

public class ModelManager {
	private static ModelManager _instance;
	public static ModelManager Instance
	{
		get {
			if(_instance==null) _instance = new ModelManager();
			return _instance; }
	}

	/// <summary>
	/// Editor 中获得模型路径
	/// </summary>
	public void ImportModel()
	{
		//string path = EditorUtility.OpenFilePanel("Select XML File", "", "fbx");
		//EditorUtility.
	}


	public void SaveModelPrefab()
	{

	}

	public void LoadModel()
	{

	}


	public void SeperateModeleSources()
	{

	}
}
