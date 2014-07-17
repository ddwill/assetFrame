using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEditor;

public class ModelManager {
	private static ModelManager _instance;
	public static ModelManager Instance
	{
		get {
			if(_instance==null) _instance = new ModelManager();
			return _instance; }
	}
	
	public enum ManagerState
	{
		Free,
		Run
	}
	
	public enum ModelImportState
	{
		none,
		ImportModel,
		AddAnimation,
		CreatePreFab,
		BuildBundle,
		SendMessageToServer
	}

	#region Property
	public List<ModelData> modelDatas = new List<ModelData> ();
		
	#endregion

	public ManagerState eState = ManagerState.Free;

	public void ChangeDataState(ModelData data)
	{
		switch(data.improtState)
		{
		case ModelImportState.none:
			break;
		case ModelImportState.AddAnimation:
			break;
		case ModelImportState.BuildBundle:
			break;
		case ModelImportState.CreatePreFab:
			break;
		case ModelImportState.ImportModel:
			break;
		}
	}

	/// <summary>
	/// Editor 中获得模型路径或是从文件中读取
	/// </summary>
	public void ImportModel()
	{
		this.modelDatas =  ReadModelsInfoList ();
		foreach(ModelData data in modelDatas)
		{
			data.improtState = ModelImportState.ImportModel;

			#region Copy fbx To Project
			///如果文件存在怎么办？
			data.inFbxPath = ConfigPath.MODEL_STORE_GLOBAL_PATH + data.userId + "/" + data.id+Path.GetExtension(data.outFbxPath);

			FileTool.CreateDirectoryRealPath (data.inFbxPath);
			File.Copy (data.outFbxPath, data.inFbxPath,true);
			AssetDatabase.Refresh ();
			#endregion

			CreatePrefab(data); 
			/**AddAnimation (data);
			SeperateModeleSources (data);
			BuidlAssetBundle (data);
			SendToServerWebFloder (data);**/
		}
	}




	/// <summary>
	/// 模型信息设置
	/// 增加碰撞体,
	/// 增加特效点
	/// 
	/// </summary>
	public void EditorModel(ModelData data)
	{

	}

	/// <summary>
	/// 分离模型和贴图
	/// 
	/// 
	/// </summary>
	public void SeperateModeleSources(ModelData data)
	{

	}

	/// <summary>
	/// 
	/// 生成预设
	/// </summary>
	public void CreatePrefab(ModelData data)
	{
		data.improtState = ModelImportState.CreatePreFab;
		data.prefabPath = ConfigPath.MODEL_STORE_PREFAB_PATH+data.userId+"/"+data.id+".prefab";
		FileTool.CreateDirectory (data.prefabPath);
		//AssetDatabase.LoadAssetAtPath(ConfigPath.tr data.inFbxPath
		Object go = AssetDatabase.LoadAssetAtPath (ConfigPath.TrimPath_Assets (data.inFbxPath), typeof(GameObject));
		//GameObject newGo =  (GameObject)PrefabUtility.InstantiatePrefab(go);
		GameObject prefabGO =  PrefabUtility.CreatePrefab (data.prefabPath,(GameObject)go);

		data.prefab = prefabGO;
		BuidlAssetBundle (data);
	}
	

