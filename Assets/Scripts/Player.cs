using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    private int boundaryValue = 8;
    [SerializeField]
    private float movementSpeed = 2;
    [SerializeField]
    private float shootingFrequency = 0.2f;
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private Text playerScoreText;
    [SerializeField]
    private Text playerLivesText;
    [SerializeField]
    private Text livesImage;
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private int maxPlayerLives = 5;

    [SerializeField]
    private Transform healthImageParent;

    [SerializeField]
    private Image healthImage;

    private int _playerScore = 0;
    private int _playerLives = 5;

    private float _defaultHealthWidth;
    private Transform _defaultHelthImageTransform;

    private const string scoreMarker = "SCORE: ";

    public int lives {
        get { return _playerLives; }
        set { _playerLives = value; }
    }

    public int score
    {
        get { return _playerScore; }
        set { _playerScore = value; playerScoreText.text = scoreMarker + _playerScore; }
    }


    void Start()
    {
        _defaultHealthWidth = healthImage.rectTransform.rect.width;
        _defaultHelthImageTransform = healthImage.transform;

        InitWithDefaultValues();
    }

    // Update is called once per frame
    void Update()
    {
        float axisMovement = Input.GetAxis("Horizontal");
        transform.Translate(axisMovement * movementSpeed * Time.deltaTime, 0, 0);

        // Setting movement constraints
        if (transform.position.x < -boundaryValue) transform.position = new Vector3(-boundaryValue, transform.position.y, 0);
        if (transform.position.x > boundaryValue) transform.position = new Vector3(boundaryValue, transform.position.y, 0);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(FireCoroutine());
        } 
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            StopAllCoroutines();
        }

    }

    private IEnumerator FireCoroutine()
    {
        while (true)
        {
            Instantiate(bulletPrefab, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);

            yield return new WaitForSeconds(shootingFrequency);
        }
    }

    public void InitWithDefaultValues()
    {
        transform.position = new Vector2(0, -4.462f);

        _playerLives = maxPlayerLives;

        for (var i = 1; i <= _playerLives; i++)
        {
            Instantiate(healthImage, healthImageParent.position + (Vector3.right * 20 * i), Quaternion.identity, healthImageParent);
        }
         
        _playerScore = 0;

        if (playerScoreText) playerScoreText.text = scoreMarker + _playerScore;
        //if (playerLivesText) playerLivesText.text = livesMarker + _playerLives;
    }

    public void ChangePoints(int points)
    {
        _playerScore += points;
        playerScoreText.text = scoreMarker + _playerScore;
    }

    public void ChangeLives(int lives)
    {
        _playerLives += lives;

        if(_playerLives < 0) _playerLives = 0;

        Destroy(healthImageParent.GetChild(healthImageParent.childCount - 1).gameObject);
        
        if (_playerLives == 0)
        {
            Time.timeScale = 0;
            gameManager.ShowRestartPanel(true);
        }
    }
}
