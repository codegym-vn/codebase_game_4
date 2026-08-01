using UnityEngine;
using UnityEngine.UI;
using TMPro;

// ════════════════════════════════════════════════════════
//  SHOP UI – Giao diện cửa hàng với 3 tab
//  Gắn vào ShopPanel (Panel chứa toàn bộ cửa hàng)
//
//  Hierarchy cần có:
//  ShopPanel (script này)
//    ├── CoinText            (TextMeshPro)
//    ├── TabAmmoBtn          (Button)
//    ├── TabHealthBtn        (Button)
//    ├── TabSkinBtn          (Button)
//    ├── AmmoPanel           (GameObject)
//    │     └── ScrollView → Viewport → AmmoContent   ← ammoContainer
//    ├── HealthPanel         (GameObject)
//    │     └── ScrollView → Viewport → HealthContent ← healthContainer
//    ├── SkinPanel           (GameObject)
//    │     └── ScrollView → Viewport → SkinContent   ← skinContainer
//    └── NotificationPanel   (GameObject)
//          └── NotifText     (TextMeshPro)
// ════════════════════════════════════════════════════════
public class ShopUI : MonoBehaviour
{
    [Header("─ Coin ─")]
    [SerializeField] private TextMeshProUGUI coinText;

    [Header("─ Tab Buttons ─")]
    [SerializeField] private Button tabAmmoBtn;
    [SerializeField] private Button tabHealthBtn;
    [SerializeField] private Button tabSkinBtn;

    [Header("─ Panels (bật/tắt khi chuyển tab) ─")]
    [SerializeField] private GameObject ammoPanel;
    [SerializeField] private GameObject healthPanel;
    [SerializeField] private GameObject skinPanel;

    [Header("─ Containers (Content của ScrollView) ─")]
    [Tooltip("AmmoContent – nơi ShopItemSlot đạn được tạo vào")]
    [SerializeField] private Transform ammoContainer;
    [Tooltip("HealthContent – nơi ShopItemSlot hồi máu được tạo vào")]
    [SerializeField] private Transform healthContainer;
    [Tooltip("SkinContent – nơi ShopItemSlot skin được tạo vào")]
    [SerializeField] private Transform skinContainer;

    [Header("─ Prefabs ─")]
    [Tooltip("Prefab ô vật phẩm (có gắn ShopItemSlot.cs)")]
    [SerializeField] private GameObject ammoSlotPrefab;
    [SerializeField] private GameObject healthSlotPrefab;
    [SerializeField] private GameObject skinSlotPrefab;

    [Header("─ Thông báo ─")]
    [SerializeField] private GameObject notificationPanel;
    [SerializeField] private TextMeshProUGUI notificationText;
    [SerializeField] private float notificationDuration = 2f;

    [Header("─ Dữ liệu ─")]
    [Tooltip("Kéo ItemDatabase ScriptableObject vào đây")]
    [SerializeField] private ItemDatabase itemDatabase;

    // ── Vòng đời ────────────────────────────────────
    private void Start()
    {
        // Lắng nghe thay đổi coin → cập nhật text
        CoinManager.Instance.OnCoinChanged.AddListener(UpdateCoinDisplay);
        UpdateCoinDisplay(CoinManager.Instance.CurrentCoins);

        // Lắng nghe kết quả mua → hiện thông báo
        ShopManager.Instance.OnPurchaseSuccess.AddListener(OnBuySuccess);
        ShopManager.Instance.OnPurchaseFailed.AddListener(OnBuyFailed);

        // Gắn tab buttons
        tabAmmoBtn?.onClick.AddListener(() => ShowTab(ItemType.Ammo));
        tabHealthBtn?.onClick.AddListener(() => ShowTab(ItemType.HealthPack));
        tabSkinBtn?.onClick.AddListener(() => ShowTab(ItemType.Skin));

        // Tạo tất cả slot vật phẩm vào container tương ứng
        PopulateAll();

        // Mặc định hiện tab Đạn
        ShowTab(ItemType.Ammo);

        if (notificationPanel) notificationPanel.SetActive(false);
    }

