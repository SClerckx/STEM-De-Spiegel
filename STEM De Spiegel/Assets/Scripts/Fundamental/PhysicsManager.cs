using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsManager : MonoBehaviour
{
    public int JoulesToCalories(float joules)
    {
        int Calories = (int)(joules * 0.238846);
        return Calories;
    }

    public string FormatSeconds(float timeInSeconds)
    {
        string FormatedSeconds;

        float hoursExact = timeInSeconds / 3600;
        if (hoursExact < 24)
        {
            int hours = (int)(timeInSeconds / 3600);
            int minutes = (int)(timeInSeconds % 3600) / 60;
            int seconds = (int)(timeInSeconds % 3600) % 60;

            FormatedSeconds = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
        }
        else
        {
            float dagen = hoursExact / 24f;
            if (dagen > 365)
            {
                float jaren = dagen / 365f;
                FormatedSeconds = jaren.ToString("F2") + " jaren";
            }
            else
            {
                FormatedSeconds = dagen.ToString("F2") + " dagen";
            }
        }

        return FormatedSeconds;
    }

    public string FormatDistance(float distanceInMeters)
    {
        string formatedDistance;

        if (distanceInMeters < 1000)
        {
            formatedDistance = distanceInMeters.ToString("F0") + " m";
        }
        else
        {
            float distanceInKiloMeters = (distanceInMeters / 1000);
            formatedDistance = distanceInKiloMeters.ToString("F2") + " km";
        }

        return formatedDistance;
    }

    public string FormatEnergy(float energyInJoules)
    {
        string formatedEnergy;

        float energyInKiloCalories = energyInJoules * 0.000239005736f;
        float energyHumanBody = energyInKiloCalories / 0.25f; //Uitgeoefende energie is ongeveer 25% van de energie die effectief word verbrand door de persoon
        formatedEnergy = energyHumanBody.ToString("F2") + " Cal / kcal";

        return formatedEnergy;
    }

    public string FormatSpeed(float speedInMS)
    {
        string formattedSpeed;

        float speedInKmh = speedInMS * 3.6f;
        formattedSpeed = speedInKmh.ToString("F2") + " km/h";

        return formattedSpeed;
    }
}
