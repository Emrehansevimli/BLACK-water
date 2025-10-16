using UnityEngine;
using TMPro; // UI bilesenleri icin
using System.Collections.Generic;

public class CraftingUIManager : MonoBehaviour
{
    // Singleton (Tekil) eriþim noktasý
    public static CraftingUIManager Instance;

    [Header("UI Bilesenleri")]
    // Uretim Paneli (Parent obje, butun UI'i tutar)
    public GameObject craftingPanel;
    // Secili tarifi gosteren metin
    public TextMeshProUGUI tarifAdiText;
    // Gerekli malzemeleri gosteren metin (veya dinamik liste)
    public TextMeshProUGUI gereksinimlerText;

    [Header("Kontrol Degerleri")]
    private TarifSO[] _mevcutTarifler; // Gelen tarif listesini tutar
    private OyuncuEnvanter _oyuncuEnvanteri;
    private int _mevcutTarifIndex = 0;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // Baslangicta paneli gizle
        if (craftingPanel != null)
            craftingPanel.SetActive(false);
    }

    // *******************************************************************
    // 1. ADIM: Crafting UI'i Açma/Kapama
    // *******************************************************************
    public void CraftingUI_Ac(OyuncuEnvanter envanter, TarifSO[] tarifler)
    {
        if (craftingPanel == null) return;

        _oyuncuEnvanteri = envanter;
        _mevcutTarifler = tarifler;

        if (tarifler.Length == 0)
        {
            Debug.Log("Bu tezgahta uretilebilecek tarif yok.");
            return;
        }

        craftingPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None; // Fareyi serbest birak
        Cursor.visible = true;

        // Ilk tarifi goster
        _mevcutTarifIndex = 0;
        GuncelTarifiGoster();
    }

    public void CraftingUI_Kapat()
    {
        if (craftingPanel == null) return;

        craftingPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked; // Fareyi oyuna kilitle
        Cursor.visible = false;

        // Temizlik
        _oyuncuEnvanteri = null;
    }

    // *******************************************************************
    // 2. ADIM: Tarifler Arasi Geçiþ
    // *******************************************************************
    void Update()
    {
        // Yalnizca panel acikken tusa basilabilir
        if (craftingPanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                IleriTarifeGec();
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                GeriTarifeGec();
            }

            // Uretim tusu
            if (Input.GetKeyDown(KeyCode.Space)) // Ornek olarak Space tusu
            {
                SeciliTarifiUret();
            }

            // Kapatma tusu
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CraftingUI_Kapat();
            }
        }
    }

    private void IleriTarifeGec()
    {
        _mevcutTarifIndex = (_mevcutTarifIndex + 1) % _mevcutTarifler.Length;
        GuncelTarifiGoster();
    }

    private void GeriTarifeGec()
    {
        _mevcutTarifIndex--;
        if (_mevcutTarifIndex < 0)
        {
            _mevcutTarifIndex = _mevcutTarifler.Length - 1;
        }
        GuncelTarifiGoster();
    }

    // *******************************************************************
    // 3. ADIM: UI'i Güncelleme ve Üretim Mantýðý
    // *******************************************************************
    private void GuncelTarifiGoster()
    {
        if (_mevcutTarifler.Length == 0) return;

        TarifSO seciliTarif = _mevcutTarifler[_mevcutTarifIndex];

        // Tarif Adini Goster
        tarifAdiText.text = $"Tarif: {seciliTarif.name}";

        // Gereksinim Metnini Olustur
        string gereksinimMetni = "Gerekenler:\n";
        bool uretilebilir = true;

        foreach (var gereksinim in seciliTarif.malzemeler)
        {
            int oyuncuMiktari = _oyuncuEnvanteri.envanter.ContainsKey(gereksinim.tip) ? _oyuncuEnvanteri.envanter[gereksinim.tip] : 0;

            // Uretilebilirlik kontrolu
            if (oyuncuMiktari < gereksinim.miktar)
            {
                uretilebilir = false;
                gereksinimMetni += $"<color=red>{gereksinim.tip}: {oyuncuMiktari}/{gereksinim.miktar}</color>\n";
            }
            else
            {
                gereksinimMetni += $"<color=green>{gereksinim.tip}: {oyuncuMiktari}/{gereksinim.miktar}</color>\n";
            }
        }

        gereksinimlerText.text = gereksinimMetni;

        if (uretilebilir)
        {
            gereksinimlerText.text += "\n<color=yellow>Üretilebilir! [BOÞLUK]</color>";
        }
    }

    public void SeciliTarifiUret()
    {
        if (_oyuncuEnvanteri == null || _mevcutTarifler.Length == 0) return;

        TarifSO seciliTarif = _mevcutTarifler[_mevcutTarifIndex];

        // Malzeme kontrolü ve üretim mantýðý burada
        if (TarifKontrolEt(seciliTarif))
        {
            UretimiGerceklestir(seciliTarif);

            // Uretimden sonra envanter degistigi icin UI'i guncelle
            GuncelTarifiGoster();
        }
        else
        {
            Debug.Log($"'{seciliTarif.name}' uretilemiyor. Yetersiz malzeme.");
        }
    }

    // Yardýmcý Metotlar (BirlestirmeAlani'ndan buraya taþýndý)
    private bool TarifKontrolEt(TarifSO tarif)
    {
        foreach (var gereksinim in tarif.malzemeler)
        {
            if (!_oyuncuEnvanteri.envanter.ContainsKey(gereksinim.tip) ||
                _oyuncuEnvanteri.envanter[gereksinim.tip] < gereksinim.miktar)
            {
                return false;
            }
        }
        return true;
    }

    private void UretimiGerceklestir(TarifSO tarif)
    {
        // Malzemeleri Tüket
        foreach (var gereksinim in tarif.malzemeler)
        {
            _oyuncuEnvanteri.EsyaKullan(gereksinim.tip, gereksinim.miktar);
        }

        // Sonucu Envantere Ekle
        _oyuncuEnvanteri.EsyaEkle(tarif.sonucEsya, tarif.sonucMiktari);

        Debug.Log($"[{tarif.name}] tarifi basarili! Uretilen: {tarif.sonucEsya} x{tarif.sonucMiktari}");
    }
}