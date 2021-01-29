using System;
using System.Collections.Generic;
using Entities.NPC.States;
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

        _generationParameters.Add(NPC_BehaviourType.Consumer, consumer);
        _generationParameters.Add(NPC_BehaviourType.NormalCustomer, normalCustomer);
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

    public void ChangeState(NPC npc, Type stateType)
    {
        Transition transition = new Transition {npc = npc, nextState = stateType};
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
            npc.state.OnEnter();
        }
        
        // Finally, clear all transitions.
        _pendingTransitions.Clear();
    }
}