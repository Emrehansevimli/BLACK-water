
using UnityEngine;

public class charactercontroller : MonoBehaviour
{

    private OyuncuEnvanter _envanter;
    private CharacterController _controller;
    private KarakterDurum _karakterDurum;

    // Hareket Hýzlarý
    public float yürümeHizi = 6.0f;
    public float zýplamaKuvveti = 8.0f;
    public float yercekimi = 20.0f;
    public float kosmaHizi = 10.0f;
    // Dikey (Y ekseni) hýzý tutan vektör
    private Vector3 _hizVektoru;


    // Karakter rotasyonu için hassasiyet
    public float fareHassasiyeti = 100f;

    // Kamerayý Inspector'da sürükleyip býrakýn
    public Transform kameraTransform;

    private float _xRotasyon = 0f;
    private Vector2 _anlikGirdi = Vector2.zero;
    private bool _sprintBasildi = false;
    private bool _ziplamaBasildi = false;
    private bool _uiAcik = false;
    // Kameranýn dikey bakýþ açýsý sýnýrlarý
    public float maxBakýþAçýsý = 90f;
    public float minBakýþAçýsý = -90f;
    void Start()
    {
        _envanter = GetComponent<OyuncuEnvanter>();
        _controller = GetComponent<CharacterController>();
        _karakterDurum = GetComponent<KarakterDurum>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Eðer kameraTransform atanmamýþsa, alt objelerden ana kamerayý bulmaya çalýþ
        if (kameraTransform == null)
        {
            kameraTransform = GetComponentInChildren<Camera>()?.transform;
        }
    }

    //void Update()
    //{
    //    if (CraftingUIManager.Instance != null && CraftingUIManager.Instance.IsPanelOpen)
    //    {
    //        // ...bu karenin Update metodundan hemen çýk.
    //        // Aþaðýdaki hareket ve rotasyon kodlarý ÇALIÞMAYACAK.
    //        return;
    //    }
    //    // ***************************************************************


    //    // YÜRÜME (Bu kodlar artýk sadece UI kapalýyken çalýþýr)
    //    float yatayGirdi = Input.GetAxis("Horizontal");
    //    float dikeyGirdi = Input.GetAxis("Vertical");

    //    bool hareketEdiyor = yatayGirdi != 0 || dikeyGirdi != 0;
    //    bool sprintBasiliyor = Input.GetKey(KeyCode.LeftShift);
    //    bool kosabilirMi = sprintBasiliyor && hareketEdiyor && _karakterDurum.StaminaVarMi();

    //    float mevcutHiz;
    //    if (kosabilirMi)
    //    {
    //        mevcutHiz = kosmaHizi;
    //        _karakterDurum.staminaKullaniliyor = true; // KarakterDurum'a haber ver: Staminayý tüket!
    //    }

    //    if (kosabilirMi)
    //    {
    //        mevcutHiz = kosmaHizi;
    //        _karakterDurum.staminaKullaniliyor = true; // KarakterDurum'a haber ver: Staminayý tüket!
    //    }
    //    else
    //    {
    //        mevcutHiz = yürümeHizi;
    //        _karakterDurum.staminaKullaniliyor = false; // KarakterDurum'a haber ver: Staminayý yenile!
    //    }

    //    Vector3 hareket = transform.right * yatayGirdi + transform.forward * dikeyGirdi;
    //    hareket = hareket.normalized * mevcutHiz;

    //    // 4. YERCEKIMI & ZIPLAMA
    //    if (_controller.isGrounded)
    //    {
    //        _hizVektoru.y = 0f;
    //        if (Input.GetButtonDown("Jump"))
    //        {
    //            _hizVektoru.y = zýplamaKuvveti;
    //        }
    //    }
    //    _hizVektoru.y -= yercekimi * Time.deltaTime;

    //    // 5. HAREKET UYGULA
    //    Vector3 sonHareket = hareket + _hizVektoru;
    //    _controller.Move(sonHareket * Time.deltaTime);
    //    // Karakter yerdeyse:
    //    if (_controller.isGrounded)
    //    {
    //        // Dikey hýzý (Y) sýfýrla, böylece yerçekimi birikimi durur.
    //        _hizVektoru.y = 0f;

    //        // Zýplama Giriþi:
    //        if (Input.GetButtonDown("Jump")) // Varsayýlan "Space" tuþu
    //        {
    //            // Zýplama kuvveti kadar dikey hýz uygula
    //            _hizVektoru.y = zýplamaKuvveti;
    //        }
    //    }



    //    // Fare girdilerini al
    //    float fareX = Input.GetAxis("Mouse X") * fareHassasiyeti * Time.deltaTime;
    //    float fareY = Input.GetAxis("Mouse Y") * fareHassasiyeti * Time.deltaTime;


    //    transform.Rotate(Vector3.up * fareX);
    //    _xRotasyon -= fareY;

    //    // Bakýþ açýsýný sýnýrla
    //    _xRotasyon = Mathf.Clamp(_xRotasyon, minBakýþAçýsý, maxBakýþAçýsý);

