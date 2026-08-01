using UnityEngine;

// ════════════════════════════════════════════════════════
//  ITEM – ScriptableObject chứa dữ liệu 1 vật phẩm
//  Tạo bằng: chuột phải > Create > Shop > Item
// ════════════════════════════════════════════════════════
[CreateAssetMenu(fileName = "NewItem", menuName = "Shop/Item")]
public class Item : ScriptableObject
{
    [Header("Thông tin cơ bản")]
    public string itemName   = "Vật phẩm mới";
    public string description = "Mô tả...";
    public Sprite icon;

    [Header("Loại vật phẩm")]
    public ItemType itemType = ItemType.Ammo;

    [Header("Giá (coin)")]
    public int price = 100;

    // ── Chỉ dùng khi ItemType = Ammo ──────────────────
    [Header("Đạn")]
    [Tooltip("Số viên đạn nhận được mỗi lần mua")]
    public int ammoAmount = 30;

    // ── Chỉ dùng khi ItemType = HealthPack ───────────
    [Header("Hồi máu")]
    [Tooltip("Lượng HP được hồi mỗi lần dùng")]
    public int healAmount = 50;

    // ── Chỉ dùng khi ItemType = Skin ─────────────────
    [Header("Skin")]
    [Tooltip("Sprite hiển thị preview skin trong shop")]
    public Sprite skinPreview;
    [Tooltip("Tick = skin mặc định, không cần mua, tự có sẵn")]
    public bool isDefault = false;
}

// ── Enum loại vật phẩm ───────────────────────────────
public enum ItemType
{
    Ammo,       // Đạn        – mua nhiều lần, cộng dồn số viên
    HealthPack, // Hồi máu    – mua nhiều lần, dùng từng cái
    Skin        // Skin        – mỗi skin chỉ mua 1 lần, equip được
}
