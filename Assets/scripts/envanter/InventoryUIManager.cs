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

    private List<EnvanterYuvaUI> _yuvaUIListesi = new List<EnvanterYuvaUI>();
    void Awake()
    {
        // Singleton yap�s�: Instance'� bu objeye atar.
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            // Sahnedeki ikinci bir kopyay� yok et
            Destroy(gameObject);
        }
        _veriSozluk = tumEsyaVerileri.ToDictionary(veri => veri.esyaTipi, veri => veri);
        // Buradan Start() metoduna veya ba�ka bir yere ta��nd�ysa, 
        // Start metodunda da GetComponent<OyuncuEnvanter>() �a�r�s� yap�labilir.
    }
    void Start()
    {

        // Oyuncunun envanterini bul
        GameObject oyuncu = GameObject.FindGameObjectWithTag("Player");

        // **** G�VENL�K KONTROL� 1 ****
        if (oyuncu == null)
        {
            Debug.LogError("HATA: Sahnedeki karakterinize 'Player' tag'ini vermeyi unuttunuz!");
            return; // Oyuncu bulunamazsa devam etme, hata vermesini engelle
        }

        _envanter = oyuncu.GetComponent<OyuncuEnvanter>();

        // **** G�VENL�K KONTROL� 2 ****
        if (_envanter == null)
        {
            Debug.LogError("HATA: 'Player' tag'li objenin uzerinde OyuncuEnvanter scripti bulunamadi!");
            return; // Script bulunamazsa devam etme
        }

        foreach (Transform child in yuvaKonteyneri)
        {
            Destroy(child.gameObject);
        }
        // Temizlik sonras� listeyi de s�f�rla
        _yuvaUIListesi.Clear();
        // Baslangicta sabit sayida bos yuva olustur
        for (int i = 0; i < _envanter.hotbarBoyutu; i++)
        {
            GameObject yeniYuvaObjesi = Instantiate(yuvaPrefab, yuvaKonteyneri);
            EnvanterYuvaUI yuvaUI = yeniYuvaObjesi.GetComponent<EnvanterYuvaUI>();
            yuvaUI.Temizle();
            _yuvaUIListesi.Add(yuvaUI);
        }

        EnvanteriGuncelle();
    }

    public void EnvanteriGuncelle()
    {
        if (_envanter == null) return;

        for (int i = 0; i < _envanter.hotbarBoyutu; i++)
        {
            // D�ZELTME: Dizi art�k EsyaTipi de�il, EnvanterYuvasi tutuyor.
            EnvanterYuvasi esyaYuvasi = _envanter.hotbarSlotlari[i];
            EnvanterYuvaUI yuvaUI = _yuvaUIListesi[i];

            // Yuvada esya var mi (miktarina bakarak anlariz)
            if (esyaYuvasi.miktar > 0)
            {
                // D�ZELTME: VeriGetir'e yuvan�n i�indeki .tip �zelli�ini g�nderiyoruz.
                EsyaVeriSO esyaVerisi = VeriGetir(esyaYuvasi.tip);
                yuvaUI.YuvayiGuncelle(esyaVerisi, esyaYuvasi.miktar);
            }
            else
            {
                yuvaUI.Temizle();
            }
        }

        HighlightGuncelle();
    }

    // YEN�: Secili yuvayi g�rsel olarak isaretleme metodu
    public void HighlightGuncelle()
    {
        for (int i = 0; i < _yuvaUIListesi.Count; i++)
        {
            // Her yuvanin uzerinde secili olup olmadigini gosteren bir metot olmali
            _yuvaUIListesi[i].SecimiAyarla(i == _envanter.seciliSlotIndex);
        }
    }
   
    public EsyaVeriSO VeriGetir(EsyaTipi tip)
    {
        if (_veriSozluk.ContainsKey(tip))
        {
            return _veriSozluk[tip];
        }
        return null;
    }
    
}