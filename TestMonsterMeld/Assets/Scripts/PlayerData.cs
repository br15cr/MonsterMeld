using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData : MonoBehaviour
{

	public int location;
	public int health;
	public float[] position;

	/*public PlayerData (Player player)
	{
		// health = player.health;
		position = new float[3];
		position[0] = player.transform.x;
		position[1] = player.transform.y;
		position[2] = player.transform.z;

	}*/

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
