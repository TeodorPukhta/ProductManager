using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace Kursova1
{
    public class MyDate
    {
        public int day;
        public int month;
        public int year;
        public MyDate()
        {
            this.day = 1;
            this.month = 1;
            this.year = 2000;
        }
        public MyDate(int day, int month, int year)
        {
            this.day = day;
            this.month = month;
            this.year = year;
        }
    }

    public class Product
    {
        private string name;
        private string type;
        private int count;
        private MyDate dateManufacture;
        private double dateExpiration;
        private double price;
        private int Id;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Type
        {
            get { return type; }
            set { type = value; }
        }
        public int Count
        {
            get { return count; }
            set { count = value; }
        }
        public double Price
        {
            get { return price; }
            set { price = value; }
        }
        public double DateExpiration
        {
            get { return dateExpiration; }
            set { dateExpiration = value; }
        }
        public MyDate DateManufacture
        {
            get { return dateManufacture; }
            set { dateManufacture = value; }
        }
        public int ID
        {
            get { return Id; }
            set { Id = value; }
        }

        public Product()
        {
            this.name = "";
            this.type = "";
            this.count = 0;
            this.dateManufacture = new MyDate();
            this.dateExpiration = 0;
            this.price = 0;
        }
        public Product(string name, string type, int count, MyDate dateManufacture,
				double dateExpiration, double price)
        {
            this.name = name;
            this.type = type;
            this.count = count;
            this.dateManufacture = new MyDate(dateManufacture.day, dateManufacture.month,
                dateManufacture.year);
            this.dateExpiration = dateExpiration;
            this.price = price;
        }
        public Product(Product p)
        {
            this.name = p.name;
            this.type = p.type;
            this.count = p.count;
            this.dateManufacture = new MyDate(p.dateManufacture.day, p.dateManufacture.month,
                p.dateManufacture.year);
            this.dateExpiration = p.dateExpiration;
            this.price = p.price;
        }

        // функція для читання даних з файлу
        public static void getProductsFile(TextReader reader, List<Product> p, Form1 form)
        {
            try
            {
                string tmp = "";
                do
                {
                    tmp = reader.ReadLine() + "\r\n";
                    string[] buff = tmp.Split('\t');
                    if (buff.Length == 7)
                        buff = buff.Where(w => w != buff[6]).ToArray();
                    if(buff.Length < 6)
                        throw new Exception("Недостаньо введених даних.");
                    if (buff.Length > 7)
                        throw new Exception("Забагато введених даних.");
                    if (buff[0] == "\r\n")
                        break;
                    checkNamenType(buff[0]);
                    checkNamenType(buff[1]);
                    foreach (string temp in buff)
                        if (temp == "")
                            throw new Exception("Використовуйте лише табуляцію у вхідному файлі (Tab).");
                    int tempCount = 0;
                    if (!int.TryParse(buff[2], out tempCount) || int.Parse(buff[2]) < 0)
                        throw new Exception("Некоректна кількість одиниць.");
                    string[] sepDateM = buff[3].Split('.', ' ');
                    foreach (string temp in sepDateM)
                        checkDateDigit(temp);
                    checkDate(sepDateM);
                    double tempExpire = 0;
                    if (!double.TryParse(buff[4], out tempExpire) || double.Parse(buff[4]) < 0)
                        throw new Exception("Некоректний термін зберігання.");
                    double tempPrice = 0;
                    if (!double.TryParse(buff[5], out tempPrice) || double.Parse(buff[5]) < 0)
                        throw new Exception("Некоректна ціна за одиницю.");
                    p.Add(new Product(buff[0], buff[1], int.Parse(buff[2]), new MyDate(int.Parse
                        (sepDateM[0]), int.Parse(sepDateM[1]), int.Parse(sepDateM[2])),
                        double.Parse(buff[4]), double.Parse(buff[5])));
                    p[p.Count - 1].ID = p.Count;
                } while (reader.Peek() != -1);
                printList(p, form);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка! " + ex.Message);
            }
        }

        // функція для перевірки імені або типу товару
        public static void checkNamenType(string name)
        {
            int iCountDash = 0;
            int iCountZnak = 0;
            foreach (char temp in name)
            {
                if (temp == '-')
                    iCountDash++;
                if (temp == '\'')
                    iCountZnak++;
                if (!Char.IsLetter(temp) && temp != '-' && temp != ' ' && temp != '\'')
                    throw new Exception("Некоректні символи у назві або типі товару.");
            }

            if (iCountDash > 1 || iCountZnak > 1)
                throw new Exception("Некоректні символи у назві або типі товару.");
        }

        // функція для перевірки символів дати зберігання товару
        public static void checkDateDigit(string date)
        {
            foreach (char temp in date)
                if (!Char.IsDigit(temp) && temp != '.')
                    throw new Exception("Некоректні символи у даті.");
        }

        // функція для перевірки дати зберігання товарів
        public static void checkDate(string[] date)
        {
            int day = int.Parse(date[0]);
            int month = int.Parse(date[1]);
            int year = int.Parse(date[2]);
            if (month > 12 || month < 1)
                throw new Exception("Некоректна дата.");
            if (year > 2100 || year < 1950)
                throw new Exception("Некоректна дата.");
            if (day < 1)
                throw new Exception("Некоректна дата.");
            if (year % 4 == 0)
            {
                if (month == 2 && day > 29)
                    throw new Exception("Некоректна дата.");
            }
            else
            {
                if (month == 2 && day > 28)
                    throw new Exception("Некоректна дата.");
            }
            int[] thirtyone = { 1, 3, 5, 6, 7, 10, 12 };
            int[] thirty = { 4, 8, 9, 11 };
            foreach (int tmp in thirtyone)
                if (month == tmp && day > 31)
                    throw new Exception("Некоректна дата.");
            foreach (int tmp in thirty)
                if (month == tmp && day > 30)
                    throw new Exception("Некоректна дата.");
        }

        // функція для виведення списку товарів у таблицю
        public static void printList(List<Product> p, Form1 form)
        {
            for (int k = 0; k < form.grid.RowCount; k++)
                for (int m = 0; m < form.grid.ColumnCount; m++)
                    form.grid.Rows[k].Cells[m].Value = "";
            form.grid.RowCount = p.Count + 1;
            int i = 0;
            foreach (Product m in p)
            {
                form.grid.Rows[i].Cells[0].Value = i + 1 + ".";
                m.ID = i + 1;
                form.grid.Rows[i].Cells[1].Value = m.Name;
                form.grid.Rows[i].Cells[2].Value = m.Type;
                form.grid.Rows[i].Cells[3].Value = m.Count;
                string date1 = "";
                if (m.DateManufacture.day < 10)
                    date1 += "0" + m.DateManufacture.day;
                else
                    date1 += m.DateManufacture.day;
                if (m.DateManufacture.month < 10)
                    date1 += ".0" + m.DateManufacture.month + "." + m.DateManufacture.year;
                else
                    date1 += "." + m.DateManufacture.month + "." + m.DateManufacture.year;
                form.grid.Rows[i].Cells[4].Value = date1;
                date1 = "";
                form.grid.Rows[i].Cells[5].Value = m.DateExpiration;
                form.grid.Rows[i].Cells[6].Value = m.Price;
                i++;
                if (i == p.Count)
                    break;
            }
            form.grid.RowCount = p.Count + 1;
        }

        // функція для визначення типу товару з середнім мінамальним терміном зберігання
        public static string minExpiration(List<Product> p, out double min, Form1 form)
        {
            string[] arr = new string[50];
            int j = 0;
            bool flag = false;
            for (int i = 0; i < p.Count; i++)
            {
                if (j == 0)
                {
                    arr[j] = p[i].Type;
                    j++;
                    continue;
                }
                for (int k = 0; k < j; k++)
                    if (arr[k] == p[i].Type)
                        flag = true;
                if (flag)
                {
                    flag = false;
                    continue;
                }
                arr[j] = p[i].Type;
                j++;
            }
            double[] averValues = new double[j];
            double[] counts = new double[j];
            for (int i = 0; i < p.Count; i++)
            {
                for (int k = 0; k < j; k++)
                {
                    if (p[i].Type == arr[k])
                    {
                        averValues[k] += p[i].DateExpiration;
                        counts[k]++;
                    }
                }
            }
            for (int i = 0; i < j; i++)
                averValues[i] /= counts[i];
            min = averValues[0];
            string minType = arr[0];
            for (int i = 1; i < j; i++)
                if (min > averValues[i])
                {
                    min = averValues[i];
                    minType = arr[i];
                }
            return minType;
        }

        // функція для видалення товару
        public static void deleteProduct(List<Product> p, int index, Form1 form)
        {
            if (index > p.Count || index < 1)
                MessageBox.Show("Помилка! " + "Товару з таким номером не існує.");
            else
            {
               p.Remove(p[index - 1]);
               printList(p,form);
            }
        }

        // функція для додавання товару
        public static void addProduct(List<Product> p, Form1 form)
        {
            try
            {
                Product.checkNamenType(form.textBox1.Text);
                Product.checkNamenType(form.textBox2.Text);
                int tempCount;
                if (!int.TryParse(form.textBox3.Text, out tempCount) || int.Parse(form.textBox3.Text) < 0)
                    throw new Exception("Некоректна кількість одиниць.");
                string theDate1 = form.date1.Value.ToShortDateString();
                string[] sepDateM = theDate1.Split('.', ' ');
                double tempEx;
                if (!double.TryParse(form.textBox5.Text, out tempEx) || double.Parse(form.textBox5.Text) < 0)
                    throw new Exception("Некоректний термін зберігання.");
                double temp;
                if (!double.TryParse(form.textBox4.Text, out temp) || double.Parse(form.textBox4.Text) < 0)
                    throw new Exception("Некоректна ціна за одиницю.");
                p.Add(new Product(form.textBox1.Text, form.textBox2.Text, int.Parse(form.textBox3.Text),
                            new MyDate(int.Parse(sepDateM[0]), int.Parse(sepDateM[1]), int.Parse(sepDateM[2])),
                            double.Parse(form.textBox5.Text), double.Parse(form.textBox4.Text)));
                p[p.Count - 1].ID = p.Count;
                form.panel1.Visible = false;
                form.textBox1.Text = "";
                form.textBox2.Text = "";
                form.textBox3.Text = "";
                form.textBox4.Text = "";
                form.textBox5.Text = "";
                form.grid.RowCount++;
                printList(p, form);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка! " + ex.Message);
            }
        }

        // функція для визначення товарів, термін зберігання яких виходить у заданому місяці
        public static void expireMonth(List<Product> p, Form1 form)
        {
            DateTime date = new DateTime(p[0].DateManufacture.year, p[0].DateManufacture.month,
                    p[0].DateManufacture.day);
            List<Product> prInMonth = new List<Product>(1);
            for (int i = 0; i < p.Count; i++)
            {
                date = new DateTime(p[i].DateManufacture.year, p[i].DateManufacture.month,
                p[i].DateManufacture.day);
                date.AddDays(p[i].DateExpiration);
                if (date.Month == form.dateTimePicker1.Value.Month && date.Year == form.dateTimePicker1.Value.Year)
                    prInMonth.Add(p[i]);
            }
            form.clearGrid();
            printList(prInMonth, form);
            if (prInMonth.Count == 0)
                MessageBox.Show("Немає товару, що задовільняє задану умову!");
            form.показатиПочатковийСписокToolStripMenuItem.Visible = true;
            form.button17.Visible = true;
            form.panel2.Visible = false;
        }

        // функція для сортування типів товарів за сумарною вартістю
        public static void sortSumPrice(List<Product> p, Form1 form)
        {
            try
            {
                if (p.Count == 0)
                    throw new Exception("Список товарів пустий.");
                string[] arr = new string[50];
                int j = 0;
                bool flag = false;
                for (int i = 0; i < p.Count; i++)
                {
                    if (j == 0)
                    {
                        arr[j] = p[i].Type;
                        j++;
                        continue;
                    }
                    for (int k = 0; k < j; k++)
                        if (arr[k] == p[i].Type)
                            flag = true;
                    if (flag)
                    {
                        flag = false;
                        continue;
                    }
                    arr[j] = p[i].Type;
                    j++;
                }
                double[] averValues = new double[j];
                for (int i = 0; i < p.Count; i++)
                {
                    for (int k = 0; k < j; k++)
                        if (p[i].Type == arr[k])
                            averValues[k] += p[i].Price * p[i].Count;
                }
                for (int i = 0; i < averValues.Length - 1; i++)
                    for (int k = 0; k < averValues.Length - 1 - i; k++)
                        if (averValues[k] < averValues[k + 1])
                        {
                            double temp = averValues[k];
                            averValues[k] = averValues[k + 1];
                            averValues[k + 1] = temp;
                            string tmp = arr[k];
                            arr[k] = arr[k + 1];
                            arr[k + 1] = tmp;
                        }
                string str = "";
                for (int i = 0; i < j; i++)
                    str += arr[i] + ": " + averValues[i] + "\n";
                MessageBox.Show(str);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка! " + ex.Message);
            }
        }

        // функція для відображення товарів у заданому діапазоні ціни за одиницю
        public static void showInRange(List<Product> p, Form1 form)
        {
            try
            {
                double first;
                if (!Double.TryParse(form.textBox6.Text, out first))
                    throw new Exception("Некоректний діапазон.");
                double second;
                if (!Double.TryParse(form.textBox7.Text, out second))
                    throw new Exception("Некоректний діапазон.");
                if (first > second)
                    throw new Exception("Некоректний діапазон.");
                if (first < 0 || second <= 0)
                    throw new Exception("Некоректний діапазон.");
                List<Product> pr = new List<Product>(1);
                for (int i = 0; i < p.Count; i++)
                    if (p[i].Price > first && p[i].Price < second)
                        pr.Add(p[i]);
                form.clearGrid();
                Product.printList(pr, form);
                if (pr.Count == 0)
                    MessageBox.Show("Немає товару, що задовільняє задану умову!");
                form.показатиПочатковийСписокToolStripMenuItem.Visible = true;
                form.button17.Visible = true;
                form.textBox6.Text = "";
                form.textBox7.Text = "";
                form.panel3.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка! " + ex.Message);
            }
        }
    }
}