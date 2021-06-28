using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    public struct Profile
    {
        public string name;
        public int reachTime;
        public int maxObstacle;
        public int obstacleScore;
        public float obsSpawnTimeMin;
        public float obsSpawnTimeMax;
        public float bonusSpawnTimeMin;
        public float bonusSpawnTimeMax;
    }

    public static GameManager instance;

    public GameObject[] obstacles;
    public GameObject[] bonus;
    public Slider healthSlider;
    public GameObject buttonArea;
    public GameObject Instruction;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI scoreTxt;
    public Text BonusScoreText;
    public Text HealthScoreText;
    public Text SpeedUpTimeText;

    // Gameplay setup
    public float PLAYER_BASE_SPEED = 1f;
    //public float MIN_PLAYER_ROTATION = -45;
    //public float MAX_PLAYER_ROTATION = 45;

    public int BONUS_SCORE = 5;
    public float BONUS_HEALTH = 10f;
    public float SPEED_BONUS_MULTIPLIER = 2;
    public float SPEED_BONUS_TIME = 5f;

    public float PLAYER_MAX_HEALTH = 100f;

    public Profile[] DIFFICULTY_PROFILE;

    //////////////////////////

    private float playerSpeed;
    private int score;
    private int high_score;
    private float playerHealth;
    private float maxY;
    private float maxX;
    private float obsSpawnTimeCount;
    private float bonusSpawnTimeCount;
    private float speedBonusTimeCount;
    float timeToHideScoreEffect = 0;
    float timeToHideHealthEffect = 0;
    float timeToHideSpeedupEffect = 0;
    float timeInGame = 0;
    Profile profile;
    bool isStart;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(instance);
        profile = DIFFICULTY_PROFILE[0];

        obsSpawnTimeCount = Random.Range(profile.obsSpawnTimeMin, profile.obsSpawnTimeMax);
        bonusSpawnTimeCount = Random.Range(profile.bonusSpawnTimeMin, profile.bonusSpawnTimeMax);
        high_score = PlayerPrefs.GetInt("game_max_score", 0);

        playerSpeed = PLAYER_BASE_SPEED;
        speedBonusTimeCount = 0;
        playerHealth = PLAYER_MAX_HEALTH;
        healthSlider.maxValue = PLAYER_MAX_HEALTH;
        maxX = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0)).x;
        maxY = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height)).y;
        Instruction.SetActive(true);
        isStart = false;
        timeInGame = 0f;
    }

    private void Update()
    {

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Ended)
            {
                Instruction.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            Instruction.SetActive(false);
        }

        if (Instruction.activeSelf)
            return;
        isStart = true;
        CalculateDifficulty();

        if (playerHealth > 0)
        {
            // obstacle spawn
            obsSpawnTimeCount -= Time.deltaTime;
            if (obsSpawnTimeCount < 0)
            {
                for (int i = 0; i < profile.maxObstacle; i++)
                {
                    SpawnRandomObstacle();
                }
                obsSpawnTimeCount = Random.Range(profile.obsSpawnTimeMin, profile.obsSpawnTimeMax);
                score += profile.obstacleScore;
            }

            // bonus spawn
            bonusSpawnTimeCount -= Time.deltaTime;
            if (bonusSpawnTimeCount < 0)
            {
                SpawnRandomBonus();
                bonusSpawnTimeCount = Random.Range(profile.bonusSpawnTimeMin, profile.bonusSpawnTimeMax);
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

        if (Time.time >= timeToHideScoreEffect)
        {
            BonusScoreText.gameObject.SetActive(false);
        }
        if (Time.time >= timeToHideHealthEffect)
        {
            HealthScoreText.gameObject.SetActive(false);
        }
        if (Time.time >= timeToHideSpeedupEffect)
        {
            SpeedUpTimeText.gameObject.SetActive(false);
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

    public void ShowBonusScore(int score)
    {
        string text = "+" + score + " point";
        BonusScoreText.text = text;
        Character character = FindObjectOfType<Character>();
        Vector3 charPos = character.gameObject.transform.position;
        BonusScoreText.gameObject.transform.position = new Vector3(charPos.x, charPos.y + 1.0f, charPos.z);
        BonusScoreText.gameObject.SetActive(true);
        timeToHideScoreEffect = Time.time + 1.0f;
    }

    public void ShowHealthBonus(float health)
    {
        string text = "+" + health + " HP";
        HealthScoreText.text = text;
        Character character = FindObjectOfType<Character>();
        Vector3 charPos = character.gameObject.transform.position;
        HealthScoreText.gameObject.transform.position = new Vector3(charPos.x, charPos.y + 1.0f, charPos.z);
        HealthScoreText.gameObject.SetActive(true);
        timeToHideHealthEffect = Time.time + 1.0f;
    }

    public void ShowSpeedUpTimeBonus(float time)
    {
        string text = "+" + time + "s Speed up";
        SpeedUpTimeText.text = text;
        Character character = FindObjectOfType<Character>();
        Vector3 charPos = character.gameObject.transform.position;
        SpeedUpTimeText.gameObject.transform.position = new Vector3(charPos.x, charPos.y + 1.0f, charPos.z);
        SpeedUpTimeText.gameObject.SetActive(true);
        timeToHideSpeedupEffect = Time.time + 1.0f;
    }

    public float GetHealth() { return playerHealth; }
    public void SetHealth(float health) { playerHealth = health; }
    public int GetScore() { return score; }
    public void SetScore(int _score) { score = _score; }
    public float GetObstacleSpawnTime() { return profile.obsSpawnTimeMin; }
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
    public bool isGameStart()
    {
        return isStart;
    }
    public void CalculateDifficulty()
    {
        timeInGame += Time.deltaTime;

        for (int i = 0; i < DIFFICULTY_PROFILE.Length; i++)
        {
            if (timeInGame > DIFFICULTY_PROFILE[i].reachTime)
                profile = DIFFICULTY_PROFILE[i];
        }
    }
}
