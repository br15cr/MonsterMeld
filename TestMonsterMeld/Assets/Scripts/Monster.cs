using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.AI;


public enum MonsterState
{
    IDLE,
    FOLLOW,
    ATTACK,
}

public enum MonsterCombatState
{
    CHASE,      // run after the enemy
    HIT,        // deal the damage
    CHARGE,     // attack recoil (go back go hit after done)
    RECOVER     // stun if hit?
}

public struct MonsterAttackInfo
{
    public int baseDamage;
    public Monster attacker;
    // add more stuff here
    public MonsterAttackInfo(Monster Attacker, int BaseDamage) {
        baseDamage = BaseDamage;
        attacker = Attacker;
    }
}

public delegate void MonsterInfoDelegate(Monster monster);
public delegate void MonsterConflictDelegate(Monster ally, Monster enemy);

public class Monster : MonoBehaviour
{
    private const float ATTACK_DISTANCE = 2.0f;
    private const float ATTACK_DELAY = 1.0f; // change to variable
    private const bool SHOW_DEBUG_TEXT = true;

    private float angle;

    private Color teamColor;

    private TextMesh healthText; // Temporary
    private NavMeshAgent body;
    private int health = 100;

    private MonsterGroup group = null;

    private MonsterState state;
    private MonsterCombatState combatState;
    private float attackWait;

    private Transform followTarget; // owner
    private Transform enemyTarget; // attack target

    //public Transform target;

    public event MonsterConflictDelegate OnDeath;
    public event MonsterConflictDelegate OnKillTarget;
    public event MonsterConflictDelegate OnAttacked;    // this monster was attacked when it was in FOLLOW or IDLE mode

    public float minDistance = 0;

    void Start() {
        body = GetComponent<NavMeshAgent>();
        healthText = GetComponentInChildren<TextMesh>();
	healthText.gameObject.SetActive(SHOW_DEBUG_TEXT);
        UpdateText(); //healthText.text = health.ToString();
        state = MonsterState.IDLE;
        body.stoppingDistance = 2;
    }

    // Update is called once per frame
    void Update() {
        //TODO: Move to separate function
        switch (state) {
        case MonsterState.IDLE:
                break;
        case MonsterState.FOLLOW:
            if(followTarget != null){ //if (target != null) {
                    if (Vector3.Distance(transform.position, followTarget.position) > minDistance)
                    {
                        if (body.isStopped)
                            body.isStopped = false;
                        body.SetDestination(followTarget.position); //body.SetDestination(target.position);
                    }else if (!body.isStopped)
                    {
                        body.isStopped = true;
                    }
            }
            break;
            case MonsterState.ATTACK:
                if(followTarget != null){ //if (target != null) {
                    switch (combatState) {
                        case MonsterCombatState.CHASE:
                            if (Vector3.Distance(transform.position, enemyTarget.transform.position) > ATTACK_DISTANCE) { //if (Vector3.Distance(transform.position, target.transform.position) > ATTACK_DISTANCE) {
                                if (body.isStopped)
                                    body.isStopped = false;
                                body.SetDestination(enemyTarget.position);
                                //Debug.Log(enemyTarget);
                            }
                            else
                            {
                                combatState = MonsterCombatState.HIT;
                            }
                            break;
                        case MonsterCombatState.HIT:
                            HitMonster();
                            combatState = MonsterCombatState.CHARGE;
                            attackWait = Time.time;
                            break;
                        case MonsterCombatState.CHARGE:
                            if (Time.time >= attackWait + ATTACK_DELAY)
                                combatState = MonsterCombatState.HIT;
                            break;
                    }
                }
            break;
        }
        // TODO: delete this if it gets out of hand
	if(SHOW_DEBUG_TEXT)
		UpdateText();

	// Temporary Self-Healing
	if(state != MonsterState.ATTACK){
		if(health < 100){
			  health++;
		}else if(health > 100){
		      health = 100;
		}
	}
    }

