
using UnityEngine;
using System.Collections.Generic;

// EsyaTipi enum'u bu dosyanin uzerine tasiyabilirsiniz veya ayri dosyada birakabilirsiniz.

public class OyuncuEnvanter : MonoBehaviour
{
    // Dictionary: EsyaTipi ve o esyadan kac tane oldugunu tutar. (Modular Yapi)
    public Dictionary<EsyaTipi, int> envanter = new Dictionary<EsyaTipi, int>();

    // Karakterin can durumunu tutar (esya kullanimi icin gerekli)
    private KarakterDurum _karakterDurum;

    void Start()
    {
        _karakterDurum = GetComponent<KarakterDurum>();
    }

    /// <summary>
    /// Envantere belirtilen turdeki esyayi ekler (Toplanabilir.cs tarafindan cagrýlýr).
    /// </summary>
    public bool EsyaEkle(EsyaTipi tip, int miktar = 1)
    {
        if (miktar <= 0) return false;

        if (envanter.ContainsKey(tip))
        {
            envanter[tip] += miktar;
        }
        else
        {
            envanter.Add(tip, miktar);
        }

        Debug.Log($"[Envanter] {miktar} adet {tip} eklendi. Toplam: {envanter[tip]}");
        // Buraya UI GUNCELLEME KODU gelecek

        return true;
    }

    /// <summary>
    /// Envanterdeki bir esyayi kullanir (Esya kullanma mekanigi).
    /// </summary>
    public bool EsyaKullan(EsyaTipi tip, int miktar = 1)
    {
        if (!_karakterDurum)
        {
            Debug.LogError("KarakterDurum scripti bulunamadi!");
            return false;
        }

        if (!envanter.ContainsKey(tip) || envanter[tip] < miktar)
        {
            Debug.Log($"[Envanter] Yeterli {tip} bulunmuyor.");
            return false;
        }

        bool kullanildi = false;

        // ** MODÜLER KULLANIM MANTIÐI **
        switch (tip)
        {
            case EsyaTipi.Elma:
                _karakterDurum.CanArtir(15); // Elma 15 can versin
                kullanildi = true;
                break;
            case EsyaTipi.Sandvic:
                _karakterDurum.CanArtir(40); // Sandviç 40 can versin
                kullanildi = true;
                break;

            
            case EsyaTipi.KagitParca_1:
                kullanildi = true;
                break;
            case EsyaTipi.KagitParca_2:
                kullanildi = true;
                break;
            case EsyaTipi.KagitParca_3:
                kullanildi = true;
                break;
            case EsyaTipi.KagitParca_4:
                kullanildi = true;
                break;
            case EsyaTipi.KirmiziAnahtar:
            case EsyaTipi.büyüsayfasý:
                Debug.Log($"{tip} kullanilamaz, sadece envanterde tutulur.");
                return false;
        }

        // Kullaným baþarýlýysa envanterden düþ
        if (kullanildi)
        {
            envanter[tip] -= miktar;
            if (envanter[tip] <= 0)
            {
                envanter.Remove(tip);
            }
        }

        return kullanildi;
    }
}

/*public class OyuncuEnvanter : MonoBehaviour
{
    // Toplanan parca ID'lerini (1, 2, 3, 4) tutar
    public List<int> toplananParcalar = new List<int>();

    [Header("Bulmaca Ayarlari")]
    public int toplamParcaSayisi = 4;

    /// <summary>
    /// Envantere yeni parca ekler.
    /// </summary>
   
    /// <returns>Ekleme basariliysa true.</returns>
    public bool ParcaEkle(int id)
    {
        if (!toplananParcalar.Contains(id))
        {
            toplananParcalar.Add(id);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Tum parcalarin toplanip toplanmadigini kontrol eder.
    /// </summary>
    public bool TumParcalarToplandiMi()
    {
        return toplananParcalar.Count == toplamParcaSayisi;
    }

    /// <summary>
    /// Parcalari envanterden temizler (Bulmaca bittiginde kullanilacak).
    /// </summary>
    public void ParcalariTemizle()
    {
        toplananParcalar.Clear();
    }
}*/
