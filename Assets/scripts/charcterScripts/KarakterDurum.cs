using UnityEngine;
using UnityEngine.UI;


public class KarakterDurum : MonoBehaviour
{
    [Header("Durum Degerleri")]
    public float maksimumCan = 100f;
    public float mevcutIhtiyac = 0f;
    public float ihtiyacArtisHizi = 1f; // Saniye basina artis
    public float giderilmeMiktari = 100f; // Tek seferde giderilen miktar
    public Slider canBariSlider;
    public Slider staminaBariSlider;

    [Header("Maksimum Degerler")]
    private float _mevcutCan;
    public float maksimumIhtiyac = 100f;
    public float kritikSeviye = 80f;
    [Header("Stamina Degerleri")]
    public float maksimumStamina = 100f;
    [SerializeField] 
    private float _mevcutStamina;
    public float staminaTuketimHizi = 20f; // Saniyede ne kadar harcayacaðý
    public float staminaYenilenmeHizi = 15f;
    [HideInInspector]
    public bool staminaKullaniliyor = false;

    void Start()
    {
        _mevcutCan = 50;
        _mevcutStamina = maksimumStamina; // Staminayý da doldur
        GuncelleCanBari();
        GuncelleStaminaBari();
    }
    void Update()
    {
        mevcutIhtiyac += ihtiyacArtisHizi * Time.deltaTime;
        mevcutIhtiyac = Mathf.Min(mevcutIhtiyac, maksimumIhtiyac);
        StaminaYonetimi();

    }
    private void GuncelleCanBari()
    {
        
        if (canBariSlider != null)
        {
            float canYuzdesi = _mevcutCan / maksimumCan;
            canBariSlider.value = canYuzdesi;
        }
    }
    private void StaminaYonetimi()
    {
        if (staminaKullaniliyor)
        {
            // Hýzlý koþuyorsak staminayý tüket
            _mevcutStamina -= staminaTuketimHizi * Time.deltaTime;
        }
        else
        {
            // Koþmuyorsak staminayý yenile
            _mevcutStamina += staminaYenilenmeHizi * Time.deltaTime;
        }

        // Deðeri 0 ile maksimum arasýnda sýkýþtýr
        _mevcutStamina = Mathf.Clamp(_mevcutStamina, 0f, maksimumStamina);

        GuncelleStaminaBari();
    }

    // Stamina olup olmadýðýný kontrol eden public metot
    public bool StaminaVarMi()
    {
        // Sadece 0'dan büyükse deðil, biraz pay býrakarak kontrol etmek daha iyi olabilir
        return _mevcutStamina > 0.1f;
    }

    private void GuncelleStaminaBari()
    {
        if (staminaBariSlider != null)
        {
            float staminaYuzdesi = _mevcutStamina / maksimumStamina;
            staminaBariSlider.value = staminaYuzdesi;
        }
    }
    public bool IhtiyaciGider()
    {
        if (mevcutIhtiyac >= giderilmeMiktari * 0.1f) // Cok az da olsa ihtiyac varsa
        {
            mevcutIhtiyac = 0f; // Tamamen sifirla

            Debug.Log("Tuvalet ihtiyaci giderildi ve sifirlandi.");
            return true;
        }

        Debug.Log("Henüz tuvalet yapmaya yeterli ihtiyac yok.");
        return false;
    }
  
    public void CanArtir(float miktar)
    {
        if (miktar <= 0)
        {
            Debug.LogWarning("Can artirma miktari pozitif olmalidir.");
            return;
        }       
        _mevcutCan += miktar;    
        _mevcutCan = Mathf.Min(_mevcutCan, maksimumCan);
        Debug.Log($"{miktar} can kazanildi. Yeni can degeri: {_mevcutCan} / {maksimumCan}");
        GuncelleCanBari();
    }

    public void HasarAl(float hasar)
    {
        if (hasar <= 0) return;

        _mevcutCan -= hasar;

        GuncelleCanBari();
        
        if (_mevcutCan <= 0)
        {
            _mevcutCan = 0;
            Olüm();
        }

        Debug.Log($"{hasar} hasar alindi. Kalan can: {_mevcutCan}");
        // Buraya Hasar UI Geri Bildirimi eklenebilir
    }

    private void Olüm()
    {
        Debug.Log($"{gameObject.name} hayatini kaybetti!");

        GuncelleCanBari();
    }
}