using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowCubes : MonoBehaviour
{
    public GameObject prefab;
    public float speed = 30;

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            var go = Instantiate(prefab, transform.position + transform.forward, Random.rotation);
            go.GetComponent<Rigidbody>().velocity = transform.forward * speed;
        }
    }
}
