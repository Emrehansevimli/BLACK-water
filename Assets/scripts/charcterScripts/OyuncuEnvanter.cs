using UnityEngine;

// Bu yapý, tek bir envanter yuvasýnýn verisini tutar.
[System.Serializable]
public struct EnvanterYuvasi
{
    public EsyaTipi tip;
    public int miktar;
}

public class OyuncuEnvanter : MonoBehaviour
{
    public int hotbarBoyutu = 8;
    public EnvanterYuvasi[] hotbarSlotlari;

    public int seciliSlotIndex = 0;
    private KarakterDurum _karakterDurum;

    void Awake()
    {
        hotbarSlotlari = new EnvanterYuvasi[hotbarBoyutu];
    }

    void Start()
    {
        _karakterDurum = GetComponent<KarakterDurum>();
    }

    public bool EsyaEkle(EsyaTipi tip)
    {
        EsyaVeriSO veri = InventoryUIManager.Instance.VeriGetir(tip);
        if (veri == null) return false;

        // 1. ADIM: Zaten var olan ve dolu olmayan bir yuvayý ara
        for (int i = 0; i < hotbarBoyutu; i++)
        {
            if (hotbarSlotlari[i].tip == tip && hotbarSlotlari[i].miktar < veri.maksIstifBoyutu)
            {
                hotbarSlotlari[i].miktar++;
                InventoryUIManager.Instance.EnvanteriGuncelle();
                return true;
            }
        }

        // 2. ADIM: Boþ bir yuva ara
        for (int i = 0; i < hotbarBoyutu; i++)
        {
            if (hotbarSlotlari[i].miktar == 0) // Boþ yuva = miktarý 0 olan yuva
            {
                hotbarSlotlari[i].tip = tip;
                hotbarSlotlari[i].miktar = 1;
                InventoryUIManager.Instance.EnvanteriGuncelle();
                return true;
            }
        }

        Debug.Log("Hotbar dolu, esya eklenemedi.");
        return false;
    }

    public void SeciliEsyayiKullan()
    {
        EnvanterYuvasi seciliYuva = hotbarSlotlari[seciliSlotIndex];
        if (seciliYuva.miktar > 0)
        {
            EsyaVeriSO veri = InventoryUIManager.Instance.VeriGetir(seciliYuva.tip);
            if (veri != null && veri.kullanilabilir)
            {
                if (_karakterDurum != null) { _karakterDurum.CanArtir(veri.canEtkisi); }

                hotbarSlotlari[seciliSlotIndex].miktar--;
                if (hotbarSlotlari[seciliSlotIndex].miktar <= 0)
                {
                    hotbarSlotlari[seciliSlotIndex].tip = EsyaTipi.Bos;
                }
                InventoryUIManager.Instance.EnvanteriGuncelle();
            }
        }
    }

    public void SeciliEsyayiAt()
    {
        EnvanterYuvasi seciliYuva = hotbarSlotlari[seciliSlotIndex];
        if (seciliYuva.miktar > 0)
        {
            EsyaVeriSO veri = InventoryUIManager.Instance.VeriGetir(seciliYuva.tip);
            if (veri != null && veri.esyaModelPrefab != null)
            {
                Vector3 atmaPozisyonu = transform.position + transform.forward * 2f + Vector3.up * 0.5f;
                Instantiate(veri.esyaModelPrefab, atmaPozisyonu, Quaternion.identity);
            }

            hotbarSlotlari[seciliSlotIndex].miktar--;
            if (hotbarSlotlari[seciliSlotIndex].miktar <= 0)
            {
                hotbarSlotlari[seciliSlotIndex].tip = EsyaTipi.Bos;
            }
            InventoryUIManager.Instance.EnvanteriGuncelle();
        }
    }
}