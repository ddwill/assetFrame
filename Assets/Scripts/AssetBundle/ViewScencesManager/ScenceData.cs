using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

/// <summary>
/// buildSeting中注册的场景的数据
/// </summary>
[System.Serializable]
public class EditorScenceData
{
	[XmlIgnore]
	private EditorBuildSettingsScene scence;
	
	public string path = "";
	
	[XmlAttribute]
	public bool isShowChild = false;
	
	public string ScenceName
	{
		get{
			string[] pathStrings =  path.Split ('/');
			string scenceName;
			scenceName = pathStrings [pathStrings.Length - 1];
			return scenceName.Split ('.')[0]; 
		}
	}

	public string ScencePath
	{
		get
		{
			return ConfigPath.SCENCE_SAVE_PATH+ScenceName+"/";
		}
	}
	
	[XmlArrayItem]
	public List<EditorScenceItemData> footItemList = new List<EditorScenceItemData> ();
	
	
	
	public EditorScenceData()
	{
		
	}
	
	public EditorScenceData(EditorBuildSettingsScene scence)
	{
		path = scence.path;
		EditorApplication.OpenScene(path);
		foreach (GameObject obj in Object.FindObjectsOfType(typeof(GameObject))) 
		{
			if (obj.transform.parent == null)
			{
				EditorScenceItemData esi = new EditorScenceItemData(this,null,obj,1);
				esi.isNeedSave = true;
				footItemList.Add(esi);
				esi.initChildGameObjectList();
			}
		}
	}
	
	public void showChild()
	{
		isShowChild = true;
	}
	
	public void closeChild()
	{
		isShowChild = false;
	}
	
	/// <summary>
	/// 保存场景到Resources
	/// </summary>
	public void saveScence (bool isReplace, string viewName)
	{

		if (isReplace) 
		{
			//删除之前的预设
			//EditorScenceData data = ScencesManager.GetScenceViewData (viewName);
			//if(data!=null)
			//{
				//string path = ConfigPath.SCENCE_SAVE_PATH+ScenceName+"/";
			FileTool.DeleteDirectoryAndChild(ScencePath);
			//}
			AssetDatabase.Refresh ();
		}


		foreach (EditorScenceItemData itemData in footItemList) 
		{
			separateSavedGameObject(itemData);
		}
		foreach (EditorScenceItemData itemData in footItemList) 
		{
			saveItemData(itemData);
		}
		
		AssetDatabase.Refresh ();
	}
	
	/// <summary>
	/// 保存gameObject到Resources用于递归
	/// </summary>
	/// <param name="itemData">Item data.</param>
	private void saveItemData(EditorScenceItemData itemData)
	{
		foreach (EditorScenceItemData childData in itemData.childItemDatas) 
		{
			saveItemData(childData);
		}
		
		
		
		if (itemData.isNeedSave&&itemData.parentPrefabPath==null) 
		{
			FileTool.CreateDirectory(itemData.Path);
			string resourcePath =  CheckPathExist(itemData.Path);
			itemData.Path = resourcePath;
			PrefabUtility.CreatePrefab("Assets"+"/"+resourcePath,itemData.go);
		}else if(itemData.parentPrefabPath !=null)
		{
			itemData.saveTransform();
		}
	}
	
	/// <summary>
	/// 递归
	/// 分离有预设连接的gameObject.
	/// 分离需要保存的gameobject
	/// </summary>
	/// <param name="itemData">Item data.</param>
	private void separateSavedGameObject(EditorScenceItemData itemData)
	{
		foreach (EditorScenceItemData childData in itemData.childItemDatas) 
		{
			separateSavedGameObject(childData);
		}
		
		PrefabType prefabaType = PrefabUtility.GetPrefabType (itemData.go);
		
		Object obj = PrefabUtility.GetPrefabParent (itemData.go);
		if (obj != null&&(prefabaType== PrefabType.Prefab|| prefabaType== PrefabType.PrefabInstance)) 
		{
			string path =  AssetDatabase.GetAssetPath(obj);
			itemData.parentPrefabPath =  path;
		}
		
		
		if (itemData.go.transform.parent != null&&itemData.isNeedSave) itemData.go.transform.parent = null;
		if (itemData.parentPrefabPath != null) 
		{
			itemData.go.transform.parent = null;
		}
	}

	private string CheckPathExist(string assetPath)
	{
		//File.Exists(
		string path = Application.dataPath +"/"+ assetPath;

		if (!File.Exists (path))
		{
			return assetPath;
		}


		string[] suffixPaths = assetPath.Split ('.');
		if (suffixPaths.Length != 2)
			Trace.LogError ("CheckPathExist..suffixPaths nums !=2");

		string[] paths =  suffixPaths[0].Split ('$');
		if (paths.Length>1) 
		{	
			assetPath = paths[0]+"$"+(int.Parse(paths[1])+1);
		}else
		{
			assetPath = paths[0]+"$"+1;
		}
		assetPath += "." + suffixPaths [1];
		return CheckPathExist(assetPath);
	}
}

/// <summary>
/// 场景Gameobject的数据
/// </summary>
[System.Serializable]
public class EditorScenceItemData
{	
	
