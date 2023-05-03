using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.SimpleStation14.Species.Shadowkin.Components
{
    [RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
    public sealed partial class ShadowkinComponent : Component
    {
        #region Random occurrences
        [ViewVariables(VVAccess.ReadWrite)]
        public float MaxedPowerAccumulator = 0f;

        [ViewVariables(VVAccess.ReadWrite)]
        public float MaxedPowerRateMin = 45f;

        [ViewVariables(VVAccess.ReadWrite)]
        public float MaxedPowerRateMax = 150f;


        [ViewVariables(VVAccess.ReadWrite)]
        public float MinPowerAccumulator = 0f;

        [ViewVariables(VVAccess.ReadWrite)]
        public float MinRateMin = 15f;

        [ViewVariables(VVAccess.ReadWrite)]
        public float MinRateMax = 60f;
        #endregion


        #region Darkening
        [ViewVariables(VVAccess.ReadWrite)]
        public bool Darken = false;

        [ViewVariables(VVAccess.ReadWrite)]
        public float DarkenRange = 5f;

        [ViewVariables(VVAccess.ReadOnly)]
        public List<EntityUid> DarkenedLights = new();

        // Accumulator for darkening
        [ViewVariables(VVAccess.ReadWrite)]
        public float DarkenRate = 0.084f; // 1/12th of a second

        [ViewVariables(VVAccess.ReadWrite)]
        public float DarkenAccumulator = 0f;
        #endregion


        #region Shader
        /// <summary>
        ///     Automatically set to eye color.
        /// </summary>
        [ViewVariables(VVAccess.ReadOnly)]
        public Vector3 TintColor = new(0.5f, 0f, 0.5f);

        /// <summary>
        ///     Based on PowerLevel.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite)]
        public float TintIntensity = 0.65f;
        #endregion


        #region Power level
        /// <summary>
        ///     Current amount of energy.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite), AutoNetworkedField]
        public float PowerLevel
        {
            get => _powerLevel;
            set => _powerLevel = Math.Clamp(value, PowerLevelMin, PowerLevelMax);
        }
        public float _powerLevel = 25f;

        /// <summary>
        ///     Don't let PowerLevel go above this value.
        /// </summary>
        [ViewVariables(VVAccess.ReadOnly), AutoNetworkedField]
        public float PowerLevelMax = PowerThresholds[ShadowkinPowerThreshold.Max];

        /// <summary>
        ///     Blackeyes if PowerLevel is this value.
        /// </summary>
        [ViewVariables(VVAccess.ReadOnly), AutoNetworkedField]
        public float PowerLevelMin = PowerThresholds[ShadowkinPowerThreshold.Min];

        /// <summary>
        ///     How much energy is gained per second.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite), AutoNetworkedField]
        public float PowerLevelGain = 2f;

        /// <summary>
        ///     Power gain multiplier
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite), AutoNetworkedField]
        public float PowerLevelGainMultiplier = 1f;

        /// <summary>
        ///     Whether to gain power or not.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite), AutoNetworkedField]
        public bool PowerLevelGainEnabled = true;

        /// <summary>
        ///     Whether they are a blackeye.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite), AutoNetworkedField]
        public bool Blackeye = false;


        public static readonly Dictionary<ShadowkinPowerThreshold, float> PowerThresholds = new()
        {
            { ShadowkinPowerThreshold.Max, 250.0f },
            { ShadowkinPowerThreshold.Great, 200.0f },
            { ShadowkinPowerThreshold.Good, 150.0f },
            { ShadowkinPowerThreshold.Okay, 100.0f },
            { ShadowkinPowerThreshold.Tired, 50.0f },
            { ShadowkinPowerThreshold.Min, 0.0f },
        };
        #endregion
    }

    [Serializable, NetSerializable]
    public enum ShadowkinPowerThreshold : byte
    {
        Max = 1 << 4,
        Great = 1 << 3,
        Good = 1 << 2,
        Okay = 1 << 1,
        Tired = 1 << 0,
        Min = 0,
    }
}
