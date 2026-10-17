using Spotlight.Contracts;
using System.Collections.Generic;
using UnityEditor.Sprites;
using UnityEngine;

namespace Spotlight.Presentation
{
    public class ProjectileVisual2D : MonoBehaviour
    {
        public Transform VisualRoot;
        public SpriteRenderer ElementOverlay;
        [SerializeField] private ParticleSystem sharedPS; //引用粒子特效
        public AnimationCurve ZCurve_Earth; //曲线
        public AnimationCurve Curve_EarthSize;
        public AnimationCurve Curve_WoodSize;
        public AnimationCurve YCurve_Wind;
        // ===== 渐变缓存（初始化一次，后面直接用）=====
        private Gradient gradFire;
        private Gradient gradFire_over;

        private Gradient gradWater;
        private Gradient gradWater_over;

        private Gradient gradEarth;
        private Gradient gradEarth_over;

        private Gradient gradWood;
        private Gradient gradWood_over;

        private Gradient gradWind;
        private Gradient gradWind_over;

        private Gradient gradThunder;
        private Gradient gradThunder_over;

        [SerializeField] private Material trailMat;
        private int lastAppliedMask = -1; //脏位
        private void Awake()
        {
            // 火 - 黄→红→黑
            gradFire = new Gradient();
            gradFire.SetKeys(
                new GradientColorKey[] {
                new GradientColorKey(new Color(1f, 1f, 1f), 0f),
                new GradientColorKey(new Color(1f, 1f, 1f), 1f)
                },
                new GradientAlphaKey[] {
                new GradientAlphaKey(1f, 0f),
                new GradientAlphaKey(1f, 1f)
                }
            );

            gradFire_over = new Gradient();
            gradFire_over.SetKeys(
                new GradientColorKey[] {
                new GradientColorKey(new Color(1f, 1f, 1f), 0f),
                new GradientColorKey(new Color(1f, 0.67f, 0f), 0.27f),
                new GradientColorKey(new Color(1f, 0.15f, 0f), 0.6f),
                new GradientColorKey(new Color(0f, 0f, 0f), 1f)
                },
                new GradientAlphaKey[] {
                new GradientAlphaKey(0f, 0f),
                new GradientAlphaKey(1f, 0.33f),
                new GradientAlphaKey(0f, 1f)
                }
            );

            // 水 - 蓝白
            gradWater = new Gradient();
            gradWater.SetKeys(
                new GradientColorKey[] {
                new GradientColorKey(new Color(0f, 1f, 1f), 0f),
                new GradientColorKey(new Color(0f, 0.22f, 1f), 1f)
                },
                new GradientAlphaKey[] {
                new GradientAlphaKey(0.4f, 0f),
                new GradientAlphaKey(0.4f, 1f)
                }
            );

            gradWater_over = new Gradient();
            gradWater_over.SetKeys(
                new GradientColorKey[] {
                new GradientColorKey(new Color(1f, 1f, 1f), 0f),
                new GradientColorKey(new Color(1f, 1f, 1f), 1f)
                },
                new GradientAlphaKey[] {
                new GradientAlphaKey(0f, 0f),
                new GradientAlphaKey(1f, 0.4f),
                new GradientAlphaKey(0f, 1f)
                }
            );

            // 土 - 土黄褐
            gradEarth = new Gradient();
            gradEarth.SetKeys(
                new GradientColorKey[] {
                new GradientColorKey(new Color(0.31f, 0.17f, 0f), 0f),
                new GradientColorKey(new Color(0.61f, 0.4f, 0f), 1f)
                },
                new GradientAlphaKey[] {
                new GradientAlphaKey(1f, 0f),
                new GradientAlphaKey(1f, 1f)
                }
            );

            // 木 - 绿
            gradWood = new Gradient();
            gradWood.SetKeys(
                new GradientColorKey[] {
                new GradientColorKey(new Color(0.57f, 0.97f, 0.45f), 0f),
                new GradientColorKey(new Color(0.36f, 0.63f, 0.3f), 1f)
                },
                new GradientAlphaKey[] {
                new GradientAlphaKey(1f, 0f),
                new GradientAlphaKey(1f, 1f)
                }
            );

            // 风 - 青绿深浅
            gradWind = new Gradient();
            gradWind.SetKeys(
                new GradientColorKey[] {
                new GradientColorKey(new Color(1f, 1f, 1f), 0f),
                new GradientColorKey(new Color(1f, 1f, 1f), 1f)
                },
                new GradientAlphaKey[] {
                new GradientAlphaKey(1f, 0f),
                new GradientAlphaKey(1f, 1f)
                }
            );

            gradWind_over = new Gradient();
            gradWind_over.SetKeys(
                new GradientColorKey[] {
                new GradientColorKey(new Color(0f, 0.8f, 0.48f), 0f),
                new GradientColorKey(new Color(0f, 0f, 0f), 1f)
                },
                new GradientAlphaKey[] {
                new GradientAlphaKey(1f, 0f),
                new GradientAlphaKey(1f, 0.56f),
                new GradientAlphaKey(0f, 1f)
                }
            );

            // 雷 - 紫白
            gradThunder = new Gradient();
            gradThunder.SetKeys(
                new GradientColorKey[] {
                new GradientColorKey(new Color(0.53f, 0f, 1f), 0f),
                new GradientColorKey(new Color(0f, 0f, 0f), 1f)
                },
                new GradientAlphaKey[] {
                new GradientAlphaKey(1f, 0f),
                new GradientAlphaKey(1f, 1f)
                }
            );

            gradThunder_over = new Gradient();
            gradThunder_over.SetKeys(
                new GradientColorKey[] {
                new GradientColorKey(new Color(1f, 1f, 1f), 0f),
                new GradientColorKey(new Color(1f, 1f, 1f), 1f)
                },
                new GradientAlphaKey[] {
                new GradientAlphaKey(1f, 0f),
                new GradientAlphaKey(1f, 0.67f),
                new GradientAlphaKey(0f, 1f)
                }
            );
        }

