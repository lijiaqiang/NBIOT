using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
// using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace MAT
{
    public partial class PowerVoiceForm : Form
    {
        public PowerVoiceForm()
        {
            InitializeComponent();
        }

        public delegate void resultDelegate(bool voicePass, bool lightPass);
        public resultDelegate m_resultDelegate;
        private int m_voicePass = 2;
        private int m_lightPass = 2;

        private void button_has_voice_Click(object sender, EventArgs e)
        {
            m_voicePass = 1;
            button_has_voice.Enabled = false;
            button_has_voice.BackColor = Color.Green;
            button_no_voice.Enabled = false;
            DoResult();
        }

        private void button_no_voice_Click(object sender, EventArgs e)
        {
            m_voicePass = 0;
            button_has_voice.Enabled = false;
            button_no_voice.Enabled = false;
            button_no_voice.BackColor = Color.Red;
            DoResult();
        }

        private void button_has_light_Click(object sender, EventArgs e)
        {
            m_lightPass = 1;
            button_has_light.Enabled = false;
            button_has_light.BackColor = Color.Green;
            button_no_light.Enabled = false;
            DoResult();
        }

        private void button_no_light_Click(object sender, EventArgs e)
        {
            m_lightPass = 0;
            button_has_light.Enabled = false;
            button_no_light.Enabled = false;
            button_no_light.BackColor = Color.Red;
            DoResult();
        }

        private void DoResult()
        {
            if (m_voicePass != 2 && m_lightPass != 2)
            {
                bool voicePass = false;
                bool lightPass = false;
                voicePass = m_voicePass == 1 ? true : false;
                lightPass = m_lightPass == 1 ? true : false;
                if (m_resultDelegate != null)
                {
                    m_resultDelegate(voicePass, lightPass);
                }
                System.Threading.Thread.Sleep(1000);
                Close();
            }
        }


    }
}
