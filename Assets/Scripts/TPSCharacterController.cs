using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSCharacterController : MonoBehaviour
{
    [SerializeField]
    private Transform characterBody;
    [SerializeField]
    private Transform cameraArm;
    private Rigidbody rigid;

    public float jumpForce;
    bool IsJump;

    Animator animator;

    float fireDelay;
    bool isFireReady;
    public Weapon equipWeapon;
    // Start is called before the first frame update
    void Start()
    {
        animator = characterBody.GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        IsJump = false;
    }

    // Update is called once per frame
    void Update()
    {
        LookAround();
        Move();
        Jump();
        Attack();
    }


    private void Move() 
    {
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        bool isMove = moveInput.magnitude != 0;
        animator.SetBool("isMove", isMove);
        if (isMove)
        {
            Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
            Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

            characterBody.forward = moveDir;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                transform.position += moveDir * Time.deltaTime * 5f;
                animator.SetBool("isRun", true);
            }
            else
            {
                animator.SetBool("isRun", false);
                transform.position += moveDir * Time.deltaTime * 2f;
            }  
        }
        //Debug.DrawRay(cameraArm.position, new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized, Color.red)
    }

    private void LookAround()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 camAngle = cameraArm.rotation.eulerAngles;
        float x = camAngle.x - mouseDelta.y;

        if(x < 180f)
        {
            x = Mathf.Clamp(x, -1f, 70f);
        }
        else
        {
            x = Mathf.Clamp(x, 335f, 361f);
        }

        cameraArm.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);
    }
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!IsJump)
            {
                IsJump = true;
                animator.SetBool("isJump", true);
                rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                //animator.SetBool("isJump", false);
            }
            else
            {
                return;
            }
        }
    }

    void Attack()
    {
        fireDelay += Time.deltaTime;
        isFireReady = equipWeapon.rate < fireDelay;
        if (Input.GetMouseButtonDown(0) && isFireReady)
        {
            equipWeapon.Use();
            animator.SetBool("isAttack", true);
            fireDelay = 0;
        }
        else
        {
            animator.SetBool("isAttack", false);
        }
        /*
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetBool("isAttack", true);
        }
        else
        {
            animator.SetBool("isAttack", false);
        }*/
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            IsJump = false;
        }
    }
}
