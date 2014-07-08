using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Text;

public class ScenceSaveWindow : EditorWindow {
	/// <summary>
	/// 滚动条位置（用于GUI）
	/// </summary>
	Vector2[] mScroll = null;

	/// <summary>
	/// 场景数量
	/// </summary>
	private int scencesNum;

	private List<EditorScenceData> scenceDataList = new List<EditorScenceData>(); 

	/// <summary>
	/// 打开编译目录设置窗口
	/// </summary>
	[MenuItem("TOOL/Load and Save Scence GameObject")]
	public static void SelectAssetBunldes()
	{
		//打开窗口
		ScenceSaveWindow assetSelectWindow = EditorWindow.GetWindow<ScenceSaveWindow>(false, "ScenceSave", true);
		assetSelectWindow.init();
	}

	public void init()
	{
		string scencePath = EditorApplication.currentScene;
		EditorApplication.SaveScene(scencePath);

		scencesNum = UnityEditor.EditorBuildSettings.scenes.Length;
		this.mScroll = new Vector2[scencesNum];

		foreach(EditorBuildSettingsScene S in UnityEditor.EditorBuildSettings.scenes)
		{
			EditorScenceData data = new EditorScenceData(S);
			this.scenceDataList.Add(data);
		}
	}

	void OnGUI()
	{
		GUILayout.BeginVertical ();
		mScroll[scencesNum-1] = GUILayout.BeginScrollView(mScroll[scencesNum-1]);

		foreach (EditorScenceData scenceData in this.scenceDataList) 
		{
			GUILayout.BeginHorizontal ();
			string buttonName = "Child";
			if(scenceData.isShowChild) buttonName = "Close";
			if(GUILayout.Button(buttonName,GUILayout.Width(50)))
			{
				if(scenceData.isShowChild) scenceData.closeChild();
				else scenceData.showChild();
			}
			GUILayout.Label("Scence: ", GUILayout.Width(60f));
			GUILayout.Label(scenceData.path, GUILayout.Width(200f));

			Color lastColor = GUI.color;
			GUI.color = Color.red;
			//scenceData.save = GUILayout.Toggle(scenceData.save, "needSave", GUILayout.Width(80f));
			if(GUILayout.Button("Save",GUILayout.Width(50)))
			{
				SaveOptionWindow saveOptionWindow = EditorWindow.GetWindow<SaveOptionWindow>(false, "ScenceSaveOption", true);
				saveOptionWindow.init (scenceData);
			}
			GUI.color = lastColor;

			GUILayout.EndHorizontal();

			if(scenceData.isShowChild)
			{
				foreach(EditorScenceItemData data in scenceData.footItemList)
				{
					ShowScenceItemData(data);
				}
			}
			GUILayout.Space(20);
		}

		GUILayout.EndScrollView();
		GUILayout.EndVertical ();
	}

	/// <summary>
	/// 在OnGUI中显示视图中的元素
	/// </summary>
	/// <param name="data">元素数据</param>
	private void ShowScenceItemData(EditorScenceItemData data)
	{
		GUILayout.BeginHorizontal ();
		GUILayout.Label(" ",GUILayout.Width(30*data.deep));
		string childButtonName = "Child";
		if(data.isShowChild) childButtonName = "Close";
		if(GUILayout.Button(childButtonName,GUILayout.Width(50)))
		{
			if(data.isShowChild) CloseItem(data);
			else {
				OpenItem(data);
			}
		}

		GUILayout.Label("Name: ", GUILayout.Width(60f));
		GUILayout.Label(data.Name, GUILayout.Width(200f));
		data.isNeedSave = GUILayout.Toggle(data.isNeedSave, "needSave", GUILayout.Width(80f));
		if (data.isNeedSave) 
		{
			GUILayout.Label("Path: ", GUILayout.Width(35f));
			data.Path = GUILayout.TextField (data.Path, GUILayout.Width (100f));
		}

		//设置子物体的保存路劲
		string setChildResourcePathButton = "ChildPath";
		if(data.isSetChildResourcePath) setChildResourcePathButton = "DeleteChildPath";
		if(GUILayout.Button(setChildResourcePathButton,GUILayout.Width(80)))
		{

			if(!data.isSetChildResourcePath) 
			{
				data.isSetChildResourcePath = true;
			}else{
				data.isSetChildResourcePath = false;
			}
		}

		if (data.isSetChildResourcePath) 
		{
			GUILayout.Label("ChildPath: ", GUILayout.Width(70f));
			data.ChildResourcePath = GUILayout.TextField (data.ChildResourcePath, GUILayout.Width (100f));
			data.AutoSaveChild =  GUILayout.Toggle(data.AutoSaveChild,"AutoSaveChild",GUILayout.Width (100f));
		}

		GUILayout.EndHorizontal();


		if (data.isShowChild)
		{
			foreach(EditorScenceItemData childData in data.childItemDatas)
			{
				ShowScenceItemData(childData);
			}
		}
		//EditorGUILayout.EndFadeGroup ();
	}

	private void CloseItem(EditorScenceItemData data)
	{
		data.closeChildObject();

	}

	private void OpenItem(EditorScenceItemData data)
	{
		data.isShowChild = true;
		//data.initChildGameObjectList ();
	}
}



