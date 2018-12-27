using System;
using System.Collections.Generic;
// using System.Linq;
using System.Text;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

namespace MAT
{
    public static class Extensions
    {
        public static void SafeCall(this Control ctrl, Action callback)
        {
            try
            {
                if (ctrl.InvokeRequired)
                    ctrl.Invoke(callback);
                else
                    callback();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.ToString());
            }
        }
    }

    class Dbj_ListView : System.Windows.Forms.ListView
    {
        private Dictionary<string,int> Dbj_Items; //Dbj_Item[""] = 0
        private const string FailStr = "FAIL";
        private const string PassStr = "PASS";

        public Dbj_ListView()
            : base()
        {
            Dbj_Items = new Dictionary<string,int>();

            ImageList imagelist = new ImageList();
            imagelist.ImageSize = new Size(1, 40);
            this.SmallImageList = imagelist;

            this.Columns.Add("测试项", 150);
            this.Columns.Add("测试结果", 80);
            this.Columns.Add("错误描述", 550);
           // this.Items.Add(new ListViewItem(new string[] { "1", "", "" }));
        }
    
        public void ResetListView()
        {//重置测试项的数据
            int rowCount = this.Items.Count;
            int i = 0;
            this.BackColor = Color.White;
            for (; i < rowCount; i++)
            {
                this.Items[i].UseItemStyleForSubItems = false;
                this.Items[i].SubItems[0].BackColor = Color.White;
                this.Items[i].SubItems[1].Text = "";
                this.Items[i].SubItems[1].BackColor = Color.White;
                this.Items[i].SubItems[2].Text = "";
                this.Items[i].SubItems[2].BackColor = Color.White;
            }

            this.Controls.Clear();
        }

        public void SetAllItemColor(int subItemIndex, Color color)
        {//设置第几列单元格的颜色
            for (int i = 0; i < this.Items.Count;i++ )
            {
                this.Items[i].UseItemStyleForSubItems = false;
                this.Items[i].SubItems[subItemIndex].BackColor = color;
            }
        }

        public void SetDetail(String index, String strError)
        {
            if (Dbj_Items.ContainsKey(index) == false)
            {
                return;
            }
            int nIndex;
            try
            {
                nIndex = (int)Dbj_Items[index];
            }
            catch (System.Collections.Generic.KeyNotFoundException ex)
            {
                System.Console.WriteLine("SetDetail(String index={0}, String strError={1}) erro:{2}",
                    index, strError, ex.Message);
                return;
            }

            this.SafeCall(delegate()
            {
                this.Items[nIndex].UseItemStyleForSubItems = false;

                this.Items[nIndex].SubItems[2].Text = strError;
            });
        }

        public void SetDetail(String index, bool p)
        {//设置是否通过，不带原因
            if (Dbj_Items.ContainsKey(index) == false)
            {
                return;
            }
            int nIndex;
            try
            {
                nIndex = (int)Dbj_Items[index];
            }
            catch (System.Collections.Generic.KeyNotFoundException ex)
            {
                System.Console.WriteLine("SetDetail(String index = {0}, bool p={1}) erro:{2}",
                    index,p, ex.Message);
                return;
            }

            this.Items[nIndex].UseItemStyleForSubItems = false;

            this.SafeCall(delegate()
            {
                if(p)
                {
                    this.Items[nIndex].SubItems[1].Text = PassStr;
                    this.Items[nIndex].SubItems[1].BackColor = Color.Green;
                }
                else
                {
                    this.Items[nIndex].SubItems[1].Text = FailStr;
                    this.Items[nIndex].SubItems[1].BackColor = Color.Red;
                }
            });
        }

        public void SetDetail(String index, bool p, String strError)
        {//设置是否通过，带原因设置
            if (Dbj_Items.ContainsKey(index) == false)
            {
                return;
            }
            int nIndex;
            try
            {
                nIndex = (int)Dbj_Items[index];
            }
            catch (System.Collections.Generic.KeyNotFoundException ex)
            {
                System.Console.WriteLine("SetDetail(String index={0}, bool p={1}, String strError={2}) erro:{3}",
                    index,p,strError,ex.Message);
                return;
            }

            this.Items[nIndex].UseItemStyleForSubItems = false;

            this.SafeCall(delegate()
            {
                this.Items[nIndex].UseItemStyleForSubItems = false;
                if (p)
                {
                    this.Items[nIndex].SubItems[1].Text = PassStr;
                    this.Items[nIndex].SubItems[1].BackColor = Color.Green;
                    this.Items[nIndex].SubItems[2].Text = strError;
                }
                else
                {
                    this.Items[nIndex].SubItems[1].Text = FailStr;
                    this.Items[nIndex].SubItems[1].BackColor = Color.Red;
                    this.Items[nIndex].SubItems[2].Text = strError;
                }
            });
        }

        public void AppendDetail(String index, String s)
        {//在描述里面添加内容
            if (Dbj_Items.ContainsKey(index) == false)
            {
                return;
            }
            int nIndex;
            try
            {
            	nIndex = (int)Dbj_Items[index];
            }
            catch (System.Collections.Generic.KeyNotFoundException ex)
            {
                System.Console.WriteLine("AppendDetail(String index={0}, String s={1}) erro:{2}",
                    index,s,ex.Message);
                return;
            }

            this.Items[nIndex].UseItemStyleForSubItems = false;

            this.SafeCall(delegate()
            {
                this.Items[nIndex].UseItemStyleForSubItems = false;
                this.Items[nIndex].SubItems[2].Text += s;
            });
        }

        public void DeleteItem(string index)
        {//删除测试项
            if (Dbj_Items.ContainsKey(index) == false)
            {
                return;
            }
            int nIndex;
            try
            {
                nIndex = (int)Dbj_Items[index];
            }
            catch (System.Collections.Generic.KeyNotFoundException ex)
            {
                System.Console.WriteLine("DeleteItem(string index={0}) erro:{1}",
                    index,ex.Message);
                return;
            }
            try
            {
                Dbj_Items.Remove(index);
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return;
            }
            this.SafeCall(delegate()
            {
                this.Items.RemoveAt(nIndex);
            });
        }

        public void AddItem(string index, string testStr)
        {
            int i = Dbj_Items.Count;
            this.Items.Add(new ListViewItem(new string[] { testStr, "", "" }));
            Dbj_Items[index] = i;
        }


        public void SetDetailDescription(String index, String content)
        {//设置测试项目的详细
            if (Dbj_Items.ContainsKey(index) == false)
            {
                return;
            }
            int nIndex;
            try
            {
                nIndex = (int)Dbj_Items[index];
            }
            catch (System.Collections.Generic.KeyNotFoundException ex)
            {
                System.Console.WriteLine("SetDetailDescription(String index={0}, String content={1}) erro:{2}",
                   index,content, ex.Message);
                return;
            }

            this.SafeCall(delegate()
            {
                 this.Items[nIndex].UseItemStyleForSubItems = false;
                 this.Items[nIndex].SubItems[2].Text = content;
            });
        }

        public void clearItem()
        {
            int nCount = this.Items.Count;
            while ((--nCount) > -1)
            {
                this.Items.RemoveAt(nCount);
            }
            
        }

        public bool isTestAll()
        {//判断是否已经全部都测试过了
            int rowCount = this.Items.Count;
            int i = 0;
            this.BackColor = Color.White;
            for (; i < rowCount; i++)
            {
                if (this.Items[i].SubItems[1].BackColor != Color.Green)
                {
                    return false;
                }
            }
            return true;
        }

        public string getErroStr()
        {
            string erroTemp = "";
            int rowCount = this.Items.Count;
            int i = 0;
            this.BackColor = Color.White;
            for (; i < rowCount; i++)
            {
                if (this.Items[i].SubItems[1].Text == FailStr || 
                    this.Items[i].SubItems[1].Text.Length == 0)
                {
                    string temp = string.Format("{0}.{1}:{2}\n",i+1, this.Items[i].SubItems[0].Text,
                        this.Items[i].SubItems[2].Text.Length == 0 ? "FAIL" : this.Items[i].SubItems[2].Text);
                    erroTemp += temp;
                }
            }
            return erroTemp;
        }

        public bool isHaveFail()
        {
            int rowCount = this.Items.Count;
            int i = 0;
            for (; i < rowCount; i++)
            {
                if (this.Items[i].SubItems[1].BackColor == Color.Red)
                {
                    return true;
                }
            }
            return false;
        }
        public int isBeTest(string index)
        {
            if (Dbj_Items.ContainsKey(index) == false)
            {
                return -1;
            }
            int nIndex;
            try
            {
                nIndex = (int)Dbj_Items[index];
            }
            catch (System.Collections.Generic.KeyNotFoundException ex)
            {

                return -1;
            }

            if (this.Items[nIndex].SubItems[1].Text.Length != 0)
            {
                if (this.Items[nIndex].SubItems[1].Text == FailStr)
                {
                    return 0;
                }
                else if (this.Items[nIndex].SubItems[1].Text == PassStr)
                {
                    return 1;
                }
            }
            return -1;
        }

        public void InitFromDictionary(Dictionary<string,string> table)
        { //通过一个字典数组来填充列表控件的数据
            Dbj_Items.Clear();
            this.clearItem();
            int i = 0;
            this.BeginUpdate();
            foreach(KeyValuePair<string,string> de in table)
            {
                string value = (string)de.Value;
                this.Items.Add(new ListViewItem(new string[] { value, "", "" }));
                Dbj_Items[(string)de.Key] = i++;
            }
            this.EndUpdate();
        }

        public int AddBtnWithEvent(string index,System.EventHandler EventFun)
        {
            if (Dbj_Items.ContainsKey(index) == false)
            {
                return -1;
            }
            int nIndex;
            try
            {
                nIndex = (int)Dbj_Items[index];
            }
            catch (System.Collections.Generic.KeyNotFoundException ex)
            {

                return -1;
            }


            
            Button btnYesTemp = new Button();
            btnYesTemp.Text = "YES";
            btnYesTemp.Click += EventFun;
            btnYesTemp.Tag = 1;
            this.Controls.Add(btnYesTemp);
            btnYesTemp.Location = new Point(this.Items[nIndex].SubItems[2].Bounds.X ,
                this.Items[nIndex].SubItems[2].Bounds.Y);
            btnYesTemp.Size = new Size(50,
                this.Items[nIndex].SubItems[2].Bounds.Height);

            Button btnNoTemp = new Button();
            btnNoTemp.Text = "NO";
            btnNoTemp.Click += EventFun;
            btnNoTemp.Tag = 0;
            this.Controls.Add(btnNoTemp);
            btnNoTemp.Location = new Point(this.Items[nIndex].SubItems[2].Bounds.X + 50,
                this.Items[nIndex].SubItems[2].Bounds.Y);
            btnNoTemp.Size = new Size(50,
                this.Items[nIndex].SubItems[2].Bounds.Height);

            return 1;
        }

        public int AddBtnWithEvent(string index,string detial,int beginPosX, System.EventHandler EventFun)
        {
            if (Dbj_Items.ContainsKey(index) == false)
            {
                return -1;
            }
            int nIndex;
            try
            {
                nIndex = (int)Dbj_Items[index];
            }
            catch (System.Collections.Generic.KeyNotFoundException ex)
            {

                return -1;
            }

            this.Items[nIndex].SubItems[2].Text = detial;

            Button btnYesTemp = new Button();
            btnYesTemp.Text = "YES";
            btnYesTemp.Click += EventFun;
            btnYesTemp.Tag = 1;
            this.Controls.Add(btnYesTemp);
            btnYesTemp.Location = new Point(this.Items[nIndex].SubItems[2].Bounds.X + beginPosX,
                this.Items[nIndex].SubItems[2].Bounds.Y);
            btnYesTemp.Size = new Size(50,
                this.Items[nIndex].SubItems[2].Bounds.Height);

            Button btnNoTemp = new Button();
            btnNoTemp.Text = "NO";
            btnNoTemp.Click += EventFun;
            btnNoTemp.Tag = 0;
            this.Controls.Add(btnNoTemp);
            btnNoTemp.Location = new Point(this.Items[nIndex].SubItems[2].Bounds.X + 50+beginPosX,
                this.Items[nIndex].SubItems[2].Bounds.Y);
            btnNoTemp.Size = new Size(50,
                this.Items[nIndex].SubItems[2].Bounds.Height);

            return 1;
        }
    }
}
