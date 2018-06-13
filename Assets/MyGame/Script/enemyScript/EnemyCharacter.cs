using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : MonoBehaviour {

    private Rigidbody rigid;
    private Animator anim;

    public float moveSpeed;
	// Use this for initialization
	void Start () {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void onUpdateFollowState(GameObject player)
    {
        Debug.Log("跟随状态中");
        anim.SetFloat("Speed", 1);

        Vector3 rota = Vector3.Lerp(transform.position, player.transform.position, Time.deltaTime);
        transform.LookAt(rota);
        rigid.velocity = (player.transform.position - transform.position).normalized * moveSpeed;
    }

    public void onUpdatePatrolState(GameObject player)
    {
        Debug.Log("巡逻状态中");
        anim.SetFloat("Speed", 1);
        Vector3 rota = Vector3.Lerp(transform.position, Vector3.zero, Time.deltaTime);
        transform.LookAt(rota);
        rigid.velocity = (Vector3.zero - transform.position).normalized * moveSpeed / 2;
    }
}
