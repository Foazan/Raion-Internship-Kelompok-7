using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class TimeFilterManager : MonoBehaviour
{
    public Volume postProcessingVolume;
    private ColorAdjustments colorAdjustments;

    void Start()
    {
        if (postProcessingVolume.profile.TryGet(out colorAdjustments))
        {
            SetMorningFilter(); 
        }
    }

    public void SetMorningFilter()
    {
        colorAdjustments.colorFilter.value = new Color(1f, 0.9f, 0.8f); 
    }

    public void SetNoonFilter()
    {
        colorAdjustments.colorFilter.value = Color.white; 
    }

    public void SetNightFilter()
    {
        colorAdjustments.colorFilter.value = new Color(0.5f, 0.5f, 1f); 
    }

    public void SetEveningFilter()
    {
        colorAdjustments.colorFilter.value = new Color(1f, 0.6f, 0.4f); 
    }

}
