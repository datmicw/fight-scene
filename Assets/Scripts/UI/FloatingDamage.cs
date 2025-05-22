using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FloatingDamage : MonoBehaviour
{
    public Text damageText;
    public float floatSpeed = 50f;
    public float duration = 1f;

    private float timer;

    public void SetDamage(float damage)
    {
        damageText.text = damage.ToString();
    }

    private void Update()
    {
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;
        timer += Time.deltaTime;

        if (timer >= duration)
        {
            Destroy(gameObject);
        }
    }
}
