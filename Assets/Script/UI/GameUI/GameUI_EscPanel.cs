using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class GameUI_EscPanel : MonoBehaviour
{
    [SerializeField]
    private Transform transform_Panel;
    [SerializeField]
    private Button btn_Quit;
    private void Start()
    {
        btn_Quit.onClick.AddListener(Quit);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            transform_Panel.gameObject.SetActive(!transform_Panel.gameObject.activeSelf);
        }
    }
    private void Quit()
    {
        MessageBroker.Default.Publish(new NetEvent.NetEvent_QuitGame() { });
    }
}
