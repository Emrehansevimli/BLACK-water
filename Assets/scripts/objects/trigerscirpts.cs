using UnityEngine;
using TMPro;

public class trigerscripts : MonoBehaviour
{
    [Header("UI & Ayarlar")]
    public string iletisimMesaji = "[E] Kullan";
    public KeyCode iletisimTusu = KeyCode.E;

    private bool _oyuncuAlaniIcinde = false;
    private IIletisim _iletisimHedefi;
    private GameObject _girenOyuncu;
    private KarakterDurum _oyuncuIhtiyac;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _oyuncuAlaniIcinde = true;
            _iletisimHedefi = GetComponent<IIletisim>();
            _girenOyuncu = other.gameObject;
            if (_iletisimHedefi != null && UIManager.Instance != null)
            {
                UIManager.Instance.UyariyiGoster(iletisimMesaji);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _oyuncuAlaniIcinde = false;
            _iletisimHedefi = null;
            _girenOyuncu = null;
            if (UIManager.Instance != null)
            {
                UIManager.Instance.UyariyiGizle();
            }
        }
    }

    void Update()
    {
        if (_oyuncuAlaniIcinde && _iletisimHedefi != null && Input.GetKeyDown(iletisimTusu))
        {
            IletisimiGerceklestir();

            if (UIManager.Instance != null)
            {
                UIManager.Instance.UyariyiGizle();
            }
        }
    }
   
    void IletisimiGerceklestir()
    {
        // Iletisim arayuzu metodu cagriliyor
        
        if (_girenOyuncu != null)
        {
            // Oyuncu objesini IletisimeGec metoduna gonder.
            _iletisimHedefi.IletisimeGec(_girenOyuncu);
        }
    }
}