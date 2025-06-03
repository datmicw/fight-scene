using UnityEngine;

// quản lý player trong game
public class PlayerManager : MonoBehaviour
{
    // singleton để truy cập toàn cục
    public static PlayerManager Instance { get; private set; }
    // tham chiếu đến gameobject player
    public GameObject Player { get; private set; }

    private void Awake()
    {
        // kiểm tra singleton, nếu đã tồn tại thì hủy đối tượng này
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        // tìm player theo tag "Player" trong scene
        Player = GameObject.FindGameObjectWithTag("Player");

        if (Player == null)
            Debug.LogError("không tìm thấy player trong scene.");
        else
            Debug.Log("player được gán trong playermanager.");
    }
}