        private static readonly Color[] ElementColors =
        {
            new Color32(64, 156, 255, 255),  // 0 水
            new Color32(255, 83, 64, 255),   // 1 火
            new Color32(190, 146, 62, 255),  // 2 土
            new Color32(62, 190, 104, 255),  // 3 木
            new Color32(89, 224, 184, 255),  // 4 风
            new Color32(177, 100, 255, 255)  // 5 雷
        };

        public void SetDirection(WorldPoint direction)
        {
            if (VisualRoot == null) return;

            float lengthSq = direction.X * direction.X + direction.Y * direction.Y;
            if (lengthSq <= 0.000001f ||
                float.IsNaN(lengthSq) ||
                float.IsInfinity(lengthSq))
            {
                return;
            }

            float angle = Mathf.Atan2(direction.Y, direction.X) * Mathf.Rad2Deg;
            VisualRoot.localRotation = Quaternion.Euler(0f, 0f, angle);
        }

        public void SetElements(int appliedElementMask)
        {
            if (lastAppliedMask == appliedElementMask) return;  // ← 没变就跳过
            lastAppliedMask = appliedElementMask;

            if (ElementOverlay == null) return;

            int index = FirstKnownElement(appliedElementMask);
            ElementOverlay.enabled = index >= 0;
            if (index >= 0)
                ElementOverlay.color = ElementColors[index];

            // ===== 新增：粒子特效扩展点 =====
            UpdateParticleEffects(appliedElementMask);
        }

        public void ResetVisual()
        {
            lastAppliedMask = -1;

            if (VisualRoot != null)
                VisualRoot.localRotation = Quaternion.Euler(0f, 0f, 0f);

            if (ElementOverlay != null)
            {
                ElementOverlay.enabled = false;
                ElementOverlay.color = Color.white;
            }

            // ===== 新增：重置时关掉粒子 =====
            DisableAllParticleEffects();
        }

        /// <summary>
        /// 原始方法，返回首个元素（不变）
        /// </summary>
        private static int FirstKnownElement(int mask)
        {
            if ((mask & 1) != 0) return 0;
            if ((mask & 2) != 0) return 1;
            if ((mask & 4) != 0) return 2;
            if ((mask & 8) != 0) return 3;
            if ((mask & 16) != 0) return 4;
            if ((mask & 32) != 0) return 5;
            return -1;
        }

