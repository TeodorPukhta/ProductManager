using System;
using System.Collections.Generic;
using Microsoft.VisualBasic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing;

namespace Kursova1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            // оформлення таблиці
            grid.ColumnCount = 7;
            grid.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grid.Columns[0].Width = 30;
            grid.Columns[0].HeaderText = "№ п/п";
            grid.Columns[1].Width = 195;
            grid.Columns[1].HeaderText = "Назва";
            grid.Columns[2].Width = 150;
            grid.Columns[2].HeaderText = "Тип";
            grid.Columns[3].Width = 80;
            grid.Columns[3].HeaderText = "Кількість одиниць";
            grid.Columns[4].Width = 107;
            grid.Columns[4].HeaderText = "Дата виготовлення";
            grid.Columns[5].Width = 125;
            grid.Columns[5].HeaderText = "Термін зберігання (у добах)";
            grid.Columns[6].Width = 82;
            grid.Columns[6].HeaderText = "Ціна за одиницю";
            for(int i = 0; i < grid.ColumnCount; i++)
                grid.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            grid.BackgroundColor = Color.WhiteSmoke;
            Image myimage = new Bitmap(@"C:\Users\Roman Kuk\Documents\Visual Studio 2015\Projects\Kursova1\Kursova1\products.jpg");
            this.BackgroundImage = myimage;
            try {
                if (button17.Visible)
                    throw new Exception("Поверніться на початковий список.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка! " + ex.Message);
            }

        }
        List<Product> productList = new List<Product>(1); // створення головного списку товарів

        // функція для відкриття файлу товарів 
        private void відкритиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    int k = productList.Count;
                    for (int i = k - 1; i >= 0; i--)
                        productList.Remove(productList[i]);
                    clearGrid();
                    Encoding enc = Encoding.GetEncoding("windows-1251");
                    TextReader reader = new StreamReader(openFileDialog1.FileName, enc);
                    /*if (reader == null)
                        throw new Exception("Файл не можливо відкрити.");
                    string exten = Path.GetExtension(openFileDialog1.FileName);
                    if (exten != ".txt")
                        throw new Exception("Файл не є текстовим.");*/
                    if (new FileInfo(openFileDialog1.FileName).Length == 0)
                        throw new Exception("Файл є пустим.");
                    Product.getProductsFile(reader, productList, this);
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка! " + ex.Message);
            }
        }

        // функція для очищення таблиці
        public void clearGrid()
        {
            for (int i = 0; i < grid.RowCount; i++)
                for (int j = 0; j < grid.ColumnCount; j++)
                    grid.Rows[i].Cells[j].Value = "";
        }

        // функція для видалення товару із списку за введеним індексом
        private void видалитиТоварToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (productList.Count == 0)
                    throw new Exception("Список товарів пустий.");
                if (button17.Visible)
                    throw new Exception("Поверніться на початковий список.");
                int countProduct = productList.Count;
                string prompt = Interaction.InputBox("Введіть номер товару для видалення:", "Видалити товар", "", -1, -1);
                int index;
                if (int.TryParse(prompt, out index))
                {
                    Product.deleteProduct(productList, index, this);
                }
                else if (prompt == "")
                    return;
                else
                    throw new Exception("Некоректний номер товару.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка! " + ex.Message);
            }
        }

        // функція для ручного додавання товару
        private void button1_Click_1(object sender, EventArgs e)
        {
            Product.addProduct(productList, this);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
        }

        private void показатиТипТоваруЗНайменшимСереднімТерміномЗберіганняToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (productList.Count == 0)
                    throw new Exception("Список товарів пустий.");
                if (button17.Visible)
                    throw new Exception("Поверніться на початковий список.");
                double minType = 0;
                string result = Product.minExpiration(productList, out minType, this);
                MessageBox.Show(result + " - тип із найменшим терміном зберігання (" + minType + ").");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка! " + ex.Message);
            }
        }

        private void показатиТовариТермінПридатностіЯкихВиходитьУЗаданомуМісяціToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (productList.Count == 0)
                    throw new Exception("Список товарів пустий.");
                if (button17.Visible)
                    throw new Exception("Поверніться на початковий список.");
                panel2.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка! " + ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (button17.Visible)
                    throw new Exception("Поверніться на початковий список.");
                panel2.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка! " + ex.Message);
            }
        }

        // функція для визначення товарів, термін зберігання яких виходить у заданому місяці
        private void button3_Click(object sender, EventArgs e)
        {
            Product.expireMonth(productList, this);
        }

        //  функція для відображення початкового списку товарів
        private void показатиПочатковийСписокToolStripMenuItem_Click(object sender, EventArgs e)
        {
            показатиПочатковийСписокToolStripMenuItem.Visible = false;
            button17.Visible = false;
            clearGrid();
            Product.printList(productList, this);
        }

        // функція для відображення типів товарів за сумарною вартістю
        private void відобразитиСумарнуВартістьТоварівВідповідноДоТипуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Product.sumPrice(productList, this);
        }

        // функція для сортування типів товарів за сумарною вартістю
        private void відсортуватиТипиТоварівПоСумарнійВартостіToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Product.sortSumPrice(productList, this);
        }

        private void показатиСписокТоварівУДіапазоніToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (productList.Count == 0)
                    throw new Exception("Список товарів пустий.");
                if (button17.Visible)
                    throw new Exception("Поверніться на початковий список.");
                panel3.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка! " + ex.Message);
            }
        }

        // функція для відображення товарів у заданому діапазоні ціни за одиницю
        private void button5_Click(object sender, EventArgs e)
        {
            Product.showInRange(productList, this);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                if (button17.Visible)
                    throw new Exception("Поверніться на початковий список.");
                panel3.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка! " + ex.Message);
            }
        }

        // функція для збереження списку із таблиці у файл
        private void зберегтиЯкToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (button17.Visible)
                    throw new Exception("Поверніться на початковий список.");
                if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                    return;
                string filename = saveFileDialog1.FileName;
                string[] lines = new string[grid.RowCount];
                for (int i = 0; i < grid.RowCount; i++)
                {
                    if (grid.Rows[i].Cells[0].Value.ToString() != "")
                        for (int j = 1; j < grid.ColumnCount; j++)
                            lines[i] += grid.Rows[i].Cells[j].Value + "\t";
                }
                Encoding enc = Encoding.GetEncoding("windows-1251");
                System.IO.File.WriteAllLines(filename, lines, enc);
                MessageBox.Show("Файл збережений");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка! " + ex.Message);
            }
        }

        private void вийтиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ввестиВручнуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (button17.Visible)
                    throw new Exception("Поверніться на початковий список.");
                panel1.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка! " + ex.Message);
            }
        }

        // функція для додавання товарів у список із файлу
        private void додатиЗФайлуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try {
                if (button17.Visible)
                    throw new Exception("Поверніться на початковий список.");
                if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Encoding enc = Encoding.GetEncoding("windows-1251");
                    TextReader reader = new StreamReader(openFileDialog1.FileName, enc);
                    if (reader == null)
                        throw new Exception("Файл не можливо відкрити.");
                    string exten = Path.GetExtension(openFileDialog1.FileName);
                    if (exten != ".txt")
                        throw new Exception("Файл не є текстовим.");
                    if (new FileInfo(openFileDialog1.FileName).Length == 0)
                        throw new Exception("Файл є пустим.");
                    Product.getProductsFile(reader, productList, this);
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка! " + ex.Message);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                if (button17.Visible)
                    throw new Exception("Поверніться на початковий список.");
                if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    int k = productList.Count;
                    for (int i = k - 1; i >= 0; i--)
                        productList.Remove(productList[i]);
                    clearGrid();
                    Encoding enc = Encoding.GetEncoding("windows-1251");
                    TextReader reader = new StreamReader(openFileDialog1.FileName, enc);
                    if (reader == null)
                        throw new Exception("Файл не можливо відкрити.");
                    string exten = Path.GetExtension(openFileDialog1.FileName);
                    if (exten != ".txt")
                        throw new Exception("Файл не є текстовим.");
                    if (new FileInfo(openFileDialog1.FileName).Length == 0)
                        throw new Exception("Файл є пустим.");
                    Product.getProductsFile(reader, productList, this);
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка! " + ex.Message);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                if (button17.Visible)
                    throw new Exception("Поверніться на початковий список.");
                if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                    return;
                string filename = saveFileDialog1.FileName;
                string[] lines = new string[grid.RowCount];
                for (int i = 0; i < productList.Count; i++)
                {
                    if (grid.Rows[i].Cells[0].Value.ToString() != "")
                        for (int j = 1; j < grid.ColumnCount; j++)
                            lines[i] += grid.Rows[i].Cells[j].Value + "\t";
                }
                Encoding enc = Encoding.GetEncoding("windows-1251");
                System.IO.File.WriteAllLines(filename, lines, enc);
                MessageBox.Show("Файл збережений");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка! " + ex.Message);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                if (button17.Visible)
                    throw new Exception("Поверніться на початковий список.");
                if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Encoding enc = Encoding.GetEncoding("windows-1251");
                    TextReader reader = new StreamReader(openFileDialog1.FileName, enc);
                    if (reader == null)
                        throw new Exception("Файл не можливо відкрити.");
                    string exten = Path.GetExtension(openFileDialog1.FileName);
                    if (exten != ".txt")
                        throw new Exception("Файл не є текстовим.");
                    if (new FileInfo(openFileDialog1.FileName).Length == 0)
                        throw new Exception("Файл є пустим.");
                    Product.getProductsFile(reader, productList, this);
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка! " + ex.Message);
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                if (button17.Visible)
                    throw new Exception("Поверніться на початковий список.");
                panel1.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка! " + ex.Message);
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                if (productList.Count == 0)
                    throw new Exception("Список товарів пустий.");
                if (button17.Visible)
                    throw new Exception("Поверніться на початковий список.");
                int countProduct = productList.Count;
                string prompt = Interaction.InputBox("Введіть номер товару для видалення:", "Видалити товар", "", -1, -1);
                int index;
                if (int.TryParse(prompt, out index))
                {
                    Product.deleteProduct(productList, index, this);
                }
                else if (prompt == "")
                    return;
                else
                    throw new Exception("Некоректний номер товару.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка! " + ex.Message);
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            try
            {
                if (productList.Count == 0)
                    throw new Exception("Список товарів пустий.");
                if (button17.Visible)
                    throw new Exception("Поверніться на початковий список.");
                double minType = 0;
                string result = Product.minExpiration(productList, out minType, this);
                MessageBox.Show(result + " - тип із найменшим терміном зберігання (" + minType + ").");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка! " + ex.Message);
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            try
            {
                if (productList.Count == 0)
                    throw new Exception("Список товарів пустий.");
                if (button17.Visible)
                    throw new Exception("Поверніться на початковий список.");
                panel2.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка! " + ex.Message);
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            try
            {
                if (productList.Count == 0)
                    throw new Exception("Список товарів пустий.");
                if (button17.Visible)
                    throw new Exception("Поверніться на початковий список.");
                Product.sortSumPrice(productList, this);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка! " + ex.Message);
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            try
            {
                if (productList.Count == 0)
                    throw new Exception("Список товарів пустий.");
                if (button17.Visible)
                    throw new Exception("Поверніться на початковий список.");
                panel3.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка! " + ex.Message);
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            button17.Visible = false;
            показатиПочатковийСписокToolStripMenuItem.Visible = false;
            clearGrid();
            Product.printList(productList, this);
        }

        private void інToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
        }

        private void проПрограмуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.Show();
        }
    }
}
