using UnityEngine;

[CreateAssetMenu(fileName = "New team", menuName = "Teams/NewTeam", order = 51)]
public class Team : ScriptableObject
{
    [SerializeField] private Color _flag;
    [SerializeField] private string _name;

    public Color Flag => _flag;

    public string Name => _name;
}
