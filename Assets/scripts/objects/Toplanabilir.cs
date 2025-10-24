using UnityEngine;

public class Toplanabilir : MonoBehaviour, IIletisim
{
    public EsyaTipi esyaTipi;
    // Miktar de�i�keni art�k burada gerekli de�il, envanter bunu kendi y�netecek.

    public void IletisimeGec(GameObject etkilesen)
    {
        OyuncuEnvanter envanter = etkilesen.GetComponent<OyuncuEnvanter>();
        if (envanter != null)
        {
            // D�ZELTME: EsyaEkle metodu art�k sadece esya tipini al�yor.
            bool eklendi = envanter.EsyaEkle(esyaTipi);

            if (eklendi)
            {
                Destroy(gameObject);
            }
        }
    }
}