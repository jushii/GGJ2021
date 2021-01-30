using System;
using System.Collections.Generic;
using UnityEngine;

public class AIManager :  MonoBehaviour, IGameService
{
    private readonly List<Transition> _pendingTransitions = new List<Transition>();
    private readonly Dictionary<NPC_BehaviourType, NPC_GenerationParameters> _generationParameters = new Dictionary<NPC_BehaviourType, NPC_GenerationParameters>();
    private EntityManager _entityManager;

    public void Setup()
    {
        _entityManager = ServiceLocator.Current.Get<EntityManager>();

        NPC_GenerationParameters consumer = new NPC_GenerationParameters();
        consumer.startingState = typeof(Consumer_Idle);
        consumer.availableStates.Add(typeof(Consumer_Idle));
        consumer.availableStates.Add(typeof(Consumer_MoveToTarget));
        consumer.availableStates.Add(typeof(Consumer_Fly));

        NPC_GenerationParameters normalCustomer = new NPC_GenerationParameters();
        normalCustomer.startingState = typeof(NormalCustomer_Idle);
        normalCustomer.availableStates.Add(typeof(NormalCustomer_Idle));
        normalCustomer.availableStates.Add(typeof(NormalCustomer_Fly));
        normalCustomer.availableStates.Add(typeof(NormalCustomer_WalkToSpawnPosition));

        NPC_GenerationParameters kid = new NPC_GenerationParameters();
        kid.startingState = typeof(Kid_Idle);
        kid.availableStates.Add(typeof(Kid_Idle));
        kid.availableStates.Add(typeof(Kid_FollowPlayer));
        kid.availableStates.Add(typeof(Kid_GoToExit));
        kid.availableStates.Add(typeof(Kid_FollowPromotionGuy));
        
        NPC_GenerationParameters mallWorker = new NPC_GenerationParameters();
        mallWorker.startingState = typeof(MallWorker_Idle);
        mallWorker.availableStates.Add(typeof(MallWorker_Idle));
        mallWorker.availableStates.Add(typeof(MallWorker_FollowPlayer));
        mallWorker.availableStates.Add(typeof(MallWorker_Fly));
        
        NPC_GenerationParameters promotionGuy = new NPC_GenerationParameters();
        promotionGuy.startingState = typeof(PromotionGuy_Idle);
        promotionGuy.availableStates.Add(typeof(PromotionGuy_Idle));
        promotionGuy.availableStates.Add(typeof(PromotionGuy_ChaseKid));
        promotionGuy.availableStates.Add(typeof(PromotionGuy_RunAwayWithKid));
        promotionGuy.availableStates.Add(typeof(PromotionGuy_Fly));

        _generationParameters.Add(NPC_BehaviourType.Consumer, consumer);
        _generationParameters.Add(NPC_BehaviourType.NormalCustomer, normalCustomer);
        _generationParameters.Add(NPC_BehaviourType.Kid, kid);
        _generationParameters.Add(NPC_BehaviourType.MallWorker, mallWorker);
        _generationParameters.Add(NPC_BehaviourType.PromotionGuy, promotionGuy);
    }

    public void SetupNPC(NPC npc)
    {
        // Get the NPC generation parameters based on the NPC behaviour type 
        NPC_GenerationParameters npcGenerationParameters = _generationParameters[npc.behaviourType];

        // Create the NPC AI states.
        for (int i = 0; i < npcGenerationParameters.availableStates.Count; i++)
        {
            Type type = npcGenerationParameters.availableStates[i];
            State state = (State) Activator.CreateInstance(type);
            state.npc = npc;
            npc.states.Add(type, state);
        }

        // Change NPC current state to the starting state and call OnEnter.
        Type startingState = npcGenerationParameters.startingState;
        npc.state = npc.states[startingState];
        npc.state.OnEnter();
    }

    public void ChangeState(NPC npc, Type stateType, object args = null)
    {
        Transition transition = new Transition {npc = npc, nextState = stateType, onEnterArgs = args};
        _pendingTransitions.Add(transition);
    }
    
    private void Update()
    {
        HandleUpdate();
        HandleTransitions();
    }

    private void HandleUpdate()
    {
        List<NPC> npcs = _entityManager.npcs;
        
        // Run update on each NPC agent.
        for (int i = 0; i < npcs.Count; i++)
        {
            NPC npc = npcs[i];
            npc.state.OnUpdate();
        }
    }
    
    private void HandleTransitions()
    {
        // Process pending state transitions.
        for (int i = 0; i < _pendingTransitions.Count; i++)
        {
            Transition transition = _pendingTransitions[i];
            NPC npc = transition.npc;
            Type nextState = transition.nextState;
            
            // Call OnExit before entering the new state.
            npc.state.OnExit();
            npc.state = null;

            // Change the state and call OnEnter on the new state.
            npc.state = npc.states[nextState];
            npc.state.OnEnter(transition.onEnterArgs);
        }
        
        // Finally, clear all transitions.
        _pendingTransitions.Clear();
    }
}