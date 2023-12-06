using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;


namespace sph07generator
{
    public partial class Form1 : Form
    {
        int[] values = new int[1000];
        static int maximumNumberOfParticles = 100000;

        int[] materialid = new int[maximumNumberOfParticles];
        int[] constraintid = new int[maximumNumberOfParticles];
        //double[][] particleCoordinate = new double[maximumNumberOfParticles][3];
        double[,] particleCoordinate = new double[maximumNumberOfParticles, 3];
        string inputFile = "";
        string outputFile = "";
        int numberOfNodes = 0;

        public Form1()
        {
            InitializeComponent();
            //control card 1
            comboAxis.SelectedIndex = 1;
            comboDiscretisation.SelectedIndex = 0;
            comboMaterials.SelectedIndex = 0;
            textBoxMaxParticles.Text = "8000";
            //control card 2
            textBoxTerminationTime.Text = "20.00E+00";
            textBoxTimeScale.Text = "0.000E+00";
            textBoxInitialTimestep.Text = "0.000E+00";
            //control card 3
            textBoxTimeStatePlots.Text = "1.100E+00";
            comboBoxPlot.SelectedIndex = 4;
            textBoxIntervalHystoryPlots.Text = "4.000E-01";
            textBoxNumberOfTimeParticles.Text = "0";
            textBoxTransducers.Text = "0";
            textBoxNumberStepsStatus.Text = "100";
            //control card 4
            radioButton1.Checked = false;
            radioButton2.Checked = true;
            radioButton3.Checked = true;
            radioButton4.Checked = true;
            radioButton5.Checked = true;
            radioButton6.Checked = false;
            radioButton7.Checked = true;
            radioButton8.Checked = false;
            radioButton9.Checked = false;
            radioButton10.Checked = false;
            //control card 5
            radioButton11.Checked = true;
            radioButton12.Checked = false;
            radioButton13.Checked = true;
            radioButton14.Checked = false;
            radioButton15.Checked = false;
            radioButton16.Checked = false;
            radioButton17.Checked = true;
            textBoxLoadCurves.Text = "0";
            //control card 6
            radioButton18.Checked = true;
            textBoxNumberOfNeighbours.Text = "40";
            //materials
            //1
            comboBoxMatType1.SelectedIndex = 0;
            textBoxDensity1.Text = "2.700E+00";
            comboBoxEOStype1.Enabled = false;
            //textBoxQuadratic1.Text = "1.5000E+00";
            //textBoxLinear1.Text = "6.0000E-02";
            textBoxArtificialViscosity1.Text = "0";
            //2
            comboBox2.SelectedIndex = 0;
            textBox29.Text = "2.700E+00";
            comboBox1.Enabled = false;
            //textBox27.Text = "1.5000E+00";
            //textBox26.Text = "6.0000E-02";
            textBox28.Text = "0";
            //3
            //comboBox4.SelectedIndex = 0;
            //textBox50.Text = "2.700E+00";
            //comboBox3.Enabled = false;
            //textBox48.Text = "1.5000E+00";
            //textBox47.Text = "6.0000E-02";
            //textBox49.Text = "0";


            // hardcoded coment this out
            textBoxQuadratic1.Text = "2.0000E+00";
            textBoxLinear1.Text = "0.5000E+00";
            textBoxMaterialMass1.Text = "8.12";
            textBoxInitialSmoothing1.Text = "0.026";
            textBoxMaterialName1.Text = "Elastic";
            tbx_Young1.Text = "1.656E+00";
            tbx_Poisson1.Text = "0.330E+00";
            tbx_Yield1.Text = "0.003E+00";
            tbx_Tangent1.Text = "0.210E+00";
            tbx_Hard1.Text = "0.000E+00";
            tbx_Effective1.Text = "0.700E+00";
            comboBox5.SelectedIndex = 0;
            //comboBox7.SelectedIndex = 0;
            
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "LSDYNA input file (*.k)|*.k";
            openFileDialog1.ShowDialog();
            inputFile = openFileDialog1.FileName;
            outputFile = inputFile;
            outputFile = Path.ChangeExtension(inputFile, ".mcm");
            TextReader tr = new StreamReader(inputFile);
            string test = "";  
            bool sphParticles = false;
            bool sphNodeList = false;
            bool terminationTime = false;
            
            
            test = tr.ReadLine();
            test = tr.ReadLine();
            
            do
            {
                test = tr.ReadLine();
                if (test != null && test.Contains("*CONTROL_TERMINATION"))
                {
                    terminationTime = true;
                }
                if (terminationTime == true)
                {
                    test = tr.ReadLine(); // $#  endtim    endcyc     dtmin    endeng    endmas
                    test = tr.ReadLine();
                    string termination = test.Substring(0, 10);
                    double terminationT = Convert.ToDouble(termination);

                    textBoxTerminationTime.Text = termination;
                    terminationTime = false;
                }
                if (test != null && test.Contains("*ELEMENT_SPH"))
                {
                    sphParticles = true;
                }
            } while (test != null && sphParticles == false);
            //NODE COUNTER
            do
            {
                test = tr.ReadLine();

                if (test != null && test.Contains("*NODE")) // the end of section
                {
                    sphNodeList = true;
                    numberOfNodes--;
                }
                else
                {
                    numberOfNodes++;
                }

            } while (test != null && sphNodeList == false);
            textBoxMaxParticles.Text = (numberOfNodes + 2600).ToString(); ;
            test = tr.ReadLine(); //$#   nid               x               y               z      tc      rc
            string xcoo;
            string ycoo;
            string zcoo;

            for (int i = 1; i <= numberOfNodes; i++)
            {
                test = tr.ReadLine();
                string nnber = test.Substring(0, 8);

                xcoo = test.Substring(8, 16);
                particleCoordinate[i, 0] = Convert.ToDouble(xcoo);
                ycoo = test.Substring(24, 16);
                particleCoordinate[i, 1] = Convert.ToDouble(ycoo);
                zcoo = test.Substring(40, 16);
                particleCoordinate[i, 2] = Convert.ToDouble(zcoo);

            }
            labelInfo.Text = numberOfNodes.ToString();
            tr.Close();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 1; i < maximumNumberOfParticles; i++)
            {
                //default material 1 
                materialid[i] = 1;
                constraintid[i] = comboBox5.SelectedIndex;
                // if there is material 2 
                if (textBox53.Text != "" && textBox53.Text != null && textBox54.Text != "" && textBox54.Text != null)
                {
                    if (Convert.ToInt32(textBox54.Text) <= i && i <= Convert.ToInt32(textBox53.Text))
                    {
                        materialid[i] = 2;
                        constraintid[i] = comboBox6.SelectedIndex;
                    }
                }

                // if there is material 3
                //if (textBox55.Text != "" && textBox55.Text != null && textBox56.Text != "" && textBox56.Text != null)
                //{
                //    if (Convert.ToInt32(textBox56.Text) <= i && i <= Convert.ToInt32(textBox55.Text))
                //    {
                //        materialid[i] = 3;
                //        constraintid[i] = comboBox7.SelectedIndex;
                //    }
                //}
            }
            
            
            
            
            
            
            TextWriter tw = new StreamWriter(outputFile);

