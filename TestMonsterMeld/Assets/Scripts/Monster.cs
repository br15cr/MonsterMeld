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
    AGRO,
}

public enum MonsterCombatState
{
    CHASE,      // run after the enemy
    HIT,        // deal the damage
    CHARGE,     // attack recoil (go back go hit after done)
    RECOVER,     // stun if hit?
    FLEE
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

public delegate void MonsterStatesDelegate(Monster monster,MonsterState state,MonsterCombatState combatState);

public class Monster : MonoBehaviour
{
    private const float ATTACK_DISTANCE = 2.0f;
    private const float ATTACK_DELAY = 1.0f; // change to variable
    private const bool CAN_AUTO_HEAL = false;
    //private const bool SHOW_DEBUG_TEXT = false;

    private float angle;

    private Color teamColor;

    //private TextMesh healthText; // Temporary
    private NavMeshAgent body;
    public int maxHealth = 100;
    private int health = 100;

    private MonsterGroup group = null;

    private MonsterState state;
    private MonsterCombatState combatState;
    private float attackWait;

    private Transform followTarget; // owner
    private Transform enemyTarget; // attack target

    private bool isDead = false;

    private Material healthbarMat;
    private Transform healthRing;

    private GameObject attackPrefab; // attackbox

    //public Transform target;

    public bool IsDead {
	get { return this.isDead; }
    }

    private SphereCollider agroSphere;

    public bool agro;

    public event MonsterConflictDelegate OnDeath;
    public event MonsterConflictDelegate OnKillTarget;
    public event MonsterConflictDelegate OnAttacked;    // this monster was attacked when it was in FOLLOW or IDLE mode
    public event MonsterStatesDelegate OnStatesChanged;

    public float minDistance = 0;

    public GameObject orbPrefab; // drops on death

    private Vector3 teleportOffset = new Vector3(5,5,0);

    void Start() {
	health = maxHealth;
        body = GetComponent<NavMeshAgent>();
	healthRing = transform.Find("HEALTH_RING");
	healthbarMat = healthRing.GetComponent<Renderer>().material;
        //healthText = GetComponentInChildren<TextMesh>();
	//healthText.gameObject.SetActive(SHOW_DEBUG_TEXT);
        //UpdateText(); //healthText.text = health.ToString();
        SetState(MonsterState.IDLE); //state = MonsterState.IDLE;
        body.stoppingDistance = 2;

	agroSphere = this.gameObject.AddComponent(typeof(SphereCollider)) as SphereCollider;
	agroSphere.radius = 2.5f;
	agroSphere.isTrigger = true;

	if(agro)
	    SetState(MonsterState.AGRO); //state = MonsterState.AGRO;

	// load attack box
	attackPrefab = Resources.Load<GameObject>("Prefabs/DamageBox");
	
	//body.enabled = false;
	body.Warp(transform.position);
    }

    // Update is called once per frame
    void Update() {
	// Run monster behaviour
        Behaviour();
	
	// TODO: delete this if it gets out of hand
	//if(SHOW_DEBUG_TEXT)
	    //UpdateText(); // Display debuging information above monster


	// HeathBar
	healthbarMat.SetFloat("_Offset",health/((float)maxHealth));

	// Temporary Self-Healing
	if(CAN_AUTO_HEAL && state != MonsterState.ATTACK){
	    if(health < maxHealth){
		health++;
	    }else if(health > maxHealth){
		health = maxHealth;
	    }
	}
//TEMPCOMMENTED OUT FOR A NULL REFERNECE ERROR 
/*	if(Vector3.Distance(this.transform.position,group.transform.position) > 30.0f){
	    transform.position = group.transform.position + teleportOffset;
	}*/
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

        //healthText.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward, Camera.main.transform.up);
	//healthRing.transform.rotation = Quaternion.LookRotation(Vector3.forward,Vector3.up);
	{
		Vector2 vec = Vector2.down;
		float div = vec.x/vec.y;
		float ang = Mathf.Atan(div)*Mathf.Rad2Deg;
		healthRing.rotation = Quaternion.Euler(0,ang-35+(healthbarMat.GetFloat("_Length")*360)/2,0);
	}
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
	OnDeath += group.MonsterDeath;
	OnKillTarget += group.MonsterKill;
	OnAttacked += group.MonsterAttacked;
    }

