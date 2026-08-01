using UnityEngine;
using UnityEngine.Events;

// ════════════════════════════════════════════════════════
//  COIN MANAGER – Singleton quản lý coin người chơi
//  Gắn vào Empty GameObject tên [CoinManager] trong scene
// ════════════════════════════════════════════════════════
public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance { get; private set; }

    [Header("Coin ban đầu khi bắt đầu game")]
    [SerializeField] private int startingCoins = 500;

    // Các script khác đăng ký event này để tự cập nhật UI
    public UnityEvent<int> OnCoinChanged = new UnityEvent<int>();

    private int _currentCoins;

    // Đọc coin hiện tại (chỉ đọc từ bên ngoài)
    public int CurrentCoins => _currentCoins;

    // ── Vòng đời ────────────────────────────────────
    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
    }
    private void Start()
    {
        LoadCoins();
    }
    // ── API công khai ────────────────────────────────

    /// <summary>Kiểm tra người chơi có đủ coin không</summary>
    public bool HasEnoughCoins(int amount) => _currentCoins >= amount;

    /// <summary>
    /// Trừ coin. Trả về true nếu thành công.
    /// Chỉ ShopManager gọi hàm này.
    /// </summary>
    public bool SpendCoins(int amount)
    {
        if (!HasEnoughCoins(amount))
        {
            Debug.Log($"[CoinManager] Không đủ coin! Cần {amount}, có {_currentCoins}");
            return false;
        }
        SetCoins(_currentCoins - amount);
        return true;
    }

    /// <summary>Cộng coin – gọi khi nhặt coin, nhận thưởng...</summary>
    public void AddCoins(int amount)
    {
        if (amount <= 0) return;
        SetCoins(_currentCoins + amount);
        Debug.Log($"[CoinManager] +{amount} coin → tổng {_currentCoins}");
    }

    // ── Nội bộ ──────────────────────────────────────
    private void SetCoins(int value)
    {
        _currentCoins = Mathf.Max(0, value);
        OnCoinChanged.Invoke(_currentCoins);
        SaveCoins();
    }

    private void SaveCoins() => DataManager.DataCoin = _currentCoins;

    public int LoadCoins()
    {
        _currentCoins = DataManager.DataCoin;
        OnCoinChanged.Invoke(_currentCoins);
        return _currentCoins;
    }

    // ── Test nhanh trong Editor ──────────────────────
    [ContextMenu("Test: +100 Coin")]
    private void TestAdd() => AddCoins(100);

    [ContextMenu("Test: Reset coin về mặc định")]
    private void TestReset() { PlayerPrefs.DeleteKey("PlayerCoins"); LoadCoins(); }
}
