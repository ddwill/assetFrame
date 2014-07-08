using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class FileTool{
	public static void CreateDirectory(string path)
	{

		string dirPath = Application.dataPath +"/"+ System.IO.Path.GetDirectoryName (path).TrimStart ("Assets/".ToCharArray ());
		bool isExists = System.IO.Directory.Exists(dirPath);

		if (!isExists) 
		{
			System.IO.Directory.CreateDirectory(dirPath);
		}
	}

	public static void DeleteDirectoryAndChild(string path)
	{
		string dirPath = Application.dataPath +"/"+ System.IO.Path.GetDirectoryName (path).TrimStart ("Assets/".ToCharArray ());
		bool isExists = System.IO.Directory.Exists(dirPath);
		if(isExists) Directory.Delete (dirPath, true);
	}


}