    public void LeaveGroup(){
	OnDeath -= group.MonsterDeath;
	OnKillTarget -= group.MonsterKill;
	OnAttacked -= group.MonsterAttacked;
	group = null;
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

    private void SetState(MonsterState newState){
	state = newState;
	if(OnStatesChanged != null)
	    OnStatesChanged(this,state,combatState);
    }

    private void SetState(MonsterCombatState newCombatState){
	state = MonsterState.ATTACK;
	combatState = newCombatState;
	if(OnStatesChanged != null)
	    OnStatesChanged(this,state,combatState);
    }

    public MonsterCombatState GetCombatState() {
        return combatState;
    }


    public void Follow(Transform target) {
        //this.target = target;
        followTarget = target;
	if(agro){
	    SetState(MonsterState.AGRO);
	}else{
	    SetState(MonsterState.FOLLOW); //state = MonsterState.FOLLOW;
	}
    }

    /// <summary>
    ///   Picks an available enemy from enemyGroup to fight and returns it.
    /// </summary>
    public virtual Monster ChooseEnemy(ReadOnlyCollection<Monster> enemyGroup){
	Monster bestEnemy = null; // best choice so far for an enemy;
	float bestDist = 0;
	Debug.Log(this.gameObject.name+" looking for enemy...");
	foreach(Monster m in enemyGroup){
	    if(!m.IsDead){
		if(bestEnemy != null){
		    float dist = Vector3.Distance(transform.position,m.transform.position);
		    if(bestEnemy.HasEnemy()){
		    
			if(!m.HasEnemy()){ // prioritize enemies that aren't alright fighting someone
			    Debug.Log("\t"+name+" would rather fight "+ m.name + " instead of " + bestEnemy.name + " because they have no enemy");
			    bestEnemy = m;
			    bestDist = dist;
			}else{
			    if(dist < bestDist){ // next prioritize close enemies
				Debug.Log("\t"+name+" would rather fight "+ m.name + " instead of " + bestEnemy.name + " because they are closer");
				bestEnemy = m;
				bestDist = dist;
			    }
			}
		    }else{
			if(!m.HasEnemy()){
			    if(dist < bestDist){
				Debug.Log("\t"+name+" would rather fight "+ m.name + " instead of " + bestEnemy.name + " because they are closer");
				bestEnemy = m;
				bestDist = dist;
			    }
			}
		    }
		    // if(!m.HasEnemy()){
		    //     if(dist < bestDist){
		    // 	bestEnemy = m;
		    // 	bestDist = dist;
		    //     }
		    // }
		}else{
		    //if(!m.HasEnemy()){
		    bestEnemy = m;
		    bestDist = Vector3.Distance(transform.position,m.transform.position);
		    Debug.Log("\t" +name + " first enemy choice is " + bestEnemy.name);
		    //}
		}
	    }
	}
	Debug.Log("\t" +name + " Found Enemy: " + bestEnemy);
	this.AttackMonster(bestEnemy);
        Debug.Log(name + " chose " + enemyTarget.name + " as their enemy");
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
	//Debug.Log("MONSTER IS NULL? " + (monster == null));
	
        if (monster.AskAttack(this) || monster.HasEnemy()) {
            this.enemyTarget = monster.transform;
	    //Debug.Log(this.name + " ATTACK APPROVED BY " + enemyTarget);
            //state = MonsterState.ATTACK;
            //combatState = MonsterCombatState.CHASE;
	    SetState(MonsterCombatState.CHASE);
            enemyTarget.GetComponent<Monster>().OnDeath += TargetDeath;
        }else{
	    Debug.LogWarning(name+"'s attack request was denied by " + monster.name);
	    //Debug.LogError("ATTACK DENIED!");
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
		//Debug.Log(name+" is setting their enemy to null");
		//enemyTarget = null;
            }
            else
            {
                //Debug.LogError("ON TARGET KILL TO NULL TARGET!!!");
            }
        }
        //combatState = MonsterCombatState.CHASE;
        //enemyTarget = null;
        //state = MonsterState.IDLE; // change to fight next monster
        
    }

