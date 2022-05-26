using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 캐릭터를 따라다니는 카메라 이동
public class Follow : MonoBehaviour
{
    // 따라다닐 대상의 Transfrom값
    public Transform target;
    // 어느정도의 거리에서 바라볼 것인지
    public Vector3 offset;

    void Update()
    {
        // 카메라의 position값에 target의 position과 설정해둔 offset을 더해서 할당한다
        transform.position = target.position + offset;
    }
}