        // ===== 新增：以下三个方法供你自由发挥 =====

        /// <summary>
        /// 解析mask，返回所有存在的元素索引列表
        /// 0=水 1=火 2=土 3=木 4=风 5=雷
        /// </summary>
        public static int[] GetActiveElementIndices(int mask)
        {
            List<int> result = new List<int>();
            if ((mask & 1) != 0) result.Add(0);
            if ((mask & 2) != 0) result.Add(1);
            if ((mask & 4) != 0) result.Add(2);
            if ((mask & 8) != 0) result.Add(3);
            if ((mask & 16) != 0) result.Add(4);
            if ((mask & 32) != 0) result.Add(5);
            return result.ToArray();
        }

        /// <summary>
        /// 扩展点：根据当前mask更新粒子特效
        /// 在这里写你自己的逻辑，原始脚本不干涉
        /// </summary>
        protected virtual void UpdateParticleEffects(int appliedElementMask)
        {
            int[] activeElements = GetActiveElementIndices(appliedElementMask);

            if (activeElements.Length == 0)
            {
                DisableAllParticleEffects();
                return;
            }

            if (sharedPS != null)
            {
                sharedPS.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }

            if (activeElements.Length == 1)
            {
                switch (activeElements[0])
                {
                    case 0: SetWater(); break;
                    case 1: SetFire(); break;
                    case 2: SetEarth(); break;
                    case 3: SetWood(); break;
                    case 4: SetWind(); break;
                    case 5: SetThunder(); break;
                }
                if (sharedPS != null)
                {
                    sharedPS.Play(true);
                }

                return;
            }

            // 混合元素：在这里写你的三维决策逻辑
            // 例：
            // bool hasFire = System.Array.Exists(activeElements, x => x == 1);
            // bool hasWater = System.Array.Exists(activeElements, x => x == 0);
            // if (hasFire && hasWater) { /* 蒸汽 */ }
            // else { /* 按你的规则组合 */ }
        }

        /// <summary>
        /// 关闭所有粒子特效（你在里面填你自己的关闭逻辑）
        /// </summary>
        protected virtual void DisableAllParticleEffects()
        {
            // 在这里关掉你挂的粒子系统
            // 例：
            // if (fireEffect != null) fireEffect.SetActive(false);
            // if (mixedPS != null) mixedPS.Stop();
        }

        private void SetWater()
        {
            var main = sharedPS.main;
            main.startLifetime = new ParticleSystem.MinMaxCurve(0.3f, 0.8f);
            main.startSpeed = 1f;
            main.startSize = new ParticleSystem.MinMaxCurve(0.03f, 0.07f);
            main.startRotation = 0;
            ParticleSystem.MinMaxGradient mg = new ParticleSystem.MinMaxGradient(gradWater);
            mg.mode = ParticleSystemGradientMode.RandomColor;
            main.startColor = mg;

            var emi = sharedPS.emission;
            emi.enabled = true;
            emi.rateOverTime = 40f;
            emi.SetBursts(new ParticleSystem.Burst[] { });

            var sha = sharedPS.shape;
            sha.enabled = true;
            sha.shapeType = ParticleSystemShapeType.Cone;
            sha.angle = 15f;
            sha.radius = 0.0001f;

            var vel = sharedPS.velocityOverLifetime;
            vel.enabled = false;

            var col = sharedPS.colorOverLifetime;
            col.enabled = true;
            ParticleSystem.MinMaxGradient mg_over = new ParticleSystem.MinMaxGradient(gradWater_over);
            col.color = mg_over;

            var si = sharedPS.sizeOverLifetime;
            si.enabled = false;

            var tra = sharedPS.trails;
            tra.enabled = false;

            var ren = sharedPS.GetComponent<ParticleSystemRenderer>();
            ren.renderMode = ParticleSystemRenderMode.Stretch;
            ren.lengthScale = 3f;
            ren.pivot = new Vector3(0f, 0.5f, 0f);
        }

