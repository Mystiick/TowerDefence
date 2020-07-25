using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Stats")]
    public int Gold;



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
        
    }

    // Update is called once per frame
    void Update()
    {
        UserInterfaceController.Instance.GoldLabel.text = Gold.ToString("N0");

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
