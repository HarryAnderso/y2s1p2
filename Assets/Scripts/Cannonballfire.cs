using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class Cannonballfire : MonoBehaviour
{
    public GameObject cannonfab;
    public Transform leftcan;
    public Transform rightcan;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(cannonfab, leftcan);
        }

        if (Input.GetMouseButtonDown(1))
        {
            Instantiate(cannonfab, rightcan);
        }

    }
}
