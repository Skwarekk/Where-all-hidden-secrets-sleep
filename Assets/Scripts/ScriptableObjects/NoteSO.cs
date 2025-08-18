using UnityEngine;

[CreateAssetMenu(fileName = "NoteSO", menuName = "SciptableObjects/NoteSO")]
public class NoteSO : ScriptableObject
{
    public string noteTitle;
    [TextArea(5, 10)]
    public string noteContent;
}
