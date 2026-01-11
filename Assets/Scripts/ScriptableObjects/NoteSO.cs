using System;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NoteSO", menuName = "SciptableObjects/NoteSO")]
public class NoteSO : ScriptableObject
{
    public string noteTitle;
    [Space(10)]
    public bool withDate;
    public Date noteDate;
    [Space(10)]
    public bool disapearAfterReading;
    [Space(10)]
    [TextArea(5, 10)]
    public string noteContent;
    [Space(30)]
    public bool isDrawing;
    public Sprite drawing;

    [Serializable]
    public struct Date
    {
        public int day;
        public int month;
        public int year;
    }
}
