using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TicaretSlot : MonoBehaviour
{
    [Header("UI Referanslari")]
    public TextMeshProUGUI adText;
    public TextMeshProUGUI fiyatText;
    public Image ikonImage;
    public Button satinAlButonu;

    // Satýn alýnacak miktarý tutan input alaný
    public TMP_InputField miktarInput;

    private EsyaVeriSO _esyaVeri;
    private TicaretYonetici _ticaretYonetici;

    void Start()
    {
        // Ticaret Yöneticisi'ne eriþim
        _ticaretYonetici = FindObjectOfType<TicaretYonetici>();
        if (_ticaretYonetici == null)
        {
            Debug.LogError("Sahnede TicaretYonetici bulunamadi!");
            enabled = false;
        }

        // Butonun týklanma olayýný atama
        satinAlButonu.onClick.AddListener(SatinAlmaIslemi);
    }

    public void Eslestir(EsyaVeriSO veri)
    {
        _esyaVeri = veri;

        adText.text = veri.gorunurAd;
        ikonImage.sprite = veri.ikon;
        fiyatText.text = $"Fiyat: {veri.satinAlmaFiyati} G"; // G: Gold (Altýn)

        // Miktar varsayýlan olarak 1 olsun
        miktarInput.text = "1";
    }

    void SatinAlmaIslemi()
    {
        if (_esyaVeri == null || _ticaretYonetici == null) return;

        // Input alanýndan miktarý çek
        if (int.TryParse(miktarInput.text, out int miktar))
        {
            if (miktar <= 0) miktar = 1; // Minimum 1 alým

            // Ticaret Yöneticisi'ne iþlemi gönder
            bool basarili = _ticaretYonetici.SatinAl(_esyaVeri.esyaTipi, miktar);

            if (basarili)
            {
                // UI'ý güncellediðimizden emin ol (EkonomiUIManager bunu otomatik yapmalý)
                // Ýsteðe baðlý: Baþarýlý mesajý göster
                Debug.Log($"Basariyla {miktar} adet {_esyaVeri.gorunurAd} satin alindi.");
            }
            else
            {
                // Ýsteðe baðlý: Baþarýsýz mesajý göster (Yetersiz Bakiye/Envanter Dolu)
            }
        }
    }
}
