using System.Collections;
using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject ballfab;
    public int ballSpawnCount = 100;
    public float spawninterval = .02f;
    public bool randomcolor = true;
    IEnumerator Start()
    {
        for(int i=0; i<ballSpawnCount; i++)
        {
            GameObject ball = Instantiate(ballfab, transform.position, Quaternion.identity);
            if(randomcolor)
            {
                Color random = new Color(Random.value, Random.value, Random.value);
                ball.GetComponent<SpriteRenderer>().color = random;

            }
            Rigidbody2D body2d = ball.GetComponent<Rigidbody2D>();

            body2d.AddForce(Random.insideUnitCircle.normalized, ForceMode2D.Impulse);
            yield return new WaitForSeconds(spawninterval);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