	/// <summary>
	/// 打assetBundle包
	/// </summary>
	/// <param name="data">Data.</param>
	public void BuidlAssetBundle(ModelData data)
	{
		data.localBundlePath = ConfigPath.MODEL_STORE_BUNDEL_PATH +data.userId+"/"+data.id;//+"/"+".assetbundle";
		data.serverBundlePath = ConfigPath.BUNDLE_OUTPUT_PATH +data.userId+"/"+data.id;

		data.improtState = ModelImportState.BuildBundle;
		FileTool.CreateDirectory (data.localBundlePath + ConfigPath.BUNDLE_ANDROID_EXTENTION);
		if(BuildPipeline.BuildAssetBundle ((Object) data.prefab, null, data.localBundlePath+ConfigPath.BUNDLE_ANDROID_EXTENTION,
		                                   BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets,BuildTarget.Android))
		{
			Trace.Log("BuildAssetBundle Success.."+ (data.localBundlePath+ConfigPath.BUNDLE_ANDROID_EXTENTION));

		}else
		{
			Trace.Log("BuildAssetBundle Fail.."+ (data.localBundlePath+ConfigPath.BUNDLE_ANDROID_EXTENTION));
		}

		if(BuildPipeline.BuildAssetBundle ((Object)data.prefab, null, data.localBundlePath+ConfigPath.BUNDLE_WEB_EXTENTION,
		                                   BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets,BuildTarget.WebPlayer))
		{
			Trace.Log("BuildAssetBundle Success.."+ (data.localBundlePath+ConfigPath.BUNDLE_WEB_EXTENTION));

		}else
		{
			Trace.Log("BuildAssetBundle Fail.."+ (data.localBundlePath+ConfigPath.BUNDLE_WEB_EXTENTION));
		}

		if( BuildPipeline.BuildAssetBundle ((Object)data.prefab, null, data.localBundlePath+ConfigPath.BUNDLE_IPHONE_EXTENTION,
		         BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets,BuildTarget.iPhone))
		{
			Trace.Log("BuildAssetBundle Success.."+ (data.localBundlePath+ConfigPath.BUNDLE_IPHONE_EXTENTION));
		}else
		{
			Trace.Log("BuildAssetBundle Fail.."+ (data.localBundlePath+ConfigPath.BUNDLE_IPHONE_EXTENTION));
		}

		AssetDatabase.Refresh ();
		SendToServerWebFloder (data);
	}

	/// <summary>
	/// 发送assetbundle 到服务器, http请求web服务器
	/// </summary>
	/// <param name="data">Data.</param>
	public void SendToServerWebFloder(ModelData data)
	{

		FileTool.CreateDirectoryRealPath (data.serverBundlePath+ConfigPath.BUNDLE_ANDROID_EXTENTION);
		File.Copy (data.localBundlePath+ConfigPath.BUNDLE_ANDROID_EXTENTION, data.serverBundlePath+ConfigPath.BUNDLE_ANDROID_EXTENTION,true);
		File.Copy (data.localBundlePath+ConfigPath.BUNDLE_IPHONE_EXTENTION, data.serverBundlePath+ConfigPath.BUNDLE_IPHONE_EXTENTION,true);
		File.Copy (data.localBundlePath+ConfigPath.BUNDLE_WEB_EXTENTION, data.serverBundlePath+ConfigPath.BUNDLE_WEB_EXTENTION,true);

        
		//http请求服务器
        DuNetManager.Instance.SendMessage_FinishBuild(data);


	}

	/// <summary>
	/// 增加动画
	/// </summary>
	public void AddAnimation(ModelData data)
	{
		data.improtState = ModelImportState.AddAnimation;

	}

	#region Get ModelData
	public ModelData GetModelDataByAssetPath(string assetPath)
	{
		foreach(ModelData data in this.modelDatas)
		{
			if(data.inFbxPath.Contains(assetPath))
				return data;
		}
		return null;
	}
	#endregion



	#region xmlEditor

	/// <summary>
	/// 读取需要加载的模型文件列表
	/// </summary>
	public List<ModelData> ReadModelsInfoList()
	{

        List<ServerModelMessage> serverDataList = XMLTool.ReadListInRoot<ServerModelMessage>(ConfigPath.MODELS_INFO_PATH, "Root");
        List<ModelData> modelDataList = new List<ModelData>();
        foreach (ServerModelMessage message in serverDataList)
        {
            if(!message.hasRead)
            {
                message.hasRead = true;
                modelDataList.Add(new ModelData(message));
            }
           
        }
        XMLTool.SaveListInRoot(serverDataList,ConfigPath.MODELS_INFO_PATH,"Root");
        return modelDataList;
	}

	/// <summary>
	/// 保存模型
	/// </summary>
	public void SaveModelInfoList()
	{

		//XMLTool.SaveListInRoot(modelDatas,ConfigPath.MODELS_INFO_PATH,"Root");
	}

	#endregion

    
	#region Test

	[MenuItem("Test/AddModelData")]
	public static void TestAddData()
	{

        List<ServerModelMessage> serverModelDatas = new List<ServerModelMessage>();
        ServerModelMessage msg = new ServerModelMessage();
        msg.id = 11;
        msg.userId = 23123;
        msg.outFbxPath = "F:\\Tree02.FBX";
        serverModelDatas.Add(msg);

        XMLTool.SaveListInRoot(serverModelDatas, "F:\\ModelInfos.xml", "Root");
	}

	[MenuItem("Test/ModelManager/ImportModel")]
	public static void TestReadData()
	{
		ModelManager.Instance.ImportModel ();

	}
	#endregion
}
