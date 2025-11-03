using UnityEngine;
using UnityEngine.InputSystem;

public class Cannonballmove : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Vector3 target;
    public Vector3 direction;
    void Start()
    {
        //target= Camera.main.ScreenToWorldPoint(Input.mousePosition);
        target = Input.mousePosition;
    }

    // Update is called once per frame
    void Update()
    {
        direction.x=(target.x-Mathf.Abs(transform.position.x));
        direction.y = (target.y - Mathf.Abs(transform.position.y));
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.AddForce(direction, ForceMode2D.Impulse);
    }
}
