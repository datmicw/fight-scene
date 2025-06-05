using UnityEngine;

// quản lý player trong game
public class PlayerManager : MonoBehaviour
{
    // singleton để truy cập dễ dàng
    public static PlayerManager Instance { get; private set; }
    // lưu trữ player
    public GameObject Player { get; private set; }

    private void Awake()
    {
        // kiểm tra nếu đã có instance thì hủy object này
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        // gán instance
        Instance = this;
    }

    // đăng ký player vào manager
    public void RegisterPlayer(GameObject playerObj)
    {
        Player = playerObj;
        Debug.Log("Player đã được đăng ký vào PlayerManager.");
    }
}
