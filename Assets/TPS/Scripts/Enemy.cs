using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float distance;
    private static readonly int Speed = Animator.StringToHash("Speed");

    private void Start()
    {
        agent.stoppingDistance = distance;
    }

    void Update()
    {
        Follow(GameManager.Instance.player.transform);
    }

    void Follow(Transform target)
    {
        if (target == null) return;
        Vector3 pos = target.position;
        agent.SetDestination(pos);
        animator.SetFloat(Speed, agent.velocity.magnitude);
    }
}
