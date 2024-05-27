using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPiece : MonoBehaviour
{
    [SerializeField] private SOShopSelection shopSelection;
    public SOShopSelection ShopSelection
    {
        get { return shopSelection; }
        set { shopSelection = value; }
    }



    private void Awake()
    {
        if (GetComponentInChildren<SpriteRenderer>().sprite != null)
        {
            GetComponentInChildren<SpriteRenderer>().sprite = shopSelection.icon;
        }

        if (transform.Find("itemText"))
        {
            GetComponentInChildren<TextMesh>().text = shopSelection.cost;
        }


    }

}
