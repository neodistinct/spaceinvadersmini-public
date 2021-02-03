using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed = 8;

    [SerializeField]
    private float outScreenBoundary = 5f;

    [SerializeField]
    private bool downDirection = false;

    [SerializeField]
    bool enemyFriendlyFire = false;

    void Update()
    {
        int directionMultiplier = downDirection ? -1 : 1;

        transform.Translate(directionMultiplier * Vector3.up * movementSpeed * Time.deltaTime);

        // When above
        if (!downDirection && transform.position.y > outScreenBoundary)
        {
            Destroy(gameObject);
        } 
        
        // When belove
        if (downDirection && transform.position.y < outScreenBoundary)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && !enemyFriendlyFire)
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
            GameManager.player.ChangePoints(10);
        }
        else if(collision.CompareTag("Player"))
        {
            Destroy(gameObject);
            GameManager.player.ChangeLives(-1);
        }

    }
}