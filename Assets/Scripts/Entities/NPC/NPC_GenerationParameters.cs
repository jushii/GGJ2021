using System;
using System.Collections.Generic;

public class NPC_GenerationParameters
{
    // The type of the state this AI will start from.
    public Type startingState;
    
    // Every state this AI has.
    public List<Type> availableStates = new List<Type>();
}