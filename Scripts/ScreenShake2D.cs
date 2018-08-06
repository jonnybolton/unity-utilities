// The MIT License

// Copyright(c) 2018 Jonny Bolton

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System.Collections;
using UnityEngine;

public class ScreenShake2D : MonoBehaviour
{
	public void Increase(float value)
	{
		strength += value;
		if (strength > 1.0f)
		{
			strength = 1.0f;
		}

		if (shakeRoutine == null)
		{
			ResetSeed();
			shakeRoutine = StartCoroutine(DoScreenShake());
		}
	}

	public void Stop()
	{
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;

		if (shakeRoutine != null)
		{
			StopCoroutine(shakeRoutine);
		}
		shakeRoutine = null;
	}

	private void Awake()
	{
		ResetSeed();
	}

	private void ResetSeed()
	{
		float seed = Random.Range(0.0f, 100.0f);
		xPerlinYLocation = seed;
		yPerlinYLocation = seed;
		rotPerlinYLocation = seed;
	}

	private IEnumerator DoScreenShake()
	{
		do
		{
			float easedStrength = easing.Evaluate(strength);
			transform.localPosition = new Vector3(
				easedStrength * (Mathf.PerlinNoise(xPerlinRow, xPerlinYLocation) - 0.5f) * xAxisMaxOffset * 2.0f,
				easedStrength * (Mathf.PerlinNoise(yPerlinRow, yPerlinYLocation) - 0.5f) * yAxisMaxOffset * 2.0f
			);
			transform.localRotation = Quaternion.Euler(0.0f, 0.0f,
			    easedStrength * (Mathf.PerlinNoise(rotPerlinRow, rotPerlinYLocation) - 0.5f) * rotationMaxOffset * 2.0f
			);

			yield return null;

			strength -= Time.deltaTime / duration;

			xPerlinYLocation += Time.deltaTime * xAxisStrength;
			yPerlinYLocation += Time.deltaTime * yAxisStrength;
			rotPerlinYLocation += Time.deltaTime * rotationStrength;
		}
		while (strength > 0.0f);

		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;

		shakeRoutine = null;
	}

	private float strength = 0.0f;
	private Coroutine shakeRoutine = null;

	private float xPerlinYLocation = 0.0f;
	private float yPerlinYLocation = 0.0f;
	private float rotPerlinYLocation = 0.0f;

	private static float xPerlinRow = 0.0f;
	private static float yPerlinRow = 10.0f;
	private static float rotPerlinRow = 20.0f;

	[SerializeField]
	private float duration;

	[SerializeField]
	private AnimationCurve easing;

	[SerializeField]
	private float xAxisMaxOffset;

	[SerializeField]
	private float xAxisStrength;

	[SerializeField]
	private float yAxisMaxOffset;

	[SerializeField]
	private float yAxisStrength;

	[SerializeField]
	private float rotationMaxOffset;

	[SerializeField]
	private float rotationStrength;
}