            string title = "SPH07 input file generated from LSDYNA K file ";
            string output = String.Format("{0,-78}{1,2}", title, 2);
            tw.WriteLine(output);
            tw.WriteLine("* Comment line");

            tw.WriteLine("*");
            tw.WriteLine("* Control card 1: Problem definition");
            //tw.WriteLine("    2    0         2      5400      8000"); //delete
            output = String.Format("{0,5}{1,5}{2,10}{3,10}{4,10}", comboAxis.SelectedIndex+1, comboDiscretisation.SelectedIndex, comboMaterials.SelectedIndex+1, numberOfNodes, Convert.ToInt32(textBoxMaxParticles.Text.ToString()));
            tw.WriteLine(output);

            tw.WriteLine("*");
            tw.WriteLine("* Control card 2: Time control");
            tw.WriteLine("*");
            //tw.WriteLine(" 20.00E+00 0.000E+00 0.000E+00"); //delete
            output = String.Format("{0,10}{1,10}{2,10}{3,10}", textBoxTerminationTime.Text, textBoxTimeScale.Text, textBoxInitialTimestep.Text, textBoxDRSF.Text);
            tw.WriteLine(output);

            tw.WriteLine("*");
            tw.WriteLine("* Control card 3: Output file control");
            tw.WriteLine("*");
            //tw.WriteLine(" 1.100E+00    4 4.000E-01         0    0       100    0    0"); //delete
            output = String.Format("{0,10}{1,5}{2,10}{3,10}{4,5}{5,10}{6,5}{7,5}", textBoxTimeStatePlots.Text, comboBoxPlot.SelectedIndex, textBoxIntervalHystoryPlots.Text, textBoxNumberOfTimeParticles.Text, textBoxTransducers.Text, textBoxNumberStepsStatus.Text, "0", "0");
            tw.WriteLine(output);