    // ── Chuyển tab ───────────────────────────────────
    private void ShowTab(ItemType type)
    {
        if (ammoPanel)   ammoPanel.SetActive(type == ItemType.Ammo);
        if (healthPanel) healthPanel.SetActive(type == ItemType.HealthPack);
        if (skinPanel)   skinPanel.SetActive(type == ItemType.Skin);
    }

    // ── Cập nhật coin text ───────────────────────────
    private void UpdateCoinDisplay(int amount)
    {
        if (coinText) coinText.text = $" {amount:N0}";
    }

    // ── Tạo slot vật phẩm ────────────────────────────
    private void PopulateAll()
    {
        PopulateContainer(ammoContainer,   ammoSlotPrefab,   ItemType.Ammo);
        PopulateContainer(healthContainer, healthSlotPrefab, ItemType.HealthPack);
        PopulateContainer(skinContainer,   skinSlotPrefab,   ItemType.Skin);
    }

    private void PopulateContainer(Transform container, GameObject prefab, ItemType type)
    {
        if (container == null || prefab == null || itemDatabase == null) return;

        // Xóa slot cũ nếu có
        foreach (Transform child in container)
            Destroy(child.gameObject);

        // Tạo 1 slot cho mỗi item trong database
        foreach (Item item in itemDatabase.GetItemsByType(type))
        {
            GameObject slot = Instantiate(prefab, container);
            ShopItemSlot slotScript = slot.GetComponent<ShopItemSlot>();
            if (slotScript != null)
                slotScript.Setup(item);
            else
                Debug.LogWarning($"[ShopUI] Prefab '{prefab.name}' thiếu component ShopItemSlot!");
        }
    }

    // ── Thông báo sau khi mua ────────────────────────
    private void OnBuySuccess(Item item)
    {
        string msg = item.itemType switch
        {
            ItemType.Ammo       => $"{item.ammoAmount} viên đạn!",
            ItemType.HealthPack => $"Mua {item.itemName} (hồi {item.healAmount} HP)",
            ItemType.Skin       => $"Mở khoá skin {item.itemName}!",
            _                   => $"Mua {item.itemName} thành công"
        };
        ShowNotification(msg, Color.green);
    }

    private void OnBuyFailed(Item item, PurchaseFailReason reason)
    {
        string msg = reason switch
        {
            PurchaseFailReason.NotEnoughCoins => "Không đủ coin!",
            PurchaseFailReason.AlreadyOwned   => "Skin này đã sở hữu rồi!",
            PurchaseFailReason.InvalidItem    => "Vật phẩm không hợp lệ!",
            _                                  => "Không thể mua!"
        };
        ShowNotification(msg, Color.red);
    }

    private void ShowNotification(string message, Color color)
    {
        if (!notificationPanel) return;
        notificationText.text  = message;
        notificationText.color = color;
        notificationPanel.SetActive(true);
        CancelInvoke(nameof(HideNotification));
        Invoke(nameof(HideNotification), notificationDuration);
    }

    private void HideNotification()
    {
        if (notificationPanel) notificationPanel.SetActive(false);
    }

    // ── Mở / Đóng shop từ nút bên ngoài ─────────────
    public void ToggleShop() => gameObject.SetActive(!gameObject.activeSelf);

    // ── Dọn event khi bị hủy ────────────────────────
    private void OnDestroy()
    {
        if (CoinManager.Instance != null)
            CoinManager.Instance.OnCoinChanged.RemoveListener(UpdateCoinDisplay);
        if (ShopManager.Instance != null)
        {
            ShopManager.Instance.OnPurchaseSuccess.RemoveListener(OnBuySuccess);
            ShopManager.Instance.OnPurchaseFailed.RemoveListener(OnBuyFailed);
        }
    }
}
