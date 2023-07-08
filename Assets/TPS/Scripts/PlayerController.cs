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
    private static readonly int MoveZ = Animator.StringToHash("MoveZ");

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
        moveDirection.y += gravity * moveSpeed * Time.deltaTime;
        characterController.Move(moveDirection);

        // if (movement.magnitude > 0)
        // {
        // }
        animator.SetFloat(RunSpeed, movement.magnitude);
        animator.SetFloat(MoveZ, v);
        
        if (Input.GetMouseButton(0))
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
}
