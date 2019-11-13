using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using System.IO;


/*
public enum CombatResult {
    FIGHTING,
    ALLY_WIN,
    ENEMY_WIN
}

public struct MonsterBattleInfo {
    public Monster ally;
    public Monster enemy;
    public CombatResult result;

    public MonsterBattleInfo(Monster Ally,Monster Enemy) {
        ally = Ally;
        enemy = Enemy;
        result = CombatResult.FIGHTING;
    }
}
*/

public class MonsterGroup : MonoBehaviour
{
    // Private Vars //
    //private List<MonsterBattleInfo> battles;
    private List<string> nameList = new List<string>();
    private List<Monster> monsters = new List<Monster>();
    private MonsterGroup enemyGroup; // group to fight against

    // Protected Vars //
    protected bool inCombat = false;

    // Public Vars //
    public GameObject monsterPrefab;
    public Vector3 spawnOffset = new Vector3(0,0,2);
    public Color groupColor;

    // Events //
    public event MonsterInfoDelegate OnAddMonster;
    public event MonsterInfoDelegate OnRemoveMonster;

    // Properties //
    public bool InCombat {
	    get { return this.inCombat; }
    }

    /// <summary>
    ///   The amount of monsters inside this group.
    /// </summary>
    public int Count {
	get { return monsters.Count; }
    }

    protected virtual void Start()
    {
        //monsters = new List<Monster>();
        //groupColor = Color.blue;
        LoadNames();

    }

    // Update is called once per frame
    protected void Update()
    {
    }

    private void LoadNames()
    {
        // https://support.unity3d.com/hc/en-us/articles/115000341143-How-do-I-read-and-write-data-from-a-text-file-
        string path = "Assets/Resources/MonsterNames.txt";
        StreamReader reader = new StreamReader(path, true);
        while (!reader.EndOfStream)
        {
            nameList.Add(reader.ReadLine());
        }
        reader.Close();
    }

    public void CreateMonster()
    {
        Monster monster = GameObject.Instantiate(monsterPrefab).GetComponent<Monster>();
        monster.transform.position = transform.position + spawnOffset;
        AddMonster(monster);
        // set monster to status quo
    }

    public void Follow(Transform target)
    {
        foreach (Monster m in monsters)
        {
            m.Follow(target);
        }
    }

    public void Attack(Monster enemyMonster) // attack MonsterGroup variant?
    {
	if(Count > 0){
	    if(enemyGroup == null){
		string team = "";
		enemyGroup = enemyMonster.GetGroup();
		foreach(Monster m in monsters){
		    team += m.name + " ";
		}
		Debug.Log(name + " group join the fight! " + team + " enter the battlefield.");
	    }
	    inCombat = true;


	    foreach(Monster m in monsters){
		if(!m.HasEnemy()){
		    m.ChooseEnemy(enemyGroup.GetMonsters());
		}
	    }
	}

	
        /*
        foreach(Monster m in monsters)
        {
            if(m.GetState() != MonsterState.ATTACK) {
                m.AttackMonster(monster);
            }
            //if(m.GetState() != MonsterState.ATTACK)
            //    m.AttackMonster(monster);
        }
        */



	    /*
        foreach (Monster e in enemyGroup.GetMonsters()) {
            if (!e.HasEnemy()) {
                bool allOccupied = true;   // if all our monsters are attacking
                foreach (Monster m in monsters) {
                    if (!m.HasEnemy()) {
                        m.AttackMonster(e);
                        allOccupied = false;
                    }
                }
                if (allOccupied)
                    return;
            }
        }*/

        // Attack Monster group
        // Find available enemy in group (doesn't have enemy and is close enough)
        // attack it
    }

    public void AddMonster(Monster monster)
    {
        monster.SetGroup(this);
        monster.SetColor(groupColor);
	//Debug.Log("NameList Count: " + nameList.Count.ToString());
        monster.name = nameList[Random.Range(0, nameList.Count)];
	    //Debug.Log("MonsterGroup: Monsters:" + monsters + " Monster: " + monster);
        monsters.Add(monster);
	if(OnAddMonster != null)
	    OnAddMonster(monster);
        //monster.OnDeath += MonsterDeath;
        //monster.OnKillTarget += MonsterKill;
    }

    public void RemoveMonster(Monster monster)
    {
	//Debug.Log("REMOVING MONSTER FROM LIST");
	// getting called a lot when broken
        // remove event subscriptions
	if(OnRemoveMonster != null)
	    OnRemoveMonster(monster);
	monster.LeaveGroup();
        monsters.Remove(monster);
    }

    public ReadOnlyCollection<Monster> GetMonsters() {
        return this.monsters.AsReadOnly();
    }

    public virtual void MonsterAttacked(Monster monster,Monster monsterEnemy) {
	// this group begins attack on enemy group
	//Debug.Log(monster.name + " IS GETTING ATTACKED!");
	Attack(monsterEnemy);
    }


    public virtual void MonsterDeath(Monster monster,Monster monsterEnemy)
    {
	RemoveMonster(monster);
	if(Count == 0){
	    //Debug.Log("NO MORE MONSTERS!");
	    inCombat = false;
	    enemyGroup = null;
	}
        //monsters.Remove(monster);
    }

    public virtual void MonsterKill(Monster monster,Monster monsterEnemy)
    {
        // find next monster to attack
        //monster.Follow(transform); // change this???
	//Debug.Log("MONSTERKILL: IS IN MONSTER GROUP? " + (monster.GetGroup() == this));
	//Debug.Log("monster is null: " + (monster == null) + " monsterEnemy is null: " + (monsterEnemy == null));
        //Debug.Log(monster.name + " KILLED ENEMY!!! NEXT TARGET...");

	if (enemyGroup != null){
	    if (enemyGroup.Count == 0) {
		//Debug.Log(monster.name + ": ENEMY COUNT AT 0!!!");

		inCombat = false;
		enemyGroup = null;
		//monster.Follow(transform);
		Follow(transform);
	    } else {
		//Debug.Log(monster.name + ": FINDING NEXT ENEMY");

		//will change states in monster script//monster.Follow(transform);

		Monster enemyFound = monster.ChooseEnemy(enemyGroup.GetMonsters());
		//if (enemyFound == null)
		//{
		//    UnityEditor.EditorApplication.isPlaying = false;
		//    throw new System.Exception("you dun goofed");
		//}
	    }
	}else{
	    Follow(transform); // temp solution, pls fix
	}
    }

    void OnGUI() {
	// if(name.Equals("Player")){
	//     if(inCombat){
	//         GUI.color = Color.red;
	//         GUI.Box(new Rect(10,10,200,50), "In Combat with:\n" + enemyGroup.name + "\nCount: \t"+ Count);
	//     }
	// }
        /*
	  GUI.color = Color.black;

	  GUI.Box(new Rect(10,10,500,20 + 10*monsters.Count),"Monsters");
	  GUI.color = Color.white;
	  int i = 0;
	  foreach (Monster m in monsters) {
	  GUI.Label(new Rect(10, 10+(i*10), 1000, 20),m.name+" Health: "+m.GetHealth().ToString()+" State: "+m.GetState().ToString() + "Combat State: "+m.GetCombatState());
	  i++;
	  }
        */
    }

    public MonsterGroup GetEnemyGroup(){
	return enemyGroup;
    }
}
