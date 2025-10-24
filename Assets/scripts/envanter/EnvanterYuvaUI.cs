using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnvanterYuvaUI : MonoBehaviour
{
    // Inspector'da bu alanlara, Prefab icindeki dogru bilesenleri s�r�kleyip birakacagiz.
    public Image ikonResmi;
    public TextMeshProUGUI miktarMetni;

    // YEN� EKLENEN: Esya ismini gosterecek metin
    public TextMeshProUGUI isimMetni;
    public GameObject secimCercevesi;
 
    public void YuvayiGuncelle(EsyaVeriSO esyaVerisi, int miktar)
    {
        if (esyaVerisi != null)
        {
            // �KONU AYARLA
            if (ikonResmi != null && esyaVerisi.ikon != null)
            {
                ikonResmi.sprite = esyaVerisi.ikon;
                ikonResmi.color = Color.white;
            }

            // M�KTARI AYARLA
            if (miktarMetni != null)
            {
                miktarMetni.text = miktar > 1 ? miktar.ToString() : "";
            }

            // YEN� EKLENEN: �SM� AYARLA
            if (isimMetni != null)
            {
                // EsyaVeriSO icindeki 'gorunurAd' degiskenini kullaniriz.
                isimMetni.text = esyaVerisi.gorunurAd;
            }
        }
        else
        {
            Temizle();
        }
    }
    public void SecimiAyarla(bool seciliMi)
    {
        if (secimCercevesi != null)
        {
            secimCercevesi.SetActive(seciliMi);
        }
    }
    /// <summary>
    /// Yuvayi bosaltir (ikonu ve metinleri gizler).
    /// </summary>
    public void Temizle()
    {
        if (ikonResmi != null)
        {
            ikonResmi.sprite = null;
            ikonResmi.color = new Color(1, 1, 1, 0);
        }

        if (miktarMetni != null)
        {
            miktarMetni.text = "";
        }

        // YEN� EKLENEN: Isim metnini de temizle
        if (isimMetni != null)
        {
            isimMetni.text = "";
        }
    }
}