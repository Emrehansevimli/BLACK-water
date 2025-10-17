using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class InventoryUIManager : MonoBehaviour
{

    public static InventoryUIManager Instance;

    [Header("UI Bilesenleri")]
    // Tek bir envanter yuvasini temsil eden Prefab
    public GameObject yuvaPrefab;
    // Yuvalarin yerlestirilecegi konteyner (GridLayoutGroup olan obje)
    public Transform yuvaKonteyneri;

    // Envanterdeki yuvalari tutariz
    private List<GameObject> _yuvalar = new List<GameObject>();
    [Header("Esya Verileri")]
    public EsyaVeriSO[] tumEsyaVerileri;

    // Veriye hizli erisim icin Dictionary (Key: EsyaTipi, Value: EsyaVeriSO)
    private Dictionary<EsyaTipi, EsyaVeriSO> _veriSozluk;
    // OyuncuEnvanter scriptine erisim
    private OyuncuEnvanter _envanter;
    void Awake()
    {
        // Singleton yapýsý: Instance'ý bu objeye atar.
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            // Sahnedeki ikinci bir kopyayý yok et
            Destroy(gameObject);
        }

        // Buradan Start() metoduna veya baþka bir yere taþýndýysa, 
        // Start metodunda da GetComponent<OyuncuEnvanter>() çaðrýsý yapýlabilir.
    }
    void Start()
    {

        _veriSozluk = tumEsyaVerileri.ToDictionary(veri => veri.esyaTipi, veri => veri);
        // Oyuncunun envanterini bul (Tag'e gore arama veya direkt Character objesi uzerinden)
        GameObject oyuncu = GameObject.FindGameObjectWithTag("Player");
        if (oyuncu != null)
        {
            _envanter = oyuncu.GetComponent<OyuncuEnvanter>();
        }

        if (_envanter != null)
        {
            // Envanter degistiginde bu metodu cagiracak bir olay (event) baglayabiliriz
            // Simdilik kolay yol olarak, guncelleme metodunu direkt cagiracagiz.

            // Ornek: _envanter.OnEnvanterDegisti += EnvanteriGuncelle;
            EnvanteriGuncelle();
        }
    }

    /// <summary>
    /// Oyuncunun envanterindeki tum esyalari UI'a yansitir.
    /// </summary>
    public void EnvanteriGuncelle()
    {
        if (_envanter == null) return;

        // 1. Onceki yuvalari temizle
        foreach (Transform child in yuvaKonteyneri)
        {
            Destroy(child.gameObject);
        }
        // _yuvalar listesi bu yontemde gereksiz hale geliyor, silebilirsiniz.

        // 2. Envanterdeki her esya icin yeni yuva olustur
        foreach (var esyaKayit in _envanter.envanter)
        {
            // Sadece miktari 0'dan buyukse goster
            if (esyaKayit.Value <= 0) continue;

            // Yeni yuvayi olustur
            GameObject yeniYuvaObjesi = Instantiate(yuvaPrefab, yuvaKonteyneri);

            // Yeni yuvadaki EnvanterYuvaUI scriptine eris
            EnvanterYuvaUI yuvaUI = yeniYuvaObjesi.GetComponent<EnvanterYuvaUI>();

            // Veri sozlügünden esya verisini al
            EsyaVeriSO esyaVerisi = _veriSozluk.ContainsKey(esyaKayit.Key) ? _veriSozluk[esyaKayit.Key] : null;

            // Yuvayi Guncelle metodunu cagir
            if (yuvaUI != null)
            {
                yuvaUI.YuvayiGuncelle(esyaVerisi, esyaKayit.Value);
            }
        }
    }
    private EsyaVeriSO VeriGetir(EsyaTipi tip)
    {
        if (_veriSozluk.ContainsKey(tip))
        {
            return _veriSozluk[tip];
        }
        return null;
    }
    // Gercek bir oyunda, bu metodun OyuncuEnvanter.cs icinden her esya eklendiginde/cikarildiginda
    // cagrilmasi gerekir.
    // **OyuncuEnvanter.cs'yi Guncellemek:**
    // OyuncuEnvanter.cs icindeki EsyaEkle ve EsyaKullan metodlarinin sonuna bu cagriyi ekleyin:
    /*
    if (InventoryUIManager.Instance != null) {
        InventoryUIManager.Instance.EnvanteriGuncelle();
    }
    */
}