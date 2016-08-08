using UnityEngine;
using System.Collections;

public class Mixer : MonoBehaviour {

	public GameObject instrument;

	private SequencerInstrument instrumentScript;

	void Awake()
	{
		instrumentScript = instrument.GetComponent<SequencerInstrument> ();
	}

	void OnAudioFilterRead(float[] samples, int channels)
	{
		for (int i = 0; i < Metronome.metro.DSPBufferSize; i++) 
		{
			samples[i] = instrumentScript.outputBuffer [i];
		}
	}
}
