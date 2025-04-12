#nullable enable
using UnityEngine;
using System; // Aggiunto per utilizzare Math

namespace ArtOfRallyCoPilot
{
    public enum CoPilotMode
    {
        ANGLE,
        DISTANCE
    }

    public struct CoPilotGeneratorConfig
    {
        public string Name;
        public int DistanceSumTolerance;
        public CoPilotMode Mode;

        public float MinimumValue;
        public float MaximumValue;

        public float WarningDistance;
    }

    public class CoPilotGenerator
    {
        public static string?[][] GenerateCoPilot(
            Vector3[] positions,
            float[] distances,
            float[] angles,
            float[] elevations,
            CoPilotGeneratorConfig[] settings
        )
        {
            var result = new string?[settings.Length][];

            for (var i = 0; i < settings.Length; i++)
            {
                var setting = settings[i];
                var coPilot = new string?[positions.Length];
                var summedList = GetMeanValues(
                    setting.Mode == CoPilotMode.ANGLE ? angles : elevations,
                    distances,
                    setting.DistanceSumTolerance
                );

                for (var j = 0; j < positions.Length; j++)
                {
                    var value = summedList[j];

                    if (value >= setting.MaximumValue || value < setting.MinimumValue) continue;

                    var index = j;
                    float distanceSum = 0f;
                    while (index >= 0 && distanceSum <= setting.WarningDistance)
                    {
                        distanceSum += distances[index];
                        index--;
                    }
                    coPilot[j] = setting.Name; // Assegna la nota all'indice corrente
                }

                result[i] = coPilot;
            }

            return result;
        }

        public static float[] GetMeanValues(float[] values, float[] distances, float tolerance)
        {
            var result = new float[values.Length];

            for (var j = 0; j < values.Length; j++)
            {
                result[j] = values[j];
                var distance = distances[j];
                for (var z = j; distance < tolerance && z < values.Length; z++)
                {
                    distance += distances[z];
                    result[j] += values[z];
                }
                result[j] /= (float)Math.Max(1, Math.Floor(distance / tolerance)); // Media semplice
            }

            return result;
        }

        public static float[] GetDistances(Vector3[] positions)
        {
            var distances = new float[positions.Length];

            for (var i = 0; i < distances.Length - 1; i++)
            {
                distances[i] = Vector3.Distance(positions[i], positions[i + 1]);
            }

            distances[distances.Length - 1] = 0;

            return distances;
        }

        public static float[] GetAngles(Vector3[] positions)
        {
            var angles = new float[positions.Length];

            for (var i = 1; i < angles.Length - 1; i++)
            {
                var a = positions[i] - positions[i - 1];
                var b = positions[i + 1] - positions[i];
                angles[i] = -Vector2.SignedAngle(new Vector2 { x = a.x, y = a.z }, new Vector2 { x = b.x, y = b.z });
            }

            angles[0] = 0;
            angles[angles.Length - 1] = 0;

            return angles;
        }

        public static float[] GetElevations(Vector3[] positions)
        {
            var elevations = new float[positions.Length];

            for (var i = 1; i < elevations.Length; i++)
            {
                elevations[i] = positions[i].y - positions[i - 1].y;
            }

            elevations[0] = 0;

            return elevations;
        }
    }
}