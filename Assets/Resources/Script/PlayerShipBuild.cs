using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShipBuild : MonoBehaviour
{
    [SerializeField] private GameObject[] shopButtons;
    private GameObject target;
    private GameObject tmpSelection;
    private GameObject textBoxPanel;
    [SerializeField] private GameObject[] visualWeapons;
    [SerializeField] private SOActorModel defaultPlayerShip;
    private GameObject playerShip;
    private GameObject buyButton;
    private GameObject bankObj;
    private int bank = 600;
    private bool purchaseMade = false;


    private void Start()
    {
        TurnOffSelectionHighlights();

        textBoxPanel = GameObject.Find("textBoxPanel");

        purchaseMade = false;
        bankObj = GameObject.Find("bank");
        bankObj.GetComponentInChildren<TextMesh>().text = bank.ToString();
        buyButton = textBoxPanel.transform.Find("BUY ?").gameObject;
        TurnOffPlayerShipVisuals();
        PreparePlayerShipForUpgrade();
    }

    private void TurnOffPlayerShipVisuals()
    {
        for (int i = 0; i < visualWeapons.Length; i++)
        {
            visualWeapons[i].gameObject.SetActive(false);
        }
    }

    private void PreparePlayerShipForUpgrade()
    {
        playerShip = Instantiate(Resources.Load("Prefab/Player/Player_Ship")) as GameObject;
        playerShip.GetComponent<Player>().enabled = false;
        playerShip.transform.position = new Vector3(0, 10000, 0);
        playerShip.GetComponent<IActorTemplate>().ActorStats(defaultPlayerShip);
    }


    private void TurnOffSelectionHighlights()
    {
        for (int i = 0; i < shopButtons.Length; i++)
        {
            shopButtons[i].SetActive(false);
        }

        //Debug.Log("All selection highlights have been turned off");
    }

    private void Update()
    {
        AttemptSelection();
    }



    private GameObject ReturnClickedObject(out RaycastHit hit)
    {
        GameObject target = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray.origin, ray.direction * 100f, out hit))
        {
            target = hit.collider.gameObject;
        }

        return target;
    }


    private void AttemptSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo;
            target = ReturnClickedObject(out hitInfo);

            if (target != null)
            {
                //Debug.Log("Player clicked on an object");

                if (target.transform.Find("itemText"))
                {
                    //Debug.Log("Object clicked is a shop menu item");

                    TurnOffSelectionHighlights();
                    Select();
                    UpdateDescriptionBox();

                    // Not already sold
                    if (target.transform.Find("itemText").GetComponent<TextMesh>().text != "SOLD")
                    {
                        Affordable();
                        LackOfCredits();
                    }
                    else
                    {
                        SoldOut();
                    }
                }
            }
        }
    }

    private void Affordable()
    {
        if (bank >= Int32.Parse(target.transform.GetComponent<ShopPiece>().ShopSelection.cost))
        {
            Debug.Log("CAN BUY");
            buyButton.SetActive(true);
        }
    }

    private void LackOfCredits()
    {
        if (bank < Int32.Parse(target.transform.Find("itemText").GetComponent<TextMesh>().text))
        {
            Debug.Log("CAN'T BUY");
        }
    }

    private void SoldOut()
    {
        Debug.Log("SOLD OUT");
    }


    private void UpdateDescriptionBox()
    {
        textBoxPanel.transform.Find("name").gameObject.GetComponent<TextMesh>().text = tmpSelection.GetComponentInParent<ShopPiece>().ShopSelection.iconName;
        textBoxPanel.transform.Find("desc").gameObject.GetComponent<TextMesh>().text = tmpSelection.GetComponentInParent<ShopPiece>().ShopSelection.description;


    }

    private void Select()
    {
        tmpSelection = target.transform.Find("SelectionQuad").gameObject;
        tmpSelection.SetActive(true);

        //Debug.Log("Object clicked by player have been highlighted.");
    }


}
