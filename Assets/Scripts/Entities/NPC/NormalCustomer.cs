using System;
using System.Collections.Generic;
using UnityEngine;

public class NormalCustomer : NPC
{
    public override void OnPunch()
    {
       aiManager.ChangeState(this, typeof(NormalCustomer_Fly));
    }

}