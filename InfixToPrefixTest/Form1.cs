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
            textBox1.Text = 
                @"1+2*(1+2*(1+2*(1+2*(1+2))))";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string text = textBox1.Text, tempNumberStr = ""; 
            var numberAndOp = new List<string>();
            
            var opStack = new Stack<char>();
            var priorityStack = new Stack<int>();
            
            var priorityBasic = 0; 
            /*
             * 
             *  priority = [ 31bits = priorityBasic][1bit = priority of op ( +- => 0 , /* => 1)]
             *  the priority of op in () should more than the the priority of op not in (), which mean
             *  the deeper of op in () , the bigger the priority of op should be. We can use priorityBasic
             *  to make priority bigger , without mess up the order of +-/* when inside the same ().
             * 
             * 
             */

            foreach (var tempChar in text)
            {
                if (tempChar == '.' || '0' <= tempChar && tempChar <= '9' )
                {
                    tempNumberStr += tempChar;
                }
                else
                {
                    if(tempNumberStr.Length != 0)
                        numberAndOp.Add(tempNumberStr);
                    
                    tempNumberStr = "";
                }

                switch (tempChar)
                {
                    case '(':
                    {
                        priorityBasic++;
                        break;
                    }
                    case ')':
                    {
                        
                        priorityBasic--;
                        while (opStack.Count != 0 && priorityStack.Peek() > ((priorityBasic << 1) | 1))  // opStack.Count will equal to priorityStack.Count
                        {
                            numberAndOp.Add(opStack.Pop().ToString());
                            priorityStack.Pop();
                        }

                        break;
                    }
                    case '+':
                    case '-':
                    {
                        while (opStack.Count != 0 && priorityStack.Peek() > (priorityBasic << 1))  // opStack.Count will equal to priorityStack.Count
                        {
                            numberAndOp.Add(opStack.Pop().ToString());
                            priorityStack.Pop();
                        }

                        opStack.Push(tempChar);
                        priorityStack.Push((priorityBasic << 1));
                        break;
                    }
                    case '*':
                    case '/' :
                    {
                        while (opStack.Count != 0 && priorityStack.Peek() > ((priorityBasic << 1) | 1))  // opStack.Count will equal to priorityStack.Count
                        {
                            numberAndOp.Add(opStack.Pop().ToString());
                            priorityStack.Pop();
                        }

                        opStack.Push(tempChar);
                        priorityStack.Push((priorityBasic << 1) | 1);
                        break;
                    }
                }
            }

            label1.Text = "";

            if(tempNumberStr!="")numberAndOp.Add(tempNumberStr);

            var resultStack = new Stack<double>();
            foreach (var s in numberAndOp)
            {
                label1.Text += s + @"  ";

                try
                {   // number
                    resultStack.Push(Convert.ToDouble(s));
                }
                catch (Exception) // not number
                {
                    var b = resultStack.Pop();
                    var a = resultStack.Pop();
                    var c = 0.0;
                    switch (s)
                    {
                        case "+":
                            c = a + b;
                            break;
                        case "-":
                            c = a - b;
                            break;
                        case "*" :
                            c = a * b;
                            break;
                        case "/" :
                            c = a / b;
                            break;
                    }
                    
                    resultStack.Push(c);
                }
                
            }

            label1.Text += @"    ";

            while (opStack.Count != 0)
            {
                try
                {
                    var s = opStack.Pop();
                    label1.Text += s+ @"  ";
                    var b = resultStack.Pop();
                    var a = resultStack.Pop();
                    var c = 0.0;
                    switch (s)
                    {
                        case '+':
                            c = a + b;
                            break;
                        case '-':
                            c = a - b;
                            break;
                        case '*' :
                            c = a * b;
                            break;
                        case '/' :
                            c = a / b;
                            break;
                    }
                    
                    resultStack.Push(c);
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            if (resultStack.Count != 0)
                label1.Text += '\n'+""+resultStack.Peek();
        }
    }
}