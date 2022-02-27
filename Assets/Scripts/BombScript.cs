using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    private Animator animator;
    bool hasAttacked = false;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasAttacked)
        {
            animator.SetTrigger("attack01");
            hasAttacked = true;
        }
    }

    public void RemoveBomb()
	{
        Destroy(gameObject);
	}
}
