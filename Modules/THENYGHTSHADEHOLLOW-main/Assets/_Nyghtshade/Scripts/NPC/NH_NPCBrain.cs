using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NH_NPCIdentity))]
public class NH_NPCBrain : MonoBehaviour
{
    public NH_NPCState currentState = NH_NPCState.Idle;

    [Header("Patrol")]
    public Transform[] patrolPoints;
    private int _patrolIndex;

    private NavMeshAgent _agent;
    private NH_NPCIdentity _identity;

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _identity = GetComponent<NH_NPCIdentity>();
    }

    void Update()
    {
        if (!_identity.isAlive || !_identity.isConscious) return;
        ProcessState();
    }

    void ProcessState()
    {
        switch (currentState)
        {
            case NH_NPCState.Patrol:
                DoPatrol();
                break;
            case NH_NPCState.Idle:
            case NH_NPCState.Working:
            case NH_NPCState.Eating:
            case NH_NPCState.YardTime:
                if (_agent != null && _agent.hasPath && _agent.remainingDistance < 0.5f)
                    _agent.ResetPath();
                break;
        }
    }

    void DoPatrol()
    {
        if (patrolPoints == null || patrolPoints.Length == 0 || _agent == null) return;

        if (!_agent.hasPath || _agent.remainingDistance < 0.5f)
        {
            _patrolIndex = (_patrolIndex + 1) % patrolPoints.Length;
            _agent.SetDestination(patrolPoints[_patrolIndex].position);
        }
    }

    public void SetState(NH_NPCState state)
    {
        currentState = state;
    }

    public void MoveTo(Vector3 position)
    {
        if (_agent != null)
            _agent.SetDestination(position);
    }

    public void StopMovement()
    {
        if (_agent != null) _agent.ResetPath();
    }

    public void Surrender()
    {
        SetState(NH_NPCState.Surrendered);
        StopMovement();
    }

    public void Flee(Vector3 fromPoint)
    {
        SetState(NH_NPCState.Fleeing);
        Vector3 fleeDir = (transform.position - fromPoint).normalized;
        MoveTo(transform.position + fleeDir * 8f);
    }
}
