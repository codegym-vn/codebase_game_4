using System.Collections.Generic;
using UnityEngine;

// ════════════════════════════════════════════════════════
//  ITEM DATABASE – ScriptableObject chứa toàn bộ item
//  Tạo bằng: chuột phải > Create > Shop > Item Database
// ════════════════════════════════════════════════════════
[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Shop/Item Database")]
public class ItemDatabase : ScriptableObject
{
    [Tooltip("Kéo tất cả Item ScriptableObject vào đây")]
    public List<Item> allItems = new List<Item>();

    // Lấy item theo loại – dùng bởi ShopUI để populate từng tab
    public List<Item> GetItemsByType(ItemType type)
        => allItems.FindAll(i => i.itemType == type);

    // Shortcut tiện lợi
    public List<Item> GetAmmoItems()       => GetItemsByType(ItemType.Ammo);
    public List<Item> GetHealthPackItems() => GetItemsByType(ItemType.HealthPack);
    public List<Item> GetSkinItems()       => GetItemsByType(ItemType.Skin);

    // Tìm theo tên – dùng khi cần tra cứu từ script khác
    public Item GetItemByName(string itemName)
        => allItems.Find(i => i.itemName == itemName);
}