	[XmlIgnore]
	public EditorScenceData scenceData;
	
	[XmlIgnore]
	public EditorScenceItemData parentData;
	
	//[XmlIgnore]
	//public const string defaultPath = "Resources/ScenceSave/";
	
	/// <summary>
	/// 场景名称
	/// </summary>
	[XmlAttribute]
	public string scenceName;
	[XmlAttribute]
	public string scencePath;
	
	[XmlIgnore]
	public GameObject go;

	[XmlArray]
	public List<EditorScenceItemData> childItemDatas = new List<EditorScenceItemData>();
	
	/// <summary>
	/// 是否显示子物体对象
	/// </summary>
	[XmlAttribute]
	public bool isShowChild = false;
	
	/// <summary>
	/// 显示缩进深度
	/// </summary>
	[XmlAttribute]
	public int deep;
	
	/// <summary>
	/// 保存路径
	/// </summary>
	private string path = null;
	
	public string Path
	{
		get{
			if(path!=null) return path;
			
			//父物体设置了"ChildPath"子物体保存路径 
			if(parentData!=null&&parentData.isSetChildResourcePath)
			{
				return parentData.ChildResourcePath+"/"+Name+".prefab";
			}
			
			//默认保存路径
			return ConfigPath.SCENCE_SAVE_PATH+scenceName+"/"+getDataPath(this)+Name+".prefab";
		}
		set
		{
			path = value;
		}
	}

	/// <summary>
	/// 是否需要保存
	/// </summary>
	[XmlAttribute]
	public bool isNeedSave;
	
	/// <summary>
	/// 是否设置子物体的保存路径
	/// </summary>
	[XmlAttribute]
	public bool isSetChildResourcePath = false;
	
	/// <summary>
	/// 子物体的保存路径
	/// </summary>
	[XmlAttribute]
	private string childResourcePath =null;
	
	public string ChildResourcePath
	{
		get
		{
			if(childResourcePath==null)
			{
				string[] pathStrings =  scenceData.path.Split ('/');
				string scenceFullName;
				scenceFullName = pathStrings [pathStrings.Length - 1];
				this.scenceName =  scenceFullName.Split ('.')[0];
				string  childPath = ConfigPath.SCENCE_SAVE_PATH+scenceName;
				return childPath;
			}
			return childResourcePath;
		}
		set
		{
			childResourcePath = value;
		}
	}


	private bool autoSaveChild =false;
	
	public bool AutoSaveChild
	{
		get
		{
			return autoSaveChild;
		}
		
		set
		{
			if(value&&!autoSaveChild)
			{
				foreach(EditorScenceItemData data in this.childItemDatas) data.isNeedSave = true;
				
			}
			
			autoSaveChild = value;
			
		}
	}
	
	/// <summary>
	/// 对应GameObject的名字
	/// </summary>
	/// <value>对应GameObject的名字</value>
	[XmlAttribute]
	public string Name
	{
		get{
			return name;
		}
		set{
			this.name = value;
		}
	}


	private string name;
	
	
	/// <summary>
	/// 预设引用路径
	/// </summary>
	public string parentPrefabPath =null;
	public string goPosition = null;
	public string goRotation = null;
	public string goScale = null;

	/// <summary>
	/// assetBundle路径
	/// </summary>
	public string bundlePath;

	public EditorScenceItemData()
	{
	}
	
	/// <summary>
	/// 初始化
	/// </summary>
	/// <param name="scenceData">场景数据</param>
	/// <param name="parentData">父物体的数据</param>
	/// <param name="go">GameObject</param>
	/// <param name="deep">深度</param>
	public EditorScenceItemData(EditorScenceData scenceData,EditorScenceItemData parentData,GameObject go,int deep)
	{
		this.scenceData = scenceData;
		this.go = go;
		this.deep = deep;
		string[] pathStrings =  scenceData.path.Split ('/');
		string scenceFullName;
		scenceFullName = pathStrings [pathStrings.Length - 1];
		this.scenceName =  scenceFullName.Split ('.')[0];
		this.parentData = parentData;
		this.scencePath = scenceData.path;
		Name = go.name;
	}
	
	/// <summary>
	/// 初始化子物体
	/// </summary>
	public void initChildGameObjectList()
	{
		if (childItemDatas.Count != 0) childItemDatas.Clear ();
		
		Transform[] transforms =  this.go.transform.GetComponentsInChildren<Transform> ();
		
		foreach (Transform tr in transforms) 
		{
			if(tr.gameObject==go) continue;
			
			childItemDatas.Add(new EditorScenceItemData(scenceData,this,tr.gameObject,deep+1));
		}
	}
	
	public void closeChildObject()
	{
		this.isShowChild = false;
	}
	
	private string getDataPath(EditorScenceItemData data)
	{
		return data.parentData!=null?(getDataPath (data.parentData)+data.Name + "/"):"";
	}
	
	public void saveTransform()
	{
		Debug.Log ("saveTransform....");
		this.goPosition = this.go.transform.localPosition.ToString();
		this.goRotation = this.go.transform.localRotation.ToString ();
		this.goScale = this.go.transform.localScale.ToString ();
		
		Debug.Log (this.goPosition);
	}
}