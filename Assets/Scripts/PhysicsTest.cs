using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PhysicsTest : MonoBehaviour
{
    public GameObject ground;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Rigidbody2D bb = GetComponent<Rigidbody2D>();
        Collider2D gb = ground.GetComponent<CompositeCollider2D>();
        if(bb.IsTouching(gb))
        {
            Debug.Log("true");
        }
    }
}