            tw.WriteLine("*");
            tw.WriteLine("* Control card 4: Input and initialization options");
            tw.WriteLine("*");
            //tw.WriteLine("    1    1    0    0"); //delete
            output = String.Format("{0,5}{1,5}{2,5}{3,5}", Convert.ToInt32(radioButton2.Checked), Convert.ToInt32(radioButton4.Checked), Convert.ToInt32(radioButton6.Checked), Convert.ToInt32(radioButton8.Checked));
            tw.WriteLine(output);

            tw.WriteLine("*");
            tw.WriteLine("* Control card 5: Analysis options");
            tw.WriteLine("*");
            //tw.WriteLine("    0    2    0    0"); //delete
            int timestepCalculationOption = 0;
            if (radioButton16.Checked) timestepCalculationOption = 1;
            if (radioButton17.Checked) timestepCalculationOption = 2;
            output = String.Format("{0,5}{1,5}{2,5}{3,5}{4,5}{5,5}{6,5}{7,5}", Convert.ToInt32(radioButton12.Checked), timestepCalculationOption.ToString(), Convert.ToInt32(radioButton14.Checked), "0", textBoxLoadCurves.Text, Convert.ToInt32(checkBox1.Checked),Convert.ToInt32(checkBox2.Checked),Convert.ToInt32(checkBox3.Checked));
            tw.WriteLine(output);

            tw.WriteLine("*");
            tw.WriteLine("* Control card 6: Interpolation options");
            tw.WriteLine("*");
            //tw.WriteLine("    0    1   40"); //delete

            int smoothingLengthVariation = 0;
            if (radioButton19.Checked) smoothingLengthVariation = 1;
            if (radioButton20.Checked) smoothingLengthVariation = 2;
            output = String.Format("{0,5}{1,5}{2,5}", smoothingLengthVariation.ToString(), "1", textBoxNumberOfNeighbours.Text);
            tw.WriteLine(output);

            tw.WriteLine("*");
            tw.WriteLine("* Control card 7: Blank at this time");
            tw.WriteLine("*");
            tw.WriteLine("");
            tw.WriteLine("*");

            //tw.WriteLine("    1    3 2.700E+00    0    00.0000E+00    02.0000E+000.5000E+00"); //delete
            int materialType = 0;
            if (comboBoxMatType1.SelectedIndex == 0)
                materialType = 1;
            else if (comboBoxMatType1.SelectedIndex == 1)
                materialType = 3;
            else if (comboBoxMatType1.SelectedIndex == 2)
                materialType = 4;
            else if (comboBoxMatType1.SelectedIndex == 3)
                materialType = 9;


            int EOSType = 0;

            if (comboBoxEOStype1.SelectedIndex == 0)
                EOSType = 4;
            else if (comboBoxEOStype1.SelectedIndex == 1)
                EOSType = 13;
            else if (comboBoxEOStype1.SelectedIndex == 2)
                EOSType = 28;
            else if (comboBoxEOStype1.SelectedIndex == 3)
                EOSType = 41;

