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
    public AIDestinationSetter aiDestinationSetter;
    public RVOController rvoController;
    public Rigidbody2D rigidbody2D;
    
    private void Start()
    {
        ServiceLocator.Current.Get<EntityManager>().RegisterNPC(this);
        ServiceLocator.Current.Get<AIManager>().SetupNPC(this);
    }

    public void StartFollowing(Transform target)
    {
        aiDestinationSetter.target = target;
        aiPath.canMove = true;
    }

    public void StopFollowing()
    {
        aiPath.canMove = false;
        aiDestinationSetter.target = null;
    }
}
