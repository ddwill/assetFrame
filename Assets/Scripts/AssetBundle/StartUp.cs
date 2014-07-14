using UnityEngine;
using UnityEditor;
using System.Collections;

[InitializeOnLoad]
public class StartUp : MonoBehaviour {
	public static bool bCheckNewModelFlag = true;

	public static float checkModelRate = 5;

	public static float lastTime =0;

	public static float s_MinDelay = 0.016666f;

	static StartUp()
	{
		EditorApplication.update += Update;

	}

	static void Update ()
	{
		//Time.time;
		if((Time.time-lastTime)/s_MinDelay>1)
		{
			lastTime = Time.time;
			CheckNewModelExist();
		}
	}

	/// <summary>
	/// 检查是否有模型更新
	/// </summary>
	public static void CheckNewModelExist()
	{
		if (false) 
		{

			ModelManager.Instance.ImportModel();
		}
	}


}
