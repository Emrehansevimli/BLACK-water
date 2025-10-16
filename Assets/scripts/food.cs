using UnityEngine;

// IIletisim arayuzunu uyguluyoruz
public class food : MonoBehaviour, IIletisim
{
    [Header("Yemek Degerleri")]
    public float iyilesmeMiktari = 25f; // Bu yiyecegin karakterin canina/enerjisine katacagi deger
    public string yemekAdi = "Elma";

    // IIletisim arayuzunden gelen zorunlu metot
   /* public void IletisimeGec(GameObject etkilesen)
    {
        // Yeme islemi baslatildiginda bu metot calisir.

        // 1. Karakterin saglik/enerji sistemine eris
        // Not: Karakterde 'KarakterDurum' adinda bir script oldugunu varsayiyoruz.
        // Bu script'i bir sonraki adimda olusturacagiz.

        KarakterDurum durum = etkilesen.GetComponent<KarakterDurum>();

        if (durum != null)
        {
            // Karakterin durumunu guncelle
            durum.CanArtir(iyilesmeMiktari);

            Debug.Log($"{etkilesen.name}, {yemekAdi} yiyerek {iyilesmeMiktari} can/enerji kazandi!");

            // 2. Yeme islemi bittikten sonra objeyi yok et
            Tüket();
        }
        else
        {
            Debug.Log("Yemegi yiyebilecek bir 'KarakterDurum' scripti bulunamadi!");
        }
    }*/
    // Yemek.cs icinde:
    public void IletisimeGec(GameObject etkilesen)
    {
        // 1. Etkilesen objesi var mi?
        if (etkilesen == null)
        {
            Debug.LogError("Yemegi tuketen oyuncu nesnesi null!");
            return;
        }

        // 2. KarakterDurum scripti var mi?
        KarakterDurum durum = etkilesen.GetComponent<KarakterDurum>();

        if (durum != null)
        {
            durum.CanArtir(iyilesmeMiktari);

            Debug.Log($"{etkilesen.name}, {yemekAdi} yiyerek {iyilesmeMiktari} can/enerji kazandi!");

            Tüket();
        }
        else
        {
            Debug.LogError($"'{etkilesen.name}' objesi uzerinde 'KarakterDurum' scripti bulunamadi!");
        }
    }
    private void Tüket()
    {
        // Objenin sahnede gorunmesini engeller ve yok eder.
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
