using UnityEngine;

public class BirlestirmeAlani : MonoBehaviour, IIletisim
{
    // Tarifler hala burada Inspector'da ayarlanir
    public TarifSO[] mevcutTarifler;

    public void IletisimeGec(GameObject etkilesen)
    {
        OyuncuEnvanter envanter = etkilesen.GetComponent<OyuncuEnvanter>();

        if (envanter == null) return;

        // Crafting UI Manager'ý çagýr ve tarif listesini gonder
        if (CraftingUIManager.Instance != null)
        {
            CraftingUIManager.Instance.CraftingUI_Ac(envanter, mevcutTarifler);
        }
    }
}