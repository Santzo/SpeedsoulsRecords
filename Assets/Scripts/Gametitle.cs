using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Gametitle : MonoBehaviour {
    public TextMeshProUGUI IGT;
    public TextMeshProUGUI RTA;
    private GameObject parent;
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (transform.parent.tag != "Content" && !SimpleExample.isLoading && !SimpleExample.changeParents) transform.SetParent(GameObject.FindGameObjectWithTag("Content").transform);
		
	}
}
