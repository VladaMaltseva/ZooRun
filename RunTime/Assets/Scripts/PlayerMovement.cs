using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Animator SkinAnimator;
    public GameManager GM;
    CapsuleCollider selfCollider;

    Rigidbody rb;
    public float JumpSpeed = 12;

    int laneNumber = 1, //отвечает за номер текущей линии
        lanesCount = 2; //три линии для бега игрока
    public float firstLanePos, //позиция нулевой линии
                laneDistance,//растояние между линиями
                sideSpeed; //скорость передвижения между линиями

    bool wannaJump = false;

    Vector3 startPosition;
    Vector3 rbVelocity;

    void Start()
    {
 
        selfCollider = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();

        startPosition = transform.position;
        SwipeController.SwipeEvent += CheckInput;
    }

    public void Pause()
    {
        rbVelocity = rb.velocity;
        rb.isKinematic = true;
        SkinAnimator.speed = 0;
    }

    public void UnPause()
    {
        rb.isKinematic = false;
        rb.velocity = rbVelocity;
        SkinAnimator.speed = 1;
    }

    private void FixedUpdate() 
    {
        rb.AddForce(new Vector3(0, Physics.gravity.y*2.5f,0), ForceMode.Acceleration);
        if (wannaJump && isGrounded()) 
        {
            rb.AddForce(new Vector3(0, JumpSpeed, 0), ForceMode.Impulse);
            wannaJump = false;
        }
    }

    void Update()
    {
       // Debug.DrawRay(transform.position, Vector3.down * 0.05f, Color.red);
        Vector3 newPos = transform.position;
        newPos.z = Mathf.Lerp(newPos.z, firstLanePos + (laneNumber * laneDistance), Time.deltaTime * sideSpeed);
        transform.position = newPos;

    }

    bool isGrounded() {
        return Physics.Raycast(transform.position, Vector3.down, 0.05f);
    }
    void CheckInput(SwipeController.SwipeType type)
    {
        if (isGrounded() && GM.CanPlay)
        {
            if (type==SwipeController.SwipeType.UP)
                wannaJump = true;
        }

        if (!GM.CanPlay)
            return;
        int sign = 0;
        if (type == SwipeController.SwipeType.LEFT) 
            sign = -1; 
        else if (type == SwipeController.SwipeType.RIGHT)
             sign = 1; 
            else 
                return;
            laneNumber += sign;
            laneNumber = Mathf.Clamp(laneNumber, 0, lanesCount);
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Trap") || !GM.CanPlay)
            return;

        StartCoroutine(Death());

    }
    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Coin"))
        {
            AudioManager.Instance.PlayCoinEffect();
            return;
        }

        GM.AddCoins(1);
        Destroy(other.gameObject);
    }

    IEnumerator Death() {
        GM.CanPlay = false;

        yield return new WaitForSeconds(.25f);
        
        GM.ShowResult();

        rbVelocity = rb.velocity;
        rb.isKinematic = true;
        SkinAnimator.speed = 0;
    }
    public void ResetPosition() 
    {

        transform.position = startPosition;
        laneNumber = 1;
        rb.isKinematic = false;
        rb.velocity = rbVelocity;
        SkinAnimator.speed = 1;
    }
}
