using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {
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
    static int backState = Animator.StringToHash("Base Layer.Back");
    static int restState = Animator.StringToHash("Base Layer.Rest");
    //现在的动画状态
    private AnimatorStateInfo currentBaseState;
    private Rigidbody rbody;
    public GameObject cam;
    private Animator anim;

    // Use this for initialization
    void Start () {
        cam = GameObject.FindWithTag("MainCamera");
        anim = GetComponent<Animator>();
        rbody = GetComponent<Rigidbody>();

        lastHorizontal = 0;
    }
	
	// Update is called once per frame
	void Update () {



        //currentBaseState = anim.GetCurrentAnimatorStateInfo(0);	
    }

    public void FixedUpdate()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        anim.SetFloat("Speed", v);
        h = Mathf.Lerp(lastHorizontal, h, Time.deltaTime * turnSpeed);//插值使动画过度平缓
        anim.SetFloat("Direction", h);
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

        //左右旋转用摄像头控制，总是转向屏幕中间位置
        Rotating(h, v);
    }
    //在屏幕中间画一个点
    //void OnGUI()
    //{
    //    float mag = cam.getCurrentPivotMagnitude(aimPivotOffset);
    //    if (mag < 0.05f)
    //        GUI.DrawTexture(new Rect(Screen.width / 2 - (crosshair.width * 0.5f),
    //                                 Screen.height / 2 - (crosshair.height * 0.5f),
    //                                 crosshair.width, crosshair.height), crosshair);
    //}

    Vector3 Rotating(float horizontal, float vertical)
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
        if (vertical < 0.5)
        {
            targetDirection = forward;
        }
        else
        {
            targetDirection = forward * vertical + right * horizontal;
        }

        // Lerp current direction to calculated target direction.
        //if (targetDirection != Vector3.zero)
        if(Mathf.Abs(horizontal) > 0.9 || Mathf.Abs(vertical) > 0.9)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            Quaternion newRotation = Quaternion.Slerp(rbody.rotation, targetRotation, turnSmoothing * Time.fixedDeltaTime);
            rbody.MoveRotation(newRotation);
            SetLastDirection(targetDirection);
        }
        // If idle, Ignore current camera facing and consider last moving direction.
        if (!(Mathf.Abs(horizontal) > 0.9 || Mathf.Abs(vertical) > 0.9))
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
    
}
