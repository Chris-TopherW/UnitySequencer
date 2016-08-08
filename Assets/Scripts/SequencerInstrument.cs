using UnityEngine;
using System.Collections;

/// <summary>
/// Base instrument voice class for audio sequencer.
/// </summary>

public class SequencerInstrument : MonoBehaviour {

	public AudioClip audioClip;

	[HideInInspector]
	public float[] outputBuffer;

	private int numChannels, sampleOffset, numSamples;
	private float[] inputBuffer;
	private bool processAudio;

	void Awake ()
	{
		numSamples = audioClip.samples;
		numChannels = audioClip.channels;

		//setup input buffer to store clip of floats
		inputBuffer = new float[numSamples];
		audioClip.GetData (inputBuffer, 0);
	}

	void Start()
	{
		//setup output buffer to carry floats to next part of program
		sampleOffset += Metronome.metro.DSPBufferSize;
		outputBuffer = new float[Metronome.metro.DSPBufferSize];
	}

	void OnAudioFilterRead(float[] samples, int channels)
	{
			for (int i = 0; i < Metronome.metro.DSPBufferSize; i++) {
				samples [i] = inputBuffer [i];
		}
	}

	//update every sample...
	public void metroUpdate()
	{
//		processAudio = true;
//		for(int i = 0; i < Metronome.metro.samplesPerSixteenth; i++)
//		{
//			OnAudioFilterRead(inputBuffer, 2);
//		}
//		processAudio = false;
	}
}