    void LateUpdate() {
        if(state == MonsterState.ATTACK && combatState != MonsterCombatState.CHASE){
            if(enemyTarget != null){
	    		   /*
            		   //transform.rotation = Quaternion.LookRotation(enemyTarget.position,transform.up); // fix this
			   Vector2 vec = new Vector2(enemyTarget.position.x-transform.position.x,enemyTarget.position.z-transform.position.z);
			   
			   float div = vec.x/vec.y; //float div = vec.y/vec.x;
			   //Debug.Log("X: "+vec.x.ToString() + " Y: "+vec.y.ToString()+"Div: " + div.ToString());
			   //angle = Mathf.Atan(div);
			   angle = Mathf.Atan(div)*Mathf.Rad2Deg;
			   if(vec.y < 0){
			   	    //Debug.Log("Y < 0!!!!!!!!!!!!");
				    angle -= 180;
			   }
			   //Debug.Log("Angle: "+(angle*Mathf.Rad2Deg()))
	    		   transform.rotation = Quaternion.Euler(0,angle,0); //transform.rotation = Quaternion.Euler(0,angle*Mathf.Rad2Deg -90,0);
			   */
			   LookAt(enemyTarget.position);
	    }
        }

	// Test look at player
	if(false && state != MonsterState.ATTACK){
            		   //transform.rotation = Quaternion.LookRotation(enemyTarget.position,transform.up); // fix this
			   Vector2 vec = new Vector2(followTarget.position.x-transform.position.x,followTarget.position.z-transform.position.z);
			   
			   float div = vec.x/vec.y;
			   //Debug.Log("X: "+vec.x.ToString() + " Y: "+vec.y.ToString()+"Div: " + div.ToString());
			   angle = Mathf.Atan(div)*Mathf.Rad2Deg;
			   if(vec.y < 0){
			   	    //Debug.Log("Y < 0!!!!!!!!!!!!");
				    angle -= 180;
			   }

			   //Debug.Log("Angle: "+(angle*Mathf.Rad2Deg()))
	    		   transform.rotation = Quaternion.Euler(0,angle,0);
	    }

        healthText.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward, Camera.main.transform.up);
    }

    /// <summary>
    /// Sets the monster's color (temporary way to show teams).
    /// </summary>
    /// <param name="color">Color to set the monster to.</param>
    public void SetColor(Color color) {
        MeshRenderer mesh = GetComponentInChildren<MeshRenderer>();
        teamColor = color;
        if(mesh != null)
        {
            mesh.material.color = color;
        }
    }

    public int GetHealth() {
        return health;
    }

    public MonsterGroup GetGroup() {
        return group;
    }

    public void SetGroup(MonsterGroup group) {
        this.group = group;
        followTarget = group.transform;
    }

    public bool HasGroup() {
        return group != null;
    }

    public bool HasEnemy() {
        return enemyTarget != null;
    }

    public MonsterState GetState() {
        return state;
    }

    public MonsterCombatState GetCombatState() {
        return combatState;
    }


    public void Follow(Transform target) {
        //this.target = target;
        followTarget = target;
        state = MonsterState.FOLLOW;
    }

    /// <summary>
    ///   Picks an available enemy from enemyGroup to fight and returns it.
    /// </summary>
    public virtual Monster ChooseEnemy(ReadOnlyCollection<Monster> enemyGroup){
	    Monster bestEnemy = null; // best choice so far for an enemy;
	    float bestDist = 0;
	    Debug.Log(this.gameObject.name+" Choosing Enemy...");
	    foreach(Monster m in enemyGroup){

	        if(bestEnemy != null){
		    float dist = Vector3.Distance(transform.position,m.transform.position);
		    if(!m.HasEnemy()){
		        if(dist < bestDist){
			    bestEnemy = m;
			    bestDist = dist;
		        }
		    }
	        }else{
	    
		    if(!m.HasEnemy()){
		        bestEnemy = m;
		        bestDist = Vector3.Distance(transform.position,m.transform.position);
		    }
	        }
	    }
	    Debug.Log(this.gameObject.name + " Found Enemy: " + bestEnemy);
	    this.AttackMonster(bestEnemy);
        Debug.Log(this.gameObject.name + " Checking Enemy: " + enemyTarget);
        return bestEnemy;
    }

    
    /// <summary>
    /// Checks to see of this monster already has an enemy. If it doesn't then the 'attacker' becomes its enemy.
    /// </summary>
    /// <param name="attacker">The monster asking.</param>
    /// <returns>If the attacker became the new enemy or not</returns>
    public bool AskAttack(Monster attacker) {
        if(enemyTarget == null) {
            enemyTarget = attacker.transform;
            return true;
        }else if(enemyTarget.GetComponent<Monster>() == attacker) {
            return true;
        }
        return false;
    }

    public Monster GetEnemy() {
        if (enemyTarget == null)
            return null;
        return enemyTarget.GetComponent<Monster>();
    }

    /// <summary>
    /// Initiate combat with a monster
    /// </summary>
    /// <param name="monster">The monster to start combat with.</param>
    public void AttackMonster(Monster monster) {
        //this.target = monster.transform;
        if (monster.AskAttack(this)) {
            this.enemyTarget = monster.transform;
	    Debug.Log(this.name + " ATTACK APPROVED BY " + enemyTarget);
            state = MonsterState.ATTACK;
            combatState = MonsterCombatState.CHASE;
            enemyTarget.GetComponent<Monster>().OnDeath += TargetDeath;
            UpdateText();
        }else{
	        Debug.LogError("ATTACK DENIED!");
	    }
    }


    /// <summary>
    /// Runs when target dies.
    /// </summary>
    /// <param name="monster">Target that died, as Monster.</param>
    public void TargetDeath(Monster monster,Monster enemy) {
        if (OnKillTarget != null) {

            // if enemy target is null then it might not call this
            if (enemyTarget != null)
            {
                OnKillTarget(this, enemyTarget.GetComponent<Monster>());
            }
            else
            {
                Debug.LogError("ON TARGET KILL TO NULL TARGET!!!");
            }
        }
        combatState = MonsterCombatState.CHASE;
        enemyTarget = null;
        UpdateText();
        //state = MonsterState.IDLE; // change to fight next monster
        
    }

    /// <summary>
    /// 'Hit' a monster in combat
    /// </summary>
    /// <param name="monster">Monster to hit.</param>
    public void HitMonster() {

	    Monster monster = enemyTarget.GetComponent<Monster>();
	    // send damage data to 'monster'
	    monster.TakeDamage(new MonsterAttackInfo(this, 10));
    }

    /// <summary>
    /// Applies damage to this monster.
    /// </summary>
    /// <param name="attack">Contains damage info.</param>
    public void TakeDamage(MonsterAttackInfo attackInfo) {
        health -= attackInfo.baseDamage;
        //healthText.text = health.ToString();
        UpdateText();
        if(health <= 0) {
            OnDeath += group.MonsterDeath;
            OnDeath(this,enemyTarget.GetComponent<Monster>());  // send event to this monster group
            //OnDeath.
            Die();
        }

        if(state != MonsterState.ATTACK) {
            if(OnAttacked != null)
                OnAttacked(this,attackInfo.attacker);
            AttackMonster(attackInfo.attacker);
        }

        //Debug.Log(this.name + " Attacked! Health: " + this.health.ToString());
    }

    private void UpdateText() {
        if(healthText != null){
	        string text = gameObject.name + "\n";
	        if(health < 100)
		    text += "HP: " + health.ToString() + "\n";
	        text += "ST: ";
	        if(state == MonsterState.ATTACK)
		    text += combatState.ToString();
	        else
		    text += state.ToString();
	        text += "\n";
	        if(followTarget != null)
		    text += "FT: " +followTarget.name + " ";
	        if(enemyTarget != null)
		    text +="ET: "+ enemyTarget.name;

	        healthText.text = text;
	    }
        //healthText.text = health.ToString() + "\n" + state.ToString() + ((state == MonsterState.ATTACK) ? " " + combatState.ToString() : "") + "\nFT: " + (followTarget != null) + " AT: " + (enemyTarget != false) + " ANGLE: " + (angle*Mathf.Rad2Deg).ToString();
    }

    private void Die() {
        Destroy(this.gameObject);
    }

    private void LookAt(Vector3 targetPos){
	//transform.rotation = Quaternion.LookRotation(enemyTarget.position,transform.up); // fix this
	Vector2 vec = new Vector2(targetPos.x-transform.position.x,targetPos.z-transform.position.z);
			   
	float div = vec.x/vec.y; //float div = vec.y/vec.x;
	//Debug.Log("X: "+vec.x.ToString() + " Y: "+vec.y.ToString()+"Div: " + div.ToString());
	//angle = Mathf.Atan(div);
	angle = Mathf.Atan(div)*Mathf.Rad2Deg;
	if(vec.y < 0){
	    //Debug.Log("Y < 0!!!!!!!!!!!!");
	    angle -= 180;
	}
	//Debug.Log("Angle: "+(angle*Mathf.Rad2Deg()))
	transform.rotation = Quaternion.Euler(0,angle,0); //transform.rotation = Quaternion.Euler(0,angle*Mathf.Rad2Deg -90,0);
    }

    void OnDrawGizmos(){
    	if(enemyTarget != null){
            Gizmos.color = teamColor;
    	    Gizmos.DrawLine(transform.position,enemyTarget.position + transform.up * 2);
        }
    }


}
