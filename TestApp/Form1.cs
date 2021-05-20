using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace TestApp
{
   
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            
            comboBox1.Items.Add("None");
            comboBox1.Items.Add("Rectangular");
            comboBox1.Items.Add("Hann");
            comboBox1.Items.Add("Hamming");
            comboBox1.Items.Add("Blackmann");
            comboBox1.Items.Add("Blackmann-Harris");
            comboBox1.Items.Add("Barlett");
            comboBox1.Items.Add("Natall");
            comboBox1.Items.Add("Gauss 0.1");
            comboBox1.Items.Add("Flat peak");
        }

        string WAVLink;

        public void button1_Click(object sender, EventArgs e)
        {

            var header = new DataStruct.WavHeader();

            // Размер заголовка
            var headerSize = Marshal.SizeOf(header);
            var fileStream = new FileStream(WAVLink, FileMode.Open, FileAccess.Read);
            var buffer = new byte[headerSize];


            fileStream.Read(buffer, 0, headerSize);
            var headerPtr = Marshal.AllocHGlobal(headerSize);// Чтобы не считывать каждое значение заголовка по отдельности, воспользуемся выделением unmanaged блока памяти
            Marshal.Copy(buffer, 0, headerPtr, headerSize); // Копируем считанные байты из файла в выделенный блок памяти
            Marshal.PtrToStructure(headerPtr, header);  // Преобразовываем указатель на блок памяти к нашей структуре

            var dataRange = header.ChunkSize / header.BlockAlign;
            var koef = 1; // костыль, по идее не нужен. используется чтобы сократить массив комплексных чисел на К, изменить масштабы и всю отрисовку

            AForge.Math.Complex[] complexData = new AForge.Math.Complex[dataRange/koef];

            int[,] data; // данные файла [номер канала, значение]

            data = new int[header.NumChannels, dataRange];
            buffer = new byte[header.Subchunk2Size];
            fileStream.Read(buffer, 0, (int)header.Subchunk2Size);

            for (int i = 5; i < header.Subchunk2Size / header.BlockAlign; i++) // по количеству блоков - должно быть по семплам
            {
                switch (header.BlockAlign / header.NumChannels) // определяем битность - длина блока на количество каналов
                {
                    case 1: // 8 бит
                        for (int y = 0; y < header.NumChannels; y++)
                        {
                            data[y, i] = buffer[i];
                        }
                        break;
                    case 2: // 16 бит
                        for (int y = 0; y < header.NumChannels; y++)
                        {
                            data[y, i] = BitConverter.ToInt16(buffer, header.BlockAlign * i);
                        }
                        break;
                    case 3: // 24 бит
                        for (int y = 0; y < header.NumChannels; y++)
                        {
                            data[y, i] = BitConverter.ToInt32(buffer, header.BlockAlign * i);
                        }
                        break;
                    case 4: // 32 бит
                        for (int y = 0; y < header.NumChannels; y++)
                        {
                            data[y, i] = BitConverter.ToInt32(buffer, header.BlockAlign * i);
                        }
                        break;
                }

            }

            // чистим графики
            chart1.Series[0].Points.Clear();
            chart2.ChartAreas[0].AxisY.IsLogarithmic = true;
            chart2.Series[0].Points.Clear();

            // строим график
            for (int i = 0; i < 1000; i++)
            {
                chart1.Series[0].Points.AddXY(i, data[0,i]);
            }

            for (int i = 0; i < dataRange/2 /koef; i++)
            {
                complexData[i] = (AForge.Math.Complex)data[0, i];
            }


            var nSize = 16384;

            var numArr = (header.ChunkSize / header.BlockAlign) / nSize;

            AForge.Math.Complex[,] newComplex = new AForge.Math.Complex[numArr, nSize];
            AForge.Math.Complex[] tempComplex = new AForge.Math.Complex[nSize];

            List<AForge.Math.Complex> finalComplex = new List<AForge.Math.Complex>();

            for (int i = 0; i < numArr; i++)
            {
                for (int j = 0; j < nSize; j++) {
                    newComplex[i, j] = complexData[j + i * nSize]; // тут записываем одномерный массив в двумерный
                }
            }

            for (int i = 0; i < numArr; i++) {

                switch (comboBox1.SelectedItem)
                {
                    case "None":

                        for (int k = 0; k < nSize; k++)
                        {
                            tempComplex[k] = newComplex[i, k]; // тут по строкам забираем данные чтобы потом их в fft
                        }; 
                        break;

                    case "Hann":

                        for (int k = 0; k < nSize; k++)
                        {
                            tempComplex[k] = newComplex[i, k] * (0.5 - 0.5 * Math.Cos((2 * Math.PI * k) / (nSize - 1))); 
                        }; 
                        break;

                    case "Rectangular":
                        for (int k = 0; k < nSize; k++)
                        {
                            tempComplex[k] = newComplex[i, k] * 1; // тут по строкам забираем данные чтобы потом их в fft и попутно умножаем на окно
                        }; 
                        break;

                    case "Hamming":
                        for (int k = 0; k < nSize; k++)
                        {
                            tempComplex[k] = newComplex[i, k] * (0.54 - 0.46 * Math.Cos((2 * Math.PI * k) / (nSize - 1))); 
                        }; 
                        break;

                    case "Blackmann":
                        for (int k = 0; k < nSize; k++)
                        {
                            tempComplex[k] = newComplex[i, k] * (0.42 - 0.5 * Math.Cos((2 * Math.PI * k) / (nSize - 1)) + 0.08 * Math.Cos((4 * Math.PI * k) / (nSize - 1)));
                        }
                        break;

                    case "Barlett":
                        for (int k = 0; k < nSize; k++)
                        {
                            tempComplex[k] = newComplex[i, k] * (nSize - 2 * Math.Abs(k - (nSize/2)))/ nSize;
                        }
                        break;

                    case "Blackmann-Harris":
                        for (int k = 0; k < nSize; k++)
                        {
                            tempComplex[k] = newComplex[i, k] * (0.35875 - 0.48829 * Math.Cos((2 * Math.PI * k) / (nSize - 1)) + 0.14128 * Math.Cos((4 * Math.PI * k) / (nSize - 1)) - 0.01168 * Math.Cos((6 * Math.PI * k) / (nSize - 1)));
                        }
                        break;

                    case "Natall":
                        for (int k = 0; k < nSize; k++)
                        {
                            tempComplex[k] = newComplex[i, k] * (0.355768 - 0.487396 * Math.Cos((2 * Math.PI * k) / (nSize - 1)) + 0.144232 * Math.Cos((4 * Math.PI * k) / (nSize - 1)) - 0.012604 * Math.Cos((6 * Math.PI * k) / (nSize - 1)));
                        }
                        break;

                    case "Gauss 0.1":
                        for (int k = 0; k < nSize; k++)
                        {
                            tempComplex[k] = newComplex[i, k] * Math.Exp(-(2 * Math.Pow(k-((nSize-1)/2),2))/(Math.Pow(nSize*0.1,2)));
                        }
                        break;

                    case "Flat peak":
                        for (int k = 0; k < nSize; k++)
                        {
                            tempComplex[k] = newComplex[i, k] * (1 - 1.93 * Math.Cos((2 * Math.PI * k) / (nSize - 1)) + 1.29 * Math.Cos((4 * Math.PI * k) / (nSize - 1)) - 0.388 * Math.Cos((6 * Math.PI * k) / (nSize - 1)) + 0.032 * Math.Cos((8 * Math.PI * k) / (nSize - 1)));
                        }
                        break;

                }

                AForge.Math.FourierTransform.FFT(tempComplex, AForge.Math.FourierTransform.Direction.Forward); // fft

                for (int j = 0; j < nSize; j++){
                    finalComplex.Add(tempComplex[j]); // добавляем результат fft в одномерный финальный массив
                }

            }


            for (int i = 0; i < finalComplex.Count/(2*numArr); i++)
            {
               chart2.Series[0].Points.AddXY(i * header.SampleRate / nSize, finalComplex[i].Magnitude);
            }

            label7.Text = Convert.ToString("SampleRate: " + header.SampleRate + " Elements: " + header.ChunkSize / header.BlockAlign + " numArr: " + numArr + " window: " + comboBox1.SelectedItem);

            fileStream.Close();

        }


        private void button2_Click(object sender, EventArgs e)
        {

            chart2.ChartAreas[0].CursorX.AutoScroll = true;
            chart2.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            int position = 0;
            int blockSize = 500; // для больших 100 для маленьких 5000
            int size = blockSize;
            chart2.ChartAreas[0].AxisX.ScaleView.Zoom(position, size);
            chart2.ChartAreas[0].AxisX.ScaleView.SmallScrollSize = blockSize;
            chart2.ChartAreas[0].AxisX.LabelStyle.Angle = 90;
            chart2.ChartAreas[0].AxisX.LabelStyle.Interval = 50; // для больших 50, для маленьник 150
        }


        public void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "WAV (*.wav)|*.wav";


            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string selectedFileName = openFileDialog1.FileName;
                WAVLink = openFileDialog1.FileName;
            }
            label7.Text = WAVLink;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }

}
