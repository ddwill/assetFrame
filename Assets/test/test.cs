using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Text;


public class test : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//EditorUtility.OpenFilePanel ("!!!", Application.dataPath, "");

		//print (Application.dataPath);
		ScencesManager.LoadScenceViewData ("test1");
		//print( ConfigPath.TrimPath_Resource (ConfigPath.RESOURCES_CONFIG_SCENCEDATA));
		//AssetDatabase.Refresh ();
		//ScencesManager.LoadScenceViewData ("test1");
		//print( File.Exists ("E:/aa.txt"));
		//BuildAssetBundlesFromDirectory
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnGUI()
	{

	}


	private void loadXML()
	{
		/**List<TestData> tlist = new List<TestData> ();
		for (int i=0; i<4; i++) 
		{
			TestData td = new TestData();
			td.ss = "111";
			td.aaa = 222;
			tlist.Add(td);
		}



		XMLTool.SaveListInRoot (tlist, ConfigPath.RESOURCES_CONFIG_PATH+"/aa.xml");

		List<TestData> testDataList =  XMLTool.ReadListInRoot<TestData> (ConfigPath.RESOURCES_CONFIG_PATH + "aa.xml","Root");


		foreach (TestData data in testDataList)
		{
			print(data.aaa+"..."+data.ss);
		}**/
	}

}

[System.Serializable]
[XmlRoot("Root")]
public class TestData
{
	[XmlAttribute("ss")]
	public string ss;
	[XmlAttribute("aaa")]
	public int aaa;


}
