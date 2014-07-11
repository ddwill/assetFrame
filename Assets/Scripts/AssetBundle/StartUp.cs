using UnityEngine;
using UnityEditor;
using System.Collections;

[InitializeOnLoad]
public class StartUp : MonoBehaviour {
	static StartUp()
	{
		EditorApplication.update += Update;
	}

	static void Update ()
	{

	}

	/// <summary>
	/// 检查是否有模型更新
	/// </summary>
	public static void CheckNewModelExist()
	{
		if (false) 
		{
			string data = "";

			ModelData modelData = new ModelData();
			ModelManager.Instance.ImportModel(modelData);
		}
	}


}
