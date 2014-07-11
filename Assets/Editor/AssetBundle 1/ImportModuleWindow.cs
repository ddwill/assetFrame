using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Text;

public class ImportModuleWindow : EditorWindow {
	public const string path = "";


	public string fbxPath = "";
	public string saveFileName = "";
	public string inputName = null;



	[MenuItem("TOOL/Import Module")]
	public static void ImportModule()
	{
		ImportModuleWindow importWindow = EditorWindow.GetWindow<ImportModuleWindow>(false, "ImportWindow", true);
		importWindow.init();
	}

	public void init()
	{

	}
	
	void OnGUI()
	{
		GUILayout.BeginVertical ();

		GUILayout.BeginHorizontal ();
		if(GUILayout.Button("SelectPath",GUILayout.Width(80)))
		{
			fbxPath = EditorUtility.OpenFilePanel("Select Fbx ", "", "fbx");
			if(fbxPath!=null)
				inputName = null;
		}
		GUILayout.Label("FbxPath: ", GUILayout.Width(60f));
		GUILayout.Label(fbxPath, GUILayout.Width(260f));
		GUILayout.EndHorizontal ();


		GUILayout.BeginHorizontal ();
		GUILayout.Label("name: ", GUILayout.Width(40f));
		string fileName = null;
		if (!fbxPath.Equals (""))
		{
			fileName =  Path.GetFileNameWithoutExtension(this.fbxPath);
			saveFileName = fileName;
		}


		if (!fbxPath.Equals (""))
		{
			saveFileName = fileName;
			if(inputName==null)
			{
				inputName = fileName;
			}
		}else
		{
			inputName = "";
		}

		inputName = GUILayout.TextField (inputName, GUILayout.Width (100f));
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();
		GUILayout.Label("SavePath", GUILayout.Width(60f));
		bool exist = File.Exists (GetMeshPath ());
		GUILayout.Label(GetMeshPath(), GUILayout.Width(660f));
		GUILayout.EndHorizontal ();

		if (exist) {
			GUI.color = Color.red;
			GUILayout.Label("Exist", GUILayout.Width(60f));
			if(GUILayout.Button("Import",GUILayout.Width(50)))
			{

				File.Delete(GetMeshPath());
				importMesh(fbxPath,inputName);

			}

			GUI.color = Color.white;
		} else 
		{
			if(GUILayout.Button("Import",GUILayout.Width(50)))
			{
				importMesh(fbxPath,inputName);
			}
		}
		ShowModelAnmationEditor ();
		GUILayout.EndVertical ();
	}

	/// <summary>
	/// 显示模型动作导入界面。
	/// </summary>
	void ShowModelAnmationEditor()
	{



	}




	/// <summary>
	/// 导入模型，如果路径已存在
	/// </summary>
	/// <param name="sourcePath">Source path.</param>
	/// <param name="fileName">File name.</param>
	public void importMesh(string sourcePath,string fileName)
	{
		string desPath = GetMeshPath ();

		File.Move (sourcePath, desPath);
		AssetDatabase.Refresh ();


		Debug.Log (desPath);
	}

	/// <summary>
	/// 切割动画
	/// </summary>
	void SplitAnimation()
	{
		
	}



	private string GetMeshPath()
	{

		return ConfigPath.MESH_STORE_GLOBAL_PATH+Path.GetFileName(this.fbxPath);
	}

	/// <summary>
	/// 获得模型的数据：
	/// 面数
	/// 动作
	/// uv
	/// 等等
	/// </summary>
	private void AnalysisMesh()
	{

	}

	/// <summary>
	/// 保存成预设
	/// </summary>
	private void SaveToPrefab()
	{

	}

	/// <summary>
	/// 添加模型数据
	/// </summary>
	private void RegesiterModelData()
	{

	}
}


