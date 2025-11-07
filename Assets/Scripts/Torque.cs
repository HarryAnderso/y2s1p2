using UnityEngine;

public class Torque : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Addtorque(15, false);
        }

        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            Addtorque(15, true);
        }
    }

    void Addtorque(float change, bool direction)
    {
        float f = 1;
        if (direction)
            { f = -1; }
        
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        float momentum = (change * Mathf.Deg2Rad) * rb.inertia;

        rb.AddTorque((momentum*f), ForceMode2D.Impulse);
    }
}
