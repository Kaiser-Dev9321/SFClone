using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: Virtual function on a winning fighter-specific WinQuotesManager, that way it can decide what quote to use on however much health they have
public class WinQuotesHolder : MonoBehaviour
{
    public WinQuoteData[] knockdownWinQuotes;
    public WinQuoteData[] superKnockdownWinQuotes;
}
