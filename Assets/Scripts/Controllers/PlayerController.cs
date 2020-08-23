using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{    
    [SerializeField, Header("Player Stats")]
    private int _gold;
    [SerializeField]
    private int _lives;
    [SerializeField]
    private int _level;

    public int Gold
    {
        get
        {
            return _gold;
        }
        set
        {
            _gold = value;
            UserInterfaceController.Instance.UpdatePlayerStats(StatType.Gold, value);
        }
    }

    public int Lives
    {
        get
        {
            return _lives;
        }
        set
        {
            _lives = value;
            UserInterfaceController.Instance.UpdatePlayerStats(StatType.Lives, value);
        }
    }

    public int Level
    {
        get
        {
            return _level;
        }
        set
        {
            _level = value;
            UserInterfaceController.Instance.UpdatePlayerStats(StatType.Level, value);
        }
    }

    #region | Instance |
    private static PlayerController _instance;
    public static PlayerController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerController>();
            }

            return _instance;
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        this.Gold = 100000;
        this.Lives = 100;
    }

    // Update is called once per frame
    void Update()
    {
        CheckUserClick();
    }

    void CheckUserClick()
    {
        BuildState currentState = UserInterfaceController.Instance.BuildPanel.CurrentBuildState;

        if ((currentState == BuildState.None || currentState == BuildState.Sell) && Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
            {
                var tc = hit.collider.gameObject.GetComponent<TowerController>();

                if (tc != null)
                {
                    UserInterfaceController.Instance.BuildPanel.Target = tc;
                    UserInterfaceController.Instance.BuildPanel.SetBuildState(BuildState.Sell);
                }
            }
        }
    }

}
