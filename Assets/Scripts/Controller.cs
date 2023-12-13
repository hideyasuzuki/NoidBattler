using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float Test1 = Input.GetAxis("L_Hori_1");
        float Test2 = Input.GetAxis("L_Hori_2");
        Debug.Log("Test1"+Test1+"Test2"+Test2);
        if (Input.GetButton("Jump_1") == true)
        {
            Debug.Log("Push");
        }
    }
}
