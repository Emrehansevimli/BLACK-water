using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    // Singleton eri�im noktas�
    public static UIManager Instance;

    // Tek bir UI metin objesini burada tutar�z
    public TextMeshProUGUI noticeText;

    private void Awake()
    {
        // Singleton yap�s�: Instance'� bu obje olarak ayarla
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Harici scriptlerin metni g�stermesi i�in metod
    public void UyariyiGoster(string mesaj)
    {
        if (noticeText != null)
        {
            noticeText.text = mesaj;
            noticeText.gameObject.SetActive(true);
        }
    }

    // Harici scriptlerin metni gizlemesi i�in metod
    public void UyariyiGizle()
    {
        if (noticeText != null)
        {
            noticeText.gameObject.SetActive(false);
        }
    }
}