using UnityEngine;

// Her bir EsyaTipi icin ayri bir veri dosyasi olusturmamizi saglar.
[CreateAssetMenu(fileName = "Esya_Yeni", menuName = "Esya/Yeni Esya Verisi")]
public class EsyaVeriSO : ScriptableObject
{
    [Header("Temel Tan�m")]
    // Bu veri dosyasinin hangi EsyaTipi'ne ait oldugunu gosterir
    public EsyaTipi esyaTipi;

    public string gorunurAd = "Yeni Oge";

    [TextArea(3, 5)]
    public string aciklama = "Bu bir oyundaki esyadir.";

    [Header("G�rsel & �stif")]
    // Her esyanin envanterdeki RESMI (ICON)
    public Sprite ikon;

    public int maksIstifBoyutu = 99; // Kac tanesinin ust uste binebilecegi (Stack size)

    [Header("Kullan�m Etkisi")]
    public float canEtkisi = 0f; 
    public bool kullanilabilir = false; 

    [Header("D�nya Modeli")]
    // YEN� EKLENEN: Esya atildiginda dunyada olusacak 3D model.
    public GameObject esyaModelPrefab;
}