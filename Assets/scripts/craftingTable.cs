using UnityEngine;

public class craftingTable : MonoBehaviour, IIletisim
{
    public void IletisimeGec(GameObject etkilesen)
    {
        OyuncuEnvanter envanter = etkilesen.GetComponent<OyuncuEnvanter>();
        if (envanter != null && envanter.TumParcalarToplandiMi())
        {
            Debug.Log("BULMACA TAMAMLANDI! Tum parcalar birlestirildi.");
            envanter.ParcalariTemizle();
            // Kap� a�ma, Yeni hedef belirleme gibi kodlar buraya gelir.
        }
        else
        {
            Debug.Log($"Bulmaca icin yeterli parca yok. Eksik: {envanter.toplamParcaSayisi - envanter.toplananParcalar.Count}");
        }
    }
}
