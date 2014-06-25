using UnityEngine;
using System.Collections;
using System.IO;
using System;
public class AsyncState
{
    public FileStream fs;
    public byte[] b;
    public LoadStreamingAssets.LoadFinish onLoadFinish;
    public AsyncState(FileStream fs, byte[] b, LoadStreamingAssets.LoadFinish onLoadFinish)
    {
        this.fs = fs;
        this.b = b;
        this.onLoadFinish = onLoadFinish;
    }
}
public class LoadStreamingAssets : MonoBehaviour
{
    /// <summary>
    /// 加载完成回调函数
    /// </summary>
    /// <param name="asset"></param>
    public delegate void LoadFinish(byte[] asset);

    /// <summary>
    /// 异步加载资源包，需要onLoadFinish回调
    /// </summary>
    /// <param name="File">/加载路径</param>
    /// <param name="onLoadFinish">回调函数</param>
    public static void LoadAssetData(string File, MonoBehaviour monoBehaviour, LoadFinish onLoadFinish)
    {
        FileInfo fileInfo;
        //Debug.Log(Application.platform);
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            fileInfo = new FileInfo(Application.dataPath + "/Raw" + File);
            //文件流
            using (FileStream fs = fileInfo.OpenRead())
            {
                byte[] b = new byte[fs.Length];
                AsyncState asyncState = new AsyncState(fs, b, onLoadFinish);
                //异步加载资源包
                fs.BeginRead(b, 0, b.Length, new AsyncCallback(AsyncReadCallback), asyncState);
            }
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            //android使用WWW异步读取资源包
            monoBehaviour.StartCoroutine(LoadAndoridAsset("jar:file://" + Application.dataPath + "!/assets" + File, onLoadFinish));
        }
        else
        {
            fileInfo = new FileInfo(Application.dataPath + "/StreamingAssets" + File);
            //本地加载
            using (FileStream fs = fileInfo.OpenRead())
            {
                byte[] b = new byte[fs.Length];
                AsyncState asyncState = new AsyncState(fs, b, onLoadFinish);
                fs.BeginRead(b, 0, b.Length, new AsyncCallback(AsyncReadCallback), asyncState);
            }
        }
    }

    /// <summary>
    /// 异步协程读取资源包
    /// </summary>
    /// <param name="File"></param>
    /// <param name="onLoadFinish"></param>
    /// <returns></returns>
    static IEnumerator LoadAndoridAsset(string File, LoadFinish onLoadFinish)
    {
        WWW www = new WWW(File);
        yield return www;
        onLoadFinish(www.bytes);
    }

    /// <summary>
    /// 异步读取资源包
    /// </summary>
    /// <param name="asyncResult"></param>
    static void AsyncReadCallback(IAsyncResult asyncResult)
    {
        AsyncState asyncState = (AsyncState)asyncResult.AsyncState;
        int readCn = asyncState.fs.EndRead(asyncResult);
        //判断是否读到内容
        if (readCn > 0)
        {
            asyncState.onLoadFinish(asyncState.b);
        }

        //if (readCn < bufferSize)
        //{
        //    asyncState.EvtHandle.Set();
        //}
        //else
        //{
        //    Array.Clear(asyncState.Buffer, 0, bufferSize);
        //    //再次执行异步读取操作
        //    asyncState.FS.BeginRead(asyncState.Buffer, 0, bufferSize, new AsyncCallback(AsyncReadCallback), asyncState);
        //}
    }
}
