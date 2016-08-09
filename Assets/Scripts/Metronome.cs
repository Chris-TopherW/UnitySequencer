using UnityEngine;
using System.Collections;

/// <summary>
/// Metronome class as sample accurate clock to run instruments.
/// </summary>

public class Metronome : MonoBehaviour {

	//instantiate static object to access from other scripts
	public static Metronome metro;

	public float BPM = 120.0f;
	[HideInInspector]
	public int currentSixteenth, currentBar, samplesPerSixteenth;
	[HideInInspector]

	private int samplesPerQuarter, currentSample, numberOfHits, metroTickSample, DSPBufferSize, DSPNumBuffers, sampleRate;

	// Use this for initialization
	void Awake () 
	{
		////////////////make sure there is only one copy of metro- singleton//////////////////
		if (metro != null)
			GameObject.Destroy (metro);
		else
			metro = this;
		DontDestroyOnLoad (this);

		////////set up audio DSP buffer size and sample rate for whole program////////////////
		AudioSettings.GetDSPBufferSize (out DSPBufferSize, out DSPNumBuffers);
		sampleRate = AudioSettings.outputSampleRate;

		/////////////////////////set up basic subdivision/////////////////////////////////////
		samplesPerQuarter = (int)(sampleRate / (BPM / 60.0f));
		samplesPerSixteenth = (int)((float)samplesPerQuarter / 4.0f);
		currentBar = 0;
	}

	void OnAudioFilterRead(float[] samples, int channels)
	{
		//this resets the tickSample which will be read by instruments to determine when tick occurs
		//I use -1 so that samples at i==0 can be recognised.
		metroTickSample = -1;

		for (int i = 0; i < DSPBufferSize; i++) 
		{
			currentSample++;

			if (currentSample == samplesPerSixteenth) 
			{
				currentSample = 0;
				currentSixteenth++;
				//this holds position of sample at which semiquaver took place
				metroTickSample = i;

				if (currentSixteenth == 16) 
				{
					currentSixteenth = 0;
					currentBar++;
				}
			}
		}
	}
	//public read functions
	public int getMetroTickSample()
	{
		return metroTickSample;
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
