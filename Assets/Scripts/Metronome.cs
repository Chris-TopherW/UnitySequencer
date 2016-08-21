using UnityEngine;
using System.Collections;

/// <summary>
/// Metronome class as sample accurate clock to run instruments.
/// </summary>

public class Metronome : MonoBehaviour
{
	//instantiate static object to access from other scripts
	public static Metronome metro;

	public float BPM = 120.0f;
	[HideInInspector]
	public int currentTick, currentBar, samplesPerTick, ticksPerBar;
	[HideInInspector]
	public bool ready;

	private int samplesPerQuarter, phasor, numberOfHits,
	DSPBufferSize, DSPNumBuffers, sampleRate, ticksPerQuarter, quartersPerBar;

	void Awake () 
	{
		//wait for instrument set up
		ready = false;
			metro = this;

		//set up audio DSP buffer size and sample rate for whole program
		AudioSettings.GetDSPBufferSize (out DSPBufferSize, out DSPNumBuffers);
		sampleRate = AudioSettings.outputSampleRate;

		ticksPerQuarter = 8;
		quartersPerBar = 4;
		ticksPerBar = ticksPerQuarter * quartersPerBar;
		//set up basic subdivision
		samplesPerQuarter = (int)(sampleRate / (BPM / 60.0f));
		samplesPerTick = (int)(samplesPerQuarter / ticksPerQuarter);
		currentTick = 0;
		currentBar = 1;
	}

	void OnAudioFilterRead(float[] samples, int channels)
	{
		for (int i = 0; i < DSPBufferSize; i++) 
		{
			phasor++;
			if (phasor == samplesPerTick) {
				phasor = 0;
				if (currentTick == quartersPerBar * ticksPerQuarter - 1) {
					currentTick = 0;
					currentBar++;
				}
				else
					currentTick++;
			}
		}
	}

	public int getDSPBufferSize()
	{
		return DSPBufferSize;
	}

	public int getSampleRate()
	{
		return sampleRate;
	}
}
