using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static WindowsFormsApp1.Emitter;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        List<Emitter> emitters = new List<Emitter>();
        Emitter emitter;

        private Color selectedColor = Color.Red;
        private int selectedRadius = 50;

        public Form1()
        {
            InitializeComponent();
            picDisplay.Image = new Bitmap(picDisplay.Width, picDisplay.Height);
            this.emitter = new Emitter.TopEmitter
            {
                Width = picDisplay.Width,
                ParticlesPerTick = 15,
            };

            emitters.Add(this.emitter);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            emitter.UpdateState();

            using (var g = Graphics.FromImage(picDisplay.Image))
            {
                g.Clear(Color.Black);
                emitter.Render(g);
            }

            picDisplay.Invalidate();
        }

        private void btnChooseColor_Click(object sender, EventArgs e)
        {
            var colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                selectedColor = colorDialog.Color;
                pnlColorPreview.BackColor = selectedColor;
            }
        }

        private void tbRadius_Scroll(object sender, EventArgs e)
        {
            selectedRadius = tbRadius.Value;
            lblRadiusValue.Text = selectedRadius.ToString();
        }

        private void picDisplay_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var colorPoint = new ColorPoint
                {
                    X = e.X,
                    Y = e.Y,
                    Radius = selectedRadius,
                    TargetColor = selectedColor
                };
                emitter.impactPoints.Add(colorPoint);
            }
            else if (e.Button == MouseButtons.Right)
            {
                IImpactPoint pointToRemove = null;
                foreach (var point in emitter.impactPoints)
                {
                    if (point is ColorPoint colorPoint)
                    {
                        double distance = Math.Sqrt(
                            Math.Pow(e.X - colorPoint.X, 2) +
                            Math.Pow(e.Y - colorPoint.Y, 2)
                        );
                        if (distance < colorPoint.Radius)
                        {
                            pointToRemove = point;
                            break;
                        }
                    }
                }
                if (pointToRemove != null)
                {
                    emitter.impactPoints.Remove(pointToRemove);
                }
            }
        }

        private void tbParticlesPerTick_Scroll(object sender, EventArgs e)
        {
            emitter.ParticlesPerTick = tbParticlesPerTick.Value;
            lblParticlesValue.Text = tbParticlesPerTick.Value.ToString();
        }
    }
}