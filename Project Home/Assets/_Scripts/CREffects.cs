using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CREffects : MonoBehaviour {

    /// <summary>
    /// Sets the active state of a game object after a delay of a specified length.
    /// </summary>
    /// <param name="obj">The object to affect.</param>
    /// <param name="active"></param>
    /// <param name="delay">How long before the active state is set.</param>
    /// <param name="endEvent">The event called when the coroutine exits.</param>
    /// <returns></returns>
    public static IEnumerator SetActiveAfterDelay(GameObject obj, bool active, float delay, Action endEvent = null)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(active);

        if(endEvent != null) { endEvent(); }
    }

    /// <summary>
    /// Sets the enabled state of a behaviour after a delay of a specified length.
    /// </summary>
    /// <param name="behaviour">The behaviour to affect.</param>
    /// <param name="enabled"></param>
    /// <param name="delay">How long before the enabled state is set.</param>
    /// <param name="endEvent">The event called when the coroutine exits.</param>
    /// <returns></returns>
    public static IEnumerator SetActiveAfterDelay(Behaviour behaviour, bool enabled, float delay, Action endEvent = null)
    {
        yield return new WaitForSeconds(delay);
        behaviour.enabled = enabled;

        if (endEvent != null) { endEvent(); }
    }

    /// <summary>
    /// Sets the emission state of a particle system after a delay of a specified length.
    /// </summary>
    /// <param name="particleSystem">The particle system to affect.</param>
    /// <param name="emit"></param>
    /// <param name="delay">How long before the emission state is set.</param>
    /// <param name="endEvent">The event called when the coroutine exits.</param>
    /// <returns></returns>
    public static IEnumerator SetActiveAfterDelay(ParticleSystem particleSystem, bool emit, float delay, Action endEvent = null)
    {
        yield return new WaitForSeconds(delay);

        ParticleSystem.EmissionModule em = particleSystem.emission;
        em.enabled = emit;

        if (endEvent != null) { endEvent(); }
    }

    /// <summary>
    /// Lerps a target transform from a specified scale to another specified scale.
    /// </summary>
    /// <param name="trans">The target transform.</param>
    /// <param name="initialScale">The scale the transform should start at.</param>
    /// <param name="targetScale">The scale the transform should end at.</param>
    /// <param name="zoomTime">How long should the zoom take?</param>
    /// <param name="endEvent">The event called when the coroutine exits.</param>
    /// <returns></returns>
    public static IEnumerator ZoomToScale(Transform trans, Vector3 initialScale, Vector3 targetScale, float zoomTime, Action endEvent = null)
    {
        trans.localScale = initialScale;
        float elapsedTime = 0;
        while (elapsedTime < zoomTime)
        {
            elapsedTime += Time.deltaTime;
            trans.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / zoomTime);
            yield return null;
        }
        trans.localScale = targetScale;

        if (endEvent != null) { endEvent(); }
    }

    /// <summary>
    /// Smoothly zooms a transform to a target scale.
    /// </summary>
    /// <param name="trans">The transform to target.</param>
    /// <param name="initialScale">The scale the transform should start at.</param>
    /// <param name="targetScale">The scale the transform should end at.</param>
    /// <param name="smoothTime">The amount of time the smoothing should take.</param>
    /// <param name="maxSpeed">The max speed of the smoothed lerp.</param>
    /// <param name="scaleError">The margin of error before exiting the damping loop (because smooth damp rarely returns exact numbers).</param>
    /// <param name="endEvent">The event called when the coroutine exits.</param>
    /// <returns></returns>
    public static IEnumerator SmoothZoomToScale(Transform trans, Vector3 initialScale, Vector3 targetScale, float smoothTime, float maxSpeed, float scaleError, Action endEvent = null)
    {
        Vector3 vel = new Vector3();
        trans.localScale = initialScale;
        scaleError = Mathf.Abs(scaleError);

        while(Vector3.Distance(trans.localScale, targetScale) > scaleError)
        {
            trans.localScale = Vector3.SmoothDamp(trans.localScale, targetScale, ref vel, smoothTime, maxSpeed);
            yield return null;
        }

        trans.localScale = targetScale;

        if (endEvent != null) { endEvent(); }
    }

    /// <summary>
    /// Fades the target canvas group to a specified alpha.
    /// </summary>
    /// <param name="cg">The target canvas group.</param>
    /// <param name="initialAlpha">The starting alpha of the canvas group.</param>
    /// <param name="targetAlpha">The alpha the canvas group is fading to.</param>
    /// <param name="fadeTime">How much time the fade should take.</param>
    /// <param name="endEvent">The event called when the coroutine exits.</param>
    /// <returns></returns>
    public static IEnumerator FadeCanvasAlpha(CanvasGroup cg, float initialAlpha, float targetAlpha, float fadeTime, Action endEvent = null)
    {
        cg.alpha = initialAlpha;
        float elapsedTime = 0;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            cg.alpha = Mathf.Lerp(initialAlpha, targetAlpha, elapsedTime / fadeTime);
            yield return null;
        }
        cg.alpha = targetAlpha;

        if(endEvent != null) { endEvent(); }
    }

    /// <summary>
    /// Lerps a specified transform to target position, scale and rotation.
    /// </summary>
    /// <param name="trans">The transform to manipulate.</param>
    /// <param name="targetPosition">The position the transform should lerp to.</param>
    /// <param name="targetScale">The scale the transform should lerp to.</param>
    /// <param name="targetRotation">The rotation the transform should lerp to.</param>
    /// <param name="totalTime">The time the lerp should take.</param>
    /// <param name="endEvent">The event called when the coroutine exits.</param>
    /// <returns></returns>
    public static IEnumerator LerpTransformTo(Transform trans, Vector3 targetPosition, Vector3 targetScale, Quaternion targetRotation, float totalTime, Action endEvent = null)
    {
        Vector3 initialPosition = trans.position;
        Vector3 initialScale = trans.localScale;
        Quaternion initialRotation = trans.rotation;

        float t = 0;

        while (t < totalTime)
        {
            t += Time.deltaTime;
            yield return null;
        }

        if (endEvent != null) { endEvent(); }
    }


    public static IEnumerator ZoomToPosition(Transform trans, Vector3 initialPosition, Vector3 targetPosition, float zoomTime, Action endEvent = null)
    {
        trans.position = initialPosition;
        float elapsedTime = 0;
        while (elapsedTime < zoomTime)
        {
            elapsedTime += Time.deltaTime;
            trans.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / zoomTime);
            yield return null;
        }
        trans.position = targetPosition;

        if (endEvent != null) { endEvent(); }
    }

}
