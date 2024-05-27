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


    private void Start()
    {
        TurnOffSelectionHighlights();

        textBoxPanel = GameObject.Find("textBoxPanel");
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
                }
            }
        }
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
