using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    private float playerHeight = 2f;

    public Camera cam;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6f;
    private float movementMultiplier = 10f;
    [SerializeField] private float airMultiplier = 0.4f;

    [Header("Sprinting")]
    [SerializeField] float walkSpeed = 4f;
    [SerializeField] float sprintSpeed = 6f;
    [SerializeField] float acceleration = 10f;
    [SerializeField] float spritFovModifier = 100f;
    [HideInInspector] public bool isSprinting;

    [Header("Jumping")]
    [SerializeField] private float jumpForce = 15f;

    [Header("Keybinds")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;

    [Header("Drag")]
    [SerializeField] float groundDrag = 6f;
    [SerializeField] float airDrag = 2f;

    private float horizontalMovement;
    private float verticalMovement;

    [Header("Ground Detections")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;
    private bool isGrounded;
    private float groundDistance = 0.4f;

    Vector3 moveDirection;
    Vector3 slopeMoveDirection;

    private Rigidbody rb;

    RaycastHit slopeHit;

    public AudioClip pickupSound;
    public FlashScreen flash;
    AudioSource source;

    float baseFOV;

    private void Start()
    {
        baseFOV = cam.fieldOfView;
        rb = GetComponent<Rigidbody>();
        source = GetComponent<AudioSource>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        MyInput();
        ControlDrag();
        CrosshairControl();  

        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);        
    }

    private void FixedUpdate()
    {
        MovePlayer();
        ControlSpeed();

        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            Jump();
        }
    }

    private void MyInput()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        moveDirection = transform.forward * verticalMovement + transform.right * horizontalMovement;
    }    

    private void MovePlayer()
    {
        if(isGrounded && !OnSlope())
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration); // Force.Acceleration, Force, Impulse 차이
        }
        else if(isGrounded && OnSlope())
        {
            rb.AddForce(slopeMoveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if(!isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier * airMultiplier, ForceMode.Acceleration);
        }
    }

    private void ControlDrag()
    {
        if(isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = airDrag;
        }
    }

    private void Jump()
    {
        Debug.Log("Jump");
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f))
        {
            if(slopeHit.normal != Vector3.up)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    private void ControlSpeed()
    {
        if(Input.GetKey(sprintKey) && isGrounded && verticalMovement > 0)
        {
            isSprinting = true;
            
            moveSpeed = Mathf.Lerp(moveSpeed, sprintSpeed, acceleration * Time.deltaTime);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, baseFOV * spritFovModifier, Time.deltaTime * 8f);
        }
        else
        {
            isSprinting = false;
            
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, acceleration * Time.deltaTime);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, baseFOV, Time.deltaTime);
        }
    }

    private void CrosshairControl()
    {
        if(isSprinting)
            DynamicCrosshair.spread = DynamicCrosshair.SPRINT_SPREAD;
        else if(isGrounded == false)
            DynamicCrosshair.spread = DynamicCrosshair.JUMP_SPREAD;
        else
            DynamicCrosshair.spread = DynamicCrosshair.WALK_SPREAD;
    }

    // 아이템 습득
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HpBonus"))
        {
            //GetComponent<PlayerHealth>().AddHealth(Managers.Data.consumableItemDict[1000].value);
            transform.Find("InventoryThings").GetComponent<Inventory>().AddItem(Managers.Data.consumableItemDict[1000].index);
        }
        else if (other.CompareTag("ArmorBonus"))
        {
            transform.Find("InventoryThings").GetComponent<Inventory>().AddItem(Managers.Data.consumableItemDict[1001].index);
        }
        else if (other.CompareTag("AmmoBonus"))
        {
            transform.Find("InventoryThings").GetComponent<Inventory>().AddItem(Managers.Data.consumableItemDict[1002].index);
        }
        else if (other.CompareTag("Engram"))
        {
            transform.Find("InventoryThings").GetComponent<Inventory>().AddItem(Managers.Data.consumableItemDict[1003].index);
        }
        else if (other.CompareTag("ExoticEngram"))
        {
            transform.Find("InventoryThings").GetComponent<Inventory>().AddItem(Managers.Data.consumableItemDict[1004].index);
        }

        if (other.CompareTag("HpBonus") || other.CompareTag("ArmorBonus") || other.CompareTag("AmmoBonus") || other.CompareTag("Engram") || other.CompareTag("ExoticEngram"))
        {
            flash.PickedUpBonus();
            source.PlayOneShot(pickupSound);
            other.gameObject.SetActive(false);
        }
    }
}
