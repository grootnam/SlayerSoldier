using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PlayerMove : MonoBehaviour
{
    // for mouse move
    float AngleX, AngleY;

    // for act
    Vector3 move;
    public bool canJump;
    bool isReloading;
    Rigidbody rigidbody;
    // for shooting
    public int leftBulletNum;
    RaycastHit ray;
    float shootingCool;
    public GameObject camera;
    public GameObject muzzleFlash;

    //for Qskill
    float QCool;
    public GameObject prefab_missile;
    Rigidbody mrigidbody;

    //for Eskill
    float ECool;
    bool isEskill;

    // 시프트 스킬 (? 왜다들 영어주석이지?) ㅋㅋㅋㅋ성재가시작했음
    public bool isShiftskill;
    public float ShiftCool = 4f;

    [System.NonSerialized]
    public float Shift_temptime;


    public bool isNoDamagetime;
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

    public float Shift_no_Damagetime; //shift 무적시간

    private void Start()
    {
        // 마우스 커서 숨김 
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        armAnimator = GameObject.Find("Arm").GetComponent<Animator>();
        rigidbody = gameObject.GetComponent<Rigidbody>();
        Shift_temptime = 0f;
        isReloading = false;
        isEskill = false;
        isShiftskill = false;
        isNoDamagetime = false;
        canJump = true;
    }

    void Update()
    {
        Jump();
        MouseMove();
        Reload();
        Eskill();
        Qskill();
        Shiftskill();
        coolTime();
        Shoot();

        if (PlayerHP <= 0 && !FindObjectOfType<UImanager>().isStageOver)
        {
            FindObjectOfType<UImanager>().stageOver();
        }
    }

    private void FixedUpdate()
    {
        WASD();
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "ground")
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

    private void OnTriggerEnter(Collider other)
    {
        //적 스킬을 맞았을때와 Shift의 무적시간이 아닐때
        if (other.tag == "enemySkill" && isNoDamagetime == false)
        {
            PlayerHP -= other.GetComponentInParent<EnemySkill>().damage;

            Debug.Log("Hit! :" + other.GetComponentInParent<EnemySkill>().damage);
        }

        if (other.gameObject.tag == "BigBall")
        {
            PlayerHP -= 30f;

            Debug.Log("HitBigBall!");
        }
    }


    /* 플레이어 움직임에 관한 코드
     */
    void WASD()
    {
        move = new Vector3(0, 0, 0);

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
            SoundManager.Instance.PlayerJumpSound();
        }
    }

    /* 마우스 움직임에 따라 바라보는 방향 변화
     */
    void MouseMove()
    {
        AngleX += Input.GetAxis("Mouse X");
        AngleY -= Input.GetAxis("Mouse Y");

        // 상하 각도 제한
        if (AngleY >= 90)
            AngleY = 90;

        if (AngleY <= -90)
            AngleY = -90;


        /* 바라보는 방향지정
            SHIFT스킬 사용시, 구르는 효과로 카메라가 살짝 내려감
        */
        if (isShiftskill == true)
        {
            camera.transform.position = Vector3.Lerp(transform.position, transform.position - new Vector3(0, 0.7f, 0), 0.5f);
        }
        else {
            camera.transform.position = Vector3.Lerp(camera.transform.position, transform.position, 0.5f);
        }
        GameObject.Find("Player_Capsule").transform.LookAt(new Vector3(gameObject.transform.forward.x, 1, gameObject.transform.forward.z));
        gameObject.transform.eulerAngles = new Vector3(AngleY, AngleX, 0.0f);

    }

    /* 일반공격 및 스킬의 쿨타임 시스템 관련 함수
     */
    void coolTime()
    {
        // 각종 쿨타임 적용
        shootingCool -= Time.deltaTime;
        QCool -= Time.deltaTime;
        ECool -= Time.deltaTime;
        Shift_temptime -= Time.deltaTime;

        // 0 이하인 경우 0으로 만듦
        if (shootingCool <= 0)
        {
            shootingCool = 0;
        }
        if (QCool <= 0)
        {
            QCool = 0;
        }
        if (ECool <= 0)
        {
            ECool = 0;
        }
        if (Shift_temptime <= 0)
        {
            Shift_temptime = 0;
        }
    }

    /* 슈팅
     */
    void Shoot()
    {
        if (Input.GetMouseButton(0) && leftBulletNum > 0 && !isReloading && !isShiftskill && shootingCool <= 0 && !isEskill)
        {
            // 슈팅 관련 연출
            // 애니메이션
            armAnimator.SetBool("isShooting", true);

            // 총구 화염
            GameObject muzzle_Flash = Instantiate(muzzleFlash);
            muzzle_Flash.transform.position = gameObject.transform.position + 1.1f * gameObject.transform.forward + 0.35f * gameObject.transform.right - 0.17f * gameObject.transform.up;
            muzzle_Flash.transform.parent = gameObject.transform;
            muzzle_Flash.transform.forward = gameObject.transform.forward;
            Destroy(muzzle_Flash, 1);

            // 발사 소리
            SoundManager.Instance.PlayAttackSound();

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
                if (ray.transform.tag == "enemy")
                {
                    ray.transform.GetComponentInParent<EnemyStatus>().HP -= PlayerAttackPower * 1;

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

            shootingCool = 0.1f / PlayerAttackSpeed;
            leftBulletNum--;

            armAnimator.SetBool("isShooting", false);
        }
    }

    //반동 회복 : 10번의 반복에 거쳐 반동회복 --> 부드러운 애니메이션화
    IEnumerator ReboundRecovery(float reboundX, float reboundY)
    {
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.00125f);

            AngleX -= reboundX * 0.35f * PlayerGunReboundRecover * 0.1f;
            AngleY += reboundY * 0.35f * PlayerGunReboundRecover * 0.1f;
            GameObject.Find("Arm").transform.position += gameObject.transform.forward * 0.01f * 0.1f;
        }
    }



    /* 재장전
     */
    void Reload()
    {
        if(Input.GetKeyDown(KeyCode.R) && (leftBulletNum != maxBulletNum) && !isEskill && !isReloading&& !isShiftskill)
        {
            isReloading = true;

            // 장전 애니메이션
            armAnimator.SetBool("isReloading", true);

            // 장전 소리?
            SoundManager.Instance.PlayReloadSound();

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

    /* Q스킬
   */
    void Qskill()
    {
        if (Input.GetKeyDown(KeyCode.Q) && QCool <= 0 && !isReloading&& !isEskill)
        {

            //반동
            float reboundX = Random.Range(-1.5f, 1.5f) * PlayerGunRebound;
            float reboundY = Random.Range(1.0f, 5.5f) * PlayerGunRebound;
            AngleX += reboundX;
            AngleY -= reboundY;
            GameObject.Find("Arm").transform.position -= gameObject.transform.forward * 0.01f;
            StartCoroutine(ReboundRecovery(reboundX, reboundY));

            //발사
            GameObject missile = GameObject.Instantiate(prefab_missile) as GameObject;
            mrigidbody = missile.GetComponent<Rigidbody>();
            missile.transform.position = GameObject.Find("Arm").transform.position;
            //missile.transform.rotation = Quaternion.LookRotation(gameObject.transform.rotation * Vector3.forward);
            missile.transform.forward = gameObject.transform.forward;
            mrigidbody.AddForce(gameObject.transform.forward * 2000.0f);
            QCool = 9.0f / PlayerAttackSpeed;
        }
    }

    /* E스킬
    */
    void Eskill()
    {
        
        if (Input.GetKeyDown(KeyCode.E) && !isEskill && ECool <= 0 &&!isReloading&& !isShiftskill)
        {
            isEskill = true;

            //애니메이션
            armAnimator.SetBool("isEskill", true);

            // 충돌 적용
            if (Physics.Raycast(transform.position, transform.forward, out ray, 0.8f))
            {
                if (ray.transform.tag == "enemy")
                {
                    Debug.Log("hit!");
                    Debug.Log(ray.transform.position);
                    // 사운드
                    SoundManager.Instance.PlaySwordSound();
                    ray.transform.GetComponent<EnemyStatus>().HP -= PlayerAttackPower * 50;
                }
            }
            else
            {
                SoundManager.Instance.PlaySwordSound2();
            }

            ECool = 2.0f / PlayerAttackSpeed;
            StartCoroutine(E(2.0f / PlayerReloadSpeed));
        }

    }
    IEnumerator E(float second)
    {
        yield return new WaitForSeconds(second);
        isEskill = false;
        armAnimator.SetBool("isEskill", false);
    }

    /*
     * Shift 스킬
     */
    void Shiftskill()
    {
        //모든 스킬과 점프 안할때 가능.
        if(Input.GetKeyDown(KeyCode.LeftShift) &&Shift_temptime<=0&&!isEskill && !isReloading&&canJump)
        {
            isShiftskill=true; //스킬 사용중.
            Shift_temptime=ShiftCool; //쿨타임 시작!
            isNoDamagetime=true; //무적!! ON!!!
            canJump=false;

            rigidbody.AddForce(move*25,ForceMode.Impulse);
            
            //무적, 회피시간.
            StartCoroutine(Shiftskill(0.45f));
            StartCoroutine(Not_damage(Shift_no_Damagetime));
        }
        
    }
    IEnumerator Shiftskill(float second)
    {
        yield return new WaitForSeconds(second);
        isShiftskill=false;
        rigidbody.velocity=Vector3.zero;
    }

    //무적시간
    IEnumerator Not_damage(float second)
    {
        yield return new WaitForSeconds(second);
        isNoDamagetime = false;
    }
}


