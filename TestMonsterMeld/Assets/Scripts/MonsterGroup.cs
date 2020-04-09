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

    private bool playerGroup; // is the players group
    private Player player;

    public Transform followTarget;
    

    // Protected Vars //
    protected bool inCombat = false;

    // Public Vars //
    public GameObject monsterPrefab;
    public Vector3 spawnOffset = new Vector3(0,0.25f,2);
    public Color groupColor;

    public Material healthbarMat;

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

    public bool IsPlayerGroup {
	get { return this.playerGroup; }
    }

    protected virtual void Start()
    {
        //monsters = new List<Monster>();
        //groupColor = Color.blue;
        LoadNames();

	if(followTarget == null){
	    followTarget = transform;
	}

	player = this.GetComponent<Player>();
	if(player != null){
	    Debug.Log("IS PLAYER!!!");
	    playerGroup = true;
	}

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

    public Monster CreateMonster()
    {
	Debug.Log("CREATED MONSTER");
        Monster monster = GameObject.Instantiate(monsterPrefab).GetComponent<Monster>();
        monster.transform.position = transform.position + spawnOffset;
	//monster.DebugPosition();
	if(healthbarMat != null){
	    Debug.Log("SETTING MONSTER'S HEALTHBAR MAT");
	    monster.SetHealthbarMat(healthbarMat);
	}
        AddMonster(monster);
	return monster;
        // set monster to status quo
    }

    public Monster CreateMonster(GameObject prefab,Vector3 position){
	Monster monster = GameObject.Instantiate(prefab).GetComponent<Monster>();
        monster.transform.position = position;
	//monster.DebugPosition();
	if(healthbarMat != null){
	    Debug.Log("SETTING MONSTER'S HEALTHBAR MAT");
	    monster.SetHealthbarMat(healthbarMat);
	}
        AddMonster(monster);
	return monster;
    }
    
    public void Follow(Transform target)
    {
        foreach (Monster m in monsters)
        {
            m.Follow(target);
        }
    }

    // public void Attack(HealthUser enemy){
    // 	if(enemy.IsMonster()){
    // 	    Attack(enemy.GetComponent<Monster>());
    // 	}else{
    // 	    Debug.Log("ATTACK PLAYER");
    // 	}
    // }

    //public void Attack(Monster enemyMonster) // attack MonsterGroup variant?
    public void Attack(HealthUser enemy)
    {
	if(Count > 0){
	    // if(enemyGroup == null){
		string team = "";
		enemyGroup = enemy.GetGroup();
		foreach(Monster m in monsters){
		    team += m.name + " ";
		}
		Debug.Log(name + " group join the fight! " + team + " enter the battlefield.");
	    //}
	    inCombat = true;


	    foreach(Monster m in monsters){
		//if(!(m.HasEnemy() && m.GetState() == MonsterState.ATTACK)){
		if(!(m.HasEnemy() && m.InAttackState())){
		    //m.ChooseEnemy(enemyGroup.GetMonsters());
		    m.ChooseEnemy(enemyGroup.GetMembers());
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
        //monster.SetColor(groupColor);
	Debug.Log("NameList Count: " + nameList.Count.ToString());
        //monster.name = nameList[Random.Range(0, nameList.Count)];
	monster.name = "Unnamed Monster";
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

    public ReadOnlyCollection<HealthUser> GetMembers() {
	List<HealthUser> members = new List<HealthUser>(monsters);
	if(player != null)
	    members.Insert(0,player);
	return members.AsReadOnly();
    }

    //public virtual void MonsterAttacked(Monster monster,Monster monsterEnemy)
    public virtual void MonsterAttacked(AttackInstanceInfo info)
    {
	// this group begins attack on enemy group
	//Debug.Log(monster.name + " IS GETTING ATTACKED!");
	Attack(info.attacker);
    }


    //public virtual void MonsterDeath(Monster monster,Monster monsterEnemy)
    public virtual void MonsterDeath(AttackInstanceInfo info)
    {
	Monster monster = info.defender.GetComponent<Monster>();
	if(monster == null){
	    return;
	}
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

	if (enemyGroup != null){
	    if (enemyGroup.Count == 0) {
		//Debug.Log(monster.name + ": ENEMY COUNT AT 0!!!");

		inCombat = false;
		enemyGroup = null;
		Follow(followTarget);

	    } else {
		//will change states in monster script//monster.Follow(transform);

		//Monster enemyFound = monster.ChooseEnemy(enemyGroup.GetMonsters());
		HealthUser enemyFound = monster.ChooseEnemy(enemyGroup.GetMembers());

	    }
	}else{
	    Follow(followTarget); // temp solution, pls fix
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

    public void SetFollowTarget(Transform t){
	followTarget = t;
    }

    public void ClearFollowTarget(){
	followTarget = transform;
    }
}
