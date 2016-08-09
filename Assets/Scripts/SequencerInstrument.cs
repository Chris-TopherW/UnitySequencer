using UnityEngine;
using System.Collections;

/// <summary>
/// Instrument voice class for audio sequencer.
/// </summary>

public class SequencerInstrument : MonoBehaviour {

	public AudioClip audioClip;
	public GameObject scoreObject;

	private Score scoreScript;
	private AudioSource audioSource;

	private int numSamples, DSPBufferingSize, playhead;
	private float gain = 1.0f;
	private string instrumentName;
	private float[] sampleBuffer;
	private bool processAudio;

	void Awake ()
	{
		audioSource = GetComponent<AudioSource> ();
		numSamples = audioClip.samples;
		DSPBufferingSize = Metronome.metro.getDSPBufferSize ();

		//Score setup
		scoreScript = scoreObject.GetComponent<Score>();
		instrumentName = this.gameObject.name;
		//Debug.Log (instrumentName);

		//setup input buffer to store clip of floats
		sampleBuffer = new float[numSamples];
		audioClip.GetData (sampleBuffer, 0);
	}

	void OnAudioFilterRead(float[] samples, int channels)
	{

		int metroTickPos = Metronome.metro.getMetroTickSample ();

		for (int i = 0; i < DSPBufferingSize; i++) {
			if (i == metroTickPos) {
				playhead = 0;
			}
			if (scoreScript.snareScore [Metronome.metro.currentSixteenth] == 1)
				gain = 1.0f;
			else
				gain = 0.0f;

			if (playhead < numSamples - 1)
				playhead++;
			samples [i] = sampleBuffer[playhead] * gain;
		}
	}
}
