using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// ════════════════════════════════════════════════════════
//  INVENTORY – Singleton quản lý túi đồ người chơi
//  Gắn vào Empty GameObject tên [Inventory] trong scene
//
//  Ammo / HealthPack : lưu Dictionary<Item, int> (số lượng)
//  Skin              : lưu List<Item> đã sở hữu + EquippedSkin
// ════════════════════════════════════════════════════════
public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }

    // Đạn và HealthPack: lưu số lượng
    private Dictionary<Item, int> _stackableItems = new Dictionary<Item, int>();

    // Skin: danh sách đã sở hữu
    private List<Item> _ownedSkins = new List<Item>();

    // Skin đang trang bị hiện tại
    public Item EquippedSkin { get; private set; }

    // ── Events ──────────────────────────────────────
    // Bắn khi túi đồ thay đổi (ShopItemSlot lắng nghe để refresh UI)
    public UnityEvent OnInventoryChanged = new UnityEvent();
    // Bắn khi equip skin (PlayerRenderer lắng nghe để đổi sprite)
    public UnityEvent<Item> OnSkinEquipped = new UnityEvent<Item>();

    // ── Vòng đời ────────────────────────────────────
    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // ════════════════════════════════════════════════
    //  THÊM VẬT PHẨM – ShopManager gọi sau khi mua
    // ════════════════════════════════════════════════
    public void AddItem(Item item, int quantity = 1)
    {
        if (item == null || quantity <= 0) return;

        switch (item.itemType)
        {
            case ItemType.Ammo:
            case ItemType.HealthPack:
                if (_stackableItems.ContainsKey(item))
                    _stackableItems[item] += quantity;
                else
                    _stackableItems[item] = quantity;
                Debug.Log($"[Inventory] +{quantity}x {item.itemName} → còn {_stackableItems[item]}");
                break;

            case ItemType.Skin:
                if (!_ownedSkins.Contains(item))
                {
                    _ownedSkins.Add(item);
                    Debug.Log($"[Inventory] Mở khoá skin: {item.itemName}");

                    // Tự trang bị nếu đây là skin đầu tiên
                    if (EquippedSkin == null)
                        EquipSkin(item);
                }
                break;
        }

        OnInventoryChanged.Invoke();
    }

    // ════════════════════════════════════════════════
    //  DÙNG VẬT PHẨM – script gameplay gọi
    //  Ví dụ: bắn súng gọi UseItem(ammoItem, 1)
    //         dùng thuốc gọi UseItem(healthItem, 1)
    // ════════════════════════════════════════════════
    public bool UseItem(Item item, int quantity = 1)
    {
        if (item == null) return false;
        if (item.itemType == ItemType.Skin)
        {
            Debug.Log("[Inventory] Skin không thể dùng kiểu này, hãy dùng EquipSkin()");
            return false;
        }

        if (!HasItem(item, quantity))
        {
            Debug.Log($"[Inventory] Không đủ {item.itemName}");
            return false;
        }

        _stackableItems[item] -= quantity;
        if (_stackableItems[item] <= 0)
            _stackableItems.Remove(item);

        OnInventoryChanged.Invoke();
        return true;
    }

    // ════════════════════════════════════════════════
    //  TRANG BỊ SKIN
    // ════════════════════════════════════════════════
    public bool EquipSkin(Item skin)
    {
        if (skin == null || skin.itemType != ItemType.Skin) return false;

        // Phải sở hữu skin, trừ skin mặc định
        if (!_ownedSkins.Contains(skin) && !skin.isDefault)
        {
            Debug.Log($"[Inventory] Chưa sở hữu skin: {skin.itemName}");
            return false;
        }

        EquippedSkin = skin;
        Debug.Log($"[Inventory] Đang mặc skin: {skin.itemName}");
        OnSkinEquipped.Invoke(skin);
        OnInventoryChanged.Invoke();
        return true;
    }

    // ════════════════════════════════════════════════
    //  TRUY VẤN
    // ════════════════════════════════════════════════

    /// <summary>
    /// Kiểm tra có vật phẩm không.
    /// Ammo/Health: kiểm tra số lượng >= quantity.
    /// Skin: kiểm tra đã sở hữu chưa.
    /// </summary>
    public bool HasItem(Item item, int quantity = 1)
    {
        if (item == null) return false;
        if (item.itemType == ItemType.Skin)
            return _ownedSkins.Contains(item);
        return _stackableItems.ContainsKey(item) && _stackableItems[item] >= quantity;
    }

    /// <summary>
    /// Số lượng hiện có.
    /// Ammo: tổng viên đạn. HealthPack: số cái. Skin: 1 nếu có, 0 nếu không.
    /// </summary>
    public int GetQuantity(Item item)
    {
        if (item == null) return 0;
        if (item.itemType == ItemType.Skin)
            return _ownedSkins.Contains(item) ? 1 : 0;
        return _stackableItems.ContainsKey(item) ? _stackableItems[item] : 0;
    }

    public List<Item> GetOwnedSkins()
        => new List<Item>(_ownedSkins);

    public Dictionary<Item, int> GetAllStackableItems()
        => new Dictionary<Item, int>(_stackableItems);
}
