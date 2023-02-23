/*********************************************************************************************
* Project: Simulation
* File   : EnvironmentManager
* Date   : 25.05.2022
* Author : Marcel Klein
*
* Manager class for setting up the time, calender and weather system of the simulation
* 
* History:
*    25.05.2022    MK    Created
*********************************************************************************************/


using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.VFX;


public enum EMonths
{
    January,
    February,
    March,
    April,
    May,
    June,
    July,
    August,
    September,
    October,
    November,
    December,
}
public enum ESeasons
{
    Spring,
    Summer,
    Autumn,
    Winter
}
public enum EWeather
{
    Rainy,
    Sunny,
    Dusty,
    Snowy
}

[ExecuteAlways]
public class EnvironmentManager : MonoBehaviour
{
    #region Variables

    [Header("Time")]
    [Tooltip("Day Length in Minutes")]
    [SerializeField]
    private float targetDayLength = 0.25f; //length of day in minutes
    [SerializeField]
    private float elapsedTime = 0;
    [SerializeField]
    private bool use24Clock = true;
    [SerializeField]
    [Range(0f, 1f)]
    private float timeOfDay = 0f; 
    [SerializeField]
    private int dayNumber = 1; //tracks the days passed
    [SerializeField]
    private int monthNumber = 0;
    [SerializeField]
    private int monthLength = 2;
    [SerializeField]
    private int yearNumber = 1;
    [SerializeField]
    private int yearLength = 11;
    [SerializeField]
    private AnimationCurve timeCurve;
    [SerializeField]
    private bool pause = false;
    private float timeScale = 100f;
    private float timeCurveNormalization = 0f;

    [Header("HUD")]
    [SerializeField]
    private TextMeshProUGUI clockText = null;
    [SerializeField]
    private TextMeshProUGUI dayText = null;
    [SerializeField]
    private TextMeshProUGUI monthText = null;
    [SerializeField]
    private TextMeshProUGUI yearText = null;
    [SerializeField]
    private TextMeshProUGUI seasonText = null;


    [Header("Weather")]
    [SerializeField]
    private Volume globalVolume = null;
    [SerializeField]
    private VolumeProfile defaultVolume = null;
    [SerializeField]
    private VolumeProfile summerVolume = null;
    [SerializeField]
    private VolumeProfile winterVolume = null;
    [SerializeField]
    private VolumeProfile currentVolume = null;
    [SerializeField]
    private EWeather currentWeather = EWeather.Snowy;
    [SerializeField]
    private VisualEffect snowEffect = null;
    [SerializeField]
    private ParticleSystem rainEffect = null;
    [SerializeField]
    private Shader terrainShader = null;
    [SerializeField]
    private List<Material> terrainMaterialList = null;
    private float snowOpacity = 0;
    private float terrainMetallic = 0;
    private float terrainSmoothness = 0;
    private float t = 0;

    [Header("Sun Light")]
    [SerializeField]
    private Transform dailyRotation = null;
    [SerializeField]
    private Light sun = null;
    [SerializeField]
    private float sunBaseIntensity = 0.4f;
    [SerializeField]
    private float sunVariation = 1.2f;
    [SerializeField]
    private Gradient sunColor = null;
    private float intensity;
    private float sunSummerBaseIntensity = 0.6f;

    [Header("Seasons")]
    [SerializeField]
    private Transform sunSeasonalRotation = null;
    [SerializeField]
    [Range(-45f, 45f)]
    private float maxSeasonalTilt = 0f;
    [SerializeField]
    private EMonths currentMonth = EMonths.January;
    [SerializeField]
    private ESeasons currentSeason = ESeasons.Winter;

    [Header("Modules")]
    private List<ModuleBase> moduleList = new List<ModuleBase>();


    public bool isDay = false;
    public bool isNight = true;

    #endregion

    #region Properties
    public float TargetDayLength
    {
        get
        {
            return targetDayLength;
        }
    }
    public float TimeOfDay
    {
        get
        {
            return timeOfDay;
        }
    }    
    public int DayNumber
    {
        get
        {
            return dayNumber;
        }
    }
    public int MonthNumber
    {
        get
        {
            return monthNumber;
        }
    }    
    public int MonthLength
    {
        get
        {
            return monthLength;
        }
    }    
    public int YearNumber
    {
        get
        {
            return yearNumber;
        }
    }    
    public float YearLength
    {
        get
        {
            return yearLength;
        }
    }
    #endregion

    #region Methods

    private void Awake()
    {
        pause = false;
        globalVolume = GetComponent<Volume>();
        currentVolume = globalVolume.profile;
        foreach (Material terrainMaterial in terrainMaterialList)
        {
            terrainMaterial.SetFloat("snowOpacity", 0f);
            snowOpacity = terrainMaterial.GetFloat("snowOpacity");
            terrainMaterial.SetFloat("_Metallic", 0f);
            terrainMetallic = terrainMaterial.GetFloat("_Metallic");
            terrainMaterial.SetFloat("_Smoothness", 0f);
            terrainSmoothness = terrainMaterial.GetFloat("_Smoothness");
        }
        NormalTimeCurve();
    }

