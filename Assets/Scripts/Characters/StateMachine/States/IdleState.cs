using System.Collections;

public class IdleState : CharacterState
{
    public override IEnumerator Action()
    {
        yield return null;
    }
}
