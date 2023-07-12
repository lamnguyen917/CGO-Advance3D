using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Animator animator;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float moveSpeed = 4;
    private static readonly int RunSpeed = Animator.StringToHash("RunSpeed");
    private static readonly int Move = Animator.StringToHash("Move");
    private static readonly int SideMove = Animator.StringToHash("SideMove");
    private static readonly int IsAiming = Animator.StringToHash("IsAiming");
    [SerializeField] private Transform weaponLeft;
    [SerializeField] private Transform weaponRight;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var v = Input.GetAxis("Vertical");
        var h = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(h, 0, v);
        var moveDirection = transform.TransformDirection(movement) * moveSpeed * Time.deltaTime;
        moveDirection.y += Physics.gravity.y * moveSpeed * Time.deltaTime;
        characterController.Move(moveDirection);
        RunLook(movement);

        // if (movement.magnitude > 0)
        // {
        // }

        var isAiming = Input.GetMouseButton(1);
        animator.SetFloat(RunSpeed, movement.magnitude);
        animator.SetFloat(Move, v);
        animator.SetBool(IsAiming, isAiming);

        if (isAiming)
        {
            CheckLookAt();
        }
    }

    void CheckLookAt()
    {
        var mousePosition = Input.mousePosition;
        mousePosition.z = 10;
        var pos = Camera.main.ScreenToWorldPoint(mousePosition);
        pos.y = transform.position.y;

        transform.LookAt(pos);
    }

    public void RunLook(Vector3 direction)
    {
        if (direction.magnitude < 0.25) return;
        direction = Quaternion.Euler(0, 0 + transform.eulerAngles.y, 0) * direction;
        Transform playerTransform = transform;
        playerTransform.rotation =
            Quaternion.Lerp(playerTransform.rotation, Quaternion.LookRotation(direction), Time.deltaTime);
        playerTransform.localEulerAngles = new Vector3(0, playerTransform.localEulerAngles.y, 0);
    }
}