            output = String.Format("{0,5}{1,5}{2,10}{3,5}{4,5}{5,10}{6,5}{7,10}{8,10}", "1", materialType.ToString(), textBoxDensity1.Text, EOSType.ToString(), "0", "0.0000E+00", textBoxArtificialViscosity1.Text, textBoxQuadratic1.Text, textBoxLinear1.Text);
            tw.WriteLine(output);

            //tw.WriteLine("      8.12     0.026"); //delete
            output = String.Format("{0,10}{1,10}", textBoxMaterialMass1.Text, textBoxInitialSmoothing1.Text);
            tw.WriteLine(output);
            
            //tw.WriteLine(textBoxMaterialName1.Text);
            //tw.WriteLine(" 1.656E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00");
            //tw.WriteLine(" 0.330E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00");
            //tw.WriteLine(" 0.003E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00");
            //tw.WriteLine(" 0.210E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00");
            //tw.WriteLine(" 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00");
            //tw.WriteLine(" 0.000E+00 0.700E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00"); ///Effective plastic strain at failure 0.7
                                                                                                              ///
            tw.WriteLine(textBoxMaterialName1.Text);

            if (comboBoxMatType1.SelectedIndex == 0) //1 (Elastic)
            {
                output = String.Format("{0,10} 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00", tbx_Young1.Text);
                tw.WriteLine(output);
                output = String.Format("{0,10} 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00", tbx_Poisson1.Text);
                tw.WriteLine(output);
                tw.WriteLine(" 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00");
                tw.WriteLine(" 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00");
                tw.WriteLine(" 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00");
                tw.WriteLine(" 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00");
                
            }
            else if (comboBoxMatType1.SelectedIndex == 1) //3 (Kinematic/Isotropic Elastic-Plastic)
            {
                output = String.Format("{0,10} 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00", tbx_Young1.Text);
                tw.WriteLine(output);
                output = String.Format("{0,10} 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00", tbx_Poisson1.Text);
                tw.WriteLine(output);
                output = String.Format("{0,10} 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00", tbx_Yield1.Text);
                tw.WriteLine(output);
                output = String.Format("{0,10} 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00", tbx_Tangent1.Text);
                tw.WriteLine(output);
                output = String.Format("{0,10} 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00", tbx_Hard1.Text);
                tw.WriteLine(output);
                output = String.Format(" 0.000E+00{0,10} 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00", tbx_Effective1.Text);
                tw.WriteLine(output);
            
            }
            else if (comboBoxMatType1.SelectedIndex == 3) //9 (Fluid)
            {
                output = String.Format("{0,10}{1,10} 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00", tbx_Young1.Text, tbx_Poisson1.Text);
                tw.WriteLine(output);
                tw.WriteLine(" 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00");
                tw.WriteLine(" 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00");
                tw.WriteLine(" 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00");
                tw.WriteLine(" 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00");
                tw.WriteLine(" 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00");
                output = String.Format("{0,72}", comboBoxEOStype1.Text);
                tw.WriteLine(output);
                if (comboBoxEOStype1.SelectedIndex == 0) // 4(Gruneisen)
                {
                    output = String.Format("{0,10}{1,10}{2,10}{3,10}{4,10}{5,10}{6,10}{7,10}", textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text, textBox6.Text, textBox7.Text, textBox8.Text);
                    tw.WriteLine(output);
 
                }
                else if (comboBoxEOStype1.SelectedIndex == 1) // 13(Perfect Gas)
                {
                    output = String.Format("{0,10}{1,10}{2,10}{3,10}", textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text);
                    tw.WriteLine(output);
                }
 
            }
            tw.WriteLine("*");

            if (comboMaterials.SelectedIndex >= 1)
            {

                tw.WriteLine("    2    3 2.700E+00    0    00.0000E+00    02.0000E+000.5000E+00");
                tw.WriteLine("     8.12      0.026");
                tw.WriteLine("Elastic  ");
                tw.WriteLine(" 1.656E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00");
                tw.WriteLine(" 0.330E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00");
                tw.WriteLine(" 0.003E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00");
                tw.WriteLine(" 0.210E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00");
                tw.WriteLine(" 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00");
                tw.WriteLine(" 0.000E+00 0.700E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00 0.000E+00");
                tw.WriteLine("*");
            }
            tw.WriteLine("*  Nodes");
            tw.WriteLine("*");



