using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Purchasing;
public class InAppManager : MonoBehaviour
{

    private static InAppManager instance;
    public static InAppManager Instance
    {
        get=> instance ?? (instance = new InAppManager());
    }
    
    StoreController m_StoreController; // The Unity Purchasing system.

    //Your products IDs. They should match the ids of your products in your store.
    public string ProductId50 = "com.tagame.platfrom.50";
    public string ProductId100 = "com.tagame.platfrom.100";
    public string ProductId500 = "com.tagame.platfrom.500";
    public string ProductId1000 = "com.tagame.platfrom.1000";
    public string ProductId2000 = "com.tagame.platfrom.2000";
    public string ProductId5000 = "com.tagame.platfrom.5000";
    public string ProductRemoveAds = "com.tagame.platfrom.removeads";
    //
    public bool isSubscribed = false;
    public string subscriptionProductId = "com.mycompany.mygame.my_vip_pass_subscription";
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            instance = null;
        }
        else
        {
            instance = this;
        }
        InitializeIAP();
    }

    async void InitializeIAP()
    {
        m_StoreController = UnityIAPServices.StoreController();

        m_StoreController.OnPurchasePending += OnPurchasePending;
        m_StoreController.OnPurchaseConfirmed += OnPurchaseConfirmed;
        m_StoreController.OnPurchaseFailed += OnPurchaseFailed;
        m_StoreController.OnCheckEntitlement += OnCheckEntitlement;
        m_StoreController.OnStoreConnected += OnStoreConnected;
        m_StoreController.OnStoreDisconnected += OnStoreDisconnected;

        m_StoreController.OnProductsFetchFailed += OnProductsFetchedFailed;
        m_StoreController.OnProductsFetched += OnProductsFetched;

        Debug.Log("Connecting to store.");
        await m_StoreController.Connect();
        //viet them code xu ly khi bat dau ket noi den cua hang nhu hien thong bao len UI, hoac co the hien thi 1 loading den khi ket noi thanh cong

    }

    void FetchProducts()
    {
        var initialProductsToFetch = new List<ProductDefinition>
            {
                new(ProductId50 , ProductType.Consumable),
                new(ProductId100, ProductType.Consumable),
                new(ProductId500, ProductType.Consumable),
                new(ProductId1000, ProductType.Consumable),
                new(ProductId2000, ProductType.Consumable),
                new(ProductId5000, ProductType.Consumable),
                new(ProductRemoveAds, ProductType.NonConsumable),
                //
                 new(subscriptionProductId, ProductType.Subscription)
            };

        m_StoreController.FetchProducts(initialProductsToFetch);
        //viet them code xu ly khi fetch san pham nhu hien thong bao len UI, hoac co the luu thong tin san pham vao 1 bien de su dung sau nay
    }

    public void BuyProductId50()
    {
        //m_StoreController.PurchaseProduct(goldProductId);
        m_StoreController.PurchaseProduct(ProductId50);
    }

    public void BuyProductId100()
    {  
        m_StoreController.PurchaseProduct(ProductId100);
    }
    public void BuyProductId500()
    {
        m_StoreController.PurchaseProduct(ProductId500);
    }
    public void BuyProductId1000()
    {
        m_StoreController.PurchaseProduct(ProductId1000);
    }
    public void BuyProductId2000()
    {
        m_StoreController.PurchaseProduct(ProductId2000);
    }
    public void BuyProductId5000()
    {
        m_StoreController.PurchaseProduct(ProductId5000);
    }
    void OnPurchaseFailed(FailedOrder order)
    {
        var product = GetFirstProductInOrder(order);
        if (product == null)
        {
            Debug.Log("Could not find product in failed order.");
        }

        Debug.Log($"Purchase failed - Product: '{product?.definition.id}'," +
                  $"PurchaseFailureReason: {order.FailureReason.ToString()},"
                  + $"Purchase Failure Details: {order.Details}");
        //viet them code xu ly khi mua hang that bai nhu hien thong bao len UI, hoac co the goi lai ham BuyProductId de thu lai
    }

    void OnPurchasePending(PendingOrder order)
    {
        var product = GetFirstProductInOrder(order);
        if (product is null)
        {
            Debug.Log("Could not find product in order.");
            return;
        }

        //Add the purchased product to the players inventory
        if (product.definition.id == ProductId50)
        {
            AddroductId50();
        }
        else if (product.definition.id == ProductId100)
        {
            AddroductId100();
        }
        else if (product.definition.id == ProductId500)
        {
            AddroductId500();
        }
        else if (product.definition.id == ProductId1000)
        {
            AddroductId1000();
        }
        else if (product.definition.id == ProductId2000)
        {
            AddroductId2000();
        }
        else if (product.definition.id == ProductId5000)
        {
            AddroductId5000();
        }
        else if (product.definition.id == ProductRemoveAds)
        {
            //viet code xu ly khi mua san pham remove ads nhu hien thong bao len UI, hoac co the luu thong tin san pham da mua vao 1 bien de su dung sau nay
            ProductRemoveAdsId();
        }
        else if(product.definition.id == subscriptionProductId)
        {
            CheckSubscription();
            //viet code xu ly khi mua san pham subscription nhu hien thong bao len UI, hoac co the luu thong tin san pham da mua vao 1 bien de su dung sau nay
        }

        Debug.Log($"Purchase complete - Product: {product.definition.id}");

        m_StoreController.ConfirmPurchase(order);
        //viet them code xu ly khi mua hang thanh cong nhu hien thong bao len UI, hoac co the luu thong tin san pham da mua vao 1 bien de su dung sau nay
    }

    private void ProductRemoveAdsId()
    {
        //viet code xu ly khi mua san pham remove ads nhu hien thong bao len UI, hoac co the luu thong tin san pham da mua vao 1 bien de su dung sau nay
    }

    private void CheckSubscription()
    {
        //viet code xu ly khi mua san pham subscription nhu hien thong bao len UI, hoac co the luu thong tin san pham da mua vao 1 bien de su dung sau nay

    }

    private void AddroductId2000()
    { 
       DataManager.DataCoin += 2000;
    }

    private void AddroductId5000()
    {
       DataManager.DataCoin += 5000;
    }

    private void AddroductId1000()
    {
        DataManager.DataCoin += 1000;
    }

    private void AddroductId500()
    {
        DataManager.DataCoin += 500;
    }

    private void AddroductId100()
    {
        DataManager.DataCoin += 100;
    }

    private void AddroductId50()
    {
        DataManager.DataCoin += 50;
    }

    void OnPurchaseConfirmed(Order order)
    {
        switch (order)
        {
            case ConfirmedOrder confirmedOrder:
                OnPurchaseConfirmed(confirmedOrder);
                break;
            case FailedOrder failedOrder:
                OnPurchaseConfirmationFailed(failedOrder);
                break;
            default:
                Debug.Log("Unknown OnPurchaseConfirmed result.");
                break;
        }
        //viet them code xu ly khi xac nhan mua hang nhu tren thanh cong hay that bai nhu hien thong bao len UI, hoac co the goi lai ham ConfirmPurchase de thu lai
    }

    void OnPurchaseConfirmed(ConfirmedOrder order)
    {
        var product = GetFirstProductInOrder(order);
        if (product == null)
        {
            Debug.Log("Could not find product in purchase confirmation.");
        }

        Debug.Log($"Purchase confirmed- Product: {product?.definition.id}");
        //viet them code xu ly khi xac nhan mua hang thanh cong nhu hien thong bao len UI, hoac co the luu thong tin san pham da mua vao 1 bien de su dung sau nay
    }

    void OnPurchaseConfirmationFailed(FailedOrder order)
    {
        var product = GetFirstProductInOrder(order);
        if (product == null)
        {
            Debug.Log("Could not find product in failed confirmation.");
        }

        Debug.Log($"Confirmation failed - Product: '{product?.definition.id}'," +
                  $"PurchaseFailureReason: {order.FailureReason.ToString()},"
                  + $"Confirmation Failure Details: {order.Details}");
        //viet them code xu ly khi xac nhan mua hang that bai nhu hien thong bao len UI, hoac co the goi lai ham ConfirmPurchase de thu lai
    }

    Product GetFirstProductInOrder(Order order)
    {
        return order.CartOrdered.Items().First()?.Product;
    }

    // Calling StoreController.Connect without a listener on the StoreController.OnStoreConnect event will result in warnings.
    void OnStoreConnected()
    {
        Debug.Log($"Store Connected.");
        FetchProducts();
        //viet them code xu ly khi ket noi den cua hang thanh cong nhu hien thong bao len UI, hoac co the fetch san pham tu cua hang de hien thi len UI
    }

    // Calling StoreController.Connect without a listener on the StoreController.OnStoreDisconnected event will result in warnings.
    void OnStoreDisconnected(StoreConnectionFailureDescription description)
    {
        Debug.Log($"Store disconnected details: {description.message}");
        //viet them code xu ly khi mat ket noi den cua hang nhu hien thong bao len UI, hoac co the goi lai ham Connect de thu ket noi lai
    }

    // Calling StoreController.Connect without listeners on StoreController.OnProductsFetched and StoreController.OnProductsFetchedFailed will result in warnings.
    void OnProductsFetched(List<Product> products)
    {
        Debug.Log($"Products fetched successfully for {products.Count} products.");
        //viet them code xu ly khi fetch san pham thanh cong nhu hien thong bao len UI, hoac luu thong tin san pham vao 1 bien de su dung sau nay
    }

    void OnProductsFetchedFailed(ProductFetchFailed failure)
    {
        Debug.Log($"Products fetch failed for {failure.FailedFetchProducts.Count} products: {failure.FailureReason}");
        ///viet them code xu ly khi fetch san pham that bai nhu hien thong bao len UI, hoac co the goi lai ham FetchProducts de thu lai
    }
    void OnCheckEntitlement(Entitlement entitlement)
    {
        if (entitlement.Product.definition.id == subscriptionProductId)
        {
            switch (entitlement.Status)
            {
                case EntitlementStatus.FullyEntitled:
                    isSubscribed = true;
                   // UpdateUI();
                    break;
                default:
                    isSubscribed = false;
                   // UpdateUI();
                    break;
            }
        }
    }
}
