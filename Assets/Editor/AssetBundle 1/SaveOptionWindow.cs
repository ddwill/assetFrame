using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

public class SaveOptionWindow : EditorWindow {



	public 	EditorScenceData scenceData;

	public string path ="";

	public string name ="";

	public string XmlPath
	{
		get
		{
			return path+name+".xml";
		}
	}

	public void init(EditorScenceData scenceData)
	{
		this.scenceData = scenceData;
		path = ConfigPath.RESOURCES_CONFIG_SCENCEDATA+scenceData.ScenceName+"/";
		name = scenceData.ScenceName;
	}
	
	void OnGUI()
	{
		GUILayout.BeginVertical ();
		GUILayout.Label("Scence: "+scenceData.ScenceName);

		GUILayout.BeginHorizontal ();
		GUILayout.Space (30);
		GUILayout.Label("Export Scence Xml. ",GUILayout.Width (200f));
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();
		GUILayout.Space (30);
		GUILayout.Label("Path:",GUILayout.Width (40f));
		GUILayout.TextField (path, GUILayout.Width (200f));
		GUILayout.Space (10);
		GUILayout.Label("Name:",GUILayout.Width (40f));
		name = GUILayout.TextField (name, GUILayout.Width (100f));
		GUILayout.Space (10);
		

		string exportButtonName = "Export";
		bool isReplace = false;
		if (File.Exists (XmlPath)) 
		{

			Color lastColor = GUI.color;
			GUI.color = Color.yellow;
			GUILayout.Label("Exist",GUILayout.Width (40f));
			GUI.color = lastColor;

			exportButtonName = "Replace";
			isReplace = true;

		}



		/**if (GUILayout.Button (exportButtonName,GUILayout.Width (65f))) 
		{
			//string path = EditorUtility.SaveFilePanel("Save XML File", "Assets/Resourcs/config", "", "xml");
			exportXML();
		}**/
		GUILayout.EndHorizontal ();
		GUILayout.EndVertical ();

		GUILayout.BeginHorizontal (GUILayout.MinHeight(25f));
		GUILayout.Space (200);
		GUI.color = Color.red;
		if (GUILayout.Button ("SaveResource",GUILayout.Width (150f))) 
		{
			scenceData.saveScence(isReplace,name);
			exportXML();
		}
		GUI.color = Color.white;
		GUILayout.EndHorizontal();
	}

	/// <summary>
	/// 导出xml
	/// </summary>
	private void exportXML()
	{

		XMLTool.SaveObject (XmlPath, scenceData);
		ScenceDataItemView sdiv = new ScenceDataItemView ();
		sdiv.name = name;
		sdiv.path = XmlPath;
		ScencesManager.AddScenceDataItemView (sdiv);
		AssetDatabase.Refresh ();
	}


}
