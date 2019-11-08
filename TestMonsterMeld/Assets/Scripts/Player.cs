using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController body;
    private MonsterGroup playerMonsters;

    public float speed = 1;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<CharacterController>();
        playerMonsters = GetComponent<MonsterGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move(Vector2 direction)
    {
        Vector3 moveDirection = new Vector3(direction.x, 0, direction.y);
        if (moveDirection.magnitude > 1)
        {
            moveDirection = moveDirection.normalized;
        }
        moveDirection *= speed;
        body.Move(moveDirection);
        float div = moveDirection.x / moveDirection.y;
        float angle = Mathf.Atan(div) * Mathf.Rad2Deg;
        if (moveDirection.y < 0)
        {
            angle -= 180;
        }
        transform.rotation = Quaternion.Euler(0, angle, 0);
    }

    public void CallMonsters()
    {
        playerMonsters.Follow(this.transform);
    }

    public void SpawnMonster()
    {
        playerMonsters.CreateMonster();
    }

    public void AttackMonsters()
    {
        Debug.Log("ATTACK");
        Collider[] cols = Physics.OverlapSphere(transform.position, 10);
        Debug.Log("Found Colliders: " + cols.Length.ToString());
        for(int i = 0; i < cols.Length; i++)
        {
            Debug.Log("Name: " + cols[i].name);
            Monster monst = cols[i].GetComponent<Monster>();
            if(monst != null)
            {
                if(monst.GetGroup() != playerMonsters)
                {
                    Debug.Log("found enemy");
                    //playerMonsters.Follow(cols[i].transform);
                    playerMonsters.Attack(monst);
                    break; // for now
                }
            }
        }
    }
}
