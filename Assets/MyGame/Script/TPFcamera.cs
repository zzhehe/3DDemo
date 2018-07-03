using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPFcamera : MonoBehaviour {

    public Transform player;
    //屏幕上中心点相对于摄像机的offset
    public Vector3 pivotOffset = new Vector3(1.0f, 1.0f, 1.0f);
    //摄像头到人物位置的offset
    public Vector3 camOffset = new Vector3(1.0f, 1.0f, 1.0f);
    //摄像机的响应速度
    public float smooth = 50f;
    //水平旋转速度
    public float HorizontalAimingSpeed = 400f;
    //垂直旋转速度
    public float verticalAimingSpeed = 400f;
    //最大的垂直角度
    public float maxVerticalAngle = 60f;
    //最小的垂直角度
    public float minVerticalAngle = -60f;

    //存储摄像机到鼠标的水平角度
    private float angleH = 0;
    //存储摄像机到鼠标的垂直角度    
    private float angleV = 0;
    //摄像机
    private Transform cam;
    //现在摄像机和人物关系的坐标点
    private Vector3 relCameraPos;
    //摄像机离Player的距离
    private float disCameraToPlayer;
    //摄像机的中心点正在插值时的offset
    public Vector3 smoothPivotOffset;
    //摄像机正在插值时的offset
    public Vector3 smoothCamOffset;
    //目标点摄像机到中心点的offset
    private Vector3 targetPivotOffset;
    //目标点摄像机到人物的offset
    private Vector3 targetCamOffset;
    //默认摄像机FOV
    private float defaultFOV;
    //目标摄像机FOV
    private float targetFOV;
    //custom定制摄像机最大垂直角度
    private float targetMaxVerticalAngle;
    //有没有在抖动
    private bool InShake;

    private void Start()
    {
        cam = this.transform;
        //摄像机初始位置，四元数乘以向量相当于把向量按照x,y,z轴旋转
        cam.position = player.position + Quaternion.identity * pivotOffset + Quaternion.identity * camOffset;
        //初始化旋转位置相当于没有旋转
        cam.rotation = Quaternion.identity;
        //摄像机坐标和player的关系，用作碰撞测试
        relCameraPos = transform.position - player.position;
        disCameraToPlayer = relCameraPos.magnitude;
        //设置引用默认值
        smoothPivotOffset = pivotOffset;
        smoothCamOffset = camOffset;

        defaultFOV = cam.GetComponent<Camera>().fieldOfView;

        ResetTargetOffsets();
        ResetFOV();
        ResetMaxVerticalAngle();

    }

    public void ResetTargetOffsets()
    {
        targetPivotOffset = pivotOffset;
        targetCamOffset = camOffset;
    }

    public void ResetFOV()
    {
        this.targetFOV = defaultFOV;
    }

    public void ResetMaxVerticalAngle()
    {
        this.targetMaxVerticalAngle = maxVerticalAngle;
    }

    private void LateUpdate()
    {
        if (InShake)
        {
            return;
        }
        //鼠标滑动的轨道
        angleH += Mathf.Clamp(Input.GetAxis("Mouse X"), -1, 1) * HorizontalAimingSpeed * Time.deltaTime;
        angleV += Mathf.Clamp(Input.GetAxis("Mouse Y"), -1, 1) * verticalAimingSpeed * Time.deltaTime;

        //限制垂直旋转角度
        angleV = Mathf.Clamp(angleV, minVerticalAngle, targetMaxVerticalAngle);
        //沿Y轴旋转的角度，也就是水平旋转
        Quaternion camYRotation = Quaternion.Euler(0, angleH, 0);
        Quaternion aimRotation = Quaternion.Euler(-angleV, angleH, 0);
        cam.rotation = aimRotation;//这里摄像头开始旋转到相应角度

        //设置FOV,取当前FOV和目标FOV的插值
        //cam.GetComponent<Camera>().fieldOfView = Mathf.Lerp(cam.GetComponent<Camera>().fieldOfView, targetFOV, Time.deltaTime);

        //测试当前摄像机坐标会不会和环境碰撞
        Vector3 baseTempPosition = player.position + camYRotation * targetPivotOffset;
        Vector3 noCollisionOffset = targetCamOffset;

        float camDistance = cam.GetComponent<Camera>().nearClipPlane;
        float FOV = cam.GetComponent<Camera>().fieldOfView * 0.5f * Mathf.Deg2Rad;//最后角度转化为弧度
        float aspect = cam.GetComponent<Camera>().aspect;

        float height = camDistance * Mathf.Tan(FOV); //计算并返回以弧度为单位 f 指定角度的正切值。 
        float width = height * aspect;

        Vector3 downPoint = noCollisionOffset + new Vector3(0, -height, camDistance);
        for (float zOffset = noCollisionOffset.z; zOffset <= 1f; zOffset += 0.02f)
        {
            if (DoubleViewingPosCheck(baseTempPosition + aimRotation * downPoint, Mathf.Abs(zOffset)) || zOffset == 1f)
            {
                break;
            }
            noCollisionOffset.z = zOffset;
            downPoint.z = zOffset;
        }

        Vector3 upPoint = noCollisionOffset + new Vector3(0, height, camDistance);
        for (float zOffset = noCollisionOffset.z; zOffset <= 1f; zOffset += 0.02f)
        {
            if (DoubleViewingPosCheck(baseTempPosition + aimRotation * upPoint, Mathf.Abs(zOffset)) || zOffset == 1f)
            {
                break;
            }
            noCollisionOffset.z = zOffset;
            upPoint.z = zOffset;
        }
        
        Vector3 rightPoint = noCollisionOffset + new Vector3(width, 0, camDistance);
        for (float zOffset = noCollisionOffset.z; zOffset <= 1f; zOffset += 0.02f)
        {
            if (DoubleViewingPosCheck(baseTempPosition + aimRotation * rightPoint, Mathf.Abs(zOffset)) || zOffset == 1f)
            {
                break;
            }
            noCollisionOffset.z = zOffset;
            rightPoint.z = zOffset;
        }

        Vector3 leftPoint = noCollisionOffset + new Vector3(-width, 0, camDistance);
        for (float zOffset = noCollisionOffset.z; zOffset <= 1f; zOffset += 0.02f)
        {
            if (DoubleViewingPosCheck(baseTempPosition + aimRotation * leftPoint, Mathf.Abs(zOffset)) || zOffset == 1f)
            {
                break;
            }
            noCollisionOffset.z = zOffset;
            leftPoint.z = zOffset;
        }

        //for (float zOffset = targetCamOffset.z; zOffset <= 0; zOffset += 0.3f)
        //{
        //    noCollisionOffset.z = zOffset - 0.3f;
        //    if (DoubleViewingPosCheck(baseTempPosition + aimRotation * noCollisionOffset, Mathf.Abs(zOffset)) || zOffset == 1f)
        //    {
        //        break;
        //    }
        //}
        //改变摄像机的位置,当有瞄准操作改变pivotoffset和camoffset时。smoothPivotOffset会改变
        smoothPivotOffset = Vector3.Lerp(smoothPivotOffset, targetPivotOffset, smooth * Time.deltaTime);
        smoothCamOffset = Vector3.Lerp(smoothCamOffset, noCollisionOffset, smooth * Time.deltaTime);
        cam.position = player.position + camYRotation * smoothPivotOffset + aimRotation * smoothCamOffset;

    }

    public void SetTargetOffsets(Vector3 newPivotOffset, Vector3 newCamOffset)
    {
        targetPivotOffset = newPivotOffset;
        targetCamOffset = newCamOffset;
    }

    //设置摄像机的高度
    public void SetYCamOffset(float y)
    {
        targetCamOffset.y = y;
    }

    public void SetFOV(float customFOV)
    {
        this.targetFOV = customFOV;
    }

    public void ResetMaxVerticalAngle(float angle)
    {
        this.targetMaxVerticalAngle = angle;
    }

    bool DoubleViewingPosCheck(Vector3 checkPos, float Offset)
    {
        //人物胶囊体的顶点高度
        float playerFocusHeight = player.GetComponent<CapsuleCollider>().height * 0.5f;

        return ViewingPosCheck(checkPos, playerFocusHeight) && ReverseViewingPosCheck(checkPos, playerFocusHeight, Offset) && ViewingPosCheck(checkPos, 0) && ReverseViewingPosCheck(checkPos, 0, Offset);
    }

    bool ViewingPosCheck(Vector3 checkPos, float deltaPlayerHeight)
    {
        RaycastHit hit;

        // 如果从检查点发出的射线到player之间碰撞到东西
        if (Physics.Raycast(checkPos, player.position + (Vector3.up * deltaPlayerHeight) - checkPos, out hit, disCameraToPlayer))
        {
            Debug.DrawLine(checkPos, hit.point, Color.red);
            // 射到的位置不是player
            if (hit.transform != player && !hit.transform.GetComponent<Collider>().isTrigger)
            {
                // This position isn't appropriate.
                return false;
            }
        }
        // If we haven't hit anything or we've hit the player, this is an appropriate position.
        return true;
    }

    bool ReverseViewingPosCheck(Vector3 checkPos, float deltaPlayerHeight, float maxDistance)
    {
        RaycastHit hit;

        if (Physics.Raycast(player.position + (Vector3.up * deltaPlayerHeight), checkPos - player.position, out hit, maxDistance))
        {
            if (hit.transform != player && hit.transform != transform && !hit.transform.GetComponent<Collider>().isTrigger)
            {
                return false;
            }
        }
        return true;
    }

    //得到摄像机轴的长度
    //public float getCurrentPivotMagnitude(Vector3 finalPivotOffset)
    //{
    //    return Mathf.Abs((finalPivotOffset - smoothPivotOffset).magnitude);
    //}

    //摄像机抖动
    public IEnumerator ShakeCamera(float shakeStrength = 0.2f, float rate = 1, float shakeTime = 0.2f)
    {
        float t = 0;
        float speed = 1 / shakeTime;
        Vector3 orgPosition = transform.localPosition;
        InShake = true;
        while (t < 1)
        {
            t += Time.deltaTime * speed;
            transform.position = orgPosition + new Vector3(Random.Range(0, rate), Random.Range(0, rate), 0) * Mathf.Lerp(shakeStrength, 0, t);
            yield return null;
        }
        transform.position = orgPosition;
        InShake = false;
    }
    
}
