using UnityEngine;

public class EnemyShip : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed = 0.5f;

    [SerializeField]
    private float sineFrequence = 0.01f;
    [SerializeField]
    private float sineMagnitude = 0.01f;
    [SerializeField]
    private float sineMovementSpeed = 0.01f;
    
    private Vector3 _horizontalLocalPosition;

    private Transform _shipMesh;
    private int _directionMultiplier = 1;

    public GameObject bulletPrefab;


    // Start is called before the first frame update
    void Start()
    {
        _shipMesh = transform.Find("EnemyShip");

        if(_shipMesh) _horizontalLocalPosition = _shipMesh.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        SineMoveShip();

        // Move parent object down
        transform.Translate(Vector3.down * movementSpeed * Time.deltaTime);
    }

    private void SineMoveShip()
    {
        // Sine move enemy ship
        if (_shipMesh.transform.localPosition.x >= 0.3f)
        {
            _directionMultiplier = -1; // FACE LEFT         

        }
        else if (_shipMesh.transform.localPosition.x <= -0.3f)
        {
            _directionMultiplier = 1; // FACE RIGhT
        }

        _horizontalLocalPosition += (_directionMultiplier * _shipMesh.transform.right) * (Time.deltaTime * sineMovementSpeed);
        _shipMesh.transform.localPosition = _horizontalLocalPosition + _shipMesh.up * Mathf.Sin(Time.time * sineFrequence) * sineMagnitude;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "BottomBarrier")
        {
            Destroy(gameObject);
            if (GameManager.player) GameManager.player.ChangeLives(-1);
        }
    }

}
