using UnityEngine;
using UnityEngine.UI;
using TMPro;

// ════════════════════════════════════════════════════════
//  SHOP ITEM SLOT – Mỗi ô vật phẩm trong cửa hàng
//  Gắn vào Prefab của từng ô vật phẩm
//
//  Prefab tối thiểu cần có:
//  SlotRoot (script này)
//    ├── ItemIcon          Image
//    ├── ItemNameText      TextMeshProUGUI
//    ├── DescriptionText   TextMeshProUGUI
//    ├── PriceText         TextMeshProUGUI
//    ├── OwnedText         TextMeshProUGUI  ← hiện "Đang có: X" (Ammo/Health)
//    ├── BuyButton         Button
//    │     └── BuyBtnText  TextMeshProUGUI
//    ├── EquipButton       Button           ← chỉ cần cho Skin
//    ├── OwnedBadge        GameObject       ← chỉ cần cho Skin ("ĐÃ CÓ")
//    └── EquippedBadge     GameObject       ← chỉ cần cho Skin ("ĐANG MẶC")
// ════════════════════════════════════════════════════════
public class ShopItemSlot : MonoBehaviour
{
    [Header("─ UI dùng cho cả 3 loại ─")]
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Button buyButton;
    [SerializeField] private TextMeshProUGUI buyBtnText;

    [Header("─ Chỉ dùng cho Ammo và HealthPack ─")]
    [Tooltip("Text hiển thị số lượng đang có, ví dụ: 'Đang có: 30 viên'")]
    [SerializeField] private TextMeshProUGUI ownedText;

    [Header("─ Chỉ dùng cho Skin ─")]
    [Tooltip("Badge hiện khi skin đã được mua")]
    [SerializeField] private GameObject ownedBadge;
    [Tooltip("Badge hiện khi skin đang được trang bị")]
    [SerializeField] private GameObject equippedBadge;
    [Tooltip("Nút trang bị skin (hiện sau khi mua)")]
    [SerializeField] private Button equipButton;

    // Item đang được hiển thị trong slot này
    private Item _item;

    // ════════════════════════════════════════════════
    //  SETUP – ShopUI gọi 1 lần sau khi Instantiate
    // ════════════════════════════════════════════════
    public void Setup(Item item)
    {
        _item = item;

        // ── Điền thông tin cơ bản ────────────────────
        if (itemIcon)     itemIcon.sprite = item.icon;
        if (itemNameText) itemNameText.text = item.itemName;

        if (descriptionText)
            descriptionText.text = item.itemType switch
            {
                ItemType.Ammo       => $"{item.description}\n+{item.ammoAmount} viên / lần mua",
                ItemType.HealthPack => $"{item.description}\nHồi {item.healAmount} HP / lần dùng",
                ItemType.Skin       => item.description,
                _                   => item.description
            };

        if (priceText)
            priceText.text = item.isDefault ? "Miễn phí" : $"{item.price} Coin";

        // ── Ẩn/hiện UI theo loại ─────────────────────
        // OwnedText chỉ dùng cho Ammo và HealthPack
        if (ownedText)
            ownedText.gameObject.SetActive(item.itemType != ItemType.Skin);

        // Badge và EquipButton chỉ dùng cho Skin
        if (ownedBadge)    ownedBadge.SetActive(false);
        if (equippedBadge) equippedBadge.SetActive(false);
        if (equipButton)   equipButton.gameObject.SetActive(false);

        // ── Gắn sự kiện nút ──────────────────────────
        if (buyButton)
        {
            buyButton.onClick.RemoveAllListeners();
            buyButton.onClick.AddListener(OnBuyClicked);
            // Skin mặc định không cần nút mua
            buyButton.gameObject.SetActive(!(item.itemType == ItemType.Skin && item.isDefault));
        }

        if (equipButton)
        {
            equipButton.onClick.RemoveAllListeners();
            equipButton.onClick.AddListener(OnEquipClicked);
        }

        // ── Đăng ký event để tự cập nhật ─────────────
        CoinManager.Instance.OnCoinChanged.AddListener(OnCoinChanged);
        Inventory.Instance.OnInventoryChanged.AddListener(RefreshUI);
        Inventory.Instance.OnSkinEquipped.AddListener(OnSkinEquipped);

        // Cập nhật ngay lần đầu
        RefreshUI();
    }

