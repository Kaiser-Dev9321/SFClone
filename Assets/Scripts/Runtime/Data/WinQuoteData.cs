using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WinQuoteData_", menuName = "Win Quote Data/New Win Quote Data")]
public class WinQuoteData : ScriptableObject
{
    [TextArea(3,10)]
    public string winQuote;
}
