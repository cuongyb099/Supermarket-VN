using KatLib.Singleton;
using UnityEngine;
using UnityEngine.Rendering;

namespace Core.DaynightCycle
{
    public class DayNight : Singleton<DayNight>
    {
        private static readonly int GlobalSunDirection = Shader.PropertyToID("GlobalSunDirection");
        private static readonly int GlobalMoonDirection = Shader.PropertyToID("GlobalMoonDirection");
        
        [Header("Time Settings")]
        [SerializeField] protected float minutePerDay = 5f;
        public float MinutePerDay => minutePerDay;

        [SerializeField] protected int startHour = 4;
        public int StartHour => startHour;

        [SerializeField] protected int currentHour;
        [SerializeField] protected int currentMinute;
        [SerializeField] protected int currentDay = 1;
        public int CurrentDay => currentDay;
        [SerializeField] protected float timeScale = 1f;
        [SerializeField] protected float restTimeScale = 16f;

        [Header("Sunlight Settings")]
        protected Material skyboxMaterial;
        
        [SerializeField] protected Material skyboxDay;
        [SerializeField] protected Material skyboxNight;
        [SerializeField] protected Light directionalLight;
        [SerializeField] protected AnimationCurve lightIntensityCurve;
        [SerializeField] protected float timeOfDayNormalized;
        [SerializeField] protected float timeElapsed;
        public float TimeElapsed => timeElapsed;
        [SerializeField] protected float secondsPerDay;
        [SerializeField] protected TimeOfDay currentTimeOfDay;
        
        protected override void Awake()
        {
            base.Awake();
            if (skyboxDay != null && skyboxNight != null)
            {
                skyboxMaterial = new Material(skyboxDay);
            }

            RenderSettings.sun = directionalLight;
            RenderSettings.skybox = skyboxMaterial;
        }

#if UNITY_EDITOR
        void OnValidate()
        {
            if (skyboxDay != null && skyboxNight != null)
            {
                skyboxMaterial = new Material(skyboxDay);
            }
        }
#endif
        
        private void Start()
        {
            this.secondsPerDay = this.minutePerDay * 60;
            this.UpdateTime();
        }
        
        private void UpdateTime()
        {
            float initialTime = this.startHour * 60f;
            this.timeElapsed = (initialTime / 1440f) * this.secondsPerDay;
        }
        
        private void Update()
        {
            this.SimulateTime();
            this.UpdateLighting();
        }
        
        private void SimulateTime()
        {
            timeElapsed += Time.deltaTime * timeScale;
            float timeInDay = (timeElapsed / secondsPerDay) * 1440;
            currentHour = (int)(timeInDay / 60) % 24;
            currentMinute = (int)(timeInDay % 60);

            if (timeInDay >= 1440)
            {
                timeElapsed -= secondsPerDay;
                currentDay++;
            }

            if (currentHour >= 6 && currentHour < 12) currentTimeOfDay = TimeOfDay.Morning;
            else if (currentHour >= 12 && currentHour < 17) currentTimeOfDay = TimeOfDay.Noon;
            else if (currentHour >= 17 && currentHour < 20) currentTimeOfDay = TimeOfDay.Evening;
            else currentTimeOfDay = TimeOfDay.Night;
        }

        private void UpdateLighting()
        {
            if (!this.directionalLight) return;
            this.timeOfDayNormalized = (this.currentHour * 60f + this.currentMinute) / 1440f;
            this.directionalLight.intensity = lightIntensityCurve.Evaluate(this.timeOfDayNormalized);

            if (skyboxDay && skyboxNight)
            {
                skyboxMaterial.Lerp(skyboxDay, skyboxNight, timeOfDayNormalized);
                RenderSettings.skybox = skyboxMaterial;
            }

            DynamicGI.UpdateEnvironment();
        }
        
        public virtual TimeOfDay GetTime()
        {
            return this.currentTimeOfDay;
        }

        public virtual bool Is(TimeOfDay timeToCheck)
        {
            return this.currentTimeOfDay == timeToCheck;
        }

        public virtual bool IsNight()
        {
            return this.Is(TimeOfDay.Night);
        }
        
        public virtual void SetTimeScale(float timeScale)
        {
            this.timeScale = timeScale;
        }
    }
}
