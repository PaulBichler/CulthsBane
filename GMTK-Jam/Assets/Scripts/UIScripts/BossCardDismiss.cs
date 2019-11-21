using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCardDismiss : MonoBehaviour
{
    public GameObject bossCard;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        Destroy(bossCard);
    }
}
