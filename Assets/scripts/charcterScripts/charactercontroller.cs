
using UnityEngine;

public class charactercontroller : MonoBehaviour
{
    // Karakter Kontrolcü bileþeni
    private CharacterController _controller;

    // Hareket Hýzlarý
    public float yürümeHizi = 6.0f;
    public float zýplamaKuvveti = 8.0f;
    public float yercekimi = 20.0f;

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
        // Baþlangýçta CharacterController bileþenini al
        _controller = GetComponent<CharacterController>();
        // Oyun baþladýðýnda fare imlecini kilitler ve gizler.
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
        // 1. Yatay Hareket Giriþlerini Al
        float yatayGirdi = Input.GetAxis("Horizontal"); // A/D veya Sol/Sað Ok
        float dikeyGirdi = Input.GetAxis("Vertical");   // W/S veya Yukarý/Aþaðý Ok

        // Karakterin yerel uzayýna (ileri, geri, saða, sola) göre hareket vektörünü hesapla
        Vector3 hareket = transform.right * yatayGirdi + transform.forward * dikeyGirdi;

        // Hýzý ve Time.deltaTime ile çarp
        hareket *= yürümeHizi;

        // 2. Yerçekimi ve Zýplama (Dikey Hareket)

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

        // Yerçekimini her karede uygula (Time.deltaTime ile kare hýzýndan baðýmsýz hale getir)
        _hizVektoru.y -= yercekimi * Time.deltaTime;

        // 3. Karakteri Hareket Ettir

        // Yatay hareket vektörünü dikey hýz ile birleþtir
        // Bu önemli: _hizVektoru.y, yerçekimi ve zýplamayý kontrol eder.
        Vector3 sonHareket = hareket + _hizVektoru;

        // Character Controller'ýn Move metodu ile karakteri hareket ettir.
        // **Move() metodu, Time.deltaTime gerektirmez** çünkü sonHareket vektörünün bir hýz (velocity) deðil,
        // bu karede karakterin ne kadar hareket edeceði (delta) olmasý beklenir. 
        // Ancak bu kodda yürüme hareketini zaten çarptýðýmýz için sadece Character Controller'ýn Move
        // metodunun beklentisini karþýlamak amacýyla sonHareket vektörünü direkt kullanýyoruz.
        _controller.Move(sonHareket * Time.deltaTime);
        // Fare girdilerini al
        float fareX = Input.GetAxis("Mouse X") * fareHassasiyeti * Time.deltaTime;
        float fareY = Input.GetAxis("Mouse Y") * fareHassasiyeti * Time.deltaTime;

        // 1. Yatay Karakter Rotasyonu (Y Ekseni)
        // Karakterin ana objesini döndürür (Saða/Sola Bakma)
        transform.Rotate(Vector3.up * fareX);

        // 2. Dikey Kamera Rotasyonu (X Ekseni)
        _xRotasyon -= fareY;

        // Bakýþ açýsýný sýnýrla
        _xRotasyon = Mathf.Clamp(_xRotasyon, minBakýþAçýsý, maxBakýþAçýsý);

        // Kameranýn dönüþünü uygula
        if (kameraTransform != null)
        {
            kameraTransform.localRotation = Quaternion.Euler(_xRotasyon, 0f, 0f);
        }
    }
}