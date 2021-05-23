using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    #region Variabeles
    public static GameManager instance;
    public HpUIElements CanvasHpUiElements;
    #endregion
    #region Monobehaviour Callbacks
    private void OnEnable()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        DontDestroyOnLoad(gameObject);
    }
    #endregion
    #region Functions

    #endregion
}
#region Serilizable classes
[System.Serializable]
public class HpUIElements
{
    public GameObject backGoundObj;
    public TMP_Text healthText;
    public Image foreImage;
}
#endregion