using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    public Player player;
    public Boss boss;
    public Text text;
    private Animator anim;
    
    void Start()
    {
        anim = GetComponent<Animator>();
	player.OnDeath += PlayerDeath;
	boss.OnDeath += BossDeath;
    }

    void Update()
    {
        
    }

    void BossDeath(AttackInstanceInfo info){
	anim.SetTrigger("Win");
    }

    void PlayerDeath(AttackInstanceInfo info){
	Debug.Log("Player Death");
	anim.SetTrigger("GameOver");
    }

    public void SetText(string newText){
	text.text = newText;
    }
}
