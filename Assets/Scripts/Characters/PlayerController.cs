using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public FloatingJoystick joystick; // tham chiếu đến joystick

    [Header("Settings")]
    public float moveSpeed = 5f; // tốc độ di chuyển
    public float punchDuration = 0.6f; // thời gian đấm

    private CharacterController characterController; // tham chiếu đến character controller
    private Animator animator; 
    private bool isPunching; 

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        if (!joystick)
        {
            joystick = FindObjectOfType<FloatingJoystick>();
            if (!joystick)
                Debug.LogWarning("No FloatingJoystick found in scene!");
        }
    }

    void Update()
    {
        if (!isPunching)
        {
            HandleMovement();
            HandlePunchInput();
        }
    }

    void HandleMovement()
    {
    Vector2 input = new Vector2(joystick.Horizontal, joystick.Vertical);
    bool walking = input.sqrMagnitude > 0.01f;

    if(animator.GetBool("isWalking") != walking)
        animator.SetBool("isWalking", walking);

    if(walking)
    {
        Vector3 move = new Vector3(input.x, 0, input.y); 
        move = Camera.main.transform.TransformDirection(move);
        move.y = 0;
        move.Normalize();

        characterController.Move(move * moveSpeed * Time.deltaTime);
        transform.forward = move;
    }
}


    void HandlePunchInput() // kiểm tra xem có nhấn nút punch không
    // kiểm tra xem có nhấn nút punch không
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0)) // nhấn chuột trái
#else
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) // nhấn vào màn hình
#endif
        {
            TriggerPunch(); 
        }
    }

    void TriggerPunch()
    {
        isPunching = true;
        animator.SetTrigger("Punching"); //punching trong animator
        animator.SetBool("isWalking", false);
        Invoke(nameof(EndPunch), punchDuration); // end punch sau 0.6 giây
    }

    void EndPunch()
    {
        isPunching = false;
    }
}
