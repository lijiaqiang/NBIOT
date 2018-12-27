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
    public partial class LEDTestForm : Form
    {
        public delegate void resultDelegate(bool isPass);
        public resultDelegate m_resultFun;
        public LEDTestForm()
        {
            InitializeComponent();
        }

        private void btnNO_Click(object sender, EventArgs e)
        {
            if (m_resultFun != null)
            {
                m_resultFun(false);
            }
            Close();
        }

        private void btnYES_Click(object sender, EventArgs e)
        {
            if (m_resultFun != null)
            {
                m_resultFun(true);
            }
            Close();
        }


    }
}
