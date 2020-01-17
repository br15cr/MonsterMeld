using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController body;
    private MonsterGroup playerMonsters;
    private OrbPouch orbs;
    private FusionBox fusionBox;

    private Vector3 velocity;

    private FusionBox box;

    public Transform backbox;

    public float speed = 1;

    public GameObject fusionBoxPrefab;
    private bool jumped = false;
    // Start is called before the first frame update
    void Start()
    {
	//backbox = transform.Find("tuffHDModel/metarig/spin/spine.001/Cube");
        body = GetComponent<CharacterController>();
        playerMonsters = GetComponent<MonsterGroup>();
	orbs = GetComponent<OrbPouch>();
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
	transform.rotation = Quaternion.LookRotation(moveDirection*-1);
	/*
        float div = moveDirection.x / moveDirection.y;
        float angle = Mathf.Atan(div) * Mathf.Rad2Deg;
        if (moveDirection.y < 0)
        {
            angle -= 180;
        }
        transform.rotation = Quaternion.Euler(0, angle, 0);
	*/
    }

  // bool OnGround(){
  //   return Physics.Raycast(transform.position + body.height/2
  // }

    void FixedUpdate(){
	if(!OnGround() || jumped){
	  velocity += Vector3.down*0.01f;
	  if(jumped)
	      jumped = false;
      //velocity = Vector3.down*0.25f;
	}else{
	    velocity = Vector3.zero;
	}
	body.Move(velocity);
    }

    public void Jump(){
	if(OnGround()){
	    velocity = Vector3.up*0.25f;
	    jumped = true;
	}
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
        //Debug.Log("ATTACK");
        Collider[] cols = Physics.OverlapSphere(transform.position, 10);
        //Debug.Log("Found Colliders: " + cols.Length.ToString());
        for(int i = 0; i < cols.Length; i++)
        {
            //Debug.Log("Name: " + cols[i].name);
            Monster monst = cols[i].GetComponent<Monster>();
            if(monst != null)
            {
                if(monst.GetGroup() != playerMonsters)
                {
                    //Debug.Log("found enemy");
                    //playerMonsters.Follow(cols[i].transform);
                    playerMonsters.Attack(monst);
                    break; // for now
                }
            }
        }
    }

    private bool OnGround(){
	RaycastHit hit;
	if(Physics.Raycast(transform.position + -Vector3.up,Vector3.down,out hit,0.1f)){
	    return true;
	}
	return false;
    }

    public bool CanMakeRecipe(Recipe rec){
	foreach(RecipeIngredient i in rec.ingredients){
	    if(!HasIngredient(i)){
		return false;
	    }
	}
	return true;
    }

    public void TakeRecipe(Recipe rec){
	foreach(RecipeIngredient i in rec.ingredients){
	    TakeIngredient(i);
	}
    }

    public bool HasIngredient(RecipeIngredient ing){
	if(ing.item == RecipeItem.ORB){
	    return orbs.Count >= ing.amount;
	}
	return false;
    }

    public void TakeIngredient(RecipeIngredient ing){
	if(ing.item == RecipeItem.ORB){
	    orbs.TakeOrbs(ing.amount);
	}
    }

    public void GrabDropBox(){
	if(fusionBox == null){
	    // Drop (Spawn) FusionBox
	    // hide backbox
	    // assign box to fusionBox;
	    RaycastHit hit;
	    if(Physics.Raycast(transform.position + -transform.forward*2,-transform.up,out hit,100)){
		fusionBox = GameObject.Instantiate(fusionBoxPrefab,hit.point,Quaternion.identity).GetComponent<FusionBox>();
		fusionBox.player = this;
		backbox.gameObject.SetActive(false);
	    }
	}else{
	    // Grab FusionBox if close enough and facing it
	    // if you grabbed it, show backbox
	    // fusionBox become null
	    /*
	    Collider[] cols = Physics.OverlapSphere(transform.position,3);
	    Debug.Log("Found "+ cols.Length.ToString() + " things");
	    for(int i = 0; i < cols.Length;i++){
		Debug.Log("\t"+cols[i].gameObject.name+" == "+fusionBox.gameObject.name);
		if(cols[i].transform.parent == null)
		    continue;
		if(cols[i].transform.parent.gameObject == fusionBox.gameObject){
		    Debug.Log("ITS IT!");
		    Destroy(fusionBox.gameObject);
		    fusionBox = null;
		    backbox.gameObject.SetActive(true);
		    break;
		}
	    }
	    */
	    if(Vector3.Distance(transform.position,fusionBox.transform.position) <= 3){
		Destroy(fusionBox.gameObject);
		fusionBox = null;
		backbox.gameObject.SetActive(true);
	    }
	}
    }

    void OnGUI(){
	GUI.Label(new Rect(10,10,100,100),"Orbs: " + orbs.Count.ToString());
	// Player Info
	GUI.Label(new Rect(10,50,100,100),"Velocity: " + velocity.ToString());
    }

  void OnDrawGizmos(){
    Gizmos.DrawRay(transform.position + -Vector3.up,Vector3.down*0.1f);
  }
}
