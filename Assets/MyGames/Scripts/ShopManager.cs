using UnityEngine;
using UnityEngine.Events;

// ════════════════════════════════════════════════════════
//  SHOP MANAGER – Singleton xử lý logic mua bán
//  Gắn vào Empty GameObject tên [ShopManager] trong scene
//  Kéo ItemDatabase vào field "Item Database" trong Inspector
// ════════════════════════════════════════════════════════
public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance { get; private set; }

    [Header("Kéo ItemDatabase ScriptableObject vào đây")]
   // [SerializeField] private ItemDatabase itemDatabase;

    // ShopUI đăng ký 2 event này để hiện thông báo
    [HideInInspector] public UnityEvent<Item> OnPurchaseSuccess
        = new UnityEvent<Item>();
    [HideInInspector] public UnityEvent<Item, PurchaseFailReason> OnPurchaseFailed
        = new UnityEvent<Item, PurchaseFailReason>();

    // ── Vòng đời ────────────────────────────────────
    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    // ════════════════════════════════════════════════
    //  HÀM CHÍNH – ShopItemSlot gọi khi nhấn nút Mua
    // ════════════════════════════════════════════════
    public PurchaseResult BuyItem(Item item, int quantity = 1)
    {
        if (item == null)
            return Fail(null, PurchaseFailReason.InvalidItem);

        // Điều hướng theo loại item
        return item.itemType switch
        {
            ItemType.Ammo       => BuyAmmo(item, quantity),
            ItemType.HealthPack => BuyHealthPack(item, quantity),
            ItemType.Skin       => BuySkin(item),
            _                   => Fail(item, PurchaseFailReason.InvalidItem)
        };
    }

    // ── Mua Đạn ──────────────────────────────────────
    // Mua được nhiều lần, số lượng cộng dồn
    private PurchaseResult BuyAmmo(Item item, int quantity)
    {
        int totalCost = item.price * quantity;

        if (!CoinManager.Instance.HasEnoughCoins(totalCost))
            return Fail(item, PurchaseFailReason.NotEnoughCoins);

        CoinManager.Instance.SpendCoins(totalCost);
        Inventory.Instance.AddItem(item, quantity);

        Debug.Log($"[ShopManager] Mua đạn: {quantity}x {item.itemName}" +
                  $" (+{item.ammoAmount * quantity} viên) -{totalCost} coin");
        return Success(item, quantity, totalCost);
    }

    // ── Mua HealthPack ────────────────────────────────
    // Mua được nhiều lần, dùng từng cái
    private PurchaseResult BuyHealthPack(Item item, int quantity)
    {
        int totalCost = item.price * quantity;

        if (!CoinManager.Instance.HasEnoughCoins(totalCost))
            return Fail(item, PurchaseFailReason.NotEnoughCoins);

        CoinManager.Instance.SpendCoins(totalCost);
        Inventory.Instance.AddItem(item, quantity);

        Debug.Log($"[ShopManager] Mua HealthPack: {quantity}x {item.itemName}" +
                  $" (hồi {item.healAmount} HP mỗi cái) -{totalCost} coin");
        return Success(item, quantity, totalCost);
    }

    // ── Mua Skin ─────────────────────────────────────
    // Mỗi skin chỉ mua được 1 lần
    private PurchaseResult BuySkin(Item item)
    {
        // Skin mặc định không cần mua
        if (item.isDefault)
            return Fail(item, PurchaseFailReason.AlreadyOwned);

        // Đã sở hữu rồi → không mua lại
        if (Inventory.Instance.HasItem(item))
            return Fail(item, PurchaseFailReason.AlreadyOwned);

        if (!CoinManager.Instance.HasEnoughCoins(item.price))
            return Fail(item, PurchaseFailReason.NotEnoughCoins);

        CoinManager.Instance.SpendCoins(item.price);
        Inventory.Instance.AddItem(item); // Inventory tự equip nếu chưa có skin nào

        Debug.Log($"[ShopManager] Mua skin: {item.itemName} -{item.price} coin");
        return Success(item, 1, item.price);
    }

    // ── Helpers ──────────────────────────────────────
    private PurchaseResult Success(Item item, int qty, int cost)
    {
        OnPurchaseSuccess.Invoke(item);
        return new PurchaseResult(true, PurchaseFailReason.None, item, qty, cost);
    }

    private PurchaseResult Fail(Item item, PurchaseFailReason reason)
    {
        Debug.Log($"[ShopManager] Mua thất bại ({reason}): {item?.itemName}");
        OnPurchaseFailed.Invoke(item, reason);
        return new PurchaseResult(false, reason, item, 0, 0);
    }
}

// ════════════════════════════════════════════════════════
//  KẾT QUẢ GIAO DỊCH – trả về sau mỗi lần mua
// ════════════════════════════════════════════════════════
public struct PurchaseResult
{
    public bool Success;
    public PurchaseFailReason FailReason;
    public Item Item;
    public int Quantity;
    public int TotalCost;

    public PurchaseResult(bool success, PurchaseFailReason reason,
                          Item item, int qty, int cost)
    {
        Success = success; FailReason = reason;
        Item = item; Quantity = qty; TotalCost = cost;
    }
}

// ── Lý do mua thất bại ───────────────────────────────
public enum PurchaseFailReason
{
    None,           // Không lỗi (mua thành công)
    NotEnoughCoins, // Không đủ coin
    AlreadyOwned,   // Skin đã sở hữu rồi
    InvalidItem     // Item null hoặc không hợp lệ
}
