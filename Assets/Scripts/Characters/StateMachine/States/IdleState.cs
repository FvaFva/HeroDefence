using System.Collections;

public class IdleState : CharacterState
{
    protected override IEnumerator Action(IFightebel _target)
    {
        yield return null;
    }
}
