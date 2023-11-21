using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject enemy;

    float top = 2.5f;
    float sid = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 center = (player.transform.position + enemy.transform.position) * 0.5f;
        transform.position = center;

        float distance = Vector3.Distance(player.transform.position, enemy.transform.position);


        Transform myTransform = this.transform;
        Vector3 pos = myTransform.position;


        pos.y += top + distance / 2;
        pos.z -= sid + distance;

        myTransform.position = pos;

    }
}
