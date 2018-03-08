using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScoreWatcher : MonoBehaviour
{
    // hracovo skore a zivoty - 
    public static int score = 0;
    public static int health;

    public static PlayerScoreWatcher Instance;
    
    public TextMeshProUGUI Health;
    public TextMeshProUGUI Score;

    // prihlasime se k odberu eventu - smrt nepritele
    private void OnEnable()
    {
        EnemyController.onEnemyDeath += addScore;
        MathBossController.bossDeath += addScore;
        PlayerStateListener.OnTakingDmgAction += TakeDmg;
        PlayerStateListener.onRessurectAction += resetStats;
        
        GameManager.onLoadAttempt += updateStats;
        GameManager.newGame += statsToDefault;
        Debug.Log("Score ENABLED");
    }

    private void statsToDefault()
    {
        score = 0;
        Score.text = score.ToString();
    }

    // odhlasime se z odberu
    private void OnDisable()
    {
        EnemyController.onEnemyDeath -= addScore;
        MathBossController.bossDeath -= addScore;
        PlayerStateListener.OnTakingDmgAction -= TakeDmg;
        PlayerStateListener.onRessurectAction -= resetStats;
        GameManager.onLoadAttempt -= updateStats;
        GameManager.newGame -= statsToDefault;
        Debug.Log("Score DISABLED");
    }



    private void resetStats(int health, int score)
    {
        addScore(score);
        TakeDmg(health);
        
    }
    
    
    // metoda provedena v pripade prijeti zpravy o smrti nepritele
    private void addScore(int addScore)
    {
        // aktualizujeme skore
        score += addScore;
        Score.text = score.ToString();
    }
    
    // metoda volana v pripade obdrzeni dmg
    private void TakeDmg(int healthRemaining)
    {
        Health.text = healthRemaining.ToString();
        health = healthRemaining;
    }
    
    private void updateStats(DataStructure ds)
    {
        score = ds.score;
        health = ds.health;
        
        Score.text = score.ToString();
        Health.text = health.ToString();
    }

    public void resetHealth()
    {
        health = 5;
        Health.text = health.ToString();
    }

    private void Awake()
    {
        Instance = this;
    }
}
