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

	#region Property
	public List<ModelData> modelDatas = new List<ModelData> ();
		
	#endregion

	public ManagerState eState = ManagerState.Free;
	/// <summary>
	/// Editor 中获得模型路径或是从文件中读取
	/// </summary>
	public void ImportModel()
	{
		this.modelDatas =  ReadModelsInfoList ();
		Debug.Log ("ImportModel!!!!!!!"+modelDatas.Count);
		foreach(ModelData data in modelDatas)
		{

			CreatePrefab(data);
			AddAnimation (data);
			SeperateModeleSources (data);
			BuidlAssetBundle (data);
			SendToServerWebFloder (data);
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
	/// 复制模型文件到目录
	/// 
	/// 生成预设
	/// </summary>
	public void CreatePrefab(ModelData data)
	{
		data.inFbxPath = ConfigPath.MODEL_STORE_GLOBAL_PATH + data.userId + "/" + data.id+Path.GetExtension(data.outFbxPath);
		Debug.Log (data.inFbxPath);
		FileTool.CreateDirectoryRealPath (data.inFbxPath);
		File.Copy (data.outFbxPath, data.inFbxPath);
		AssetDatabase.Refresh ();
	}

	/// <summary>
	/// 打assetBundle包
	/// </summary>
	/// <param name="data">Data.</param>
	public void BuidlAssetBundle(ModelData data)
	{

	}

	/// <summary>
	/// 发送assetbundle 到服务器,通知服务器更新，memcach.
	/// </summary>
	/// <param name="data">Data.</param>
	public void SendToServerWebFloder(ModelData data)
	{

	}

	/// <summary>
	/// 增加动画
	/// </summary>
	public void AddAnimation(ModelData data)
	{

	}

	#region xmlEditor

	/// <summary>
	/// 读取需要加载的模型文件列表
	/// </summary>
	public List<ModelData> ReadModelsInfoList()
	{

		List<ModelData> modelDataList = XMLTool.ReadListInRoot<ModelData>(ConfigPath.MODELS_INFO_PATH,"Root");

		return modelDataList;
	}

	/// <summary>
	/// 保存模型
	/// </summary>
	public void SaveModelInfoList()
	{

		XMLTool.SaveListInRoot(modelDatas,ConfigPath.MODELS_INFO_PATH,"Root");
	}

	#endregion


	#region Test

	[MenuItem("Test/AddModelData")]
	public static void TestAddData()
	{
		List<ModelData> modelDatas = new List<ModelData> ();
		ModelData data = new ModelData ();
		data.id = 11;
		data.userId = 23123;
		data.outFbxPath = "F:\\Tree02.FBX";
		modelDatas.Add (data);

		XMLTool.SaveListInRoot(modelDatas,"F:\\ModelInfos.xml","Root");
	}

	[MenuItem("Test/ModelManager/ImportModel")]
	public static void TestReadData()
	{
		ModelManager.Instance.ImportModel ();

	}
	#endregion
}
