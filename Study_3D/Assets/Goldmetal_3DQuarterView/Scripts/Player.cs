using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed; 
    float hAxis;
    float vAxis;
    bool wDown;

    Vector3 moveVec;

    Animator animator;

    private void Awake()
    {
        // 자식 오브젝트의 컴포넌트 중 Animator를 할당
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        // 플레이어 이동
        #region
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        // left shift를 눌러서 Walk가 활성화 된다면 1을 반환
        wDown = Input.GetButton("Walk");

        // hAxis와 vAxis로 새로운 벡터를 만들고 normalized해준다
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        // 오브젝트의 position값에 moveVec과 속, deltaTime을 곱한 값을 더해준다
        // 걷기일때에는 0.3을 곱한다
        transform.position += moveVec * speed * (wDown ? 0.3f : 1.0f) * Time.deltaTime;

        // moveVec이 zero가 아니라면 animation "isRun"을 실행한다
        animator.SetBool("isRun", moveVec != Vector3.zero);
        // wDown이 true라면 animation "isWalk"를 실행한다
        animator.SetBool("isWalk", wDown);

        // 캐릭터가 나아가는 방향을 바라봄
        transform.LookAt(transform.position + moveVec);
        #endregion
    }
}
