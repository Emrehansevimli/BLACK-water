using UnityEngine;

public class Toplanabilir : MonoBehaviour, IIletisim
{
    public EsyaTipi esyaTipi;
    // Miktar deðiþkeni artýk burada gerekli deðil, envanter bunu kendi yönetecek.

    public void IletisimeGec(GameObject etkilesen)
    {
        OyuncuEnvanter envanter = etkilesen.GetComponent<OyuncuEnvanter>();
        if (envanter != null)
        {
            // DÜZELTME: EsyaEkle metodu artýk sadece esya tipini alýyor.
            bool eklendi = envanter.EsyaEkle(esyaTipi);

            if (eklendi)
            {
                Destroy(gameObject);
            }
        }
    }
}