    /// <summary>
    /// 'Hit' a monster in combat
    /// </summary>
    /// <param name="monster">Monster to hit.</param>
    public void HitMonster() {
	if(Vector3.Distance(transform.position,enemyTarget.position) <= ATTACK_DISTANCE){ // only apply the attack if they're close enough
	    Monster monster = enemyTarget.GetComponent<Monster>();
	    // send damage data to 'monster'
	    //monster.TakeDamage(new MonsterAttackInfo(this, 10));
	    // spawn attack box instead of directly sending damage
	    AttackBox attack = Instantiate(attackPrefab,transform.position + transform.forward*(ATTACK_DISTANCE/2),Quaternion.identity).GetComponent<AttackBox>();
	    //attack.SetAttacker(this);
	    attack.SetInfo(new MonsterAttackInfo(this,10));
	}
    }

    /// <summary>
    /// Applies damage to this monster.
    /// </summary>
    /// <param name="attack">Contains damage info.</param>
    public void TakeDamage(MonsterAttackInfo attackInfo) {
        health -= attackInfo.baseDamage;
        //healthText.text = health.ToString();
        if(health <= 0) {
	    // Should this be here???
            //OnDeath += group.MonsterDeath;
            //OnDeath(this,enemyTarget.GetComponent<Monster>());  // send event to this monster group
            //OnDeath.
	    if(attackInfo.attacker != null){
		Debug.Log("Alas poor " + name + " was killed by " +attackInfo.attacker.name);
		
	    }else{
		Debug.Log("Alas poor " + name + " was killed by natural causes (probably a status effect)");
	    }
            Die(attackInfo);
        }

        if(state != MonsterState.ATTACK && attackInfo.attacker != null && !attackInfo.attacker.IsDead) {
            AttackMonster(attackInfo.attacker);
            if(OnAttacked != null)
                OnAttacked(this,attackInfo.attacker);
        }

        //Debug.Log(this.name + " Attacked! Health: " + this.health.ToString());
    }

    /*
    private void UpdateText() {
        if(healthText != null){
	        string text = gameObject.name + "\n";
	        if(health < 100){
		    text += "HP: "; //text += "HP: " + health.ToString() + "\n";
		    for(int i = 0; i < health/10;i++){
			text += "X";
		    }
		    text += "\n";
		}
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
    */

    private void Die(MonsterAttackInfo finalBlow) {
	isDead = true;
	Debug.Log(name + " Died");
	/*
	if(enemyTarget != null){
	    OnDeath(this,enemyTarget.GetComponent<Monster>());  // send event to this monster group
	}else{
	    OnDeath(this,null);
	}*/
	OnDeath(this,finalBlow.attacker);
	Debug.Log(name + " Getting Destroyed!");
	//DropLoot();
        Destroy(this.gameObject);
	Debug.Log(name + " Destroyed?!?");
    }

    private void DropLoot(){
	for(int i = 0; i < 8; i++){
	    Orb o = GameObject.Instantiate(orbPrefab,transform.position + transform.up*1,Quaternion.identity).GetComponent<Orb>();
	    o.Jump();
	}
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

	//transform.rotation = Quaternion.Euler(0,angle,0); 
	//transform.rotation = Quaternion.Euler(0,angle*Mathf.Rad2Deg -90,0);
    }

