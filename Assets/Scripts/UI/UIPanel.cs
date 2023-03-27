using UnityEngine;

public class UIPanel : MonoBehaviour
{
    public void ChangeVision(bool newVision)
    {
        gameObject.SetActive(newVision);
    }
}
