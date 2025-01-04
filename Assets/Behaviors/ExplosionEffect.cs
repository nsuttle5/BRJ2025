using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    private ParticleSystem explosionPS;
    private ParticleSystem.MainModule mainModule;

    void Awake()
    {
        // Create the main explosion particle system
        explosionPS = gameObject.AddComponent<ParticleSystem>();
        mainModule = explosionPS.main;

        // Main module settings
        mainModule.duration = 0.5f;
        mainModule.loop = false;
        mainModule.startSpeed = new ParticleSystem.MinMaxCurve(3f, 6f);
        mainModule.startSize = new ParticleSystem.MinMaxCurve(0.5f, 1.5f);
        mainModule.startLifetime = new ParticleSystem.MinMaxCurve(0.5f, 1f);
        mainModule.maxParticles = 100;
        mainModule.simulationSpace = ParticleSystemSimulationSpace.World;

        // Emission module
        var emission = explosionPS.emission;
        emission.enabled = true;
        emission.rateOverTime = 0;
        emission.SetBursts(new[] {
            new ParticleSystem.Burst(0f, 50)
        });

        // Shape module
        var shape = explosionPS.shape;
        shape.enabled = true;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 0.1f;
        shape.radiusThickness = 1f;

        // Color over lifetime
        var colorOverLifetime = explosionPS.colorOverLifetime;
        colorOverLifetime.enabled = true;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] {
                new GradientColorKey(Color.yellow, 0.0f),
                new GradientColorKey(new Color(1f, 0.4f, 0f), 0.2f), // Orange
                new GradientColorKey(Color.red, 0.5f),
                new GradientColorKey(Color.grey, 1.0f)
            },
            new GradientAlphaKey[] {
                new GradientAlphaKey(1f, 0f),
                new GradientAlphaKey(1f, 0.5f),
                new GradientAlphaKey(0f, 1f)
            }
        );
        colorOverLifetime.color = gradient;

        // Size over lifetime
        var sizeOverLifetime = explosionPS.sizeOverLifetime;
        sizeOverLifetime.enabled = true;
        AnimationCurve curve = new AnimationCurve();
        curve.AddKey(0f, 0.5f);
        curve.AddKey(0.3f, 1f);
        curve.AddKey(1f, 0f);
        sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1f, curve);

        // Velocity over lifetime (for outward explosion force)
        var velocityOverLifetime = explosionPS.velocityOverLifetime;
        velocityOverLifetime.enabled = true;
        velocityOverLifetime.radial = new ParticleSystem.MinMaxCurve(
            -2f,
            new AnimationCurve(new Keyframe[] {
                new Keyframe(0f, 1f),
                new Keyframe(1f, 0.1f)
            })
        );

        // Create child system for sparks
        CreateSparkSystem();
    }

    private void CreateSparkSystem()
    {
        GameObject sparkObj = new GameObject("Sparks");
        sparkObj.transform.parent = transform;
        sparkObj.transform.localPosition = Vector3.zero;

        ParticleSystem sparkPS = sparkObj.AddComponent<ParticleSystem>();
        var mainModule = sparkPS.main;

        // Main module settings for sparks
        mainModule.duration = 1f;
        mainModule.loop = false;
        mainModule.startSpeed = new ParticleSystem.MinMaxCurve(8f, 12f);
        mainModule.startSize = new ParticleSystem.MinMaxCurve(0.05f, 0.15f);
        mainModule.startLifetime = new ParticleSystem.MinMaxCurve(0.3f, 0.6f);
        mainModule.maxParticles = 30;

        // Emission for sparks
        var emission = sparkPS.emission;
        emission.SetBursts(new[] {
            new ParticleSystem.Burst(0f, 30)
        });

        // Trails for sparks
        var trails = sparkPS.trails;
        trails.enabled = true;
        trails.ratio = 1;
        trails.lifetime = new ParticleSystem.MinMaxCurve(0.2f);
        trails.minVertexDistance = 0.1f;

        // Color over lifetime for sparks
        var colorOverLifetime = sparkPS.colorOverLifetime;
        colorOverLifetime.enabled = true;
        Gradient sparkGradient = new Gradient();
        sparkGradient.SetKeys(
            new GradientColorKey[] {
                new GradientColorKey(Color.white, 0.0f),
                new GradientColorKey(Color.yellow, 0.5f),
                new GradientColorKey(Color.red, 1.0f)
            },
            new GradientAlphaKey[] {
                new GradientAlphaKey(1f, 0f),
                new GradientAlphaKey(0f, 1f)
            }
        );
        colorOverLifetime.color = sparkGradient;
    }

    public void Play()
    {
        // Stop any existing playback first
        explosionPS.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        // Start the new explosion
        explosionPS.Play(true);

        // Also play any child particle systems
        var childSystems = GetComponentsInChildren<ParticleSystem>();
        foreach (var ps in childSystems)
        {
            if (ps != explosionPS)
            {
                ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                ps.Play(true);
            }
        }

        // Destroy the game object after all particles are done
        float longestDuration = 0f;
        foreach (var ps in childSystems)
        {
            float systemDuration = ps.main.duration + ps.main.startLifetime.constantMax;
            if (systemDuration > longestDuration)
            {
                longestDuration = systemDuration;
            }
        }
        Destroy(gameObject, longestDuration);
    }
}
