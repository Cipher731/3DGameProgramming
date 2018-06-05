using UnityEngine;
using UnityEngine.UI;

public class UGuiHealthBarScript : MonoBehaviour
{
    public int CurrentHealth;
    public Image HealthBar;

    private void Update()
    {
        var targetRotation = Quaternion.LookRotation(Camera.main.transform.position - transform.position);
        transform.rotation = targetRotation;
        HealthBar.fillAmount = CurrentHealth / 100.0f;
    }
}