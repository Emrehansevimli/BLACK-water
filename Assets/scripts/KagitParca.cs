using UnityEngine;

// IIletisim arayuzunu uyguluyoruz
public class KagitParca : MonoBehaviour, IIletisim
{
    [Header("Parca Bilgisi")]
    // Hangi parca oldugunu gosteren ID (1'den 4'e kadar)
    public int parcaID = 1;

    // IIletisim arayuzunden gelen zorunlu metot
    public void IletisimeGec(GameObject etkilesen)
    {
        // 1. Oyuncunun Envanter Scriptine eris
        OyuncuEnvanter envanter = etkilesen.GetComponent<OyuncuEnvanter>();

        if (envanter != null)
        {
            // 2. Parcanin envantere eklenmesini sagla
            bool eklendi = envanter.ParcaEkle(parcaID);

            if (eklendi)
            {
                Debug.Log($"Kagit Parca {parcaID} toplandi.");

                // 3. Parca toplandiktan sonra objeyi sahnede yok et/gizle
                gameObject.SetActive(false);
                Destroy(gameObject);
            }
            else
            {
                // (Örn: Zaten bu parcaya sahipsin)
                Debug.Log($"Kagit Parca {parcaID} zaten envanterdeydi.");
            }
        }
        else
        {
            Debug.LogError("Etkilesen objede 'OyuncuEnvanter' scripti bulunamadi!");
        }
    }
}