    //    // Kameranýn dönüþünü uygula
    //    if (kameraTransform != null)
    //    {
    //        kameraTransform.localRotation = Quaternion.Euler(_xRotasyon, 0f, 0f);
    //    }
    //    HotbarInputKontrol();
    //}
    void Update()
    {
        // 1. UI Kontrolü (Hareketi hemen kes)
        _uiAcik = CraftingUIManager.Instance != null && CraftingUIManager.Instance.IsPanelOpen;
        if (_uiAcik)
        {
            // UI açýkken fare imlecini göster
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            _anlikGirdi = Vector2.zero; // Hareketi sýfýrla
            _sprintBasildi = false;
            return;
        }
        else
        {
            // UI kapalýyken imleci kilitle
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        // 2. YÜRÜME INPUT'u AL ve KAYDET
        _anlikGirdi.x = Input.GetAxis("Horizontal");
        _anlikGirdi.y = Input.GetAxis("Vertical");
        _sprintBasildi = Input.GetKey(KeyCode.LeftShift);

        // Zýplama input'unu sadece basýldýðý anda alýyoruz ve FixedUpdate'e býrakýyoruz
        if (Input.GetButtonDown("Jump"))
        {
            _ziplamaBasildi = true; // Sadece basýldýðý frame'de true yapar
        }

        // 3. FARE GÝRDÝSÝ VE KAMERA
        float fareX = Input.GetAxis("Mouse X") * fareHassasiyeti * Time.deltaTime;
        float fareY = Input.GetAxis("Mouse Y") * fareHassasiyeti * Time.deltaTime;

        transform.Rotate(Vector3.up * fareX);
        _xRotasyon -= fareY;

        _xRotasyon = Mathf.Clamp(_xRotasyon, minBakýþAçýsý, maxBakýþAçýsý);

        if (kameraTransform != null)
        {
            kameraTransform.localRotation = Quaternion.Euler(_xRotasyon, 0f, 0f);
        }

        // 4. HOTBAR INPUT
        HotbarInputKontrol();
    }

    // ***************************************************************
    // Fiziksel Hareketi ve Yerçekimini Ýþler (Sabit Zaman Döngüsü)
    // ***************************************************************
    void FixedUpdate()
    {
        if (_uiAcik) return; // UI açýksa FixedUpdate'te de hareket etme

        bool hareketEdiyor = _anlikGirdi.x != 0 || _anlikGirdi.y != 0;
        bool kosabilirMi = _sprintBasildi && hareketEdiyor && _karakterDurum.StaminaVarMi();

        float mevcutHiz;

        // HIZ VE STAMINA HESABI (FixedUpdate'te stamina kullanýmýný belirle)
        if (kosabilirMi)
        {
            mevcutHiz = kosmaHizi;
            _karakterDurum.staminaKullaniliyor = true; // Staminayý tüket
        }
        else
        {
            mevcutHiz = yürümeHizi;
            _karakterDurum.staminaKullaniliyor = false; // Staminayý yenile
        }

        // 1. YÜRÜME HESAPLAMASI
        // Input'u al ve hýzý uygula
        Vector3 hareket = transform.right * _anlikGirdi.x + transform.forward * _anlikGirdi.y;
        hareket = hareket.normalized * mevcutHiz;

        // 2. YERCEKÝMÝ & ZIPLAMA
        if (_controller.isGrounded)
        {
            _hizVektoru.y = 0f;

            // Sadece zýplama isteði varsa uygula
            if (_ziplamaBasildi)
            {
                _hizVektoru.y = zýplamaKuvveti;
                _ziplamaBasildi = false; // Zýplama input'unu HEMEN sýfýrla!
            }
        }

        // Yerçekimini uygulamaya devam et
        _hizVektoru.y -= yercekimi * Time.fixedDeltaTime;

        // 3. HAREKET UYGULA (Sonuç olarak gecikmesiz ve tutarlý hareket)
        Vector3 sonHareket = hareket + _hizVektoru;
        _controller.Move(sonHareket * Time.fixedDeltaTime);
    }
    private void HotbarInputKontrol()
    {
        if (_envanter == null) return;

        // 1'den 8'e kadar tuslari kontrol et
        for (int i = 0; i < _envanter.hotbarBoyutu; i++)
        {
            // Eðer bir sayý tuþuna basýlýrsa...
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                // ...oyuncunun seçtiði slotu güncelle...
                _envanter.seciliSlotIndex = i;

                // ...VE HEMEN ARDINDAN UI YÖNETÝCÝSÝNE GÖRSELÝ GÜNCELLEMESÝNÝ SÖYLE!
                if (InventoryUIManager.Instance != null)
                {
                    InventoryUIManager.Instance.HighlightGuncelle();
                }

                Debug.Log($"{i + 1}. yuva secildi.");
                return;
            }
        }
        // F tusu = Kullan
        if (Input.GetKeyDown(KeyCode.F))
        {
            _envanter.SeciliEsyayiKullan();
        }

        // Q tusu = At
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _envanter.SeciliEsyayiAt();
        }
    }
}