    private void Update()
    {
        if (!pause)
        {
            UpdateTimeScale();
            UpdateTimeAndSeasons();
            UpdateClock();
            UpdateWeather();
        }

        //Checking for Day or Night
        if (timeOfDay < 0.25f)
        {
            isDay = false;
            isNight = true;
        }
        else if (timeOfDay > 0.75f)
        {
            isDay = false;
            isNight = true;
        }
        else if (timeOfDay > 0.25f)
        {
            isDay = true;
            isNight = false;
        }
        else if (timeOfDay < 0.75f)
        {
            isDay = true;
            isNight = false;
        }

        AdjustSunRotation();
        SunIntensity();
        AdjustSunColor();
        UpdateModules(); //will update modules each frame
    }

    private void OnDisable()
    {
        snowOpacity = 0f;
        terrainMetallic = 0f;
        terrainSmoothness = 0f;
        currentWeather = EWeather.Snowy;
        currentMonth = EMonths.January;
        currentSeason = ESeasons.Winter;
        dayNumber = 1;
        monthNumber = 0;
        yearNumber = 1;
    }
    private void OnEnable()
    {
        snowOpacity = 0f;
        terrainMetallic = 0f;
        terrainSmoothness = 0f;
        currentWeather = EWeather.Snowy;
        currentMonth = EMonths.January;
        currentSeason = ESeasons.Winter;
    }


    //Making time pass faster at night and slower at day by using an animation curve
    private void NormalTimeCurve()
    {
        float stepSize = 0.01f;
        int numberSteps = Mathf.FloorToInt(1f / stepSize);
        float curveTotal = 0;

        //Numerical Integration
        for (int i = 0; i < numberSteps; i++)
        {
            curveTotal += timeCurve.Evaluate(i * stepSize);
        }

        timeCurveNormalization = curveTotal / numberSteps; //keeps day length at target value
    }

    //Calender methods
    private void UpdateTimeScale()
    {
        timeScale = 24 / (targetDayLength / 60);
        timeScale *= timeCurve.Evaluate(elapsedTime / (targetDayLength * 60)); //changes timescale based on time curve
        timeScale /= timeCurveNormalization; //keeps day length at target value
    }
    private void UpdateTimeAndSeasons()
    {
        //Time
        timeOfDay += Time.deltaTime * timeScale / 86400; // seconds in a day
        elapsedTime += Time.deltaTime;
        if (timeOfDay > 1) //new day
        {
            elapsedTime = 0;
            dayNumber++;
            timeOfDay -= 1;

            if (dayNumber > monthLength) //new month
            {
                monthNumber++;
                currentMonth = EMonths.January + monthNumber;
                dayNumber = 1;
            }
            if (monthNumber > yearLength) //new year
            {
                yearNumber++;
                currentMonth = EMonths.January;
                monthNumber = 0;
            }
        }

        //Months and Seasons
        switch (currentMonth)
        {
            case EMonths.January:
                currentSeason = ESeasons.Winter;
                break;
            case EMonths.February:
                currentSeason = ESeasons.Spring;
                break;
            case EMonths.March:
                currentSeason = ESeasons.Spring;
                break;
            case EMonths.April:
                currentSeason = ESeasons.Spring;
                break;
            case EMonths.May:
                currentSeason = ESeasons.Summer;
                break;
            case EMonths.June:
                currentSeason = ESeasons.Summer;
                break;
            case EMonths.July:
                currentSeason = ESeasons.Summer;
                break;
            case EMonths.August:
                currentSeason = ESeasons.Autumn;
                break;
            case EMonths.September:
                currentSeason = ESeasons.Autumn;
                break;
            case EMonths.October:
                currentSeason = ESeasons.Autumn;
                break;
            case EMonths.November:
                currentSeason = ESeasons.Winter;
                break;
            case EMonths.December:
                currentSeason = ESeasons.Winter;
                break;
            default:
                break;
        }
    }
    private void UpdateClock()
    {
        float time = elapsedTime / (targetDayLength * 60);
        float hour = Mathf.FloorToInt(time * 24);
        float minute = Mathf.FloorToInt(((time * 24) - hour) * 60);

        string hourString;
        string minuteString;
        string dayString;
        string yearString;

        //Clock Setup
        if (!use24Clock && hour > 12)
            hour -= 12;

        if (hour < 10)
            hourString = "0" + hour.ToString();
        else
            hourString = hour.ToString();

        if (minute < 10)
            minuteString = "0" + minute.ToString();
        else
            minuteString = minute.ToString();

        if (use24Clock)
            clockText.text = "Clock:\t" + hourString + " : " + minuteString;
        else if (time > 0.5f)
            clockText.text = "Clock:\t" + hourString + " : " + minuteString + " pm";
        else
            clockText.text = "Clock:\t" + hourString + " : " + minuteString + " am";

        //Day Setup
        if (DayNumber != 0)
        {
            if (DayNumber < 10)
                dayString = "0" + DayNumber.ToString();
            else
                dayString = DayNumber.ToString();

            dayText.text = "Day: " + dayString;
        }
        else
        {
            dayText.text = "Day: 000";
        }

        //Month Setup
        monthText.text = $"Month: {currentMonth}";

        //Year Setup
        if (yearNumber != 0)
        {
            if (yearNumber < 10)
                yearString = "000" + yearNumber.ToString();
            else if (yearNumber < 100)
                yearString = "00" + yearNumber.ToString();
            else if (yearNumber < 1000)
                yearString = "0" + yearNumber.ToString();
            else
                yearString = yearNumber.ToString();

            yearText.text = "Year: " + yearString;
        }
        else
        {
            yearText.text = "Year: 0000";
        }

        //Season Setup
        seasonText.text = $"Season: {currentSeason}";
    }

