using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
[AddComponentMenu("DangSon/GameManager")]
public class GameManager : MonoBehaviour
{
    [Header("Game Manager")]
    public GameObject ninjaPrefabs;
    public GameObject knightPrefabs;

    [Header("Player Spawm")]
    public Transform spawnPoint;
    private int coin;
    
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<GameManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("GameManager");
                    instance = obj.AddComponent<GameManager>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
      
        SapwmCharacter();
        if (GameEvent.eventCoin==null)
        {
          GameEvent.eventCoin=new UnityEvent<int>();
        }
       if(GameEvent.eventHealth == null) 
       {
          GameEvent.eventHealth=new UnityEvent<int>();
       }
       if(GameEvent.eventUpdateUI==null)
       {
          GameEvent.eventUpdateUI=new UnityEvent();
       }
       if(GameEvent.eventCoinsCompleted==null) 
       {
          GameEvent.eventCoinsCompleted=new UnityEvent<int>();
       }
        this.coin = DataManager.DataCoin;
        GameEvent.eventCoin.AddListener(AddCoin);
    }

    private void SapwmCharacter()
    {
        string character = CharacterSelector.Instance.GetSelectedCharacter();
        GameObject prefab = null;
        if(character=="Ninja")
        {
         prefab = ninjaPrefabs;
        }
        else if(character=="Knight")
        {
         prefab = knightPrefabs;
        }
        if(prefab!=null)
        {
           Vector3 pos = spawnPoint !=null ? spawnPoint.position : Vector3.zero;
           Instantiate(prefab, pos, quaternion.identity);
        }
    }
    public void AddCoin(int amount)
    {
        coin += amount;
        DataManager.DataCoin = coin;
        GameEvent.eventCoinsCompleted?.Invoke(coin);
    }
    public int LoadCoin()
    {             
       return coin;
    }
}
