using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class LineDrawer : MonoBehaviour
    {
        public LineRenderer LineRenderer;
        public float drawDuration = 1f;
        public float delayBetweenPoints = 0.3f;

        public IEnumerator DrawMatchLine(List<Slot> connectors)
        {
            float elapsedTime = 0;
            Vector3 initialPos = connectors[0].transform.position;
            LineRenderer.positionCount = 2;
            for (int i = 0; i < 2; i++)
            {
                LineRenderer.SetPosition(i, initialPos);
            }
            Vector3 lastPos = initialPos;
            for (int i = 1; i < connectors.Count; i++)
            {
                while (elapsedTime < drawDuration)
                {
                    float t = Mathf.Clamp01(elapsedTime / drawDuration);
                    Vector3 currentPosition = Vector3.Lerp(lastPos, connectors[i].transform.position, t);
                    LineRenderer.SetPosition(i, currentPosition);
                    elapsedTime += Time.deltaTime;

                    
                    yield return null;
                }
                elapsedTime = 0;
                lastPos = connectors[i].transform.position;
                LineRenderer.positionCount++;
                LineRenderer.SetPosition(i + 1 , lastPos);
                yield return new WaitForSeconds(delayBetweenPoints);
            }
       
        }
    }
}