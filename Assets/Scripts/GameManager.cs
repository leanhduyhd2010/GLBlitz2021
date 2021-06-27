using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject[] obstacles;
    public GameObject[] bonus;
    public Slider healthSlider;
    public GameObject buttonArea;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI scoreTxt;

    // Gameplay setup
    public float PLAYER_BASE_SPEED = 1f;
    public float MIN_PLAYER_ROTATION = -45;
    public float MAX_PLAYER_ROTATION = 45;

    public int BONUS_SCORE = 5;
    public float BONUS_HEALTH = 10f;
    public float SPEED_BONUS_MULTIPLIER = 2;
    public float SPEED_BONUS_TIME = 5f;

    public float MIN_TIME_TO_SPAWN_OBSTACLE = 0.2f;
    public float MAX_TIME_TO_SPAWN_OBSTACLE = 1f;
    public float MIN_TIME_TO_SPAWN_BONUS = 15f;
    public float MAX_TIME_TO_SPAWN_BONUS = 25f;

    public float PLAYER_MAX_HEALTH = 100f;
    //////////////////////////

    private float playerSpeed;
    private int score;
    private int high_score;
    private float playerHealth;
    private float maxY;
    private float maxX;
    private float obsSpawnTime;
    private float obsSpawnTimeCount;
    private float bonusSpawnTimeCount;
    private float speedBonusTimeCount;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(instance);

        bonusSpawnTimeCount = Random.Range(MIN_TIME_TO_SPAWN_BONUS, MAX_TIME_TO_SPAWN_BONUS);
        high_score = PlayerPrefs.GetInt("game_max_score",0);

        speedBonusTimeCount = 0;
        obsSpawnTime = MAX_TIME_TO_SPAWN_OBSTACLE;
        obsSpawnTimeCount = MAX_TIME_TO_SPAWN_OBSTACLE;
        playerHealth = PLAYER_MAX_HEALTH;
        healthSlider.maxValue = PLAYER_MAX_HEALTH;
        maxX = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0)).x;
        maxY = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height)).y;
    }

    private void Update()
    {
        if (playerHealth > 0)
        {   
            // obstacle spawn
            obsSpawnTimeCount -= Time.deltaTime;
            if (obsSpawnTimeCount < 0)
            {
                SpawnRandomObstacle();
                obsSpawnTimeCount = obsSpawnTime;
                ++score;
            }
            if (obsSpawnTime > MIN_TIME_TO_SPAWN_OBSTACLE)
            {
                obsSpawnTime -= Time.deltaTime / 100;
            }
            else
            {
                obsSpawnTime = MIN_TIME_TO_SPAWN_OBSTACLE;
            }
            

            // bonus spawn
            bonusSpawnTimeCount -= Time.deltaTime;
            if (bonusSpawnTimeCount < 0)
            {
                SpawnRandomBonus();
                bonusSpawnTimeCount = Random.Range(MIN_TIME_TO_SPAWN_BONUS, MAX_TIME_TO_SPAWN_BONUS);
            }

            // bonus power
            if (speedBonusTimeCount > 0)
            {
                playerSpeed = PLAYER_BASE_SPEED * SPEED_BONUS_MULTIPLIER;
                speedBonusTimeCount -= Time.deltaTime;
            }
            else
            {
                playerSpeed = PLAYER_BASE_SPEED;
                speedBonusTimeCount = 0;
            }

        }
        else
        {
            if (score > high_score)
            {
                high_score = score;
                PlayerPrefs.SetInt("game_max_score", score);
            }
            

            buttonArea.SetActive(true);
            scoreText.text = "Score: " + score;
            highScoreText.text = "HighScore: " + high_score;
        }
        
    }

    private void OnGUI()
    {
        float t = Time.deltaTime / 1f;
        healthSlider.value = Mathf.Lerp(healthSlider.value, playerHealth, t);
        scoreTxt.text = "Score: " + score.ToString();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Game");
    }


    public float GetHealth() { return playerHealth; }
    public void SetHealth(float health) { playerHealth = health; }
    public int GetScore() { return score; }
    public void SetScore(int _score) { score = _score; }
    public float GetObstacleSpawnTime() { return obsSpawnTime; }
    public float GetWorldScreenSizeX() { return maxX; }
    public float GetWorldScreenSizeY() { return maxY; }
    public float GetSpeedBonusTime() { return speedBonusTimeCount; }
    public void SetSpeedBonusTime(float time) { speedBonusTimeCount = time; }
    public float GetPlayerSpeed() { return playerSpeed; }
    public void SetPlayerSpeed(float speed) { playerSpeed = speed; }
    private void SpawnRandomObstacle()
    {
        float spawnPosY = maxY + 1f;
        float spawnPosX = Random.Range(-maxX, maxX);
        int randomObsIdx = Random.Range(0, obstacles.Length);
        Instantiate(obstacles[randomObsIdx], new Vector3(spawnPosX, spawnPosY), Quaternion.identity);
    }

    private void SpawnRandomBonus()
    {
        float spawnPosY = maxY + 1f;
        float spawnPosX = Random.Range(-maxX, maxX);
        int randomBonusIdx = Random.Range(0, bonus.Length);
        Instantiate(bonus[randomBonusIdx], new Vector3(spawnPosX, spawnPosY), Quaternion.identity);
    }
}
