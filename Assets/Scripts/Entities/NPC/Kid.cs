using System.Collections.Generic;
using UnityEngine;

public class Kid : NPC
{
    public static int kidModelPrefabIndex = 0;
    
    public GameObject kidModelPivot;
    public Animator kidAnimator;
    public List<GameObject> kidModelPrefabs;
    private AnimatorControllerParameter _tmpParameterKid;
    private List<int> _kidAnimationParemeterIDs = new List<int>();

    public bool isRescued;
    public override void Start()
    {
        ServiceLocator.Current.Get<EntityManager>().RegisterNPC(this);

        SpawnKidModel();
        
        spawnPosition = transform.position;

        aiManager = ServiceLocator.Current.Get<AIManager>();
        aiManager.SetupNPC(this);

        stunned = false;
    }

    private void SpawnKidModel()
    {
        GameObject kidModelPrefab = kidModelPrefabs[kidModelPrefabIndex];
        GameObject kidModel = Instantiate(kidModelPrefab, kidModelPivot.transform);
        kidAnimator = kidModel.GetComponent<Animator>();
        kidModelPrefabIndex = (kidModelPrefabIndex + 1) % kidModelPrefabs.Count;
        kidModelPivot.GetComponent<SpriteRenderer>().enabled = false;
        CacheKidAnimatorParameterIds();
    }

    public void PlayIdleAnimation()
    {
        ResetKidAnimatorTriggers();
        kidAnimator.SetTrigger("Idle");
    }

    public void PlayWalkAnimation()
    {
        ResetKidAnimatorTriggers();
        kidAnimator.SetTrigger("Walk");
    }
    
    private void CacheKidAnimatorParameterIds()
    {
        for (int i = 0; i < kidAnimator.parameters.Length; i++)
        {
            _tmpParameterKid = kidAnimator.parameters[i];
            if (_tmpParameterKid.type == AnimatorControllerParameterType.Trigger)
            {
                _kidAnimationParemeterIDs.Add(Animator.StringToHash(_tmpParameterKid.name));
            }
        }
    }
    
    private void ResetKidAnimatorTriggers()
    {
        foreach (int id in _kidAnimationParemeterIDs)
        {
            kidAnimator.ResetTrigger(id);
        }
    }
}