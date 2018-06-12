using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
    public Transform TargetTrans;
    //
    public Vector3 startVelocity;
    //当前初始的速度
    public Vector3 velocity = Vector3.forward;
    //物体的质量，这个计算的是加速度所以要用计算出来的力sumForce除以质量mass
    public float mass = 1f;

    //所有加在物体上的综合的力
    public Vector3 sumForce = Vector3.zero;
    //分离物体的力
    public Vector3 separationForce = Vector3.zero;
    //队列中的力
    public Vector3 alignmentForce = Vector3.zero;
    //使物体聚集的力
    public Vector3 cohesionForce = Vector3.zero;

    //需要分离的距离
    public float separationDistance = 1.0f;
    //需要形成队列物体的距离
    public float alignmentDistance = 6.0f;
    //需要聚集的距离
    public float cohesionDistance = 6.0f;

    //分离物体的力的权重
    public float separationWeight = 1f;
    //队列中的力的权重
    public float alignmentWeight = 1f;
    //使物体聚集的力的权重
    public float cohesionWeight = 1f;

    //检查的间隔
    public float checkInterval = 0.2f;
    //存放需要分离的物体
    public List<GameObject> separationNeighbors = new List<GameObject>();
    //存放需要队列的物体
    public List<GameObject> alignmentNeighbors = new List<GameObject>();
    //存放需要聚集的物体
    public List<GameObject> cohesionNeighbors = new List<GameObject>();

    // Use this for initialization
    void Start () {
        InvokeRepeating("calcForce", 0, checkInterval);
        startVelocity = velocity;
	}
	
	// Update is called once per frame
	void Update () {
        //加速度
        Vector3 a = sumForce / mass;
        
        velocity += a * Time.deltaTime;
        //把物体朝向方向变为速度方向
        Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(velocity), Time.deltaTime);
        transform.Translate(transform.forward * Time.deltaTime, Space.World);

	}

    /// <summary>
    /// 计算物体的力
    /// </summary>
    void calcForce()
    {
        //先清空
        //分离物体的力
        separationForce = Vector3.zero;
        //队列中的力
        alignmentForce = Vector3.zero;
        //使物体聚集的力
        cohesionForce = Vector3.zero;
        separationNeighbors.Clear();

        #region 计算分离的力

        //检查物体周围的物体
        Collider[] colliders = Physics.OverlapSphere(transform.position, separationDistance);

        foreach (var c in colliders)
        {
            if (c != null && c.gameObject != this.gameObject)
            {
                separationNeighbors.Add(c.gameObject);
            }
        }
        //计算分离的力
        foreach (var neighbor in separationNeighbors)
        {
            Vector3 dir = transform.position - neighbor.transform.position;
            //力的单位方向向量除以距离，越远施加的力就越小，所以与距离成反比
            separationForce += dir.normalized / dir.magnitude;
        }

        if (separationNeighbors.Count > 0)
        {
            //计算权重
            separationForce *= separationWeight;
            //力的合力
            sumForce += separationForce;
        }

        #endregion

        #region 计算队列的力
        //计算队列的力
        colliders = Physics.OverlapSphere(transform.position, alignmentDistance);
        foreach (var c in colliders)
        {
            if (c != null && c.gameObject != this.gameObject)
            {
                alignmentNeighbors.Add(c.gameObject);
            }
        }
        //计算队列想要走向的方向，为周围物体走的方向的平均值，开始朝向为零向量
        Vector3 avgDir = Vector3.zero;
        foreach (var neighbor in alignmentNeighbors)
        {
            avgDir += neighbor.transform.position;
        }

        if (alignmentNeighbors.Count > 0)
        {
            //除法注意分母不能为零
            avgDir /= alignmentNeighbors.Count;
            alignmentForce = avgDir - transform.forward;//要施加力的方向
            alignmentForce *= alignmentWeight;
            //力的合力
            sumForce += alignmentForce;
        }
        #endregion

        #region 计算聚集的力
        if (alignmentNeighbors.Count > 0)
        {
            //聚集力的方向是指向几个物体中间的质心
            Vector3 center = Vector3.zero;//先置零
            foreach (var neighbor in alignmentNeighbors)
            {
                center += neighbor.transform.position;
            }
            center /= alignmentNeighbors.Count;
            Vector3 cohesiondir = center - transform.position;
            cohesionForce += cohesiondir.normalized * velocity.magnitude;
            cohesionForce *= cohesionWeight;
            sumForce += cohesionForce;
        }

        #endregion

        //保持恒定飞行速度的力,有目标则不需要这个力
        //Vector3 engineForce = velocity.normalized * startVelocity.magnitude;//保持恒定力的方向
        //sumForce += engineForce;


        //朝某一个目标移动
        Vector3 targetDir = TargetTrans.position - transform.position;
        sumForce += (targetDir.normalized - transform.forward);//*speed
    }
}
