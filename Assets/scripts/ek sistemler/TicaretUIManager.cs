using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TicaretUIManager : MonoBehaviour
{
    public static TicaretUIManager Instance;

    [Header("UI Referanslari")]
    public GameObject ticaretPaneli; // Ana panel (Adim 1)
    public Transform satisListesiIcerik; // Scroll View'in Content objesi
    public GameObject ticaretSlotPrefab; // Her bir eþya için oluþturulacak prefab

    [Header("Veri Referanslari")]
    public EsyaTipi[] satilacakEsyalar; // NPC'nin sattýðý eþya tipleri

    public bool _isOpen = false;

    void Awake()
    {
        // Singleton kontrolü
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        // Baþlangýçta paneli kapalý tut
        ticaretPaneli.SetActive(false);
    }

    // ***************************************************************
    // ARAYÜZ KONTROLÜ
    // ***************************************************************
    public bool IsOpen
    {
        get { return _isOpen; }
    }
    public void PaneliAc()
    {
        if (ticaretPaneli == null)
        {
            Debug.LogError("TICARET PANELI HATA: TicaretPaneli objesi Inspector'da baglanmamis!");
            return; // Ýþlemi durdur
        }
        Debug.Log("Ticaret Paneli ACILIYOR...");
        _isOpen = true;
        ticaretPaneli.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        // Listeyi her açýlýþta yeniden oluþtur (fiyatlar/stok deðiþebilir)
        ListeyiDoldur();
    }

    public void PaneliKapat()
    {
        _isOpen = false;
        ticaretPaneli.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // ***************************************************************
    // LÝSTE DOLDURMA MANTIÐI
    // ***************************************************************

    void ListeyiDoldur()
    {
        // Önce eski slotlari temizle
        foreach (Transform child in satisListesiIcerik)
        {
            Destroy(child.gameObject);
        }

        // Satýlacak her eþya için slot oluþtur
        foreach (EsyaTipi tip in satilacakEsyalar)
        {
            // InventoryUIManager'dan eþya verisini çekmek için VeriGetir metodu gereklidir.
            EsyaVeriSO veri = InventoryUIManager.Instance.VeriGetir(tip);

            if (veri != null)
            {
                // Yeni slotu oluþtur
                GameObject slot = Instantiate(ticaretSlotPrefab, satisListesiIcerik);

                // TicaretSlot bileþenini al ve verileri ata
                TicaretSlot slotScript = slot.GetComponent<TicaretSlot>();
                if (slotScript != null)
                {
                    slotScript.Eslestir(veri);
                }
            }
        }
    }
}
