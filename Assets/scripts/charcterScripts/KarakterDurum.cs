using UnityEngine;

public class KarakterDurum : MonoBehaviour
{
    [Header("Durum Degerleri")]
    public float maksimumCan = 100f;
    public float mevcutIhtiyac = 0f;
    public float ihtiyacArtisHizi = 1f; // Saniye basina artis
    public float giderilmeMiktari = 100f; // Tek seferde giderilen miktar

    [Header("Maksimum Degerler")]
    private float _mevcutCan;
    public float maksimumIhtiyac = 100f;
    public float kritikSeviye = 80f;
    //[Header("UI Geri Bildirim")]
    // Not: Can barý veya metin guncellemesi icin daha sonra buraya UI referanslari eklenebilir.
    // Simdilik sadece Debug.Log ile gosteriyoruz.

    void Start()
    {
        // Oyun basladiginda cani maksimum seviyeye ayarla
        _mevcutCan = maksimumCan;
        Debug.Log($"Karakterin can degeri: {_mevcutCan} / {maksimumCan}");
    }
    void Update()
    {
        // 1. Ýhtiyaci zamanla artir (Her zaman artýyor)
        mevcutIhtiyac += ihtiyacArtisHizi * Time.deltaTime;
        mevcutIhtiyac = Mathf.Min(mevcutIhtiyac, maksimumIhtiyac);

        // 2. Kritik seviye kontrolu (Opsiyonel: Ses/Gorsel uyari)
       /* if (mevcutIhtiyac >= kritikSeviye && !UIManager.Instance.uyariGorunur)
        {
            // Belki karakterin yüzü kýzarýr veya bir ses gelir.
        }*/
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

        // Mevcut cani ekle
        _mevcutCan += miktar;

        // Cani maksimum degeri gecmeyecek sekilde sinirla
        _mevcutCan = Mathf.Min(_mevcutCan, maksimumCan);

        Debug.Log($"{miktar} can kazanildi. Yeni can degeri: {_mevcutCan} / {maksimumCan}");

        // Buraya UI Guncelleme metodu cagrýlabilir
    }

    /// <summary>
    /// Karakterin canini belirtilen miktar kadar azaltir (Hasar Alma).
    /// </summary>
    /// <param name="hasar">Uygulanacak hasar degeri (pozitif olmali).</param>
    public void HasarAl(float hasar)
    {
        if (hasar <= 0) return;

        _mevcutCan -= hasar;

        // Can 0'in altina duserse olum islemlerini baslat
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

        // Karakteri yok etme, yeniden dogma veya oyun sonu ekranini tetikleme islemleri buraya gelir.
        // Ornek: GetComponent<OyuncuKontrol>().enabled = false;
    }
}