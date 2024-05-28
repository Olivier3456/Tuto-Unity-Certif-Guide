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

    // For ads
    private string placementId_rewardedVideo = "rewardedVideo";
    string gameId = "1234567";


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

        // For ads
        CheckPlatform();
    }


    private void CheckPlatform()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            gameId = "PLACEHOLDER_IPHONE_GAME_ID";
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            gameId = "PLACEHOLDER_ANDROID_GAME_ID";
        }

        //Monetization.Initialize(gameId, false);
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
                else if (target.name == "BUY ?")
                {
                    BuyItem();
                }
                else if (target.name == "WATCH AD")
                {
                    WatchAdvert();
                }
                else if (target.name == "START")
                {
                    StartGame();
                }
            }
        }
    }

    private void WatchAdvert()
    {
        //if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            ShowRewardedAds();
        }
    }

    private void ShowRewardedAds()
    {
        StartCoroutine(WaitForAd());
    }

    private IEnumerator WaitForAd()
    {
        string placementId = placementId_rewardedVideo;
        //while (!Monetization.IsReady(placementId))
        {
            yield return null;
        }

        //ShowAdPlacementContent ad = null;
        //ad = Monetization.GetPlacementContent(placementId) as ShowAdPlacementContent;
        //if (ad != null)
        //{
        //    ad.Show(AdFinished);
        //}


        // Calling the placeholder function instead of the real one:
        AdFinished();
    }

    // Real function declaration:
    // private void AdFinished(ShowResult result)
    // Placeholder function declaration:
    private void AdFinished()
    {
        //if (result == ShowResult.Finished)
        {
            bank += 300;
            bankObj.GetComponentInChildren<TextMesh>().text = bank.ToString();
            TurnOffSelectionHighlights();
        }
    }






    private void BuyItem()
    {
        Debug.Log("PURCHASED");
        purchaseMade = true;
        buyButton.SetActive(false);
        tmpSelection.SetActive(false);

        for (int i = 0; i < visualWeapons.Length; i++)
        {
            if (visualWeapons[i].name == tmpSelection.transform.parent.gameObject.GetComponent<ShopPiece>().ShopSelection.iconName)
            {
                visualWeapons[i].SetActive(true);
            }
        }

        UpgradeToShip(tmpSelection.transform.parent.gameObject.GetComponent<ShopPiece>().ShopSelection.iconName);

        bank -= Int32.Parse(tmpSelection.transform.parent.GetComponent<ShopPiece>().ShopSelection.cost);
        bankObj.transform.Find("bankText").GetComponent<TextMesh>().text = bank.ToString();
        tmpSelection.transform.parent.transform.Find("itemText").GetComponent<TextMesh>().text = "SOLD";
    }

    private void StartGame()
    {
        if (purchaseMade)
        {
            playerShip.name = "UpgradedShip";
            if (playerShip.transform.Find("energy +1(Clone)"))
            {
                playerShip.GetComponent<Player>().Health = 2;
                Debug.Log("Player has a health of 2");
            }
            DontDestroyOnLoad(playerShip);
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene("testLevel");
    }

    private void UpgradeToShip(string upgrade)
    {
        GameObject shipItem = GameObject.Instantiate(Resources.Load($"Prefab/Player/{upgrade}")) as GameObject;
        shipItem.transform.SetParent(playerShip.transform);
        shipItem.transform.localPosition = Vector3.zero;
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
