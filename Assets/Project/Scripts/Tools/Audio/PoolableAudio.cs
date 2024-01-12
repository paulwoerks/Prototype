using UnityEngine;
using UnityEngine.Audio;
using Sirenix.OdinInspector;

namespace PocketHeroes.Audio
{
    /// <summary>
    /// Audio File including settings that can be played by pooling it.
    /// </summary>
    [System.Serializable]
    public class PoolableAudio
    {
        enum AudioFilePriority { Lowest = 32, Low = 64, Medium = 128, High = 192, Highest = 255 }

        #region Fields

        #region Required

        [Title("Required", "Required to play a poolable Sound")]

        [SerializeField] bool hasVariants;

        [Required]
        [Tooltip("path of the file")]
        [HideIf("@hasVariants == true")]
        [SerializeField] AudioClip clip;
        [ShowIf("@hasVariants == true")]
        [SerializeField] AudioClip[] variants;
        public AudioClip Clip => clip;
        [Required]
        [Tooltip("The group the sound is played in.")]
        [SerializeField] AudioMixerGroup mixerGroup;
        public AudioMixerGroup MixerGroup => mixerGroup;

        [Tooltip("How important the sound is. Less important sounds will be stopped when there is too much going on.")]
        [SerializeField] AudioFilePriority priority = AudioFilePriority.Medium;
        public int Priority => (int)priority;

        [Tooltip("The Type of sound")]
        [SerializeField] bool loop;

        [Tooltip("3D Sounds need a parent, otherwise it's treated as 2d")]
        [SerializeField] Transform parent;
        public Transform Parent => parent;

        #endregion

        [Title("Transitions", "Settings applying to every sound.")]

        [SerializeField] bool doFade;
        [ShowIf("@doFade == true")]
        [SerializeField] float fadeIn;
        public float FadeIn => fadeIn;
        [ShowIf("@doFade == true")]
        [SerializeField] float fadeOut;
        public float FadeOut => FadeOut;

        [SerializeField] bool hasDelay;
        [ShowIf("@hasDelay == true")]
        [SerializeField] float delayIn;
        public float DelayIn => delayIn;
        [ShowIf("@hasDelay == true")]
        [SerializeField] float delayOut;
        public float DelayOut => delayOut;


        [Title("Sound Settings")]
        [Range(0f, 1f)] 
        [SerializeField] float volume = 1f;
        [Range(0.1f, 2f)]
        [SerializeField] float pitch = 1f;

        [SerializeField] bool playRandomVolume;
        [ShowIf("@playRandomVolume == true")]
        [MinMaxSlider(0.0f, 1.0f, true)]
        [Tooltip("Range when playing random volume")]
        [SerializeField] Vector2 volumeRange = new Vector2(0.8f, 1f);
        [SerializeField] bool playRandomPitch;
        [ShowIf("@playRandomPitch == true")]
        [MinMaxSlider(0.1f, 2.0f, true)]
        [Tooltip("Range when playing random pitch.")]
        [SerializeField] Vector2 pitchRange = new Vector2(0.9f, 1.1f);

        [Tooltip("Left/Right ear shift. Default = 0f")]
        [Range(-1.0f, 1.0f)]
        [SerializeField] float panStereo;
        [Tooltip("Sets the amount of the output signal that gets routed to the reverb zones. The amount is linear in the (0 - 1) range, but allows for a 10 dB amplification in the (1 - 1.1) range which can be useful to achieve the effect of near-field and distant sounds.")]
        [Range(0.0f, 1.1f)]
        [SerializeField] float reverbZoneMix = 1f;


        [Title("Spatialization", "3D Sound settings. Only relevant for sounds played in 3D space.")]
        [Tooltip("Sets how much the 3D engine has an effect on the audio source.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] float spatialBlend = 0f;
        [HideIf("@spatialBlend == 0f")]
        [Tooltip("Determines how much doppler effect will be applied to this audio source (if is set to 0, then no effect is applied).")]
        [Range(0.0f, 1.0f)]
        [SerializeField] float dopplerLevel = 0f;
        [HideIf("@spatialBlend == 0f")]
        [Tooltip("[Logarithmic]: The sound is loud when you are close to the audio source, but when you get away from the object it decreases significantly fast.\n\n" +
        "[Linear]: The further away from the audio source you go, the less you can hear it.\n\n" +
        "[Custom]: Don't use it.")]
        [SerializeField] AudioRolloffMode rollOffMode = AudioRolloffMode.Logarithmic;
        [HideIf("@spatialBlend == 0f")]
        [MinMaxSlider(1, 1000, true)]
        [Tooltip("Within the MinDistance, the sound will stay at loudest possible. Outside MinDistance it will begin to attenuate. Increase the MinDistance of a sound to make it ‘louder’ in a 3d world, and decrease it to make it ‘quieter’ in a 3d world. MaxDistance is The distance where the sound stops attenuating at. Beyond this point it will stay at the volume it would be at MaxDistance units from the listener and will not attenuate any more.")]
        [SerializeField] Vector2 distanceRange = new Vector2(1, 1000);


        [Title("Ignores", "You can ignore cetain effects applied to channels or listeners.")]
        [SerializeField] bool showIgnores;

        [Tooltip("This is to quickly “by-pass” filter effects applied to the audio source. An easy way to turn all effects on/off.")]
        [ShowIf("@showIgnores")]
        [SerializeField] bool ignoreBypassEffects;

        [Tooltip("This is to quickly turn all Listener effects on/off.")]
        [ShowIf("@showIgnores")]
        [SerializeField] bool ignoreBypassListenerEffects;

        [Tooltip("This is to quickly turn all Reverb Zones on/off.")]
        [ShowIf("@showIgnores")][EnableIf("@reverbZoneMix > 0f")]
        [SerializeField] bool ignoreBypassReverbZones;

        [Tooltip("Ignores the volume on the listener.")]
        [ShowIf("@showIgnores")]
        [SerializeField] bool ignoreListenerVolume;

        [Tooltip("Ignores if the listener is on pause.")]
        [ShowIf("@showIgnores")]
        [SerializeField] bool ignoreListenerPause;

        public string Name => Clip.name;
        #endregion

        #region Public
        public void Play() => AudioPooler.Play(this);

        public void Stop() => AudioPooler.Stop(this);

        public AudioSource SetupAudioSource(AudioSource audioSource)
        {
            if (hasVariants && variants.Length > 0)
            {
                clip = variants[Random.Range(0, variants.Length)];
            }

            audioSource.clip = Clip;
            audioSource.outputAudioMixerGroup = MixerGroup;
            audioSource.volume = playRandomVolume ? Random.Range(volumeRange.x, volumeRange.y) : volume;
            audioSource.pitch = playRandomPitch ? Random.Range(pitchRange.x, pitchRange.y) : pitch;
            audioSource.loop = loop;
            audioSource.priority = Priority;
            audioSource.panStereo = panStereo;
            audioSource.reverbZoneMix = reverbZoneMix;

            audioSource.spatialBlend = spatialBlend;
            audioSource.dopplerLevel = dopplerLevel;
            audioSource.rolloffMode = rollOffMode;
            audioSource.minDistance = distanceRange.x;
            audioSource.maxDistance = distanceRange.y;

            return audioSource;
        }
        #endregion
    }
}
