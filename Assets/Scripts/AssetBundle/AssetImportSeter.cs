using UnityEngine;
using System.Collections;
using UnityEditor;

public class AssetImportSeter : AssetPostprocessor {
	void OnPreprocessModel()
	{
		ModelData modelData =  ModelManager.Instance.GetModelDataByAssetPath(assetPath);
		if(modelData!=null)
		{
			///解析模型文件
			#region Analysis ModelFile
			
			#endregion

			//Clip Animation in the FBX  

			//ModelImporter importer = assetImporter as ModelImporter;
			Debug.Log("!!!!!!!!!!!OnPreprocessModel");
			/**editorImporterUtil.clipArrayListCreater creater = new editorImporterUtil.clipArrayListCreater();
			creater.addClip("idle", 0, 50, true, WrapMode.Loop);
			textureImporter.clipAnimations = creater.getArray();**/
		}
	}

	void OnPostprocessModel(GameObject go)
	{
		/**Debug.Log("OnPostprocessModel....."+assetPath);
		ModelData modelData =  ModelManager.Instance.GetModelDataByAssetPath(assetPath);
		if(modelData!=null)
		{

			ModelManager.Instance.CreatePrefab(modelData,go);
		}**/
	}
}
