using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterCounter : MonoBehaviour
{
    public Player player;
    private Text text;
    
    void Start()
    {
        text = GetComponent<Text>();
    }

    void Update()
    {
        text.text = "Monsters\n"+player.GetGroup().Count.ToString();
    }
}
