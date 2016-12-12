using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
public class Camera : NetworkBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        //if (GetComponent<NetworkView>().isMine)
        //{
        //    gameObject.GetComponent<Camera>().enabled = true;
        //}
        //else
        //{
        //    gameObject.GetComponent<Camera>().enabled = false;
        //}
    }
}