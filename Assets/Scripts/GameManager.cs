using System.Collections;
using System.Linq;
using UnityEngine;

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
                    if(enemies.Length > i) // Doing couroutine-safe check. To be remade.
                        Instantiate(enemies[i].GetComponent<EnemyShip>().bulletPrefab, enemies[i].transform.position + new Vector3(0, -0.5f, 0), Quaternion.identity);
                }
            }
        }
    }

    private IEnumerator SpawnEnemy()
    {
        while (true) {

            float enemyXposition = Random.Range(-horizontalRange, horizontalRange);

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
        }
    }

}
