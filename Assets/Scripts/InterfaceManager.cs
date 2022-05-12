using UnityEngine;
using TMPro;
using DG.Tweening;

public class InterfaceManager : MonoBehaviour
{

    public PikminManager pikminManager;
    public Onion onion;
    public TextMeshProUGUI pikminCountText;
    public TextMeshProUGUI pokoText;
    public TextMeshProUGUI totalOnion;
    public TextMeshProUGUI pikminOutside;
    public TextMeshProUGUI redInside;
    public TextMeshProUGUI redOutside;
    public TextMeshProUGUI yellowInside;
    public TextMeshProUGUI yellowOutside;
    public TextMeshProUGUI blueInside;
    public TextMeshProUGUI blueOutside;
    public TextMeshProUGUI cyanInside;
    public TextMeshProUGUI cyanOutside;
    public TextMeshProUGUI caveName;
    public TextMeshProUGUI pikminSelected;


    void Start()
    {
        pikminManager.pikminFollow.AddListener((x) => UpdatePikminNumber(x));
        pikminManager.pikminDied.AddListener((x) => UpdatePikminNumber(x));
        int number = CarryObject.pokototal;
        UpdatePokoNumber(number);
    }

    void UpdatePikminNumber(int num)
    {
        pikminCountText.transform.DOComplete();
        pikminCountText.transform.DOPunchScale(Vector3.one/3, .3f, 10, 1);
        pikminCountText.text = num.ToString();
    }

    void UpdatePokoNumber(int num)
    {
        pokoText.transform.DOComplete();
        pokoText.transform.DOPunchScale(Vector3.one / 3, .3f, 10, 1);
        pokoText.text = num.ToString();
    }

    public void UpdateTotalOnion(int num)
    {
        totalOnion.transform.DOComplete();
        totalOnion.transform.DOPunchScale(Vector3.one / 3, .3f, 10, 1).SetUpdate(true);
        totalOnion.text = num.ToString();
    }

    public void UpdatePikminOutside(int num)
    {
        pikminOutside.transform.DOComplete();
        pikminOutside.transform.DOPunchScale(Vector3.one / 3, .3f, 10, 1).SetUpdate(true);
        pikminOutside.text = num.ToString();
    }

    public void UpdateRedInside(int num)
    {
        redInside.transform.DOComplete();
        redInside.transform.DOPunchScale(Vector3.one / 3, .3f, 10, 1).SetUpdate(true);
        redInside.text = num.ToString();
    }

    public void UpdateRedOutside(int num)
    {
        redOutside.transform.DOComplete();
        redOutside.transform.DOPunchScale(Vector3.one / 3, .3f, 10, 1).SetUpdate(true);
        redOutside.text = num.ToString();
    }

    public void UpdateYellowInside(int num)
    {
        yellowInside.transform.DOComplete();
        yellowInside.transform.DOPunchScale(Vector3.one / 3, .3f, 10, 1).SetUpdate(true);
        yellowInside.text = num.ToString();
    }

    public void UpdateYellowOutside(int num)
    {
        yellowOutside.transform.DOComplete();
        yellowOutside.transform.DOPunchScale(Vector3.one / 3, .3f, 10, 1).SetUpdate(true);
        yellowOutside.text = num.ToString();
    }

    public void UpdateBlueInside(int num)
    {
        blueInside.transform.DOComplete();
        blueInside.transform.DOPunchScale(Vector3.one / 3, .3f, 10, 1).SetUpdate(true);
        blueInside.text = num.ToString();
    }

    public void UpdateBlueOutside(int num)
    {
        blueOutside.transform.DOComplete();
        blueOutside.transform.DOPunchScale(Vector3.one / 3, .3f, 10, 1).SetUpdate(true);
        blueOutside.text = num.ToString();
    }

    public void UpdateCyanInside(int num)
    {
        cyanInside.transform.DOComplete();
        cyanInside.transform.DOPunchScale(Vector3.one / 3, .3f, 10, 1).SetUpdate(true);
        cyanInside.text = num.ToString();
    }

    public void UpdateCyanOutside(int num)
    {
        cyanOutside.transform.DOComplete();
        cyanOutside.transform.DOPunchScale(Vector3.one / 3, .3f, 10, 1).SetUpdate(true);
        cyanOutside.text = num.ToString();
    }

    public void UpdateCaveName(string name)
    {
        caveName.text = name.ToString();
    }

    public void UpdatePikminSelected(string name)
    {
        pikminSelected.text = name.ToString();
    }
}
