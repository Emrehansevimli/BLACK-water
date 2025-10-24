using UnityEngine;

public class BirlestirmeAlani : MonoBehaviour, IIletisim
{
    // Tarifler hala burada Inspector'da ayarlanir
    public TarifSO[] mevcutTarifler;

    public void IletisimeGec(GameObject etkilesen)
    {
        
            OyuncuEnvanter envanter = etkilesen.GetComponent<OyuncuEnvanter>();
            if (envanter == null) return;

            // Crafting UI Manager'� �a��r ve tarif listesini g�nder
            if (CraftingUIManager.Instance != null)
            {
                CraftingUIManager.Instance.CraftingUI_Ac(envanter, mevcutTarifler);
            }
        
    }
}