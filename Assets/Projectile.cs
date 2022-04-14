using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 destination;
    
    private void Awake() {
        GetComponent<RectTransform>().SetParent(FindObjectOfType<Canvas>().transform);
        Vector3 vec = GetComponent<RectTransform>().localPosition;
        GetComponent<RectTransform>().localPosition = new Vector3(vec.x, vec.y, -50);
        GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
    }
    
    void Update()
    { 
        GetComponent<RectTransform>().localPosition = Vector3.Lerp(GetComponent<RectTransform>().localPosition, destination, .000005f);
    }
}
