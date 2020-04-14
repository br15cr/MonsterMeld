using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public HealthUser owner;

    private RectTransform bar;
    private RectTransform barBack;

    private float backWidth;
    
    void Start()
    {
        bar = transform.Find("Healthback/Health") as RectTransform;
	barBack = bar.parent as RectTransform;
	bar.sizeDelta = new Vector2(0,bar.sizeDelta.y);
	backWidth = barBack.sizeDelta.x;
    }

    void Update()
    {
        bar.sizeDelta = new Vector2(backWidth*owner.GetHealth().GetHealthFloat(),bar.sizeDelta.y);
    }
}
