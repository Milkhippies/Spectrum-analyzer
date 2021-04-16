using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Windows.Forms;

namespace TestApp
{
   
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        string WAVLink;

        private void button1_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            var header = new data.WavHeader();

            // Размер заголовка
            var headerSize = Marshal.SizeOf(header);
            var fileStream = new FileStream(WAVLink, FileMode.Open, FileAccess.Read);
            var buffer = new byte[headerSize];


            fileStream.Read(buffer, 0, headerSize);
            var headerPtr = Marshal.AllocHGlobal(headerSize);// Чтобы не считывать каждое значение заголовка по отдельности, воспользуемся выделением unmanaged блока памяти
            Marshal.Copy(buffer, 0, headerPtr, headerSize); // Копируем считанные байты из файла в выделенный блок памяти
            Marshal.PtrToStructure(headerPtr, header);  // Преобразовываем указатель на блок памяти к нашей структуре

            // Посчитаем длительность воспроизведения в секундах
            var durationSeconds = 1.0 * header.Subchunk2Size / (header.BitsPerSample / 8.0) / header.NumChannels / header.SampleRate;
            var durationMinutes = (int)Math.Floor(durationSeconds / 60);
            durationSeconds = durationSeconds - (durationMinutes * 60);

            Int16[] Hwav = new Int16[header.ChunkSize];
            AForge.Math.Complex[] spectr = new AForge.Math.Complex[header.ChunkSize / header.BlockAlign];
            AForge.Math.Complex[] complexData = new AForge.Math.Complex[header.ChunkSize / header.BlockAlign];

            int[,] data; // данные файла [номер канала, значение]

            data = new int[header.NumChannels, header.ChunkSize / header.BlockAlign];
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

            for (int i = 0; i < header.ChunkSize / header.BlockAlign; i++)
            {
                complexData[i] = (AForge.Math.Complex)data[0, i];
            }


            var dir = new AForge.Math.FourierTransform.Direction();
            dir = (AForge.Math.FourierTransform.Direction)1;
            AForge.Math.FourierTransform.DFT(complexData, dir);


            for (int i = 0; i < header.ChunkSize / header.BlockAlign / 2; i++)
            {
                chart2.Series[0].Points.AddXY(i*header.SampleRate/(header.ChunkSize/header.BlockAlign), complexData[i].Magnitude+0.01);
            }

            label7.Text = Convert.ToString("SampleRate: " + header.SampleRate + " Elements: " + header.ChunkSize / header.BlockAlign);

            fileStream.Close();

        }


       

        private void button2_Click(object sender, EventArgs e)
        {

            chart2.ChartAreas[0].CursorX.AutoScroll = true;
            chart2.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            int position = 0;
            int blockSize = 5000;
            int size = blockSize;
            chart2.ChartAreas[0].AxisX.ScaleView.Zoom(position, size);
            chart2.ChartAreas[0].AxisX.ScaleView.SmallScrollSize = blockSize;
            chart2.ChartAreas[0].AxisX.LabelStyle.Angle = 90;
            chart2.ChartAreas[0].AxisX.LabelStyle.Interval = 150;
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
    }

}