    //Weather methods
    private void UpdateWeather()
    {
        switch (currentSeason)
        {
            case ESeasons.Spring:
                currentWeather = EWeather.Rainy;
                var stopSnowEvent = snowEffect.CreateVFXEventAttribute();
                snowEffect.SendEvent(VisualEffectAsset.StopEventID, stopSnowEvent);
                rainEffect.Play();
                globalVolume.profile = defaultVolume;
                //Customize the terrain Material according to snow- and rainfall
                if (snowOpacity > 0)
                {
                    snowOpacity = Mathf.Lerp(snowOpacity, 0, t);
                    terrainSmoothness = Mathf.Lerp(terrainSmoothness, 0.5f, t);
                    t += 0.0001f * Time.deltaTime;
                    foreach (Material terrainMaterial in terrainMaterialList)
                    {

                        terrainMaterial.SetFloat("snowOpacity", snowOpacity);
                        if (terrainSmoothness < 0.5f)
                        {
                            terrainMaterial.SetFloat("_Smoothness", terrainSmoothness);
                        }
                    }
                }
                break;
            case ESeasons.Summer:
                currentWeather = EWeather.Sunny;
                rainEffect.Stop();
                globalVolume.profile = summerVolume;
                if (terrainSmoothness > 0)
                {
                    terrainSmoothness = Mathf.Lerp(terrainSmoothness, 0f, t);
                    t += 0.0001f * Time.deltaTime;
                    foreach (Material terrainMaterial in terrainMaterialList)
                    {

                        terrainMaterial.SetFloat("_Smoothness", terrainSmoothness);
                    }
                }
                break;
            case ESeasons.Autumn:
                currentWeather = EWeather.Dusty;
                rainEffect.Play();
                globalVolume.profile = defaultVolume;
                if (terrainMetallic < 0.5f)
                {
                    terrainSmoothness = Mathf.Lerp(terrainSmoothness, 0.5f, t);
                    t += 0.0001f * Time.deltaTime;
                    foreach (Material terrainMaterial in terrainMaterialList)
                    {

                        terrainMaterial.SetFloat("_Smoothness", terrainSmoothness);
                    }
                }
                break;
            case ESeasons.Winter:
                currentWeather = EWeather.Snowy;
                var playSnowEvent = snowEffect.CreateVFXEventAttribute();
                snowEffect.SendEvent(VisualEffectAsset.PlayEventID, playSnowEvent);
                rainEffect.Stop();
                globalVolume.profile = winterVolume;
                if (snowOpacity < 1f)
                {
                    snowOpacity = Mathf.Lerp(snowOpacity, 1, t);
                    t += 0.0001f * Time.deltaTime;
                    foreach (Material terrainMaterial in terrainMaterialList)
                    {

                        terrainMaterial.SetFloat("snowOpacity",snowOpacity);
                    }
                }
                break;
            default:
                break;
        }
    }

    //Sun manager
    private void AdjustSunRotation()
    {
        float sunAngle = timeOfDay * 360f;
        dailyRotation.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, sunAngle));

        float seasonalAngle = -maxSeasonalTilt * Mathf.Cos(MonthNumber / yearLength * 2f * Mathf.PI);
        sunSeasonalRotation.localRotation = Quaternion.Euler(new Vector3(seasonalAngle, 0f, 0f));
    }
    private void SunIntensity()
    {
        //Scalar product for projection issues
        intensity = Vector3.Dot(sun.transform.forward, Vector3.down);
        intensity = Mathf.Clamp01(intensity);

        if (currentWeather == EWeather.Sunny || currentWeather == EWeather.Rainy)
        {
            sun.intensity = intensity * sunVariation + sunSummerBaseIntensity;
        }
        else
        {
            sun.intensity = intensity * sunVariation + sunBaseIntensity;
        }
    }
    private void AdjustSunColor()
    {
        sun.color = sunColor.Evaluate(intensity);
    }

    //Modules manager
    public void AddModule(ModuleBase module)
    {
        moduleList.Add(module);
    }
    public void RemoveModule(ModuleBase module)
    {
        moduleList.Remove(module);
    }
    //Update each module based on current sun intensity
    private void UpdateModules()
    {
        foreach (ModuleBase module in moduleList)
        {
            module.UpdateModule(intensity);
        }
    }

    #endregion
}
