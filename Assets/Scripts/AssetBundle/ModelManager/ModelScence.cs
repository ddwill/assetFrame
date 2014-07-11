using UnityEngine;
using System.Collections;

/// <summary>
/// 
/// </summary>
public class ModelScence : MonoBehaviour {

	private static ModelScence _instance;

	public static ModelScence Instance
	{
		get
		{
			return _instance;
		}
	}
 	
	public string modelPath;

	public Transform ModelPosition;

	public GameObject model;

	void Awake()
	{
		_instance = this;
	}

	// Use this for initialization
	void Start () {
		model =  (GameObject) Instantiate(Resources.Load (modelPath));
		model.transform.position = ModelPosition.position;
		model.transform.rotation = ModelPosition.rotation;
	}
	
	// Update is called once per frame
	void Update () {
		
	}


}