    protected virtual void Behaviour(){
	switch (state) {
	    case MonsterState.IDLE:
		IdleBehaviour();
                break;
	    case MonsterState.FOLLOW:
		FollowBehaviour();
		break;
	case MonsterState.AGRO:
	    TenseBehaviour();
	    break;
            case MonsterState.ATTACK:
                AttackBehaviour();
		break;
        }
    }

    protected virtual void TenseBehaviour(){
	
    }


    protected virtual void IdleBehaviour(){
	
    }

    protected virtual void FollowBehaviour(){
	if(followTarget != null){ //if (target != null) {
	    if (Vector3.Distance(transform.position, followTarget.position) > minDistance)
	    {
		if (body.isStopped)
		    body.isStopped = false;
		body.SetDestination(followTarget.position); //body.SetDestination(target.position);
	    }else if (!body.isStopped) {
                        body.isStopped = true;
	    }
	}
    }

    protected virtual void ChaseBehaviour(){
	// Get close enough to enemy
	if (Vector3.Distance(transform.position, enemyTarget.transform.position) > ATTACK_DISTANCE) { //if (Vector3.Distance(transform.position, target.transform.position) > ATTACK_DISTANCE) {
			
	    if (body.isStopped)
		body.isStopped = false;
			
	    body.SetDestination(enemyTarget.position);
	    //Debug.Log(enemyTarget);
	}
	else
	    {
		// when close enough to the enemy
		SetState(MonsterCombatState.HIT);
	    }
    }

    protected virtual void HitBehaviour(){
	HitMonster();
	SetState(MonsterCombatState.CHARGE);
	// update the last time attacked
	attackWait = Time.time;
    }

    protected virtual void ChargeBehaviour(){
	if(Vector3.Distance(transform.position,enemyTarget.position) > ATTACK_DISTANCE) // if the enemy becomes too far away, chase it again
	    SetState(MonsterCombatState.CHASE);
	if (Time.time >= attackWait + ATTACK_DELAY)
	    SetState(MonsterCombatState.HIT);
	if(health <= 25)
	    SetState(MonsterCombatState.FLEE);
    }

    protected virtual void FleeBehaviour(){
	if(!group.InCombat){
	    group.Attack(enemyTarget.GetComponent<Monster>());
	}
	FollowBehaviour();
    }

    protected virtual void AttackBehaviour(){
	if(enemyTarget != null){ //if (target != null) {
	    switch (combatState) {
		// CHASE //
	    case MonsterCombatState.CHASE:
		ChaseBehaviour();
		    break;
		    // HIT //
		case MonsterCombatState.HIT:
		    HitBehaviour();
		    break;
		    // RECHARGE
		case MonsterCombatState.CHARGE:
		    ChargeBehaviour();
		    break;
		case MonsterCombatState.FLEE:
		    FleeBehaviour();
		    //FollowBehaviour();
		    break;
	    }
	}else{
	    Debug.LogWarning(name + " HAS NO ONE TO ATTACK");
	    // Follow(followTarget);
	    // move to attack wait for new target
	    // wait for MonsterGroup to assign enemy
	}
    }

    void OnDrawGizmos(){
    	if(enemyTarget != null){
            Gizmos.color = teamColor;
    	    Gizmos.DrawLine(transform.position,enemyTarget.position + transform.up * 2);
	    Gizmos.DrawLine(enemyTarget.position + transform.up*2,enemyTarget.position);
        }
    }

    void OnTriggerEnter(Collider c){
	if(state == MonsterState.AGRO){
	    // attack enemy
	    Monster m = c.gameObject.GetComponent<Monster>();
	    if(m != null){
		if(m.GetGroup() != this.group){
		    Debug.Log("AGROED MONSTER");
		    group.Attack(m);
		    AttackMonster(m);
		    Debug.Log(group.InCombat);
		}
	    }
	}
    }

    public void DebugPosition(){
	Debug.Log(this.gameObject.name+" Spawned at " + transform.position.ToString());
    }

    public void Warp(Vector3 pos){
	body.Warp(pos);
    }
}
