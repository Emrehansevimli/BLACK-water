using UnityEngine;

public class TicaretYonetici : MonoBehaviour
{
    private KarakterDurum _karakterDurum;
    public OyuncuEnvanter _oyuncuEnvanter;

    void Start()
    {
        // Oyuncu referanslarýný al
        GameObject oyuncu = GameObject.FindGameObjectWithTag("Player");
        if (oyuncu != null)
        {
            _karakterDurum = oyuncu.GetComponent<KarakterDurum>();
            _oyuncuEnvanter = oyuncu.GetComponent<OyuncuEnvanter>();
        }
        else
        {
            Debug.LogError("Ticaret sistemi icin 'Player' tag'li obje bulunamadi.");
            this.enabled = false;
        }
    }

    // ***************************************************************
    // SATIN ALMA ÝÞLEMÝ (Oyuncu NPC'den Alýr)
    // ***************************************************************
    public bool SatinAl(EsyaTipi esyaTipi, int miktar)
    {
        if (miktar <= 0) return false;

        // Eþya verisini bul
        EsyaVeriSO veri = InventoryUIManager.Instance.VeriGetir(esyaTipi);
        if (veri == null) return false;

        int toplamMaliyet = veri.satinAlmaFiyati * miktar;

        // 1. Para Kontrolü
        if (_karakterDurum.MevcutPara < toplamMaliyet)
        {
            Debug.Log("Ticaret: Yetersiz bakiye!");
            return false;
        }

        // 2. Envanter Kontrolü ve Ekleme
        if (_oyuncuEnvanter.EsyaEkle(esyaTipi, miktar))
        {
            // 3. Ýþlem Baþarýlý: Parayý Çýkar
            _karakterDurum.ParaCikar(toplamMaliyet);
            return true;
        }
        else
        {
            Debug.Log("Ticaret: Envanter dolu.");
            return false;
        }
    }

    // ***************************************************************
    // SATMA ÝÞLEMÝ (Oyuncu NPC'ye Satar)
    // ***************************************************************
    public bool Sat(EsyaTipi esyaTipi, int miktar)
    {
        if (miktar <= 0) return false;

        // Eþya verisini bul
        EsyaVeriSO veri = InventoryUIManager.Instance.VeriGetir(esyaTipi);
        if (veri == null) return false;
        if (!veri.satilabilirMi)
        {
            Debug.Log($"Ticaret: {esyaTipi} adli esya satilamaz.");
            return false;
        }
        int toplamGelir = veri.satmaFiyati * miktar;

        // 1. Envanter Kontrolü ve Çýkarma
        if (_oyuncuEnvanter.EsyaCikar(esyaTipi, miktar))
        {
            // 2. Ýþlem Baþarýlý: Parayý Ekle
            _karakterDurum.ParaEkle(toplamGelir);
            return true;
        }
        else
        {
            Debug.Log("Ticaret: Satmak istediðiniz eþyadan yeterli miktarda yok.");
            return false;
        }
    }
}