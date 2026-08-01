using System.Collections;
using TMPro;
using UnityEngine;

public class UIShopManager : MonoBehaviour
{
    public TextMeshProUGUI coinText;
    private void Start()
    {
        UpdateCoin();
        StartCoroutine(UpdateCoin());
    }

    public void OnClickBuy50()
    {
       InAppManager.Instance.BuyProductId50();
    }
    public void OnClickBuy100()
    {
       InAppManager.Instance.BuyProductId100();
    }    
    public void OnClickBuy500()
    {
       InAppManager.Instance.BuyProductId500();
    }
    public void OnClickBuy1000()
    {
        InAppManager.Instance.BuyProductId1000();
    }
    public void OnClickBuy2000()
    {
        InAppManager.Instance.BuyProductId2000();
    }
    public void OnClickBuy5000()
    {
       InAppManager.Instance.BuyProductId5000();
    }
    IEnumerator UpdateCoin()
    {
        while (true)
        {
            coinText.text = DataManager.DataCoin.ToString();
            yield return new WaitForSeconds(0.5f);
        }
    }
}