            for (int parcounter = 1; parcounter <= numberOfNodes; parcounter++)
            {
                if (comboAxis.SelectedIndex == 0)            //1D
                {
                    output = String.Format("{0,8}{1,5}{2,20}{3,20}{4,7}", parcounter, constraintid[parcounter], particleCoordinate[parcounter, 0], particleCoordinate[parcounter, 1], materialid[parcounter]);
                }
                if (comboAxis.SelectedIndex == 1)            //2D
                {
                    output = String.Format("{0,8}{1,5}{2,20}{3,20}{4,7}", parcounter, constraintid[parcounter], particleCoordinate[parcounter, 0], particleCoordinate[parcounter, 1], materialid[parcounter]);
                }
                if (comboAxis.SelectedIndex == 2)            //3D
                {
                    output = String.Format("{0,8}{1,5}{2,20}{3,20}{4,20}{5,7}", parcounter, constraintid[parcounter], particleCoordinate[parcounter, 0], particleCoordinate[parcounter, 1], particleCoordinate[parcounter, 2], materialid[parcounter]);
                }
                    tw.WriteLine(output);
            }


            tw.WriteLine("*");
            tw.WriteLine("*  Time History Nodes");
            tw.WriteLine("*");
            tw.WriteLine("* 1  ");
            tw.WriteLine("*");
            tw.WriteLine("* Transducers");
            tw.WriteLine("*");
            tw.WriteLine("* 0.000E+00 0.200E+00    1");
            tw.WriteLine("* 0.300E+00-0.200E+00    2");
            tw.WriteLine("*-0.300E+00-0.200E+00    2");
            tw.WriteLine("*");
            tw.WriteLine("* Initial Velocities");
            tw.WriteLine("*");
            tw.WriteLine("       1  1.00E-02 0.000E+00");
            tw.WriteLine("    2700  1.00E-02 0.000E+00");
            tw.WriteLine("    2701 -1.00E-02 0.000E+00");
            tw.WriteLine("    5400 -1.00E-02 0.000E+00");
            tw.WriteLine("*");
            tw.WriteLine("* Contact");
            tw.WriteLine("*");
            tw.WriteLine("   -1    1");
            tw.WriteLine(" 0.02000E+00 4.00000E+00");
            tw.WriteLine(" 0.02000E+00 4.00000E+00");
            //tw.WriteLine("*");
            //tw.WriteLine("*");
            //tw.WriteLine("*");
            //tw.WriteLine("*");
            //tw.WriteLine("*");
            //tw.WriteLine("*");
            //tw.WriteLine("*");
            //tw.WriteLine("*");
            //tw.WriteLine("*");
            //tw.WriteLine("*");
            //tw.WriteLine("*");
            //tw.WriteLine("*");
            //tw.WriteLine("*");
            //tw.WriteLine("*");
            //tw.WriteLine("*");
            //tw.WriteLine("*");
            //tw.WriteLine("*");
            //tw.WriteLine("*");
            //tw.WriteLine("*");
            
            
            
            tw.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            radioButton2.Checked = !radioButton1.Checked;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            radioButton1.Checked = !radioButton2.Checked;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label24_Click(object sender, EventArgs e)
        {

        }

        private void label29_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            groupBox10.Text = comboBoxMatType1.Text;
            if (comboBoxMatType1.SelectedIndex == 3)
            {
                comboBoxEOStype1.Enabled = true;
                comboBoxEOStype1.SelectedIndex = 0;
                comboBoxEOStype1.Text = "Gruneisen";
                groupGruneisen1.Visible = true;
            }
            else
            {
                comboBoxEOStype1.Enabled = false;
                comboBoxEOStype1.Text = "";
                groupGruneisen1.Visible = false;
            }
            if (comboBoxMatType1.SelectedIndex == 1) //elastic-plastic
            {
                lbl_yield1.Visible = true;
                lbl_Tangent1.Visible = true;
                lbl_Hard1.Visible = true;
                lbl_effP1.Visible = true;
                lbl_effPl1.Visible = true;

                tbx_Yield1.Visible = true;
                tbx_Tangent1.Visible = true;
                tbx_Hard1.Visible = true;
                tbx_Effective1.Visible = true;

            }
            else
            {
                lbl_yield1.Visible = false;
                lbl_Tangent1.Visible = false;
                lbl_Hard1.Visible = false;
                lbl_effP1.Visible = false;
                lbl_effPl1.Visible = false;

                tbx_Yield1.Visible = false;
                tbx_Tangent1.Visible = false;
                tbx_Hard1.Visible = false;
                tbx_Effective1.Visible = false;
            }

