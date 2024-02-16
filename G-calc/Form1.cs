namespace g_calc
{
    public partial class gcalc : Form
    {
        String entrystring = "";
        double workingvalue;
        double operand1;
        double operand2;
        double result;
        bool displayresult;
        char operation = '\0';
        /*String resultstring;*/

        long f64ashex;
        int f32ashex;
        short f16ashex;

        public gcalc()
        {
            InitializeComponent();
            operand1 = 0;
            operand2 = 0;
            result = 0;
            displayresult = false;
        }

        private void gcalc_Load(object sender, EventArgs e)
        {
            ActiveControl = buttonequals;
        }

        private double converthexstring(string h)
        {
            double result = 0;
            long temp;
            if ((h.Length & 1) == 1) h = String.Format("0{0}", h);

            temp = Int64.Parse(h, System.Globalization.NumberStyles.HexNumber);

            result = (double)temp;

            return result;
        }

        private double convertfloat16hexstring(string h)
        {
            double result = 0;
            Half temp;
            if ((h.Length & 1) == 1) h = String.Format("0{0}", h);

            temp = BitConverter.Int16BitsToHalf((short)Int64.Parse(h, System.Globalization.NumberStyles.HexNumber));

            result = (double)temp;

            return result;
        }

        private double convertfloat32hexstring(string h)
        {
            double result = 0;
            float temp;
            if ((h.Length & 1) == 1) h = String.Format("0{0}", h);

            temp = BitConverter.Int32BitsToSingle((int)Int64.Parse(h, System.Globalization.NumberStyles.HexNumber));

            result = (double)temp;

            return result;
        }

        private double convertfloat64hexstring(string h)
        {
            double result = 0;
            double temp;
            if ((h.Length & 1) == 1) h = String.Format("0{0}", h);

            temp = BitConverter.Int64BitsToDouble(Int64.Parse(h, System.Globalization.NumberStyles.HexNumber));

            result = temp;

            return result;
        }

        private void updateworking(bool ignoreentrystring)
        {
            Half f16;
            float f32;
            /*long temphex;*/

            if (displayresult) workingvalue = result;
            else if ((!ignoreentrystring) && (entrystring.Length != 0))
            {
                if (cbInteger.Checked) workingvalue = (double)Convert.ToInt64(entrystring);
                if (cbLongInteger.Checked) workingvalue = (double)Convert.ToInt64(entrystring);
                if (cbHexadecimal.Checked) workingvalue = converthexstring(entrystring);

                if (cbFloat16Hex.Checked) workingvalue = convertfloat16hexstring(entrystring);
                if (cbFloat32Hex.Checked) workingvalue = convertfloat32hexstring(entrystring);
                if (cbFloat64Hex.Checked) workingvalue = convertfloat64hexstring(entrystring);

                if (cbFloat16.Checked) { f16 = (Half)Convert.ToDouble(entrystring); workingvalue = (double)(f16); }
                if (cbFloat32.Checked) { f32 = (float)Convert.ToDouble(entrystring); workingvalue = (double)(f32); }
                if (cbFloat64.Checked) workingvalue = Convert.ToDouble(entrystring);
            }

            f16 = (Half)workingvalue;
            f32 = (float)workingvalue;

            lblIntegerEntry.Text = String.Format("{0:d}", (int)(workingvalue));
            LblLongEntry.Text = String.Format("{0}", (long)workingvalue);
            lblHexadecimalEntry.Text = String.Format("0x{0:X8}", (int)workingvalue);
            lblLongHexEntry.Text = String.Format("0x{0:X}", (long)workingvalue);
            f16ashex = BitConverter.HalfToInt16Bits(f16);
            lblFloat16Hex.Text = String.Format("0x{0}", f16ashex.ToString("X"));
            f32ashex = BitConverter.SingleToInt32Bits(f32);
            lblFloat32Hex.Text = String.Format("0x{0}", f32ashex.ToString("X"));
            f64ashex = BitConverter.DoubleToInt64Bits(workingvalue);
            lblFloat64Hex.Text = String.Format("0x{0}", f64ashex.ToString("X"));

            lblFloat16Entry.Text = String.Format("{0:f}", f16);
            lblFloat32Entry.Text = String.Format("{0:f}", f32);
            lblFloat64Entry.Text = String.Format("{0:f}", workingvalue);

            updatecalculation();
        }

        private void updatecalculation()
        {
            if ((cbInteger.Checked) || (cbLongInteger.Checked) || (cbFloat16.Checked) || (cbFloat32.Checked) || (cbFloat64.Checked))
            {
                if (displayresult) lblcalculation.Text = String.Format("{0} {1}\n{2} =\n{3}", operand1, operation, operand2, result);
                else if (operation == '\0') lblcalculation.Text = String.Format("{0}", workingvalue);
                else if (operand2 == 0) lblcalculation.Text = String.Format("{0} {1}\n {2}", operand1, operation, workingvalue);

            }
            if (cbHexadecimal.Checked)
            {
                if (displayresult) lblcalculation.Text = String.Format("{0:X} {1}\n{2:X} =\n{3:X}", (int)operand1, operation, (int)operand2, (int)result);
                else if (operation == '\0') lblcalculation.Text = String.Format("{0:X}", (int)workingvalue);
                else lblcalculation.Text = String.Format("{0:X} {1}\n {2:X}", (int)operand1, operation, (int)workingvalue);
            }
            if (cbFloat16Hex.Checked)
            {
                if (displayresult) lblcalculation.Text = String.Format("{0:X} {1}\n{2:X} =\n{3:X}", (int)operand1, operation, (int)operand2, (int)result);
                else if (operation == '\0') lblcalculation.Text = String.Format("{0:X}", f16ashex);
                else lblcalculation.Text = String.Format("{0:X} {1}\n {2:X}", (int)operand1, operation, (int)workingvalue);
            }

            if (cbFloat32Hex.Checked)
            {
                if (displayresult) lblcalculation.Text = String.Format("{0:X} {1}\n{2:X} =\n{3:X}", (int)operand1, operation, (int)operand2, (int)result);
                else if (operation == '\0') lblcalculation.Text = String.Format("{0:X}", f32ashex);
                else lblcalculation.Text = String.Format("{0:X} {1}\n {2:X}", (int)operand1, operation, (int)workingvalue);
            }

            if (cbFloat64Hex.Checked)
            {
                /*resultstring = String.Format("{0}", result);*/
                if (displayresult) lblcalculation.Text = String.Format("{0:X} {1}\n{2:X} =\n{3:X}", (int)operand1, operation, (int)operand2, (int)result);
                else if (operation == '\0') lblcalculation.Text = String.Format("{0:X}", f64ashex);
                else lblcalculation.Text = String.Format("{0:X} {1}\n {2:X}", (int)operand1, operation, (int)workingvalue);
            }

        }

        private void enter1()
        {
            entrystring = String.Format("{0}1", entrystring);
            updateworking(false);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            enter1();
            buttonequals.Focus();
        }

        private void enter2()
        {
            entrystring = String.Format("{0}2", entrystring);
            updateworking(false);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            enter2();
            buttonequals.Focus();
        }

        private void enter3()
        {
            entrystring = String.Format("{0}3", entrystring);
            updateworking(false);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            enter3();
            buttonequals.Focus();
        }

        private void enter4()
        {
            entrystring = String.Format("{0}4", entrystring);
            updateworking(false);
        }
        private void button4_Click(object sender, EventArgs e)
        {
            enter4();
            buttonequals.Focus();
        }

        private void enter5()
        {
            entrystring = String.Format("{0}5", entrystring);
            updateworking(false);
        }
        private void button5_Click(object sender, EventArgs e)
        {
            enter5();
            buttonequals.Focus();
        }

        private void enter6()
        {
            entrystring = String.Format("{0}6", entrystring);
            updateworking(false);
        }
        private void button6_Click(object sender, EventArgs e)
        {
            enter6();
            buttonequals.Focus();
        }

        private void enter7()
        {
            entrystring = String.Format("{0}7", entrystring);
            updateworking(false);
        }
        private void button7_Click(object sender, EventArgs e)
        {
            enter7();
            buttonequals.Focus();
        }

        private void enter8()
        {
            entrystring = String.Format("{0}8", entrystring);
            updateworking(false);
        }
        private void button8_Click(object sender, EventArgs e)
        {
            enter8();
            buttonequals.Focus();
        }

        private void enter9()
        {
            entrystring = String.Format("{0}9", entrystring);
            updateworking(false);
        }
        private void button9_Click(object sender, EventArgs e)
        {
            enter9();
            buttonequals.Focus();
        }

        private void enter0()
        {
            entrystring = String.Format("{0}0", entrystring);
            updateworking(false);
        }
        private void button0_Click(object sender, EventArgs e)
        {
            enter0();
            buttonequals.Focus();
        }

        private void enterpoint()
        {
            if (cbInteger.Checked || cbHexadecimal.Checked) { }
            else entrystring = String.Format("{0}.", entrystring);
            updateworking(false);
            buttonequals.Focus();
        }

        private void buttonpoint_Click(object sender, EventArgs e)
        {
            enterpoint();
        }

        private void buttonsign_Click(object sender, EventArgs e)
        {
            workingvalue *= -1;

            updateworking(true);
            buttonequals.Focus();
        }

        private void EnableHexButtons()
        {
            buttonA.Enabled = true;
            buttonB.Enabled = true;
            buttonC.Enabled = true;
            buttonD.Enabled = true;
            buttonE.Enabled = true;
            buttonF.Enabled = true;
            buttonpoint.Enabled = false;
        }

        private void DisableHexButtons()
        {
            buttonA.Enabled = false;
            buttonB.Enabled = false;
            buttonC.Enabled = false;
            buttonD.Enabled = false;
            buttonE.Enabled = false;
            buttonF.Enabled = false;
            buttonpoint.Enabled = true;
        }
        private void selectdisplay_Integer()
        {
            cbHexadecimal.Checked = false;
            cbLongInteger.Checked = false;
            cbLongHex.Checked = false;
            cbFloat16.Checked = false;
            cbFloat16Hex.Checked = false;
            cbFloat32.Checked = false;
            cbFloat32Hex.Checked = false;
            cbFloat64.Checked = false;
            cbFloat64Hex.Checked = false;

            cbInteger.Checked = true;
            DisableHexButtons();
        }

        private void selectdisplay_Hexadecimal()
        {
            cbInteger.Checked = false;
            cbLongInteger.Checked = false;
            cbLongHex.Checked = false;
            cbFloat16.Checked = false;
            cbFloat16Hex.Checked = false;
            cbFloat32.Checked = false;
            cbFloat32Hex.Checked = false;
            cbFloat64.Checked = false;
            cbFloat64Hex.Checked = false;

            cbHexadecimal.Checked = true;

            EnableHexButtons();
        }

        private void selectdisplay_LongInteger()
        {
            cbHexadecimal.Checked = false;
            cbLongHex.Checked = false;
            cbFloat16.Checked = false;
            cbFloat16Hex.Checked = false;
            cbFloat32.Checked = false;
            cbFloat32Hex.Checked = false;
            cbFloat64.Checked = false;
            cbFloat64Hex.Checked = false;

            cbLongInteger.Checked = true;
            DisableHexButtons();
        }

        private void selectdisplay_LongHex()
        {
            cbInteger.Checked = false;
            cbLongInteger.Checked = false;
            cbFloat16.Checked = false;
            cbFloat16Hex.Checked = false;
            cbFloat32.Checked = false;
            cbFloat32Hex.Checked = false;
            cbFloat64.Checked = false;
            cbFloat64Hex.Checked = false;

            cbLongHex.Checked = true;
            EnableHexButtons();
        }

        private void selectdisplay_Float16()
        {
            cbInteger.Checked = false;
            cbHexadecimal.Checked = false;
            cbLongInteger.Checked = false;
            cbLongHex.Checked = false;
            cbFloat16Hex.Checked = false;
            cbFloat32.Checked = false;
            cbFloat32Hex.Checked = false;
            cbFloat64.Checked = false;
            cbFloat64Hex.Checked = false;

            cbFloat16.Checked = true;
            DisableHexButtons();
        }

        private void selectdisplay_Float16Hex()
        {
            cbInteger.Checked = false;
            cbHexadecimal.Checked = false;
            cbLongInteger.Checked = false;
            cbLongHex.Checked = false;
            cbFloat16.Checked = false;
            cbFloat32.Checked = false;
            cbFloat32Hex.Checked = false;
            cbFloat64.Checked = false;
            cbFloat64Hex.Checked = false;

            cbFloat16Hex.Checked = true;
            EnableHexButtons();

        }
        private void selectdisplay_Float32()
        {
            cbInteger.Checked = false;
            cbHexadecimal.Checked = false;
            cbLongInteger.Checked = false;
            cbLongHex.Checked = false;
            cbFloat16.Checked = false;
            cbFloat16Hex.Checked = false;
            cbFloat32Hex.Checked = false;
            cbFloat64.Checked = false;
            cbFloat64Hex.Checked = false;

            cbFloat32.Checked = true;
            DisableHexButtons();
        }

        private void selectdisplay_Float32Hex()
        {
            cbInteger.Checked = false;
            cbHexadecimal.Checked = false;
            cbLongInteger.Checked = false;
            cbLongHex.Checked = false;
            cbFloat16.Checked = false;
            cbFloat16Hex.Checked = false;
            cbFloat32.Checked = false;
            cbFloat64.Checked = false;
            cbFloat64Hex.Checked = false;

            cbFloat32Hex.Checked = true;
            EnableHexButtons();
        }
        private void selectdisplay_Float64()
        {
            cbInteger.Checked = false;
            cbHexadecimal.Checked = false;
            cbLongInteger.Checked = false;
            cbLongHex.Checked = false;
            cbFloat16.Checked = false;
            cbFloat16Hex.Checked = false;
            cbFloat32.Checked = false;
            cbFloat32Hex.Checked = false;
            cbFloat64Hex.Checked = false;

            cbFloat64.Checked = true;
            DisableHexButtons();
        }

        private void selectdisplay_Float64Hex()
        {
            cbInteger.Checked = false;
            cbHexadecimal.Checked = false;
            cbLongInteger.Checked = false;
            cbLongHex.Checked = false;
            cbFloat16.Checked = false;
            cbFloat16Hex.Checked = false;
            cbFloat32.Checked = false;
            cbFloat32Hex.Checked = false;
            cbFloat64.Checked = false;

            cbFloat64Hex.Checked = true;
            EnableHexButtons();
        }

        private void lblFloat16Entry_DoubleClick(object sender, EventArgs e)
        {
            selectdisplay_Float16();
            buttonequals.Focus();
        }

        private void lblFloat16Hex_DoubleClick(object sender, EventArgs e)
        {
            selectdisplay_Float16Hex();
            buttonequals.Focus();
        }

        private void lblFloat32Entry_DoubleClick(object sender, EventArgs e)
        {
            selectdisplay_Float32();
            buttonequals.Focus();
        }

        private void lblFloat32Hex_DoubleClick(object sender, EventArgs e)
        {
            selectdisplay_Float32Hex();
            buttonequals.Focus();
        }

        private void lblFloat64Entry_DoubleClick(object sender, EventArgs e)
        {
            selectdisplay_Float64();
            buttonequals.Focus();
        }

        private void lblFloat64Hex_DoubleClick(object sender, EventArgs e)
        {
            selectdisplay_Float64Hex();
            buttonequals.Focus();
        }

        private void lblIntegerEntry_DoubleClick(object sender, EventArgs e)
        {
            selectdisplay_Integer();
            buttonequals.Focus();
        }

        private void lblHexadecimalEntry_DoubleClick(object sender, EventArgs e)
        {
            selectdisplay_Hexadecimal();
            buttonequals.Focus();
        }

        private void cbHexadecimal_Click(object sender, EventArgs e)
        {
            selectdisplay_Hexadecimal();
            buttonequals.Focus();
        }

        private void cbInteger_Click(object sender, EventArgs e)
        {
            selectdisplay_Integer();
            buttonequals.Focus();
        }

        private void cbFloat64Hex_Click(object sender, EventArgs e)
        {
            selectdisplay_Float64Hex();
            buttonequals.Focus();
        }

        private void cbFloat64_Click(object sender, EventArgs e)
        {
            selectdisplay_Float64();
            buttonequals.Focus();
        }

        private void cbFloat32Hex_Click(object sender, EventArgs e)
        {
            selectdisplay_Float32Hex();
            buttonequals.Focus();
        }

        private void cbFloat32_Click(object sender, EventArgs e)
        {
            selectdisplay_Float32();
            buttonequals.Focus();
        }

        private void cbFloat16Hex_Click(object sender, EventArgs e)
        {
            selectdisplay_Float16Hex();
            buttonequals.Focus();
        }

        private void cbFloat16_Click(object sender, EventArgs e)
        {
            selectdisplay_Float16();
            buttonequals.Focus();
        }

        private void lblFloat16Entry_Click(object sender, EventArgs e)
        {
            String s = lblFloat16Entry.Text;
            if (s.Length > 0)
            {
                if(s.StartsWith("0x")) s = s.Substring(2);
                if (s.StartsWith("0X")) s = s.Substring(2);
                Clipboard.SetText(s);
            }
            buttonequals.Focus();
        }

        private void lblFloat16Hex_Click(object sender, EventArgs e)
        {
            string s = lblFloat16Hex.Text;
            if (s.Length > 0)
            {
                if (s.StartsWith("0x")) s = s.Substring(2);
                if (s.StartsWith("0X")) s = s.Substring(2);
                Clipboard.SetText(s);
            }
            buttonequals.Focus();
        }

        private void lblFloat32Entry_Click(object sender, EventArgs e)
        {
            String s = lblFloat32Entry.Text;
            if (s.Length > 0)
            {
                if (s.StartsWith("0x")) s = s.Substring(2);
                if (s.StartsWith("0X")) s = s.Substring(2);
                Clipboard.SetText(s);
            }
            buttonequals.Focus();
        }

        private void lblFloat32Hex_Click(object sender, EventArgs e)
        {
            String s = lblFloat32Hex.Text;
            if (s.Length > 0)
            {
                if (s.StartsWith("0x")) s = s.Substring(2);
                if (s.StartsWith("0X")) s = s.Substring(2);
                Clipboard.SetText(s);
            }
            buttonequals.Focus();
        }

        private void lblFloat64Entry_Click_1(object sender, EventArgs e)
        {
            if(lblFloat64Entry.Text.Length > 0) Clipboard.SetText(lblFloat64Entry.Text);
            buttonequals.Focus();
        }

        private void lblFloat64Hex_Click_1(object sender, EventArgs e)
        {
            String s = lblFloat64Hex.Text;
            if (s.Length > 0)
            {
                if (s.StartsWith("0x")) s = s.Substring(2);
                if (s.StartsWith("0X")) s = s.Substring(2);
                Clipboard.SetText(s);
            }
            buttonequals.Focus();
        }

        private void lblIntegerEntry_Click(object sender, EventArgs e)
        {
            if(lblIntegerEntry.Text.Length > 0) Clipboard.SetText(lblIntegerEntry.Text);
            buttonequals.Focus();
        }

        private void lblHexadecimalEntry_Click(object sender, EventArgs e)
        {
            String s = lblHexadecimalEntry.Text;
            if (s.Length > 0)
            {
                if (s.StartsWith("0x")) s = s.Substring(2);
                if (s.StartsWith("0X")) s = s.Substring(2);
                Clipboard.SetText(s);
            }
            buttonequals.Focus();
        }

        private void cbLongInteger_Click(object sender, EventArgs e)
        {
            selectdisplay_LongInteger();
            buttonequals.Focus();
        }

        private void LblLongEntry_Click(object sender, EventArgs e)
        {
            if(LblLongEntry.Text.Length > 0) Clipboard.SetText(LblLongEntry.Text);
            buttonequals.Focus();
        }

        private void LblLongEntry_DoubleClick(object sender, EventArgs e)
        {
            selectdisplay_LongInteger();
            buttonequals.Focus();
        }

        private void cbLongHex_Click(object sender, EventArgs e)
        {
            selectdisplay_LongHex();
            buttonequals.Focus();
        }

        private void lblLongHexEntry_Click(object sender, EventArgs e)
        {
            String s = lblLongHexEntry.Text;
            if (s.Length > 0)
            {
                if (s.StartsWith("0x")) s = s.Substring(2);
                if (s.StartsWith("0X")) s = s.Substring(2);
                Clipboard.SetText(s);
            }
            buttonequals.Focus();
        }

        private void lblLongHexEntry_DoubleClick(object sender, EventArgs e)
        {
            selectdisplay_LongHex();
            buttonequals.Focus();
        }

        private void enterequals()
        {
            operand2 = workingvalue;
            workingvalue = 0;
            entrystring = "";
            displayresult = true;
            switch (operation)
            {
                case '+': result = operand1 + operand2; break;
                case '-': result = operand1 - operand2; break;
                case '*': result = operand1 * operand2; break;
                case '/': result = operand1 / operand2; break;
                case '&': result = (long)operand1 & (long)operand2; break;
                case '|': result = (long)operand1 | (long)operand2; break;
                case '^': result = (long)operand1 ^ (long)operand2; break;
                case '%': result = operand1 % operand2; break;
                case '£': result = (double)((long)operand1 ^ -1); break;
                case 'l': result = Math.Log10(operand1); break;
                case 'n': result = Math.Log(operand1, operand2); break;
                case 'p': result = Math.Pow(operand1, operand2); break;
                case 's': result = Math.Abs(operand1); break;
                case '1': result = operand1 * Math.Pow(10, operand2); break;
                case '!': result = factorial(operand1); break;
                case '<': result = (double)((int)operand1 << (int)operand2); break;
                case '>': result = (double)((int)operand1 >> (int)operand2); break;
                case '\0':
                default: result = operand1; break;
            }
            updateworking(false);
        }

        private void buttonequals_Click(object sender, EventArgs e)
        {
            enterequals();
        }

        private double factorial(double o)
        {
            uint x = (uint)o;
            uint result = 1;
            for (uint i = 1; i <= x; i++)
            {
                result *= i;
            }
            return result;
        }

        private void enterAdd()
        {
            if (displayresult)
            {
                operand1 = result;
                displayresult = false;
                operand2 = 0;
            }
            else
            {
                operand1 = workingvalue;
            }
            operation = '+';
            workingvalue = 0;
            entrystring = "";
            updateworking(true);
            buttonequals.Focus();
        }

        private void buttonadd_Click(object sender, EventArgs e)
        {
            enterAdd();
        }

        private void entersubtract()
        {
            if (displayresult)
            {
                operand1 = result;
                displayresult = false;
                operand2 = 0;
            }
            else
            {
                operand1 = workingvalue;
            }
            operation = '-';
            workingvalue = 0;
            entrystring = "";
            updateworking(true);
            buttonequals.Focus();
        }

        private void buttonsubtract_Click(object sender, EventArgs e)
        {
            entersubtract();
        }

        private void entermultiply()
        {
            if (displayresult)
            {
                operand1 = result;
                displayresult = false;
                operand2 = 0;
            }
            else
            {
                operand1 = workingvalue;
            }
            operation = '*';
            workingvalue = 0;
            entrystring = "";
            updateworking(true);
            buttonequals.Focus();
        }

        private void buttonmultiply_Click(object sender, EventArgs e)
        {
            entermultiply();
        }

        private void enterdivide()
        {
            if (displayresult)
            {
                operand1 = result;
                displayresult = false;
                operand2 = 0;
            }
            else
            {
                operand1 = workingvalue;
            }
            operation = '/';
            workingvalue = 0;
            entrystring = "";
            updateworking(true);
            buttonequals.Focus();
        }

        private void buttondivide_Click(object sender, EventArgs e)
        {
            enterdivide();
        }

        private void enterA()
        {
            entrystring = String.Format("{0}A", entrystring);
            updateworking(false);
        }

        private void buttonA_Click(object sender, EventArgs e)
        {
            enterA();
            buttonequals.Focus();
        }

        private void enterB()
        {
            entrystring = String.Format("{0}B", entrystring);
            updateworking(false);
        }

        private void buttonB_Click(object sender, EventArgs e)
        {
            enterB();
            buttonequals.Focus();
        }

        private void enterC()
        {
            entrystring = String.Format("{0}C", entrystring);
            updateworking(false);
        }

        private void buttonC_Click(object sender, EventArgs e)
        {
            enterC();
            buttonequals.Focus();
        }

        private void enterD()
        {
            entrystring = String.Format("{0}D", entrystring);
            updateworking(false);
        }

        private void buttonD_Click(object sender, EventArgs e)
        {
            enterD();
            buttonequals.Focus();
        }

        private void enterE()
        {
            entrystring = String.Format("{0}E", entrystring);
            updateworking(false);
        }

        private void buttonE_Click(object sender, EventArgs e)
        {
            enterE();
            buttonequals.Focus();
        }

        private void enterF()
        {
            entrystring = String.Format("{0}F", entrystring);
            updateworking(false);
        }

        private void buttonF_Click(object sender, EventArgs e)
        {
            enterF();
            buttonequals.Focus();
        }

        private void buttonPi_MouseClick(object sender, MouseEventArgs e)
        {
            workingvalue = Math.PI;
            updateworking(true);
            buttonequals.Focus();
        }

        private void buttonEuler_Click(object sender, EventArgs e)
        {
            workingvalue = Math.E;
            updateworking(true);
            buttonequals.Focus();
        }

        private void enterXor()
        {
            if (displayresult)
            {
                operand1 = result;
                displayresult = false;
                operand2 = 0;
            }
            else
            {
                operand1 = workingvalue;
            }
            operation = '^';
            workingvalue = 0;
            entrystring = "";
            updateworking(true);
            buttonequals.Focus();
        }

        private void buttonXor_Click(object sender, EventArgs e)
        {
            enterXor();
        }

        private void enterOr()
        {
            if (displayresult)
            {
                operand1 = result;
                displayresult = false;
                operand2 = 0;
            }
            else
            {
                operand1 = workingvalue;
            }
            operation = '|';
            workingvalue = 0;
            entrystring = "";
            updateworking(true);
            buttonequals.Focus();
        }

        private void buttonOr_Click(object sender, EventArgs e)
        {
            enterOr();
        }

        private void enterAnd()
        {
            if (displayresult)
            {
                operand1 = result;
                displayresult = false;
                operand2 = 0;
            }
            else
            {
                operand1 = workingvalue;
            }
            operation = '&';
            workingvalue = 0;
            entrystring = "";
            updateworking(true);
            buttonequals.Focus();
        }

        private void buttonAnd_Click(object sender, EventArgs e)
        {
            enterAnd();
        }

        private void enterMod()
        {
            if (displayresult)
            {
                operand1 = result;
                displayresult = false;
                operand2 = 0;
            }
            else
            {
                operand1 = workingvalue;
            }
            operation = '%';
            workingvalue = 0;
            entrystring = "";
            updateworking(true);
            buttonequals.Focus();
        }

        private void buttonMod_Click(object sender, EventArgs e)
        {
            enterMod();
        }

        private void enterln()
        {
            if (displayresult)
            {
                operand1 = result;
                displayresult = false;
                operand2 = 0;
            }
            else
            {
                operand1 = workingvalue;
            }
            operation = 'n';
            workingvalue = 0;
            entrystring = "";
            updateworking(true);
            buttonequals.Focus();
        }

        private void buttonln_Click(object sender, EventArgs e)
        {
            enterln();
        }

        private void enterlog()
        {
            if (displayresult)
            {
                operand1 = result;
                displayresult = false;
                operand2 = 0;
            }
            else
            {
                operand1 = workingvalue;
            }
            operation = 'l';
            workingvalue = 0;
            entrystring = "";
            updateworking(true);
            buttonequals.Focus();
        }

        private void buttonlog_Click(object sender, EventArgs e)
        {
            enterlog();
        }

        private void calculationcopy()
        {
            if (displayresult) Clipboard.SetText(String.Format("{0}", result));
            buttonequals.Focus();
        }

        private void lblcalculation_Click(object sender, EventArgs e)
        {
            calculationcopy();
        }

        private void enterCE()
        {
            operand1 = 0;
            operand2 = 0;
            workingvalue = 0;
            entrystring = "";
            result = 0;
            operation = '\0';
            displayresult = false;
            updateworking(false);
            buttonequals.Focus();
        }

        private void buttonCE_Click(object sender, EventArgs e)
        {
            enterCE();
        }

        private void enterCLR()
        {
            entrystring = "";
            workingvalue = 0;
            updateworking(false);
            buttonequals.Focus();
        }

        private void buttonCLR_Click(object sender, EventArgs e)
        {
            enterCLR();
        }

        private void enterpower()
        {
            if (displayresult)
            {
                operand1 = result;
                displayresult = false;
                operand2 = 0;
            }
            else
            {
                operand1 = workingvalue;
            }
            operation = 'p';
            workingvalue = 0;
            entrystring = "";
            updateworking(true);
            buttonequals.Focus();

        }
        private void buttonpower_Click(object sender, EventArgs e)
        {
            enterpower();
        }

        private void buttonabs_Click(object sender, EventArgs e)
        {
            if (displayresult)
            {
                operand1 = result;
                displayresult = false;
                operand2 = 0;
            }
            else
            {
                operand1 = workingvalue;
            }
            operation = 's';
            workingvalue = 0;
            entrystring = "";
            updateworking(true);
            buttonequals.Focus();
        }

        private void button10power_Click(object sender, EventArgs e)
        {
            if (displayresult)
            {
                operand1 = result;
                displayresult = false;
                operand2 = 0;
            }
            else
            {
                operand1 = workingvalue;
            }
            operation = '1';
            workingvalue = 0;
            entrystring = "";
            updateworking(true);
            buttonequals.Focus();

        }

        private void enterfactorial()
        {
            if (displayresult)
            {
                operand1 = result;
                displayresult = false;
                operand2 = 0;
            }
            else
            {
                operand1 = workingvalue;
            }
            operation = '!';
            workingvalue = 0;
            entrystring = "";
            updateworking(true);
            buttonequals.Focus();
        }

        private void buttonfactorial_Click(object sender, EventArgs e)
        {
            enterfactorial();
        }

        private void buttonSin_Click(object sender, EventArgs e)
        {

        }

        private void buttonCos_Click(object sender, EventArgs e)
        {

        }

        private void buttonTan_Click(object sender, EventArgs e)
        {

        }

        private void buttonarcsin_Click(object sender, EventArgs e)
        {

        }

        private void buttonarccos_Click(object sender, EventArgs e)
        {

        }

        private void buttonarctan_Click(object sender, EventArgs e)
        {

        }

        private void buttoninv_Click(object sender, EventArgs e)
        {
            if (displayresult)
            {
                operand1 = result;
                displayresult = false;
                operand2 = 0;
            }
            else
            {
                operand1 = workingvalue;
            }
            operation = '£';
            workingvalue = 0;
            entrystring = "";
            updateworking(true);
            buttonequals.Focus();

        }

        private void enterLT()
        {
            if (displayresult)
            {
                operand1 = result;
                displayresult = false;
                operand2 = 0;
            }
            else
            {
                operand1 = workingvalue;
            }
            operation = '<';
            workingvalue = 0;
            entrystring = "";
            updateworking(true);
            buttonequals.Focus();
        }

        private void buttonLT_Click(object sender, EventArgs e)
        {
            enterLT(); 
        }

        private void enterMT()
        {
            if (displayresult)
            {
                operand1 = result;
                displayresult = false;
                operand2 = 0;
            }
            else
            {
                operand1 = workingvalue;
            }
            operation = '>';
            workingvalue = 0;
            entrystring = "";
            updateworking(true);
            buttonequals.Focus();
        }

        private void buttonMT_Click(object sender, EventArgs e)
        {
            enterMT();
        }

        private void keypress(KeyEventArgs e) //Keys keyCode)
        {
            if (Control.ModifierKeys == Keys.Shift)
            {
                switch (e.KeyCode)
                {
                    case Keys.D1:
                        enterfactorial(); break;
                    case Keys.D5:
                        enterMod(); break;
                    case Keys.D6:
                        enterXor(); break;
                    case Keys.D7:
                        enterAnd(); break; 
                    case Keys.D8:
                        entermultiply(); break;
                    case Keys.Oemcomma:
                        enterLT(); break;
                    case Keys.OemPeriod:
                        enterMT(); break;
                    case Keys.Oem5:
                        enterOr(); break;
                    case Keys.Oemplus:
                        enterAdd(); break;

                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    /* case (char)(0x16): paste into workingstring*/
                    case Keys.D0:
                    case Keys.NumPad0:
                        enter0(); break;
                    case Keys.D1:
                    case Keys.NumPad1:
                        enter1(); break;
                    case Keys.D2:
                    case Keys.NumPad2:
                        enter2(); break;
                    case Keys.D3:
                    case Keys.NumPad3:
                        enter3(); break;
                    case Keys.D4:
                    case Keys.NumPad4:
                        enter4(); break;
                    case Keys.D5:
                    case Keys.NumPad5:
                        enter5(); break;
                    case Keys.D6:
                    case Keys.NumPad6:
                        enter6(); break;
                    case Keys.D7:
                    case Keys.NumPad7:
                        enter7(); break;
                    case Keys.D8:
                    case Keys.NumPad8:
                        enter8(); break;
                    case Keys.D9:
                    case Keys.NumPad9:
                        enter9(); break;
                    case Keys.A:
                        if (buttonA.Enabled) enterA(); break;
                    case Keys.B:
                        if (buttonB.Enabled) enterB(); break;
                    case Keys.C:
                        if (buttonC.Enabled) enterC(); break;
                    case Keys.D:
                        if (buttonD.Enabled) enterD(); break;
                    case Keys.E:
                        if (buttonE.Enabled) enterE(); break;
                    case Keys.F:
                        if (buttonF.Enabled) enterF(); break;
                    case Keys.Add:
                        enterAdd(); break;
                    case Keys.Subtract:
                    case Keys.OemMinus:
                        entersubtract(); break;
                    case Keys.Multiply:
                        entermultiply(); break;
                    case Keys.Divide:
                        case Keys.Oem2:
                        enterdivide(); break;
                    case Keys.Oemplus:
                        enterequals(); break;
                    case Keys.Escape:
                        enterCLR(); break;
                    case Keys.OemPeriod: 
                        enterpoint(); break;
                        
                }
            }

        }

        private void buttonequals_KeyDown(object sender, KeyEventArgs e)
        {
            /* we will force focus to equals after every event, so it can catch all button presses and be the default when return is pressed on the keyboard */
            if (e.Control && e.KeyCode == Keys.V)
            {
                entrystring = Clipboard.GetText(); /*special ctrl+v case*/
                updateworking(false);
                return;
            }
            if (e.Control && e.KeyCode == Keys.C)
            {
                /*special ctrl+c case*/
                calculationcopy(); return;
            }
            if (e.KeyCode == Keys.ShiftKey) return;
            keypress(e); //.KeyCode);
        }
        
    }
}
