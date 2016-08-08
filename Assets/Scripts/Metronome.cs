using UnityEngine;
using System.Collections;

public class Metronome : MonoBehaviour {

	public static Metronome metro;
	public GameObject[] instruments;

	public float BPM = 120.0f;
	[HideInInspector]
	public int DSPBufferSize, DSPNumBuffers, sampleRate, currentSixteenth, currentBar, samplesPerSixteenth;
	[HideInInspector]
	public bool[] metroOutArray;

	private int samplesPerQuarter, currentSample, numberOfHits;
	private SequencerInstrument[] instrumentScripts;

	// Use this for initialization
	void Awake () 
	{
		////////////////make sure there is only one copy of metro////////////////////
		if (metro != null)
			GameObject.Destroy (metro);
		else
			metro = this;
		DontDestroyOnLoad (this);

		////////set up audio DSP buffer size and sample rate for whole program///////
		AudioSettings.GetDSPBufferSize (out DSPBufferSize, out DSPNumBuffers);
		metroOutArray = new bool[DSPBufferSize];
		sampleRate = AudioSettings.outputSampleRate;

		/////////////////////////set up basic subdivision////////////////////////////
		samplesPerQuarter = (int)(sampleRate / (BPM / 60.0f));
		samplesPerSixteenth = (int) ((float)samplesPerQuarter / 4.0f);

		/////////////set up instrument script array and link it to scripts//////////
		instrumentScripts = new SequencerInstrument[instruments.Length];
		for (int i = 0; i < instrumentScripts.Length; i++)
			instrumentScripts [i] = instruments [i].GetComponent<SequencerInstrument> ();
	}

	void OnAudioFilterRead(float[] samples, int channels)
	{
		for (int i = 0; i < DSPBufferSize; i++) 
		{
			//keeps track of sample index and rolls over at samples per tick
			currentSample++;
			metroUpdate ();
			if (currentSample == samplesPerSixteenth)
			{
				currentSample = 0;
				currentSixteenth++;

				if (currentSixteenth == 17)
					currentSixteenth = 1;

			}
//				if (metroOutArray [i] == true)
//					samples [i] = 1.0f;
//				else
//					samples [i] = 0.0f;
		}
	}

	public void metroUpdate()
	{
		for (int i = 0; i < instruments.Length; i++) 
		{
			instrumentScripts [i].metroUpdate ();
		}
	}
}