            if (comboBoxMatType1.SelectedIndex == 2) // granular
            {
                lbl_Young1.Text = "Granular Coefficieint1";
                lbl_poisson1.Text = "Granular Coefficieint2";
            }
            else if (comboBoxMatType1.SelectedIndex == 3) // fluid
            {
                lbl_Young1.Text = "Pressure cutoff";
                lbl_poisson1.Text = "Viscosity coefficient";
            }
            else
            {
                lbl_Young1.Text = "Young's Modulus, E";
                lbl_poisson1.Text = "Poisson's Ratio, v";
            }
        }

        private void comboMaterials_SelectedIndexChanged(object sender, EventArgs e)
        {
            panel1.Visible = true;
            panel2.Visible = true;
            if (comboMaterials.SelectedIndex >= 1) panel1.Visible = false;
            if (comboMaterials.SelectedIndex >= 2) panel2.Visible = false;
        }

        private void label38_Click(object sender, EventArgs e)
        {

        }

        private void label40_Click(object sender, EventArgs e)
        {

        }

        private void comboBoxEOStype1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxEOStype1.SelectedIndex == 0)
            {
                groupGruneisen1.Text = "Gruneisen options";
                label3.Text = "Velocity Curve Intercept";
                label3.Visible = true;
                label36.Text = "First Slope Coefficient";
                label36.Visible = true;
                label4.Text = "Second Slope Coefficient";
                label4.Visible = true;
                label37.Text = "Third Slope Coefficient";
                label37.Visible = true;
                label5.Text = "Gruneisen Coefficient";
                label5.Visible = true;
                label6.Text = "First order volume";
                label6.Visible = true;
                label38.Text = "correction coefficient";
                label38.Visible = true;
                label39.Text = "Initial internal energy";
                label39.Visible = true;
                label40.Text = "initial relative volume";
                label40.Visible = true;
                textBox1.Visible = true;
                textBox2.Visible = true;
                textBox3.Visible = true;
                textBox4.Visible = true;
                textBox5.Visible = true;
                textBox6.Visible = true;
                textBox7.Visible = true;
                textBox8.Visible = true;

            }
            else if (comboBoxEOStype1.SelectedIndex == 1)
            {
                groupGruneisen1.Text = "Perfect Gas options";

                label3.Text = "Gamma";
                label3.Visible = true;
                label36.Text = "R";
                label36.Visible = true;
                label4.Text = "Initial internal energyt";
                label4.Visible = true;
                label37.Text = "Initial relative volume";
                label37.Visible = true;
                label5.Visible = false;
                label6.Visible = false;
                label38.Visible = false;
                label39.Visible = false;
                label40.Visible = false;
                textBox1.Visible = true;
                textBox2.Visible = true;
                textBox3.Visible = true;
                textBox4.Visible = true;
                textBox5.Visible = false;
                textBox6.Visible = false;
                textBox7.Visible = false;
                textBox8.Visible = false;
                
            }
            else if (comboBoxEOStype1.SelectedIndex == 2)
            {
                groupGruneisen1.Text = "Murnaghan Quasi-Incopressible";

                label3.Text = "B";
                label3.Visible = true;
                label36.Text = "Gamma";
                label36.Visible = true;
                label4.Visible = false;
                label37.Visible = false;
                label5.Visible = false;
                label6.Visible = false;
                label38.Visible = false;
                label39.Visible = false;
                label40.Visible = false;
                textBox1.Visible = true;
                textBox2.Visible = true;
                textBox3.Visible = false;
                textBox4.Visible = false;
                textBox5.Visible = false;
                textBox6.Visible = false;
                textBox7.Visible = false;
                textBox8.Visible = false;

            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            groupBox11.Text = comboBox2.Text;
            if (comboBox2.SelectedIndex == 3)
            {
                comboBox1.Enabled = true;
                comboBox1.SelectedIndex = 0;
                comboBox1.Text = "Gruneisen";
                groupBox12.Visible = true;
            }
            else
            {
                comboBox1.Enabled = false;
                comboBox1.Text = "";
                groupBox12.Visible = false;
            }
            if (comboBox2.SelectedIndex == 1) //elastic-plastic
            {
                lbl_yield1.Visible = true;
                lbl_Tangent1.Visible = true;
                lbl_Hard1.Visible = true;
                lbl_effP1.Visible = true;
                lbl_effPl1.Visible = true;

                tbx_Yield1.Visible = true;
                tbx_Tangent1.Visible = true;
                tbx_Hard1.Visible = true;
                tbx_Effective1.Visible = true;

            }
            else
            {
                lbl_yield1.Visible = false;
                lbl_Tangent1.Visible = false;
                lbl_Hard1.Visible = false;
                lbl_effP1.Visible = false;
                lbl_effPl1.Visible = false;

                tbx_Yield1.Visible = false;
                tbx_Tangent1.Visible = false;
                tbx_Hard1.Visible = false;
                tbx_Effective1.Visible = false;
            }

            if (comboBoxMatType1.SelectedIndex == 2) // granular
            {
                lbl_Young1.Text = "Granular Coefficieint1";
                lbl_poisson1.Text = "Granular Coefficieint2";
            }
            else if (comboBoxMatType1.SelectedIndex == 3) // fluid
            {
                lbl_Young1.Text = "Pressure cutoff";
                lbl_poisson1.Text = "Viscosity coefficient";
            }
            else
            {
                lbl_Young1.Text = "Young's Modulus, E";
                lbl_poisson1.Text = "Poisson's Ratio, v";
            }
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            groupBox10.Text = comboBoxMatType1.Text;
            if (comboBoxMatType1.SelectedIndex == 3)
            {
                comboBoxEOStype1.Enabled = true;
                comboBoxEOStype1.SelectedIndex = 0;
                comboBoxEOStype1.Text = "Gruneisen";
                groupGruneisen1.Visible = true;
            }
            else
            {
                comboBoxEOStype1.Enabled = false;
                comboBoxEOStype1.Text = "";
                groupGruneisen1.Visible = false;
            }
            if (comboBoxMatType1.SelectedIndex == 1) //elastic-plastic
            {
                lbl_yield1.Visible = true;
                lbl_Tangent1.Visible = true;
                lbl_Hard1.Visible = true;
                lbl_effP1.Visible = true;
                lbl_effPl1.Visible = true;

                tbx_Yield1.Visible = true;
                tbx_Tangent1.Visible = true;
                tbx_Hard1.Visible = true;
                tbx_Effective1.Visible = true;

            }
            else
            {
                lbl_yield1.Visible = false;
                lbl_Tangent1.Visible = false;
                lbl_Hard1.Visible = false;
                lbl_effP1.Visible = false;
                lbl_effPl1.Visible = false;

                tbx_Yield1.Visible = false;
                tbx_Tangent1.Visible = false;
                tbx_Hard1.Visible = false;
                tbx_Effective1.Visible = false;
            }

            if (comboBoxMatType1.SelectedIndex == 2) // granular
            {
                lbl_Young1.Text = "Granular Coefficieint1";
                lbl_poisson1.Text = "Granular Coefficieint2";
            }
            else if (comboBoxMatType1.SelectedIndex == 3) // fluid
            {
                lbl_Young1.Text = "Pressure cutoff";
                lbl_poisson1.Text = "Viscosity coefficient";
            }
            else
            {
                lbl_Young1.Text = "Young's Modulus, E";
                lbl_poisson1.Text = "Poisson's Ratio, v";
            }
        }
    }
}
