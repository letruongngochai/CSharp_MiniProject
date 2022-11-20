using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bai06
{
    public partial class Form1 : Form
    {
        Button[] number;
        Button[] operators;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            number = new Button[10] { Zero, One, Two, Three, Four, Five, Six, Seven, Eight, Nine };
            foreach(var num in number)
            {
                num.Click += new EventHandler(NumClick);
                num.MouseEnter += new EventHandler(NumMouseEnter);
                num.MouseLeave += new EventHandler(NumMouseLeave);
            }

            operators = new Button[4] { Divide, Multiply, Substract, Plus};
            foreach (var opr in operators)
            {
                opr.Click += new EventHandler(OperatorClick);
                opr.MouseEnter += new EventHandler(NumMouseEnter);
                opr.MouseLeave += new EventHandler(NumMouseLeave);
            }

            Dot.Click += new EventHandler(DotClick);
            Dot.MouseEnter += new EventHandler(NumMouseEnter);
            Dot.MouseEnter += new EventHandler(NumMouseLeave);

            Calc.Click += new EventHandler(CalcClick);
            Calc.MouseEnter += new EventHandler(NumMouseEnter);
            Calc.MouseEnter += new EventHandler(NumMouseLeave);
        }

        private void CalcClick(object sender, EventArgs e)
        {
            double arg1; 
            Double.TryParse(PreviousNum.Text, out arg1);
            double arg2;  
            Double.TryParse(CurNum.Text, out arg2);
            double kq = 0;
            if (Operator.Text == "/" && CurNum.Text == "0")
                MessageBox.Show("Không thể chia cho 0!!!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                switch (Operator.Text)
                {
                    case "-":
                        kq = arg1 - arg2;
                        CurNum.Text = kq.ToString();
                        break;
                    case "+":
                        kq = arg1 + arg2;
                        CurNum.Text = kq.ToString();
                        break;
                    case "*":
                        kq = arg1 * arg2;
                        CurNum.Text = kq.ToString();
                        break;
                    case "/":
                        kq = arg1 / arg2;
                        CurNum.Text = kq.ToString();
                        break;
                    default:
                        break;
                }
                PreviousNum.Text = "";
                Operator.Text = "";
            }    
            
        }

        private void DotClick(object sender, EventArgs e)
        {
            CurNum.Text += ".";
        }
        private void OperatorClick(object sender, EventArgs e)
        {
            try
            {
                double num = Double.Parse(CurNum.Text);
                Operator.Text = (sender as Button).Text;
                PreviousNum.Text = CurNum.Text;
                CurNum.Text = "";
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid number");
            }
        }
        private void NumClick(object sender, EventArgs e)
        {
            if (CurNum.Text == "0")
                CurNum.Text = "";
            CurNum.Text += (sender as Button).Text;
        }
        private void NumMouseEnter(object sender, EventArgs e)
        {
            (sender as Button).BackColor = Color.SpringGreen;
        }
        private void NumMouseLeave(object sender, EventArgs e)
        {
            (sender as Button).BackColor = Color.White;
        }

        private void Clear_Click(object sender, EventArgs e)
        {
            CurNum.Text = "0";
            Operator.Text = "";
            PreviousNum.Text = "";
        }

        private void ClearAll_Click(object sender, EventArgs e)
        {
            CurNum.Text = "0";
        }
    }
}
