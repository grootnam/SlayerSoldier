using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // for mouse move
    float AngleX, AngleY;

    // for act
    bool canJump;
    bool isReloading;

    // for shooting
    public int leftBulletNum;
    RaycastHit ray;
    float shootingCool;
    public GameObject muzzleFlash;

    // for animation
    Animator armAnimator;

    /*
     * character ability
     */
    public float PlayerHP;                      //체력
    public float PlayerSpeed;                   //이동속도
    public float PlayerAttackSpeed;             //공격속도
    public float PlayerAttackPower;             //공격력
    public float PlayerReloadSpeed;             //장전속도
    public float PlayerGunRebound;              //총기반동
    public float PlayerGunReboundRecover;       //총기반동회복
    public int maxBulletNum;                    //장전 탄 수

    private void Start()
    {
        // 마우스 커서 숨김 lc ghkaus qkRdmfh dksskrkrp
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        armAnimator = GameObject.Find("Arm").GetComponent<Animator>();
        isReloading = false;
        canJump = true;
    }

    void Update()
    {
        Jump();
        Reload();
        coolTime();
    }

    private void FixedUpdate()
    {
        WASD();
        MouseMove();
        Shoot();
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.transform.tag == "ground")
        {
            canJump = false;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.tag == "ground")
        {
            canJump = true;
        }
    }




    /* 플레이어 움직임에 관한 코드
     */
    void WASD()
    {
        Vector3 move = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W))
        {
            move += gameObject.transform.forward;
        }

        if (Input.GetKey(KeyCode.A))
        {
            move -= gameObject.transform.right;
        }
        if (Input.GetKey(KeyCode.S))
        {
            move -= gameObject.transform.forward;
        }
        if (Input.GetKey(KeyCode.D))
        {
            move += gameObject.transform.right;
        }

        move.y = 0;
        move.Normalize();

        gameObject.transform.position += move * 0.25f * PlayerSpeed;
    }

    /* 점프
     */
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            canJump = false;
            gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, 150, 0));
        }
    }

    /* 마우스 움직임에 따라 바라보는 방향 변화
     */
    void MouseMove()
    {
        AngleX += Input.GetAxis("Mouse X") * 3f;
        AngleY -= Input.GetAxis("Mouse Y") * 3f;

        // 상하 각도 제한
        if (AngleY >= 90)
            AngleY = 90;

        if (AngleY <= -90)
            AngleY = -90;


        // 바라보는 방향을 지정
        gameObject.transform.eulerAngles = new Vector3(AngleY, AngleX, 0.0f);
        GameObject.Find("Player_Capsule").transform.LookAt(new Vector3(gameObject.transform.forward.x, 1, gameObject.transform.forward.z));
    }

    /* 일반공격 및 스킬의 쿨타임 시스템 관련 함수
     */
    void coolTime()
    {
        // 각종 쿨타임 적용
        shootingCool -= Time.deltaTime;


        // 0 이하인 경우 0으로 만듦
        if(shootingCool <=0)
        {
            shootingCool = 0;
        }
    }

    /* 슈팅
     */
    void Shoot()
    {
        if(Input.GetMouseButton(0) && leftBulletNum > 0 && !isReloading && shootingCool <= 0)
        {
            // 슈팅 관련 연출
            // 애니메이션
            armAnimator.SetBool("isShooting", true);

            // 총구 화염
            GameObject muzzle_Flash = Instantiate(muzzleFlash);
            muzzle_Flash.transform.position = gameObject.transform.position + 1.1f*gameObject.transform.forward + 0.35f * gameObject.transform.right - 0.17f*gameObject.transform.up;
            muzzle_Flash.transform.parent = gameObject.transform;
            muzzle_Flash.transform.forward = gameObject.transform.forward;
            Destroy(muzzle_Flash, 1);

            // 발사 소리

            // 반동?
            float reboundX = Random.Range(-1.5f, 1.5f) * PlayerGunRebound;
            float reboundY = Random.Range(0.1f, 2.5f) * PlayerGunRebound;
            AngleX += reboundX;
            AngleY -= reboundY;
            GameObject.Find("Arm").transform.position -= gameObject.transform.forward * 0.01f;
            StartCoroutine(ReboundRecovery(reboundX, reboundY));

            // 충돌 적용
            if (Physics.Raycast(transform.position, transform.forward, out ray, 100))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * ray.distance, Color.yellow);

                if(ray.transform.tag == "enemy")
                {
                    ray.transform.GetComponent<EnemyStatus>().HP -= PlayerAttackPower * 1;

                    Debug.Log("hit!");
                    Debug.Log(ray.transform.position);

                    // 타격 이펙트
                    GameObject hit_flash = Instantiate(muzzleFlash);
                    hit_flash.transform.position = ray.point;
                    hit_flash.transform.parent = null;
                    hit_flash.transform.forward = gameObject.transform.forward;
                    Destroy(hit_flash, 1);
                }
            }

            shootingCool = 0.2f / PlayerAttackSpeed;
            leftBulletNum--;

            armAnimator.SetBool("isShooting", false);
        }
    }

    //반동 회복 : 10번의 반복에 거쳐 반동회복 --> 부드러운 애니메이션화
    IEnumerator ReboundRecovery(float reboundX, float reboundY)
    {
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.0125f*0.1f);

            AngleX -= reboundX * 0.35f * PlayerGunReboundRecover * 0.1f;
            AngleY += reboundY * 0.35f * PlayerGunReboundRecover * 0.1f;
            GameObject.Find("Arm").transform.position += gameObject.transform.forward * 0.01f * 0.1f;
        }
    }



    /* 재장전
     */
    void Reload()
    {
        if(Input.GetKeyDown(KeyCode.R) && (leftBulletNum != maxBulletNum))
        {
            isReloading = true;

            // 장전 애니메이션
            armAnimator.SetBool("isReloading", true);

            // 장전 소리?

            // 장전 처리
            StartCoroutine(Reloader(3.0f/PlayerReloadSpeed));
        }
    }
    IEnumerator Reloader(float second)
    {
        yield return new WaitForSeconds(second);
        isReloading = false;
        leftBulletNum = maxBulletNum;
        armAnimator.SetBool("isReloading", false);
    }


}
