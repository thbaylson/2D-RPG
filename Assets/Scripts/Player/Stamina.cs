using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stamina : Singleton<Stamina>
{
    [SerializeField] private Sprite fullStaminaImage, emptyStaminaImage;
    [SerializeField] private int staminaRefreshRate = 3;

    private const string STAMINA_CONTAINER = "StaminaContainer";

    public int CurrentStamina { get; private set; }
    private Transform staminaContainer;
    private int startingStamina = 3;
    private int maxStamina;

    protected override void Awake()
    {
        base.Awake();
        maxStamina = startingStamina;
        CurrentStamina = startingStamina;
    }

    private void Start()
    {
        staminaContainer = GameObject.Find(STAMINA_CONTAINER).transform;
    }

    public void UseStamina()
    {
        CurrentStamina--;
        UpdateStaminaImages();
        // TODO: This feels gross to stop all when there's only one to stop. If I add more later, remember that this is here!
        StopAllCoroutines();
        StartCoroutine(RefreshStaminaRoutine());
    }

    public void RefreshStamina()
    {
        // Make sure we never have more stamina than the max
        if(CurrentStamina < maxStamina && !PlayerHealth.Instance.IsDead)
        {
            CurrentStamina++;
            UpdateStaminaImages();
        }
    }

    public void ReplenishStaminaOnDeath()
    {
        CurrentStamina = startingStamina;
        UpdateStaminaImages();
    }

    private void UpdateStaminaImages()
    {
        for(int i = 0; i < maxStamina; i++)
        {
            Transform child = staminaContainer.GetChild(i);
            Image image = child?.GetComponent<Image>();

            // Index starts at 0, but stamina globes start counting at 1
            if(i <= CurrentStamina - 1)
            {
                image.sprite = fullStaminaImage;
            }
            else
            {
                image.sprite = emptyStaminaImage;
            }
        }
    }

    // TODO: Add visual feedback for stamina refreshing over time.
    private IEnumerator RefreshStaminaRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(staminaRefreshRate);
            RefreshStamina();
        }
    }
}
