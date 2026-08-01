using UnityEngine;
using UnityEngine.Events;
[AddComponentMenu("DangSon/GameEvent")]
public static class GameEvent 
{
    public static UnityEvent eventUpdateUI;
    public static UnityEvent<int> eventCoin;
    public static UnityEvent<int> eventHealth;
    public static UnityEvent<int> eventCoinsCompleted;
}
