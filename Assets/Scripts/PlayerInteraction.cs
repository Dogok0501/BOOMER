using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionDistance;

    public Text interactionText;
    public GameObject interactionHoldUI;
    public Image intercationProgressBar;

    private void Start()
    {
       
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2 / 4, Screen.height / 2 / 4, 0));
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, interactionDistance))
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();


            if(interactable != null)
            {
                HandleInteraction(interactable);
                interactionText.text = interactable.GetDescription();

                interactionHoldUI.SetActive(interactable.interactionType == Interactable.InteractionType.Hold);
            }
            else
            {
                interactionText.text = "";
                interactionHoldUI.SetActive(false);
            }
        }        
    }

    void HandleInteraction(Interactable interactable)
    {
        switch (interactable.interactionType)
        {
            case Interactable.InteractionType.Click:
                if(Input.GetKeyDown(KeyCode.E))
                {
                    interactable.Interact();
                }
                break;
            case Interactable.InteractionType.Hold:
                if (Input.GetKey(KeyCode.E))
                {
                    interactable.IncreaseHoldTime();
                    if(interactable.GetHoldTime() > 1f)
                    {
                        interactable.Interact();
                        interactable.ResetHoldTime();
                    }                    
                }
                else
                {
                    interactable.ResetHoldTime();
                }
                intercationProgressBar.fillAmount = interactable.GetHoldTime();
                break;
            case Interactable.InteractionType.Minigame:
                //¹Ì±¸Çö
                break;
        }
    }
}
