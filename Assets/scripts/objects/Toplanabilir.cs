using UnityEngine;

public class Toplanabilir : MonoBehaviour, IIletisim
{
    public EsyaTipi esyaTipi;
    

    public void IletisimeGec(GameObject etkilesen)
    {
        OyuncuEnvanter envanter = etkilesen.GetComponent<OyuncuEnvanter>();
        if (envanter != null)
        {
            
            bool eklendi = envanter.EsyaEkle(esyaTipi,1);

            if (eklendi)
            {
                Destroy(gameObject);
            }
        }
    }
}