        private void SetFire()
        {
            var main = sharedPS.main;
            main.startLifetime = 0.5f;
            main.startSpeed = 1f;
            main.startSize = 0.1f;
            main.startRotation = new ParticleSystem.MinMaxCurve(0f, 360f * Mathf.Deg2Rad);
            ParticleSystem.MinMaxGradient mg = new ParticleSystem.MinMaxGradient(gradFire);
            mg.mode = ParticleSystemGradientMode.RandomColor;
            main.startColor = mg;

            var emi = sharedPS.emission;
            emi.enabled = true;
            emi.rateOverTime = 20f;
            emi.SetBursts(new ParticleSystem.Burst[] { });

            var sha = sharedPS.shape;
            sha.enabled = true;
            sha.shapeType = ParticleSystemShapeType.Cone;
            sha.angle = 15f;
            sha.radius = 0.01f;

            var vel = sharedPS.velocityOverLifetime;
            vel.enabled = false;

            var col = sharedPS.colorOverLifetime;
            col.enabled = true;
            ParticleSystem.MinMaxGradient mg_over = new ParticleSystem.MinMaxGradient(gradFire_over);
            col.color = mg_over;

            var si = sharedPS.sizeOverLifetime;
            si.enabled = false;

            var tra = sharedPS.trails;
            tra.enabled = false;

            var ren = sharedPS.GetComponent<ParticleSystemRenderer>();
            ren.renderMode = ParticleSystemRenderMode.Billboard;
            ren.pivot = new Vector3(0f, 0f, 0f);
        }

        private void SetEarth()
        {
            var main = sharedPS.main;
            main.startLifetime = 0.8f;
            main.startSpeed = new ParticleSystem.MinMaxCurve(0.6f, 1f);
            main.startSize = new ParticleSystem.MinMaxCurve(0.08f, 0.15f);
            main.startRotation = new ParticleSystem.MinMaxCurve(0f, 360f * Mathf.Deg2Rad);
            ParticleSystem.MinMaxGradient mg = new ParticleSystem.MinMaxGradient(gradEarth);
            mg.mode = ParticleSystemGradientMode.RandomColor;
            main.startColor = mg;

            var emi = sharedPS.emission;
            emi.enabled = true;
            emi.rateOverTime = 5f;
            emi.SetBursts(new ParticleSystem.Burst[] { });

            var sha = sharedPS.shape;
            sha.enabled = true;
            sha.shapeType = ParticleSystemShapeType.Cone;
            sha.angle = 15f;
            sha.radius = 0.05f;

            var vel = sharedPS.velocityOverLifetime;
            vel.enabled = true;
            vel.x = new ParticleSystem.MinMaxCurve(1f, AnimationCurve.Constant(0f, 1f, 0f));
            vel.y = new ParticleSystem.MinMaxCurve(1f, AnimationCurve.Constant(0f, 1f, 0f));
            vel.z = new ParticleSystem.MinMaxCurve(1f, ZCurve_Earth);

            var col = sharedPS.colorOverLifetime;
            col.enabled = false;

            var si = sharedPS.sizeOverLifetime;
            si.enabled = true;
            si.size = new ParticleSystem.MinMaxCurve(1f, Curve_EarthSize);

            var tra = sharedPS.trails;
            tra.enabled = false;

            var ren = sharedPS.GetComponent<ParticleSystemRenderer>();
            ren.renderMode = ParticleSystemRenderMode.Billboard;
            ren.pivot = new Vector3(0f, 0f, 0f);
        }

        private void SetWood()
        {
            var main = sharedPS.main;
            main.startLifetime = 1f;
            main.startSpeed = 0.5f;
            main.startSize = 0.1f;
            main.startRotation = new ParticleSystem.MinMaxCurve(0f, 360f * Mathf.Deg2Rad);
            ParticleSystem.MinMaxGradient mg = new ParticleSystem.MinMaxGradient(gradWood);
            mg.mode = ParticleSystemGradientMode.RandomColor;
            main.startColor = mg;

            var emi = sharedPS.emission;
            emi.enabled = true;
            emi.rateOverTime = 0f;
            emi.SetBursts(new ParticleSystem.Burst[] { new ParticleSystem.Burst(0f, 10, 10, 1, 0.01f) });

            var sha = sharedPS.shape;
            sha.enabled = true;
            sha.shapeType = ParticleSystemShapeType.Cone;
            sha.angle = 15f;
            sha.radius = 0.01f;

            var vel = sharedPS.velocityOverLifetime;
            vel.enabled = false;

            var col = sharedPS.colorOverLifetime;
            col.enabled = false;

            var si = sharedPS.sizeOverLifetime;
            si.enabled = true;
            si.size = new ParticleSystem.MinMaxCurve(1f, Curve_WoodSize);

            var tra = sharedPS.trails;
            tra.enabled = false;

            var ren = sharedPS.GetComponent<ParticleSystemRenderer>();
            ren.renderMode = ParticleSystemRenderMode.Billboard;
            ren.pivot = new Vector3(0f, 0f, 0f);
        }

