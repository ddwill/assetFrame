using UnityEngine;
using System.Collections;

public class DuNetEvent {
    public DuNetManager.DelResponse response;
    public string url;
    public WWW www;


    public void ExcuteEvent()
    {
        response(www);
    }
}