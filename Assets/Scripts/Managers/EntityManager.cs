using System.Collections.Generic;
using UnityEngine;

// Keeps track of all entities in the game.
public class EntityManager : MonoBehaviour, IGameService
{
    public readonly List<Player> players = new List<Player>();
    public readonly List<NPC> npcs = new List<NPC>();
    
    // private const float MINIMUM_FOLLOW_PLAYER_DISTANCE = 10.0f;
    
    public void RegisterPlayer(Player player)
    {
        players.Add(player);
    }
    
    public void RegisterNPC(NPC npc)
    {
        npcs.Add(npc);
    }

    // private void UpdateNPCState()
    // {
    //     for (int i = 0; i < npcs.Count; i++)
    //     {
    //         NPC npc = this.npcs[i];
    //
    //         switch (npc.behaviourType)
    //         {
    //             case NPCType.Idle: UpdateNPCState_Idle(npc); break;
    //             case NPCType.FollowPlayer: UpdateNPCState_FollowPlayer(npc); break;
    //         }
    //     }
    // }

    // private void UpdateNPCState_Idle(NPC npc)
    // {
    //     // Start following the nearest player (if distance from player to NPC is below a certain threshold).
    //     Vector2 npcPosition = npc.transform.position;
    //
    //     for (int i = 0; i < players.Count; i++)
    //     {
    //         Player player = players[i];
    //         Vector2 playerPosition = player.transform.position;
    //         if (Vector2.Distance(npcPosition, playerPosition) <= MINIMUM_FOLLOW_PLAYER_DISTANCE)
    //         {
    //             npc.StartFollowing(player.transform);
    //             npc.behaviourType = NPCType.FollowPlayer;
    //             break;
    //         }
    //     }
    // }
    
    // private void UpdateNPCState_FollowPlayer(NPC npc)
    // {
    //     // Check if the NPC should stop following player(s) if they are too far. Otherwise keep following or change
    //     // the follow target to the nearest player.
    //     Vector2 npcPosition = npc.transform.position;
    //
    //     float minDst = float.MaxValue;
    //     int playerIndex = -1;
    //     
    //     for (int i = 0; i < players.Count; i++)
    //     {
    //         Player player = players[i];
    //         Vector2 playerPosition = player.transform.position;
    //         float dst = Vector2.Distance(npcPosition, playerPosition);
    //         if (dst < minDst)
    //         {
    //             minDst = dst;
    //             playerIndex = i;
    //         }
    //     }
    //     
    //     if (minDst > MINIMUM_FOLLOW_PLAYER_DISTANCE)
    //     {
    //         // Stop following.
    //         npc.StopFollowing();
    //         npc.behaviourType = NPCType.Idle;
    //     }
    //     else
    //     {
    //         // Keep following (and change target to the nearest player if necessary).
    //         npc.StartFollowing(players[playerIndex].transform);
    //     }
    // }
}
