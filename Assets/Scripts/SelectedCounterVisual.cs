using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField]private ClearCounter clearcounter;
    [SerializeField]private GameObject highlightVisual;
    private void Start()
    {
        Player.Instance.OnSelectedCounterChanged += Player_OnselectedCounterChanged; 
    }

    private void Player_OnselectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        if(e.selectedCounter == clearcounter)
            Show();
        else
            Hide();

    }

    private void Show()
    {
        highlightVisual.SetActive(true);
    }

    private void Hide()
    {
        highlightVisual.SetActive(false);
    }
}
