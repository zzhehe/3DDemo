using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    //奔跑速度
    public float RunSpeed = 5.0f;
    // 後退速度
    public float backSpeed = 2.0f;
    //旋转速度
    public float rotateSpeed = 1.0f;
    //左右动画过渡的速度
    public float turnSpeed = 2.0f;
    //上一次左右移动的值
    private float lastHorizontal;
    //转向的平滑度
    public float turnSmoothing = 2.0f;
    //物体的移动向量
    private Vector3 velocity;
    //最后人物面向的方向
    private Vector3 lastDirection;
    //水平移动量（左右）
    public float h;
    //竖直移动量（前后）
    public float v;

    static int idleState = Animator.StringToHash("Base Layer.Idle");
    static int locoState = Animator.StringToHash("Base Layer.Locomotion");
    static int jumpState = Animator.StringToHash("Base Layer.Jump");
    static int restState = Animator.StringToHash("Base Layer.Rest");
    
    private Rigidbody rbody;
    public GameObject cam;
    private Animator anim;
    public float targetAttackWeight;
    private float curAttackWalkWeight;
    private float curAttackWeight;
    public bool IsInBattle;

    private void Awake()
    {
        cam = GameObject.FindWithTag("MainCamera");
        anim = GetComponent<Animator>();
        rbody = GetComponent<Rigidbody>();
    }

    // Use this for initialization
    void Start()
    {
        lastHorizontal = 0;
    }

    // Update is called once per frame
    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
    }

    public void FixedUpdate()
    {
    }

    public void Move()
    {
        anim.SetFloat("Speed", v);
        h = Mathf.Lerp(lastHorizontal, h, Time.deltaTime * turnSpeed);//插值使动画过度平缓
        anim.SetFloat("Direction", h);
        anim.Play("Locomotion");
        lastHorizontal = h;
        
        //z轴方向移动的方向向量
        velocity = new Vector3(0, 0, v);
        //从局部坐标转换到世界坐标
        velocity = transform.TransformDirection(velocity);

        if (v > 0.1)
        {
            velocity *= RunSpeed;//前进速度       
        }
        else if (v < -0.1)
        {
            velocity *= backSpeed; //后退速度
        }

        // 人物前后移动
        //transform.localPosition += velocity * Time.deltaTime;
        rbody.AddForce(velocity, ForceMode.Force);
    }
    
    public Vector3 Rotating()
    {

        //if (cam.transform.forward == transform.forward) return new Vector3(0, 0, 0);
        //摄像机的方向向量从模型坐标转成世界坐标
        Vector3 forward = cam.transform.TransformDirection(Vector3.forward);

        //把Y轴方向向量设为零，保证人物不会向上下移动
        forward.y = 0.0f;
        forward = forward.normalized;//变成单位向量

        //摄像机的右方向向量，根据方向向量和键位的移动量得出物体转向的最终方向
        Vector3 right = new Vector3(forward.z, 0, -forward.x);
        //转向的目标方向
        Vector3 targetDirection = new Vector3(0, 0, 0);
        if (v < 0.5)
        {
            targetDirection = forward;
        }
        else
        {
            targetDirection = forward * v + right * h;
        }

        // Lerp current direction to calculated target direction.
        //if (targetDirection != Vector3.zero)
        if (Mathf.Abs(h) > 0.9 || Mathf.Abs(v) > 0.9)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            Quaternion newRotation = Quaternion.Slerp(rbody.rotation, targetRotation, turnSmoothing * Time.fixedDeltaTime);
            rbody.MoveRotation(newRotation);
            SetLastDirection(targetDirection);
        }
        // If idle, Ignore current camera facing and consider last moving direction.
        if (!(Mathf.Abs(h) > 0.9 || Mathf.Abs(v) > 0.9))
        {
            Repositioning();
        }

        return targetDirection;
    }


    public void SetLastDirection(Vector3 direction)
    {
        lastDirection = direction;
    }

    // Put the player on a standing up position based on last direction faced.
    public void Repositioning()
    {
        if (lastDirection != Vector3.zero)
        {
            lastDirection.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(lastDirection);
            Quaternion newRotation = Quaternion.Slerp(rbody.rotation, targetRotation, turnSmoothing * Time.fixedDeltaTime);
            rbody.MoveRotation(newRotation);
        }
    }

    public void OnEnterRunState()
    {
        Debug.Log("进入跑步状态");
        if (IsInBattle)
        {
            targetAttackWeight = 1.0f;
            curAttackWalkWeight = anim.GetLayerWeight(anim.GetLayerIndex("AttackWalk"));
            curAttackWeight = anim.GetLayerWeight(anim.GetLayerIndex("Attack"));
        }
        else
        {
            targetAttackWeight = 0f;
            curAttackWalkWeight = anim.GetLayerWeight(anim.GetLayerIndex("AttackWalk"));
            curAttackWeight = anim.GetLayerWeight(anim.GetLayerIndex("Attack"));
        }
    }

    public void OnUpDateRunState()
    {
        if (IsInBattle)
        {
            curAttackWeight = Mathf.Lerp(curAttackWeight, 1 - targetAttackWeight, 0.1f);
            curAttackWalkWeight = Mathf.Lerp(curAttackWalkWeight, targetAttackWeight, 0.1f);
            anim.SetLayerWeight(anim.GetLayerIndex("Attack"), curAttackWeight);
            anim.SetLayerWeight(anim.GetLayerIndex("AttackWalk"), curAttackWalkWeight);
        }
        else
        {
            curAttackWeight = Mathf.Lerp(curAttackWeight, targetAttackWeight, 0.1f);
            curAttackWalkWeight = Mathf.Lerp(curAttackWalkWeight, targetAttackWeight, 0.1f);
            anim.SetLayerWeight(anim.GetLayerIndex("Attack"), curAttackWeight);
            anim.SetLayerWeight(anim.GetLayerIndex("AttackWalk"), curAttackWalkWeight);
        }
        
    }

    public void OnEnterIdleState()
    {
        Debug.Log("进入无战斗状态");
        anim.SetBool("IsJump", false);
        //anim.SetFloat("Speed", 0);
        //anim.SetFloat("Direction", 0);
        targetAttackWeight = 0f;
        curAttackWalkWeight = anim.GetLayerWeight(anim.GetLayerIndex("AttackWalk"));
        curAttackWeight = anim.GetLayerWeight(anim.GetLayerIndex("Attack"));
    }

    public void OnUpDateIdleState()
    {
        float curH = anim.GetFloat("Speed");
        float curV = anim.GetFloat("Direction");
        anim.SetFloat("Speed", Mathf.Lerp(curH, 0, 0.05f));
        anim.SetFloat("Direction", Mathf.Lerp(curV, 0, 0.05f));

        curAttackWeight = Mathf.Lerp(curAttackWeight, targetAttackWeight, 0.1f);
        curAttackWalkWeight = Mathf.Lerp(curAttackWalkWeight, targetAttackWeight, 0.1f);
        anim.SetLayerWeight(anim.GetLayerIndex("Attack"), curAttackWeight);
        anim.SetLayerWeight(anim.GetLayerIndex("AttackWalk"), curAttackWalkWeight);
    }

    public void OnEnterJumpState()
    {
        Debug.Log("进入跳跃状态");
        anim.SetBool("IsJump", true);
    }

    public void OnEnterAttackIdleState()
    {
        Debug.Log("进入攻击准备状态");
        targetAttackWeight = 1.0f;
        curAttackWalkWeight = anim.GetLayerWeight(anim.GetLayerIndex("AttackWalk"));
        curAttackWeight = anim.GetLayerWeight(anim.GetLayerIndex("Attack"));
        IsInBattle = true;
    }

    public void OnUpdateAttackIdleState()
    {
        curAttackWeight = Mathf.Lerp(curAttackWeight, targetAttackWeight, 0.1f);
        curAttackWalkWeight = Mathf.Lerp(curAttackWalkWeight, 1 - targetAttackWeight, 0.1f);
        anim.SetLayerWeight(anim.GetLayerIndex("Attack"), curAttackWeight);
        anim.SetLayerWeight(anim.GetLayerIndex("AttackWalk"), curAttackWalkWeight);
    }


    public void OnEnterAttackWalkState()
    {
        Debug.Log("进入攻击跑步状态");
        targetAttackWeight = 1.0f;
        curAttackWalkWeight = anim.GetLayerWeight(anim.GetLayerIndex("AttackWalk"));
        curAttackWeight = anim.GetLayerWeight(anim.GetLayerIndex("Attack"));
    }



    public void OnUpDateAttackWalkState()
    {
        curAttackWeight = Mathf.Lerp(curAttackWeight, 1 - targetAttackWeight, 0.1f);
        curAttackWalkWeight = Mathf.Lerp(curAttackWalkWeight, targetAttackWeight, 0.1f);
        anim.SetLayerWeight(anim.GetLayerIndex("Attack"), curAttackWeight);
        anim.SetLayerWeight(anim.GetLayerIndex("AttackWalk"), curAttackWalkWeight);
    }

    public void OnEnterAttackDoState()
    {
        Debug.Log("人物攻击了一下");
        anim.SetTrigger("Attack");
        targetAttackWeight = 1.0f;
        curAttackWalkWeight = anim.GetLayerWeight(anim.GetLayerIndex("AttackWalk"));
        curAttackWeight = anim.GetLayerWeight(anim.GetLayerIndex("Attack"));
        IsInBattle = true;
    }

    public void OnUpDateAttackDoState()
    {
        curAttackWeight = Mathf.Lerp(curAttackWeight, targetAttackWeight, 0.1f);
        curAttackWalkWeight = Mathf.Lerp(curAttackWalkWeight, 1 - targetAttackWeight, 0.1f);
        anim.SetLayerWeight(anim.GetLayerIndex("Attack"), curAttackWeight);
        anim.SetLayerWeight(anim.GetLayerIndex("AttackWalk"), curAttackWalkWeight);
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(cam.GetComponent<TPFcamera>().ShakeCamera());
            anim.SetTrigger("Attack");
        }
        
    }
}
