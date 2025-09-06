using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinQuotesManager : MonoBehaviour
{
    public GameObject quotesEmpty;

    public WinQuotesHolder assignedWinQuotesHolder;

    public WinQuoteData winQuoteToUse;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void LoadQuote(EntityScript winningFighter, EntityScript losingFighter)
    {
        Transform winQuoteParent = quotesEmpty.transform.Find(winningFighter.fighterName);
        Transform winQuote = winQuoteParent.Find(losingFighter.fighterName);
        WinQuoteData actualWinQuote;

        if (!winQuote)
        {
            actualWinQuote = winQuoteParent.Find("Generic").GetComponent<WinQuotesHolder>().knockdownWinQuotes[0];
        }
        else
        {
            actualWinQuote = winQuote.GetComponent<WinQuotesHolder>().knockdownWinQuotes[0];
        }

        winQuoteToUse = actualWinQuote;
    }
}
