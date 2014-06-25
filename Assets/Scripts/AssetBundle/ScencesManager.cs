using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Text;

public class ScencesManager{
	public const string c_scenceDataXMLName = ConfigPath.RESOURCES_CONFIG_SCENCEDATA+"scenceDatas.xml";


	private static List<ScenceDataItemView> scenceDataItemList;



	/// <summary>
	/// 加载场景视图数据(Resourcs)
	/// </summary>
	/// <param name="name">Name.</param>
	public static void LoadScenceViewData(string name)
	{

		ScenceDataItemView item = GetScenceView (name);
		if (item == null) 
		{
			Trace.LogError("LoadScenceViewData..."+name+"..is null");
		}

		EditorScenceData esd = XMLTool.LoadObject<EditorScenceData> (item.path);

		foreach (EditorScenceItemData esid in esd.footItemList) 
		{

			LoadEditorScenceItemData(esid);
		}
	}

	/// <summary>
	/// 获得场景视图数据
	/// </summary>
	/// <param name="name">场景视图名字</param>
	public static EditorScenceData GetScenceViewData(string name)
	{
		ScenceDataItemView item = GetScenceView (name);
		if (item == null) 
		{
			//Trace.LogError("LoadScenceViewData..."+name+"..is null");
			return null;
		}
		
		EditorScenceData esd = XMLTool.LoadObject<EditorScenceData> (item.path);
		
		return esd;
	}

	public static void AddScenceDataItemView(ScenceDataItemView sdiv)
	{
		ScenceDataItemView oldSdiv = GetScenceView (sdiv.name);
		if(oldSdiv!=null)
		{
			sdiv.path = oldSdiv.path;
		}else
		{
			scenceDataItemList.Add (sdiv);
		}

		SaveScenceViewData ();
	}

	public static void SaveScenceViewData ()
	{
		/**FileTool.CreateDirectory (c_scenceDataXMLName);
		using (FileStream fs = new FileStream(c_scenceDataXMLName, FileMode.Create, FileAccess.Write))
		{
			//XmlSerializer xmlser = new XmlSerializer(typeof(EditorScenceData));
			//xmlser.Serialize(fs,this.scenceData);

		}**/

		XMLTool.SaveListInRoot (scenceDataItemList, c_scenceDataXMLName, "root");
	}

	/// <summary>
	/// Gets the scence view.
	/// </summary>
	/// <returns>The scence view.</returns>
	/// <param name="name">Name.</param>
	private static ScenceDataItemView GetScenceView(string name)
	{
		if (scenceDataItemList == null) 
		{
			LoadAllScenceData();
		}

		foreach (ScenceDataItemView  item in scenceDataItemList) 
		{
			if(item.name.Equals(name))
			{
				return item;
			}
		}
		return null;
	}

	private static void  LoadAllScenceData()
	{
		scenceDataItemList = XMLTool.ReadListInRoot<ScenceDataItemView>(c_scenceDataXMLName,"root");
	}


	private static EditorScenceItemData LoadEditorScenceItemData(EditorScenceItemData esid)
	{
		EditorScenceItemData itemData = esid;

		Debug.Log(esid.Path);

		//解析设置保存的GameObject
		if (esid.isNeedSave) {
			string path = esid.Path.TrimStart ("/Resourcs".ToCharArray ());

			path = path.Split('.')[0];
			GameObject goPrefab = (GameObject)Resources.Load (path);
			if(goPrefab==null)
			{
				Trace.LogError("EditorScenceItemData....path resources is null.."+path);
				return null;
			}
			esid.go = (GameObject)Transform.Instantiate (goPrefab);
			esid.go.name = esid.Name;
			if(esid.parentData!=null)
				esid.go.transform.parent =  esid.parentData.go.transform;
		}


		foreach (EditorScenceItemData  childData in esid.childItemDatas)
		{
			childData.parentData = esid;
			LoadEditorScenceItemData(childData);
		}
		return itemData;

	}

}

/// <summary>
/// 编辑场景的数据
/// </summary>
[System.Serializable]
public class ScenceDataItemView
{
	[XmlAttribute]
	public string name;

	[XmlAttribute]
	public string path;
}
