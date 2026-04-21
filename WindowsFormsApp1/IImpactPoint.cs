using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public abstract class IImpactPoint
    {
        public float X;
        public float Y;

        public abstract void ImpactParticle(Particle particle);

        public virtual void Render(Graphics g)
        {
            g.FillEllipse(
                    new SolidBrush(Color.Red),
                    X - 5,
                    Y - 5,
                    10,
                    10
                );
        }
    }
    public class GravityPoint : IImpactPoint
    {
        public int Power = 100;

        public override void ImpactParticle(Particle particle)
        {
            float gX = X - particle.X;
            float gY = Y - particle.Y;

            double r = Math.Sqrt(gX * gX + gY * gY);
            if (r + particle.Radius < Power / 2)
            {
                float r2 = (float)Math.Max(100, gX * gX + gY * gY);
                particle.SpeedX += gX * Power / r2;
                particle.SpeedY += gY * Power / r2;
            }
        }

        public override void Render(Graphics g)
        {
            g.DrawEllipse(
                   new Pen(Color.Red),
                   X - Power / 2,
                   Y - Power / 2,
                   Power,
                   Power
            );
        }
    }
    public class AntiGravityPoint : IImpactPoint
    {
        public int Power = 100;

        public override void ImpactParticle(Particle particle)
        {
            float gX = X - particle.X;
            float gY = Y - particle.Y;
            float r2 = (float)Math.Max(100, gX * gX + gY * gY);

            particle.SpeedX -= gX * Power / r2;
            particle.SpeedY -= gY * Power / r2;
        }
    }

    public class ColorPoint : IImpactPoint
    {
        public int Radius = 50;
        public Color TargetColor = Color.Red;
        public int ParticlesPassed = 0;

        private HashSet<Particle> particlesInside = new HashSet<Particle>();

        public override void ImpactParticle(Particle particle)
        {
            float gX = X - particle.X;
            float gY = Y - particle.Y;
            double distance = Math.Sqrt(gX * gX + gY * gY);

            if (distance < Radius)
            {
                if (!particlesInside.Contains(particle))
                {
                    ParticlesPassed++;
                    particlesInside.Add(particle);
                }

                if (particle is ParticleColorful colorfulParticle)
                {
                    colorfulParticle.FromColor = TargetColor;
                }
            }
            else
            {
                if (particlesInside.Contains(particle))
                {
                    particlesInside.Remove(particle);
                }
            }
        }

        public override void Render(Graphics g)
        {
            g.DrawEllipse(
                new Pen(TargetColor, 2),
                X - Radius,
                Y - Radius,
                Radius * 2,
                Radius * 2
            );

            var stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;

            var text = $"Частиц: {ParticlesPassed}";
            var font = new Font("Verdana", 10);
            var size = g.MeasureString(text, font);

            g.FillRectangle(
                new SolidBrush(Color.FromArgb(200, Color.Black)),
                X - size.Width / 2 - 3,
                Y - size.Height / 2 - 3,
                size.Width + 6,
                size.Height + 6
            );

            g.DrawString(
                text,
                font,
                new SolidBrush(Color.White),
                X,
                Y,
                stringFormat
            );
        }
    }
    public class RadarPoint : IImpactPoint
    {
        public int Radius = 50;
        public int ParticlesCount = 0;
        public bool Enabled = true;

        public override void ImpactParticle(Particle particle)
        {
            if (!Enabled) return;

            float gX = X - particle.X;
            float gY = Y - particle.Y;
            double distance = Math.Sqrt(gX * gX + gY * gY);

            if (distance < Radius)
            {
                ParticlesCount++;
            }
        }

        public override void Render(Graphics g)
        {
            if (!Enabled) return;

            g.DrawEllipse(
                new Pen(Color.Cyan, 2),
                X - Radius,
                Y - Radius,
                Radius * 2,
                Radius * 2
            );

            var stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;

            var text = $"В зоне: {ParticlesCount}";
            var font = new Font("Verdana", 10);
            var size = g.MeasureString(text, font);

            g.FillRectangle(
                new SolidBrush(Color.FromArgb(200, Color.Black)),
                X - size.Width / 2 - 3,
                Y - size.Height / 2 - 3,
                size.Width + 6,
                size.Height + 6
            );

            g.DrawString(
                text,
                font,
                new SolidBrush(Color.Cyan),
                X,
                Y,
                stringFormat
            );
        }
    }
}