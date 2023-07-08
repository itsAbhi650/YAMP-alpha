using CSCore;
using CSCore.Streams.Effects;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace YAMP_alpha
{
    public static class EffectManager
    {
        private static DmoChorusEffect chorusEffect;
        private static DmoCompressorEffect compressorEffect;
        private static DmoDistortionEffect distortionEffect;
        private static DmoEchoEffect echoEffect;
        private static DmoFlangerEffect flangerEffect;
        private static DmoGargleEffect gargleEffect;
        private static DmoWavesReverbEffect wavesReverbEffect;
        private static PitchShifter pitchShifter;
        private static event EventHandler PropertyChanged;
        private static void OnPropertyChanged()
        {
            PropertyChanged?.Invoke(null, EventArgs.Empty);
        }

        public static Dictionary<string, object> EffectDictionary = new Dictionary<string, object>();

        private static bool IsPropertyChanged
        {
            set
            {
                if (value)
                {
                    OnPropertyChanged();
                }
            }
        }

        public static bool StoreAll { get => GetAll(); }

        public static List<object> AllEffects { get; private set; } = new List<object> { chorusEffect, compressorEffect, distortionEffect, echoEffect, flangerEffect, gargleEffect, wavesReverbEffect, pitchShifter };

        private static bool GetAll()
        {
            try
            {
                chorusEffect = YAMPVars.ChorusEffect;
                EffectDictionary.Add("Chorus", chorusEffect);
                compressorEffect = YAMPVars.CompressorEffect;
                EffectDictionary.Add("Compressor", compressorEffect);
                distortionEffect = YAMPVars.DistortionEffect;
                EffectDictionary.Add("Distortion", distortionEffect);
                echoEffect = YAMPVars.EchoEffect;
                EffectDictionary.Add("Echo", echoEffect);
                flangerEffect = YAMPVars.FlangerEffect;
                EffectDictionary.Add("Flanger", flangerEffect);
                wavesReverbEffect = YAMPVars.WavesReverbEffect;
                EffectDictionary.Add("WavesReverb", wavesReverbEffect);
                pitchShifter = YAMPVars.PitchShiftEffect;
                EffectDictionary.Add("PitchShift", pitchShifter);
                gargleEffect = YAMPVars.GargleEffect;
                EffectDictionary.Add("gargle", gargleEffect);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        public static bool ResetAll()
        {
            GetAll();
            object _effectObj = null;
            try
            {
                for (int i = 0; i < AllEffects.Count; i++)
                {
                    if (AllEffects[i] != null)
                    {
                        Type EffectType = AllEffects[i].GetType();
                        if (EffectType == typeof(DmoChorusEffect))
                        {
                            _effectObj = new DmoChorusEffect(((DmoChorusEffect)AllEffects[i]).BaseSource);
                        }
                        else if (EffectType == typeof(DmoCompressorEffect))
                        {
                            _effectObj = new DmoCompressorEffect(((DmoCompressorEffect)AllEffects[i]).BaseSource);
                        }
                        else if (EffectType == typeof(DmoDistortionEffect))
                        {
                            _effectObj = new DmoDistortionEffect(((DmoDistortionEffect)AllEffects[i]).BaseSource);
                        }
                        else if (EffectType == typeof(DmoEchoEffect))
                        {
                            _effectObj = new DmoEchoEffect(((DmoEchoEffect)AllEffects[i]).BaseSource);
                        }
                        else if (EffectType == typeof(DmoFlangerEffect))
                        {
                            _effectObj = new DmoFlangerEffect(((DmoFlangerEffect)AllEffects[i]).BaseSource);
                        }
                        else if (EffectType == typeof(DmoGargleEffect))
                        {
                            _effectObj = new DmoGargleEffect(((DmoGargleEffect)AllEffects[i]).BaseSource);
                        }
                        else if (EffectType == typeof(DmoWavesReverbEffect))
                        {
                            _effectObj = new DmoWavesReverbEffect(((DmoWavesReverbEffect)AllEffects[i]).BaseSource);
                        }
                        else if (EffectType == typeof(PitchShifter))
                        {
                            _effectObj = new PitchShifter(((PitchShifter)AllEffects[i]).BaseSource);
                        }
                        AllEffects[i] = _effectObj;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
