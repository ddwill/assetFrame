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


		if(GUILayout.Button("Import",GUILayout.Width(50)))
		{
			importMesh();
		}
		GUILayout.EndHorizontal ();
		GUILayout.EndVertical ();
	}

	public void importMesh()
	{
		Debug.Log (Application.dataPath);

		ConfigPath.MODEL_STORE_GLOBAL_PATH+
	}
}

public class ImportModuleData
{
	public string resourcePath;
	public string fileName;
}
