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
    public int receivedHits;
    public bool stunned;
    private Animator _animator;
    private AnimatorControllerParameter tmpParameter;
    private List<int> _cachedParameterIds = new List<int>();
    
    // Hit sprite logic.
    public SpriteRenderer spriteRenderer;
    public int HitSpriteIndexTop
    {
        get
        {
            int indexToReturn = _hitSpriteIndexTop;
            _hitSpriteIndexTop = (_hitSpriteIndexTop + 1) % 2;
            return indexToReturn;
        }
    }

    public int HitSpriteIndexBottom
    {
        get
        {
            int indexToReturn = _hitSpriteIndexBottom;
            _hitSpriteIndexBottom = (_hitSpriteIndexBottom + 1) % 2;
            return indexToReturn;
        }
    }
    public int HitSpriteIndexLeft
    {
        get
        {
            int indexToReturn = _hitSpriteIndexLeft;
            _hitSpriteIndexLeft = (_hitSpriteIndexLeft + 1) % 2;
            return indexToReturn;
        }
    }
    public int HitSpriteIndexRight
    {
        get
        {
            int indexToReturn = _hitSpriteIndexRight;
            _hitSpriteIndexRight = (_hitSpriteIndexRight + 1) % 2;
            return indexToReturn;
        }
    }
    private int _hitSpriteIndexTop = 0;
    private int _hitSpriteIndexBottom = 0;
    private int _hitSpriteIndexLeft = 0;
    private int _hitSpriteIndexRight = 0;
    public List<Sprite> topHitSprites;
    public List<Sprite> bottomHitSprites;
    public List<Sprite> leftHitSprites;
    public List<Sprite> rightHitSprites;

    public Animator Animator
    {
        get
        {
            try
            {
                return GetComponent<Animator>();
            }
            catch (Exception)
            {
                Debug.Log("No animator component on this entity");
                throw new Exception();
            }
        }
    }

    public Kid followerKid;

    public virtual void Start()
    {
        ServiceLocator.Current.Get<EntityManager>().RegisterNPC(this);

        spawnPosition = transform.position;
        
        aiManager = ServiceLocator.Current.Get<AIManager>();
        aiManager.SetupNPC(this);

        stunned = false;

        _animator = GetComponent<Animator>();
        CacheAnimatorParameterIds();

        // GameManager.onPlayerSpawned += OnPlayerSpawned;
    }

    public void ResetReceivedHits()
    {
        receivedHits = 0;
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
    
    public virtual void OnStartPunch() { }
    
    public virtual void OnEndPunch() { }
    
    private void CacheAnimatorParameterIds()
    {
        for (int i = 0; i < _animator.parameters.Length; i++)
        {
            tmpParameter = _animator.parameters[i];
            if (tmpParameter.type == AnimatorControllerParameterType.Trigger)
            {
                _cachedParameterIds.Add(Animator.StringToHash(tmpParameter.name));
            }
        }
    }
    public void ResetAnimatorTriggers()
    {
        foreach (int id in _cachedParameterIds)
        {
            _animator.ResetTrigger(id);
        }
    }

}
