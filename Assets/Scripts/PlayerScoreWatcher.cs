using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScoreWatcher : MonoBehaviour
{
    // hracovo skore
    private int score = 0;
    // componenta textu v enginu
    private TextMesh scoreMesh = null;

    private void Start()
    {
        scoreMesh = gameObject.GetComponent<TextMesh>();
        // zakladni nastaveni score na 0
        scoreMesh.text = "0";
    }
    // prihlasime se k odberu eventu - smrt nepritele
    private void OnEnable()
    {
        EnemyController.onEnemyDeath += addScore;
    }
    // odhlasime se z odberu
    private void OnDisable()
    {
        EnemyController.onEnemyDeath -= addScore;
    }
    
    // metoda provedena v pripade prijeti zpravy o smrti nepritele
    private void addScore(int addScore)
    {
        // aktualizujeme skore
        score += addScore;
        scoreMesh.text = score.ToString();
    }
}
