using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTransaction : CharacterStateTransaction
{
    protected override bool IsSuitableTarget(Target target)
    {
        return target.IstEmpte && target.IsFightebel == false;
    }
}
