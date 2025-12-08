
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

    void Update()
    {
        if (CraftingUIManager.Instance != null && CraftingUIManager.Instance.IsPanelOpen)
        {
            // ...bu karenin Update metodundan hemen çýk.
            // Aþaðýdaki hareket ve rotasyon kodlarý ÇALIÞMAYACAK.
            return;
        }
        // ***************************************************************


        // YÜRÜME (Bu kodlar artýk sadece UI kapalýyken çalýþýr)
        float yatayGirdi = Input.GetAxis("Horizontal");
        float dikeyGirdi = Input.GetAxis("Vertical");

        bool hareketEdiyor = yatayGirdi != 0 || dikeyGirdi != 0;
        bool sprintBasiliyor = Input.GetKey(KeyCode.LeftShift);
        bool kosabilirMi = sprintBasiliyor && hareketEdiyor && _karakterDurum.StaminaVarMi();

        float mevcutHiz;
        if (kosabilirMi)
        {
            mevcutHiz = kosmaHizi;
            _karakterDurum.staminaKullaniliyor = true; // KarakterDurum'a haber ver: Staminayý tüket!
        }
  
        if (kosabilirMi)
        {
            mevcutHiz = kosmaHizi;
            _karakterDurum.staminaKullaniliyor = true; // KarakterDurum'a haber ver: Staminayý tüket!
        }
        else
        {
            mevcutHiz = yürümeHizi;
            _karakterDurum.staminaKullaniliyor = false; // KarakterDurum'a haber ver: Staminayý yenile!
        }

        Vector3 hareket = transform.right * yatayGirdi + transform.forward * dikeyGirdi;
        hareket = hareket.normalized * mevcutHiz;

        // 4. YERCEKIMI & ZIPLAMA
        if (_controller.isGrounded)
        {
            _hizVektoru.y = 0f;
            if (Input.GetButtonDown("Jump"))
            {
                _hizVektoru.y = zýplamaKuvveti;
            }
        }
        _hizVektoru.y -= yercekimi * Time.deltaTime;

        // 5. HAREKET UYGULA
        Vector3 sonHareket = hareket + _hizVektoru;
        _controller.Move(sonHareket * Time.deltaTime);
        // Karakter yerdeyse:
        if (_controller.isGrounded)
        {
            // Dikey hýzý (Y) sýfýrla, böylece yerçekimi birikimi durur.
            _hizVektoru.y = 0f;

            // Zýplama Giriþi:
            if (Input.GetButtonDown("Jump")) // Varsayýlan "Space" tuþu
            {
                // Zýplama kuvveti kadar dikey hýz uygula
                _hizVektoru.y = zýplamaKuvveti;
            }
        }


       
        // Fare girdilerini al
        float fareX = Input.GetAxis("Mouse X") * fareHassasiyeti * Time.deltaTime;
        float fareY = Input.GetAxis("Mouse Y") * fareHassasiyeti * Time.deltaTime;


        transform.Rotate(Vector3.up * fareX);
        _xRotasyon -= fareY;

        // Bakýþ açýsýný sýnýrla
        _xRotasyon = Mathf.Clamp(_xRotasyon, minBakýþAçýsý, maxBakýþAçýsý);

        // Kameranýn dönüþünü uygula
        if (kameraTransform != null)
        {
            kameraTransform.localRotation = Quaternion.Euler(_xRotasyon, 0f, 0f);
        }
        HotbarInputKontrol();
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