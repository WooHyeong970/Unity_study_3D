using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public GameObject[] weapons;
    public bool[] hasWeapons;
    float dSpeed; // 회피할 때 스피드
    float offsetSpeed; // 원래 스피드

    float hAxis;
    float vAxis;
    bool wDown;
    bool jDown;
    bool iDown;
    bool sDown1;
    bool sDown2;
    bool sDown3;

    bool isJump;
    bool isDodge;
    bool isSwap;

    Vector3 moveVec;
    // 점프 혹은 dodge를 할 때의 방향벡터
    Vector3 dodgeVector;

    Rigidbody rigid;
    Animator animator;

    GameObject nearObject;
    GameObject equipWeapon;

    private void Awake()
    {
        // 자식 오브젝트의 컴포넌트 중 Animator를 할당
        animator = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();

        offsetSpeed = speed;
        dSpeed = speed * 3;
    }

    private void Update()
    {
        GetInput();
        Move();
        Turn();
        Jump();
        Dodge();
        Swap();
        Interaction();
    }

    // 플레이어 Movement
    #region
    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk");
        jDown = Input.GetButtonDown("Jump");
        iDown = Input.GetButtonDown("Interaction");
        sDown1 = Input.GetButtonDown("Swap1");
        sDown2 = Input.GetButtonDown("Swap2");
        sDown3 = Input.GetButtonDown("Swap3");
    }

    void Move()
    {
        // hAxis와 vAxis로 새로운 벡터를 만들고 normalized해준다
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        if(isDodge) // 만약 지금 회피중이라면
        {
            // moveVec은 무조건 dodgeVector방향으로 전환
            moveVec = dodgeVector;
        }


        // ** 참고 **
        //if(isDodge)
        //{
        //    moveVec = new Vector3(hAxis, 0, vAxis);
        //}


        // 오브젝트의 position값에 moveVec과 속, deltaTime을 곱한 값을 더해준다
        // 걷기일때에는 0.3을 곱한다
        transform.position += moveVec * speed * (wDown ? 0.3f : 1.0f) * Time.deltaTime;

        // moveVec이 zero가 아니라면 animation "isRun"을 실행한다
        animator.SetBool("isRun", moveVec != Vector3.zero);

        // wDown이 true라면 animation "isWalk"를 실행한다
        animator.SetBool("isWalk", wDown);
    }

    void Turn()
    {
        // 캐릭터가 나아가는 방향을 바라봄
        transform.LookAt(transform.position + moveVec);
    }

    void Jump()
    {
        // 점프키를 눌렀고 점프상태가 아니라면
        if(jDown && !isJump && !isDodge && moveVec == Vector3.zero)
        {
            // Vector.up방향으로 AddForce시키고
            rigid.AddForce(Vector3.up * 15, ForceMode.Impulse);

            // isJump animation실행
            animator.SetBool("isJump", true);
            animator.SetTrigger("doJump");

            // 연속 점프 방지를 위한 변수
            isJump = true;
        }
    }

    void Dodge()
    {
        if (jDown && !isJump && !isDodge && moveVec != Vector3.zero)
        {
            // 닷지할 때 방향을 dodgeVector에 저장
            dodgeVector = moveVec;
            // 속도만 3배로
            speed = dSpeed;
            animator.SetTrigger("doDodge");
            isDodge = true;

            Invoke("DodgeOut", 0.4f);
        }
    }

    void Swap()
    {
        int weaponIndex = -1;
        if (sDown1)
        {
            if (hasWeapons[0] == false) return;
            weaponIndex = 0;
        }
        if (sDown2)
        {
            if (hasWeapons[1] == false) return;
            weaponIndex = 1;
        }
        if (sDown3)
        {
            if (hasWeapons[2] == false) return;
            weaponIndex = 2;
        }
        if ((sDown1 || sDown2 || sDown3) && !isJump && !isDodge)
        {
            if(equipWeapon != null)
                equipWeapon.SetActive(false);
            equipWeapon = weapons[weaponIndex];
            equipWeapon.SetActive(true);

            animator.SetTrigger("doSwap");
            isSwap = true;

            Invoke("SwapOut", 0.4f);
        }
    }

    void Interaction()
    {
        if(iDown && nearObject != null && !isJump && !isDodge)
        {
            if(nearObject.tag == "Weapon")
            {
                Item item = nearObject.GetComponent<Item>();
                int weaponIndex = item.value;
                hasWeapons[weaponIndex] = true;

                Destroy(nearObject);
            }
        }
    }
    #endregion

    private void OnCollisionEnter(Collision collision)
    {
        // 충돌한 오브젝트의 tag가 Floor라면
        if(collision.gameObject.tag == "Floor")
        {
            // 점프 animation을 중지시키고 isJump는 false
            animator.SetBool("isJump", false);
            isJump = false;
        }
    }

    void DodgeOut()
    {
        speed = offsetSpeed;
        isDodge = false;
    }

    void SwapOut()
    {
        isSwap = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Weapon")
        {
            nearObject = other.gameObject;
        }

        Debug.Log(nearObject.name);
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Weapon")
        {
            nearObject = null;
        }
    }
}
