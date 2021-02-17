
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyShipPrefab;
    [SerializeField]
    private float spawnVerticalPosition = 5.66f;
    [SerializeField]
    private float horizontalRange = 8f;
    [SerializeField]
    private GameObject restartPanel;
    [SerializeField]
    private float enemyShootingRate = 2;
    [SerializeField]
    private int maxShootingEnemyCount = 5;
    
    [SerializeField]
    private InputField playerNameText;
    [SerializeField]
    private Text scoreTableText;
    [SerializeField]
    GameObject playerInputPanel;
    
    private const int columnsCount = 6;
    private const int columnWidth = 3;

    public static Player player;

    private void Awake()
    {
        GameObject playerNode = GameObject.Find("Player");
        if (playerNode) player = playerNode.GetComponent<Player>();
    }

    void Start()
    {
        if (enemyShipPrefab) { 
            StartCoroutine(SpawnEnemy());
        }

        StartCoroutine(EnemiesShoot());
    }

    private IEnumerator EnemiesShoot()
    {
        while(true)
        {
            yield return new WaitForSeconds(enemyShootingRate);

            int shootingCount = Random.Range(1, maxShootingEnemyCount);

            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            Debug.Log("Enemies count:" + enemies.Length);

            if (enemies.Length > 0) {
                // Lets shuffle all available enemies
                enemies = enemies.OrderBy(n => System.Guid.NewGuid()).ToArray();

                // Now lets make them shoot
                for(int i = 0; i < shootingCount; i++)
                {
                    if(enemies.Length > i) // Doing coroutine-safe check. To be remade.
                        Instantiate(enemies[i].GetComponent<EnemyShip>().bulletPrefab, enemies[i].transform.position + new Vector3(0, -0.5f, 0), Quaternion.identity);
                }
            }
        }
    }

    private IEnumerator SpawnEnemy()
    {
        while (true) {

            int columnNnumber = Random.Range(0, columnsCount);

            float enemyXposition = columnNnumber * columnWidth - horizontalRange ;

            Instantiate(enemyShipPrefab, new Vector2(enemyXposition, spawnVerticalPosition), Quaternion.identity);

            yield return new WaitForSeconds(0.8f);
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
        foreach (GameObject enemy in enemies) { Destroy(enemy); }
        foreach (GameObject bullet in bullets) { Destroy(bullet); }

        Time.timeScale = 1;

        // Reset player position        
        player.InitWithDefaultValues();

        ShowRestartPanel(false);
    }

    public void ShowRestartPanel(bool show)
    {
        if (restartPanel != null)
        {
            restartPanel.SetActive(show);
            if (playerInputPanel) playerInputPanel.SetActive(true);

            // Displaying scores stored in Player Settings
            scoreTableText.text = Settings.GetSavedScores();
            playerNameText.text = string.Empty;

        }
    }

    public void SavePlayerScore()
    {
        string playerName = playerNameText.text;

        if (playerName != string.Empty)
        {
            string playerScoreString = playerName + " " + player.score;

            Settings.SaveScore(playerScoreString);

            if (playerInputPanel) playerInputPanel.SetActive(false);
            
            // Re-fetch stored scores
            scoreTableText.text = Settings.GetSavedScores();

        }
    }

    public void ClearScore()
    {
        Settings.ClearScores();

        // Re-fetch stored scores
        scoreTableText.text = Settings.GetSavedScores();
    }
}
