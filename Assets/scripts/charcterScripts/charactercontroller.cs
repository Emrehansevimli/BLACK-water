
using UnityEngine;

public class charactercontroller : MonoBehaviour
{
    // Karakter Kontrolc� bile�eni
    private CharacterController _controller;

    // Hareket H�zlar�
    public float y�r�meHizi = 6.0f;
    public float z�plamaKuvveti = 8.0f;
    public float yercekimi = 20.0f;

    // Dikey (Y ekseni) h�z� tutan vekt�r
    private Vector3 _hizVektoru;


    // Karakter rotasyonu i�in hassasiyet
    public float fareHassasiyeti = 100f;

    // Kameray� Inspector'da s�r�kleyip b�rak�n
    public Transform kameraTransform;

    private float _xRotasyon = 0f;

    // Kameran�n dikey bak�� a��s� s�n�rlar�
    public float maxBak��A��s� = 90f;
    public float minBak��A��s� = -90f;
    void Start()
    {
        // Ba�lang��ta CharacterController bile�enini al
        _controller = GetComponent<CharacterController>();
        // Oyun ba�lad���nda fare imlecini kilitler ve gizler.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // E�er kameraTransform atanmam��sa, alt objelerden ana kameray� bulmaya �al��
        if (kameraTransform == null)
        {
            kameraTransform = GetComponentInChildren<Camera>()?.transform;
        }
    }

    void Update()
    {
        // 1. Yatay Hareket Giri�lerini Al
        float yatayGirdi = Input.GetAxis("Horizontal"); // A/D veya Sol/Sa� Ok
        float dikeyGirdi = Input.GetAxis("Vertical");   // W/S veya Yukar�/A�a�� Ok

        // Karakterin yerel uzay�na (ileri, geri, sa�a, sola) g�re hareket vekt�r�n� hesapla
        Vector3 hareket = transform.right * yatayGirdi + transform.forward * dikeyGirdi;

        // H�z� ve Time.deltaTime ile �arp
        hareket *= y�r�meHizi;

        // 2. Yer�ekimi ve Z�plama (Dikey Hareket)

        // Karakter yerdeyse:
        if (_controller.isGrounded)
        {
            // Dikey h�z� (Y) s�f�rla, b�ylece yer�ekimi birikimi durur.
            _hizVektoru.y = 0f;

            // Z�plama Giri�i:
            if (Input.GetButtonDown("Jump")) // Varsay�lan "Space" tu�u
            {
                // Z�plama kuvveti kadar dikey h�z uygula
                _hizVektoru.y = z�plamaKuvveti;
            }
        }

        // Yer�ekimini her karede uygula (Time.deltaTime ile kare h�z�ndan ba��ms�z hale getir)
        _hizVektoru.y -= yercekimi * Time.deltaTime;

        // 3. Karakteri Hareket Ettir

        // Yatay hareket vekt�r�n� dikey h�z ile birle�tir
        // Bu �nemli: _hizVektoru.y, yer�ekimi ve z�plamay� kontrol eder.
        Vector3 sonHareket = hareket + _hizVektoru;

        // Character Controller'�n Move metodu ile karakteri hareket ettir.
        // **Move() metodu, Time.deltaTime gerektirmez** ��nk� sonHareket vekt�r�n�n bir h�z (velocity) de�il,
        // bu karede karakterin ne kadar hareket edece�i (delta) olmas� beklenir. 
        // Ancak bu kodda y�r�me hareketini zaten �arpt���m�z i�in sadece Character Controller'�n Move
        // metodunun beklentisini kar��lamak amac�yla sonHareket vekt�r�n� direkt kullan�yoruz.
        _controller.Move(sonHareket * Time.deltaTime);
        // Fare girdilerini al
        float fareX = Input.GetAxis("Mouse X") * fareHassasiyeti * Time.deltaTime;
        float fareY = Input.GetAxis("Mouse Y") * fareHassasiyeti * Time.deltaTime;

        // 1. Yatay Karakter Rotasyonu (Y Ekseni)
        // Karakterin ana objesini d�nd�r�r (Sa�a/Sola Bakma)
        transform.Rotate(Vector3.up * fareX);

        // 2. Dikey Kamera Rotasyonu (X Ekseni)
        _xRotasyon -= fareY;

        // Bak�� a��s�n� s�n�rla
        _xRotasyon = Mathf.Clamp(_xRotasyon, minBak��A��s�, maxBak��A��s�);

        // Kameran�n d�n���n� uygula
        if (kameraTransform != null)
        {
            kameraTransform.localRotation = Quaternion.Euler(_xRotasyon, 0f, 0f);
        }
    }
}