    // ════════════════════════════════════════════════
    //  SỰ KIỆN NÚT
    // ════════════════════════════════════════════════
    private void OnBuyClicked()
    {
        ShopManager.Instance.BuyItem(_item);
        // RefreshUI sẽ được gọi tự động qua OnInventoryChanged
    }

    private void OnEquipClicked()
    {
        Inventory.Instance.EquipSkin(_item);
        // RefreshUI sẽ được gọi tự động qua OnSkinEquipped
    }

    // ════════════════════════════════════════════════
    //  CẬP NHẬT GIAO DIỆN
    // ════════════════════════════════════════════════
    private void OnCoinChanged(int _) => RefreshUI();
    private void OnSkinEquipped(Item _) => RefreshUI();

    private void RefreshUI()
    {
        if (_item == null) return;

        switch (_item.itemType)
        {
            case ItemType.Ammo:
            case ItemType.HealthPack:
                RefreshStackable();
                break;
            case ItemType.Skin:
                RefreshSkin();
                break;
        }
    }

    // Cập nhật UI cho Ammo và HealthPack
    private void RefreshStackable()
    {
        int qty = Inventory.Instance.GetQuantity(_item);

        if (ownedText)
        {
            if (_item.itemType == ItemType.Ammo)
                ownedText.text = qty > 0 ? $"Đang có: {qty} viên" : "Hết đạn";
            else
                ownedText.text = qty > 0 ? $"Đang có: {qty} cái" : "Không có";
        }

        bool canAfford = CoinManager.Instance.HasEnoughCoins(_item.price);
        SetBuyButtonState(canAfford, canAfford ? "Mua" : "Thiếu coin");
    }

    // Cập nhật UI cho Skin
    private void RefreshSkin()
    {
        bool owned    = _item.isDefault || Inventory.Instance.HasItem(_item);
        bool equipped = Inventory.Instance.EquippedSkin == _item;

        // Badges
        if (ownedBadge)    ownedBadge.SetActive(owned && !equipped);
        if (equippedBadge) equippedBadge.SetActive(equipped);

        // Nút MUA: hiện khi chưa sở hữu
        if (buyButton)
            buyButton.gameObject.SetActive(!owned);

        // Nút TRANG BỊ: hiện khi đã sở hữu nhưng chưa mặc
        if (equipButton)
            equipButton.gameObject.SetActive(owned && !equipped);

        // Cập nhật trạng thái nút mua nếu đang hiện
        if (!owned)
        {
            bool canAfford = CoinManager.Instance.HasEnoughCoins(_item.price);
            SetBuyButtonState(canAfford, canAfford ? "Mua skin" : "Thiếu coin");
        }
    }

    // Thay đổi màu + text + interactable của nút Mua
    private void SetBuyButtonState(bool canBuy, string label)
    {
        if (buyButton)
        {
            buyButton.interactable = canBuy;
            var colors = buyButton.colors;
            colors.normalColor = canBuy
                ? new Color(0.18f, 0.75f, 0.38f)  // xanh lá
                : new Color(0.35f, 0.35f, 0.45f); // xám
            buyButton.colors = colors;
        }
        if (buyBtnText) buyBtnText.text = label;
    }

    // ════════════════════════════════════════════════
    //  DỌN DẸP – tránh memory leak
    // ════════════════════════════════════════════════
    private void OnDestroy()
    {
        if (CoinManager.Instance != null)
            CoinManager.Instance.OnCoinChanged.RemoveListener(OnCoinChanged);
        if (Inventory.Instance != null)
        {
            Inventory.Instance.OnInventoryChanged.RemoveListener(RefreshUI);
            Inventory.Instance.OnSkinEquipped.RemoveListener(OnSkinEquipped);
        }
    }
}
