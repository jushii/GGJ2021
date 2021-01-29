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
    public Seeker seeker;
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
        aiDestinationSetter.targetPosition = null;
        aiDestinationSetter.targetObject = null;
        aiPath.canMove = false;
    }
    
    public void StartFollowing(Transform target)
    {
        aiDestinationSetter.targetPosition = null;
        aiDestinationSetter.targetObject = target;
        aiPath.canMove = true;
    }

    public void StopFollowing()
    {
        aiDestinationSetter.targetPosition = null;
        aiDestinationSetter.targetObject = null;
        aiPath.canMove = false;
    }

    public void SetPath(Path p)
    {
        StopFollowing();
        aiPath.canMove = true;
        aiPath.SetPath(p);
    }

    public void ClearPath()
    {
        StopFollowing();
        aiPath.canMove = false;
        aiPath.SetPath(null);
    }
    
    public virtual void OnPunch() { }
}
