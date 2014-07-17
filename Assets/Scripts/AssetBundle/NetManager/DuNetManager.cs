using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System;
using UnityEditor;

public class DuNetManager{

    public delegate void DelResponse(WWW www);

    private static DuNetManager _instance;
    public static DuNetManager Instance
    {
        get
        {
            if (_instance == null) _instance = new DuNetManager();
            return _instance;
        }
    }

    List<DuNetEvent> netEventList = new List<DuNetEvent>();


    void sendMessage(string url, DelResponse response)
    {
        DuNetEvent netEvent = new DuNetEvent();
        netEvent.www  = new WWW(url);
        netEvent.response = response;
        netEvent.url = url;
        this.netEventList.Add(netEvent);
    }

    public void SendMessage_FinishBuild(ModelData data)
    {
        string c="UploadSource";
        string m = "finishBuildBundle";
        
        string url = ConfigPath.SERVER_PATH+ "c="+c+"&m="+m+"&bundlePath="+data.getBundlePath(BuildTarget.WebPlayer);
        sendMessage(url, ResponseMessage_FinishBuild);
    }

    public void ResponseMessage_FinishBuild(WWW www)
    {
        Debug.Log(www.error);
        Debug.Log(www.text);
    }

    public void CheckNetEvent()
    {
        this.netEventList.RemoveAll(delegate(DuNetEvent netEvent)
        {
            Debug.Log("CheckNetEvent");
             if(netEvent.www.isDone)
             {
                 netEvent.ExcuteEvent();
             }
             return netEvent.www.isDone;
        });
    }
}
