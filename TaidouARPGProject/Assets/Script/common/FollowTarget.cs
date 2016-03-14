using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class FollowTarget : MonoBehaviour {

    public Vector3 offset;
    private Transform playerBip;
    public float smoothing = 1;

	// Use this for initialization
	void Start () {
        playerBip = GameObject.FindGameObjectWithTag("Player").transform.Find("Bip01");
        
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        //transform.position = playerBip.position + offset;
        Vector3 targetPos = playerBip.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPos, smoothing * Time.deltaTime);
	}
}