        private void SetWind()
        {
            var main = sharedPS.main;
            main.startLifetime = 0.5f;
            main.startSpeed = 1f;
            main.startSize = 0.01f;
            main.startRotation = 0f;
            ParticleSystem.MinMaxGradient mg = new ParticleSystem.MinMaxGradient(gradWind);
            mg.mode = ParticleSystemGradientMode.RandomColor;
            main.startColor = mg;

            var emi = sharedPS.emission;
            emi.enabled = true;
            emi.rateOverTime = 20f;
            emi.SetBursts(new ParticleSystem.Burst[] { });

            var sha = sharedPS.shape;
            sha.enabled = true;
            sha.shapeType = ParticleSystemShapeType.Mesh;

            var vel = sharedPS.velocityOverLifetime;
            vel.enabled = true;
            vel.x = new ParticleSystem.MinMaxCurve(1f, AnimationCurve.Constant(0f, 1f, 0f));
            vel.y = new ParticleSystem.MinMaxCurve(1f, YCurve_Wind);
            vel.z = new ParticleSystem.MinMaxCurve(1f, AnimationCurve.Constant(0f, 1f, 0f));

            var col = sharedPS.colorOverLifetime;
            col.enabled = true;
            ParticleSystem.MinMaxGradient mg_over = new ParticleSystem.MinMaxGradient(gradWind_over);
            col.color = mg_over;

            var si = sharedPS.sizeOverLifetime;
            si.enabled = false;

            var tra = sharedPS.trails;
            tra.enabled = true;
            tra.mode = ParticleSystemTrailMode.Ribbon;
            tra.widthOverTrail = 7f;

            var ren = sharedPS.GetComponent<ParticleSystemRenderer>();
            ren.renderMode = ParticleSystemRenderMode.Billboard;
            ren.pivot = new Vector3(0f, 0f, 0f);
            ren.trailMaterial = trailMat;
        }

        private void SetThunder()
        {
            var main = sharedPS.main;
            main.startLifetime = new ParticleSystem.MinMaxCurve(0.3f, 0.6f);
            main.startSpeed = 1f;
            main.startSize = 0.01f;
            main.startRotation = 0;
            ParticleSystem.MinMaxGradient mg = new ParticleSystem.MinMaxGradient(gradThunder);
            mg.mode = ParticleSystemGradientMode.RandomColor;
            main.startColor = mg;

            var emi = sharedPS.emission;
            emi.enabled = true;
            emi.rateOverTime = 30f;
            emi.SetBursts(new ParticleSystem.Burst[] { });

            var sha = sharedPS.shape;
            sha.enabled = true;
            sha.shapeType = ParticleSystemShapeType.Cone;
            sha.angle = 20f;
            sha.radius = 0.0001f;

            var vel = sharedPS.velocityOverLifetime;
            vel.enabled = false;

            var col = sharedPS.colorOverLifetime;
            col.enabled = true;
            ParticleSystem.MinMaxGradient mg_over = new ParticleSystem.MinMaxGradient(gradThunder_over);
            col.color = mg_over;

            var si = sharedPS.sizeOverLifetime;
            si.enabled = false;

            var tra = sharedPS.trails;
            tra.enabled = true;
            tra.mode = ParticleSystemTrailMode.Ribbon;
            tra.widthOverTrail = 4f;

            var ren = sharedPS.GetComponent<ParticleSystemRenderer>();
            ren.renderMode = ParticleSystemRenderMode.Billboard;
            ren.pivot = new Vector3(0f, 0f, 0f);
            ren.trailMaterial = trailMat;
        }
    }
}