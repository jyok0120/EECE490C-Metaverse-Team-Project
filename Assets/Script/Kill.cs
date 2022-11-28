using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kill : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.GetComponent<MeshRenderer>().enabled = false;
        other.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 2, 0);
        other.gameObject.GetComponent<MeshCollider>().enabled = false;
        int n = other.gameObject.transform.childCount;
        for (int i = 0; i < n; i++)
        {
            GameObject gameObject = other.gameObject.transform.GetChild(i).gameObject;
            gameObject.GetComponent<Rigidbody>().position = other.gameObject.GetComponent<Rigidbody>().position;//new Vector3(0,0,0);
            gameObject.GetComponent<Rigidbody>().velocity = new Vector3(-5 + 10 * i / (n-1), 0, 0);
            gameObject.GetComponent<MeshRenderer>().enabled = true;

        }

    }
}
