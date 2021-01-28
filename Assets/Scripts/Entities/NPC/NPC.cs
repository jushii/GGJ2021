using System;
using System.Collections.Generic;
using Pathfinding;
using Pathfinding.RVO;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public NPC_BehaviourType behaviourType = NPC_BehaviourType.None;
    public State state;
    public Dictionary<Type, State> states = new Dictionary<Type, State>();
    public AIPath aiPath;
    public AIDestinationSetterNPC aiDestinationSetter;
    public RVOController rvoController;
    public Rigidbody2D rigidbody2D;
    public AIManager aiManager;
    public Vector3 spawnPosition;
    
    private void Start()
    {
        ServiceLocator.Current.Get<EntityManager>().RegisterNPC(this);

        spawnPosition = transform.position;
        
        aiManager = ServiceLocator.Current.Get<AIManager>();
        aiManager.SetupNPC(this);
    }

    public void MoveTo(Vector3 position)
    {
        aiDestinationSetter.targetPosition = position;
        aiPath.canMove = true;
    }

    public void StopMoving()
    {
        aiPath.canMove = false;
    }
    
    public void StartFollowing(Transform target)
    {
        aiDestinationSetter.targetObject = target;
        aiPath.canMove = true;
    }

    public void StopFollowing()
    {
        aiPath.canMove = false;
        aiDestinationSetter.targetPosition = null;
    }

    public virtual void OnPunch() { }
}
