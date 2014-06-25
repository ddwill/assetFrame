using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using System.IO;

using System.Xml.Serialization;

public class XMLTool {
	/// <summary>
	/// 读取根目录下子节点的数据
	/// </summary>
	/// <returns>子节点下的所有数据</returns>
	/// <param name="path">绝对路径</param>
	/// <param name="rootName">根节点的名字</param>
	public static List<T> ReadListInRoot<T>(string path,string rootName)
	{
		XmlDocument doc = new XmlDocument();
	
		doc.Load (path);
		XmlNode xn = doc.SelectSingleNode(rootName);

		List<T> tList = new List<T> ();

		foreach (XmlNode node in xn) 
		{

			using (var sw = new System.IO.StringWriter())
			{
				using (var xw = new System.Xml.XmlTextWriter(sw))
				{
					xw.Formatting = System.Xml.Formatting.Indented;
					//xw.Indentation = indentation;
					//node.WriteContentTo(xw);
					node.WriteTo(xw);
				}
				tList.Add((T)DeserializeObject(sw.ToString(),typeof(T)));
			}
		}
		return tList;
	}


	/// <summary>
	/// 保存列表数据到指定路径
	/// </summary>
	/// <param name="tList">列表数据</param>
	/// <param name="path">路径</param>
	/// <param name="rootName">根节点名字</param>
	/// <typeparam name="T">数据泛型类</typeparam>
	public static void SaveListInRoot<T>(List<T> tList,string path,string rootName)
	{
		XmlDocument rootDoc = new XmlDocument();
		//path += Application.dataPath;
		XmlNode rootNode = rootDoc.CreateNode (XmlNodeType.Element, rootName, null);
		rootDoc.AppendChild (rootNode);

		foreach (T t in tList) 
		{
			MemoryStream stm = new MemoryStream();
			
			StreamWriter stw = new StreamWriter(stm);

			XmlSerializer xs = new XmlSerializer(typeof(T));

			XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
			namespaces.Add(string.Empty, string.Empty);
			xs.Serialize(stw,t,namespaces);
			stm.Flush();
			stm.Position = 0;

			StreamReader sr = new StreamReader(stm);
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(sr.ReadToEnd());
			XmlNode newNode = doc.DocumentElement;

			rootDoc.DocumentElement.AppendChild(
				rootDoc.ImportNode(doc.DocumentElement, true)
				);

			//stm.Read
		}

		rootDoc.Save (path);
	}

	//读取XML文件
	public string LoadXML(string fileName)
	{
		StreamReader sReader = File.OpenText(fileName);
		string dataString = sReader.ReadToEnd();
		sReader.Close();

		return dataString;
	}

	//判断是否存在文件
	public bool hasFile(String fileName)
	{
		return File.Exists(fileName);
	}

	/// <summary>
	/// utf8转换
	/// </summary>
	/// <returns>The f8 byte array to string.</returns>
	/// <param name="characters">Characters.</param>
	public static string UTF8ByteArrayToString(byte[] characters  )
	{     
		UTF8Encoding encoding  = new UTF8Encoding();
		string constructedString  = encoding.GetString(characters);
		return (constructedString);
	}

	public static byte[] StringToUTF8ByteArray(String pXmlString )
	{
		UTF8Encoding encoding  = new UTF8Encoding();
		byte[] byteArray  = encoding.GetBytes(pXmlString);
		return byteArray;
	}

	/// <summary>
	/// 序列化
	/// </summary>
	/// <returns>The object.</returns>
	/// <param name="pObject">对象</param>
	/// <param name="ty">Ty.</param>
	public static string SerializeObject(object pObject,System.Type ty)
	{
		string XmlizedString   = null;
		MemoryStream memoryStream  = new MemoryStream();
		XmlSerializer xs  = new XmlSerializer(ty); 
		XmlTextWriter xmlTextWriter  = new XmlTextWriter(memoryStream, Encoding.UTF8);
		xs.Serialize(xmlTextWriter, pObject);
		memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
		XmlizedString = UTF8ByteArrayToString(memoryStream.ToArray());
		return XmlizedString;
	}

	/// <summary>
	/// 反序列化字符串到对象
	/// </summary>
	/// <returns>The object.</returns>
	/// <param name="pXmlizedString">P xmlized string.</param>
	/// <param name="ty">Ty.</param>
	public static object DeserializeObject(string pXmlizedString , System.Type ty)
	{

		XmlSerializer xs  = new XmlSerializer(ty);
		XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
		namespaces.Add(string.Empty, string.Empty);

		MemoryStream memoryStream  = new MemoryStream(StringToUTF8ByteArray(pXmlizedString));
		/*XmlTextWriter xmlTextWriter   = */new XmlTextWriter(memoryStream, Encoding.UTF8);
		return xs.Deserialize(memoryStream);
	}


	public static T LoadObject<T>(string path)
	{
		T t ;
		//using(FileStream fs =new
		XmlDocument xmlDoc = new XmlDocument ();
		xmlDoc.Load (path);
		XmlSerializer xs = new XmlSerializer (typeof(T));


		using (var sw = new System.IO.StringWriter())
		{
			using (var xw = new System.Xml.XmlTextWriter(sw))
			{
				xw.Formatting = System.Xml.Formatting.Indented;
				xmlDoc.WriteTo(xw);
			}
			t =  (T)DeserializeObject(sw.ToString(),typeof(T));
		}
		
		return t;
	}

	public static void SaveObject<T>(string path,T t)
	{
		FileTool.CreateDirectory (path);
		using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
		{
			XmlSerializer xmlser = new XmlSerializer(typeof(T));
			xmlser.Serialize(fs,t);
		}
	}


	/**
	private static T ConvertNode<T>(XmlNode node) where T: class
	{
		MemoryStream stm = new MemoryStream();
		
		StreamWriter stw = new StreamWriter(stm);
		stw.Write(node.OuterXml);
		stw.Flush();
		
		stm.Position = 0;
		
		XmlSerializer ser = new XmlSerializer(typeof(T));
		T result = (ser.Deserialize(stm) as T);
		
		return result;
	}**/

}
