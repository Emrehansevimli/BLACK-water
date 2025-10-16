using UnityEngine;

public class Toplanabilir : MonoBehaviour, IIletisim
{
    [Header("Toplanacak Esya")]
    // Hangi esya oldugunu ve miktarini Inspector'dan ayarla
    public EsyaTipi esyaTipi;
    public int miktar = 1;

    // IIletisim arayuzunden gelen zorunlu metot
    public void IletisimeGec(GameObject etkilesen)
    {
        // 1. Oyuncunun Envanter Scriptine eris
        OyuncuEnvanter envanter = etkilesen.GetComponent<OyuncuEnvanter>();

        if (envanter != null)
        {
            // 2. Esyayi envantere ekle
            envanter.EsyaEkle(esyaTipi, miktar);

            // 3. Obje toplandiktan sonra kendini yok et
            Toplandi();
        }
        else
        {
            Debug.LogError("Etkilesen objede 'OyuncuEnvanter' scripti bulunamadi!");
        }
    }

    private void Toplandi()
    {
        // Gorsel bir efekt/ses cikartilabilir

        // Objenin sahnede gorunmesini engeller ve yok eder.
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}