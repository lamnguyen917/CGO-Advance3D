using System;
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

    [SerializeField] private Transform leftFoot;
    [SerializeField] private Transform rightFoot;
    [SerializeField] private float maxHitDistance = 1;
    [SerializeField] private MeshFilter meshFilter;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            var mesh = meshFilter.mesh;
            // Debug.Log(mesh.vertices);
            // Debug.Log(mesh.triangles);
            foreach (var vert in mesh.normals)
            {
                Debug.Log(vert);
            }
        }

        var v = Input.GetAxis("Vertical");
        var h = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(h, 0, v);
        var moveDirection = transform.TransformDirection(movement) * moveSpeed * Time.deltaTime;
        moveDirection.y += Physics.gravity.y * moveSpeed * Time.deltaTime;
        characterController.Move(moveDirection);
        // RunLook(movement);

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

    private void RunLook(Vector3 direction)
    {
        if (direction.magnitude < 0.25) return;
        direction = Quaternion.Euler(0, 0 + transform.eulerAngles.y, 0) * direction;
        Transform playerTransform = transform;
        playerTransform.rotation =
            Quaternion.Lerp(playerTransform.rotation, Quaternion.LookRotation(direction), Time.deltaTime);
        playerTransform.localEulerAngles = new Vector3(0, playerTransform.localEulerAngles.y, 0);
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (animator == null) return;
        // animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
        // animator.SetIKPosition(AvatarIKGoal.LeftFoot, leftFoot.position);

        // Debug.DrawRay(leftFoot.position + leftFootOffset, Vector3.down * 100, Color.red);
        Ray leftRay = new Ray(leftFoot.position + Vector3.up, Vector3.down);
        if (Physics.Raycast(leftRay, out RaycastHit leftHit, maxHitDistance))
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
            animator.SetIKPosition(AvatarIKGoal.LeftFoot, leftHit.point);

            animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1);
            Vector3 rotAxis = Vector3.Cross(Vector3.up, leftHit.normal);
            float angle = Vector3.Angle(Vector3.up, leftHit.normal);
            Quaternion rot = Quaternion.AngleAxis(angle, rotAxis);

            animator.SetIKRotation(AvatarIKGoal.LeftFoot, rot);
        }
        else
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 0);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 0);
        }

        Ray rightRay = new Ray(rightFoot.position + Vector3.up, Vector3.down);
        if (Physics.Raycast(rightRay, out RaycastHit rightHit, maxHitDistance))
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);
            animator.SetIKPosition(AvatarIKGoal.RightFoot, rightHit.point);

            animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1);
            Vector3 rotAxis = Vector3.Cross(Vector3.up, rightHit.normal);
            float angle = Vector3.Angle(Vector3.up, rightHit.normal);
            Quaternion rot = Quaternion.AngleAxis(angle, rotAxis);

            animator.SetIKRotation(AvatarIKGoal.RightFoot, rot);
        }
        else
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 0);
            animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 0);
        }
    }
}
