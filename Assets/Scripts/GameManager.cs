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
        public int difficultyLevel;
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
    public BonusText BonusScoreText;
    public BonusText HealthScoreText;
    public BonusText SpeedUpTimeText;

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
    private int numberOfObstacle;
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
        numberOfObstacle = 0;
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
        

        if (playerHealth > 0)
        {
            isStart = true;
            timeInGame += Time.deltaTime;
            CalculateDifficulty();

            SoundManager.instance.PlayPlayingBackgroundSound();

            // obstacle spawn
            obsSpawnTimeCount -= Time.deltaTime;
            if (obsSpawnTimeCount < 0 && numberOfObstacle <= profile.maxObstacle)
            {
                SpawnRandomObstacle();
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
            SoundManager.instance.PlayAfterDeathBackgroundSound();
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

    public void ShowBonusScore(int score)
    {
        string text = "+" + score + " points";
        Vector3 charPos = GetCharacterPosition();
        BonusScoreText.gameObject.transform.position = new Vector3(charPos.x, charPos.y + 1.0f, charPos.z);
        BonusScoreText.ShowText(text);
    }

    public void ShowHealthBonus(float health)
    {
        string text = "+" + health + " HP";
        Vector3 charPos = GetCharacterPosition();
        HealthScoreText.gameObject.transform.position = new Vector3(charPos.x, charPos.y + 1.0f, charPos.z);
        HealthScoreText.ShowText(text);
    }

    public void ShowSpeedUpTimeBonus(float time)
    {
        string text = "+" + time + "s speed up";
        Vector3 charPos = GetCharacterPosition();
        SpeedUpTimeText.gameObject.transform.position = new Vector3(charPos.x, charPos.y + 1.0f, charPos.z);
        SpeedUpTimeText.ShowText(text);
    }

    public Vector3 GetCharacterPosition()
    {
        Character character = FindObjectOfType<Character>();
        if (character)
            return character.gameObject.transform.position;
        else
            return new Vector3(0, 0, 0);
    }
    
    public float GetHealth() { return playerHealth; }
    public void SetHealth(float health) { playerHealth = health; }
    public float GetPlayerMaxHealth() { return PLAYER_MAX_HEALTH; }
    public int GetScore() { return score; }
    public void SetScore(int _score) { score = _score; }
    public float GetObstacleSpawnTime() { return profile.obsSpawnTimeMin; }
    public float GetWorldScreenSizeX() { return maxX; }
    public float GetWorldScreenSizeY() { return maxY; }
    public float GetSpeedBonusTime() { return speedBonusTimeCount; }
    public void SetSpeedBonusTime(float time) { speedBonusTimeCount = time; }
    public float GetPlayerSpeed() { return playerSpeed; }
    public void SetPlayerSpeed(float speed) { playerSpeed = speed; }
    public Profile GetProfile() { return profile; }
    public void IncreaseNumberOfObstacle() { ++numberOfObstacle; }
    public void DecreaseNumberOfObstacle() { --numberOfObstacle; }
    private void SpawnRandomObstacle()
    {
        float spawnPosY = maxY + 1f;
        float spawnPosX = Random.Range(-maxX, maxX);
        int randomObsIdx = Random.Range(0, obstacles.Length);
        Instantiate(obstacles[randomObsIdx], new Vector3(spawnPosX, spawnPosY), Quaternion.identity);
        IncreaseNumberOfObstacle();
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
        for (int i = profile.difficultyLevel - 1; i < DIFFICULTY_PROFILE.Length; i++)
        {
            if (timeInGame > DIFFICULTY_PROFILE[i].reachTime)
            {
                if (profile.difficultyLevel != DIFFICULTY_PROFILE[i].difficultyLevel)
                {
                    profile = DIFFICULTY_PROFILE[i];
                    //SoundManager.instance.PlayLevelUpSound();
                    break;
                }
                
            }
        }
    }
}
