using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace InfixToPrefixTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string text = textBox1.Text, tempNumber = ""; 
            List<string> numberAndOp = new List<string>();
            Stack<char> opStack = new Stack<char>();
            Stack<int> priorStack = new Stack<int>();
            int priorBasic = 0;
            for (int i = 0; i < text.Length; i++)
            {

                char tempChar = text[i];
                if ('0' <= tempChar && tempChar <= '9')
                {
                    tempNumber += tempChar;
                }
                else
                {
                    if(tempNumber.Length != 0)
                        numberAndOp.Add(tempNumber);
                    
                    tempNumber = "";
                }

                if (tempChar == '(')
                    priorBasic++;
                else if (tempChar == ')')
                {
                    priorBasic--;
                    while (opStack.Count != 0 && priorStack.Peek() > (priorBasic << 1)+ 1)
                    {
                        numberAndOp.Add(opStack.Pop().ToString());
                        priorStack.Pop();
                    }

                }
                else if (tempChar == '+')
                {
                    while (opStack.Count != 0 && priorStack.Peek() > priorBasic << 1)
                    {
                        numberAndOp.Add(opStack.Pop().ToString());
                        priorStack.Pop();
                    }

                    opStack.Push(tempChar);
                    priorStack.Push(priorBasic << 1);
                }
                else if (tempChar == '*')
                {
                    while (opStack.Count != 0 && priorStack.Peek() > (priorBasic << 1) + 1)
                    {
                        numberAndOp.Add(opStack.Pop().ToString());
                        priorStack.Pop();
                    }

                    opStack.Push(tempChar);
                    priorStack.Push(priorBasic << 1+1);

                }
            }







            label1.Text = "";

            if(tempNumber!="")numberAndOp.Add(tempNumber);

            foreach (string s in numberAndOp)
            {
                label1.Text += s + " ";
            }

            while (opStack.Count != 0)
            {
                label1.Text += opStack.Pop() + " ";
            }

        }
    }
}
