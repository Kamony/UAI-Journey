using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerScoreWatcher : MonoBehaviour
{
    // hracovo skore
    private int score = 0;
    
    public TextMeshProUGUI Health;
    public TextMeshProUGUI Score;

    // prihlasime se k odberu eventu - smrt nepritele
    private void OnEnable()
    {
        EnemyController.onEnemyDeath += addScore;
        MathBossController.bossDeath += addScore;
        PlayerStateListener.OnTakingDmgAction += TakeDmg;
        Debug.Log("Score ENABLED");
    }
    
   // odhlasime se z odberu
    private void OnDisable()
    {
        EnemyController.onEnemyDeath -= addScore;
        MathBossController.bossDeath -= addScore;
        PlayerStateListener.OnTakingDmgAction -= TakeDmg;
        Debug.Log("Score DISABLED");
    }

    // metoda provedena v pripade prijeti zpravy o smrti nepritele
    private void addScore(int addScore)
    {
        // aktualizujeme skore
        score += addScore;
        Score.text = score.ToString();
    }
    
    private void TakeDmg(int healthRemaining)
    {
        Health.text = healthRemaining.ToString();
    }
}
