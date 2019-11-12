using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroupHUD : MonoBehaviour
{
    private GameObject infoTemplate;
    private List<MonsterInfoPanel> panels = new List<MonsterInfoPanel>();
    public MonsterGroup group;
    
    void Start() {
	infoTemplate = Resources.Load<GameObject>("Prefabs/UI/Info Panel");
	group.OnAddMonster += MonsterAdded;
	group.OnRemoveMonster += MonsterRemoved;
	foreach(Monster m in group.GetMonsters()){
	    Debug.Log("HALLO!");
	    AddPanel(m);
	}
    }

    private void AddPanel(Monster m){
	MonsterInfoPanel panel = Instantiate(infoTemplate).GetComponent<MonsterInfoPanel>();
	panel.name = m.name + " " + panel.name;
	panel.transform.SetParent(this.transform);
	panel.SetMonster(m);
	panels.Add(panel);
    }

    private void MonsterAdded(Monster m){
	AddPanel(m);
    }

    private void MonsterRemoved(Monster m){
	Debug.Log("MONSTER REMOVED...");
	foreach(MonsterInfoPanel p in panels){
	    if(p.Monster == m){
		Debug.Log("MATCHED MONSTER");
		MonsterInfoPanel pan = p;
		panels.Remove(p);
		Destroy(pan.gameObject);
		break;
	    }
	}
    }

    void Update() {
        
    }
}
