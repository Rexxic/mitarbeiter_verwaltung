using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace mitarbeiter_verwaltung
{
    public partial class Form1 : Form
    {
        private const string DATABASE = ".\\database.csv";
        private List<Employee> employees = new List<Employee>();

        public Form1()
        {
            InitializeComponent();
            GenerateFields(typeof(Employee).GetProperties());
            LoadData();
            UpdateList();
        }

        void UpdateList()
        {
            listView1.Items.Clear();
            foreach (Employee e in employees)
            {
                Console.WriteLine(e);
                ListViewItem item = new ListViewItem($"{e.Id} : {e.Vorname} {e.Name}");
                item.Tag = e;
                listView1.Items.Add(item);
            }
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            listView1.Update();
        }

        void GenerateFields(PropertyInfo[] fields)
        {
            var k = 0;
            foreach (PropertyInfo field in fields)
            {
                if (field.PropertyType.IsEnum)
                {
                    ComboBox box = new ComboBox();
                    box.Location = new System.Drawing.Point(200, 100 + k * 35);
                    box.Name = field.Name + ":cb";
                    box.Size = new System.Drawing.Size(200, 32);
                    box.DataSource = Enum.GetValues(field.PropertyType);
                    this.splitContainer1.Panel1.Controls.Add(box);
                }
                else if (field.PropertyType == typeof(DateTime))
                {
                    DateTimePicker box = new DateTimePicker();
                    box.Location = new System.Drawing.Point(200, 100 + k * 35);
                    box.Name = field.Name + ":cb";
                    box.Size = new System.Drawing.Size(200, 32);
                    this.splitContainer1.Panel1.Controls.Add(box);
                }
                else
                {
                    TextBox box = new TextBox();
                    box.Location = new System.Drawing.Point(200, 100 + k * 35);
                    box.Name = field.Name + ":txt";
                    box.Size = new System.Drawing.Size(200, 32);
                    if (box.Name.Equals("Id:txt"))
                    {
                        box.Enabled = false;
                    }
                    this.splitContainer1.Panel1.Controls.Add(box);
                }

                Label label = new Label();
                label.AutoSize = true;
                label.Location = new System.Drawing.Point(50, 103 + k * 35);
                label.Name = field.Name;
                label.Size = new System.Drawing.Size(150, 26);
                label.Text = field.Name;
                this.splitContainer1.Panel1.Controls.Add(label);
                k++;
            }

            Button buttonDelete = new Button();
            buttonDelete.Location = new System.Drawing.Point(50, 110 + k * 35);
            buttonDelete.Name = "delete";
            buttonDelete.Size = new System.Drawing.Size(100, 40);
            buttonDelete.Text = "Löschen";
            buttonDelete.UseVisualStyleBackColor = true;
            buttonDelete.Click += new EventHandler(DeleteButtonClick);
            this.splitContainer1.Panel1.Controls.Add(buttonDelete);

            Button buttonSave = new Button();
            buttonSave.Location = new System.Drawing.Point(175, 110 + k * 35);
            buttonSave.Name = "save";
            buttonSave.Size = new System.Drawing.Size(100, 40);
            buttonSave.Text = "Speichern";
            buttonSave.UseVisualStyleBackColor = true;
            buttonSave.Click += new EventHandler(SaveButtonClick);
            this.splitContainer1.Panel1.Controls.Add(buttonSave);

            Button buttonNew = new Button();
            buttonNew.Location = new System.Drawing.Point(300, 110 + k * 35);
            buttonNew.Name = "new";
            buttonNew.Size = new System.Drawing.Size(100, 40);
            buttonNew.Text = "Neu";
            buttonNew.UseVisualStyleBackColor = true;
            buttonNew.Click += new EventHandler(NewButtonClick);
            this.splitContainer1.Panel1.Controls.Add(buttonNew);
        }

        private Employee FromForm()
        {
            Employee employee = new Employee();
            foreach (Control c in splitContainer1.Panel1.Controls)
            {
                int splitterIndex = c.Name.IndexOf(':');
                if (splitterIndex == -1)
                {
                    continue;
                }
                string propertyName = c.Name.Substring(0, splitterIndex);
                PropertyInfo property = typeof(Employee).GetProperty(propertyName);
                Console.WriteLine($"{c} | {propertyName} | {property}");
                if (c is TextBox)
                {
                    TextBox textBox = (TextBox)c;
                    try
                    {
                        object value;
                        if (property.PropertyType == typeof(string))
                        {
                            value = textBox.Text;
                        }
                        else
                        {
                            if (property.PropertyType == typeof(int) && textBox.Text.Equals(""))
                            {
                                continue;
                            }
                            value = Convert.ChangeType(textBox.Text, property.PropertyType);
                        }
                        property.SetValue(employee, value);
                    }
                    catch (Exception ex)
                    {
                        var message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                        throw new Exception(propertyName + ": " + message, ex);
                    }
                }
                else
                {
                    object o = null;
                    if (c is DateTimePicker)
                    {
                        DateTimePicker dateTimePicker = (DateTimePicker)c;
                        o = dateTimePicker.Value;
                    }
                    else if (c is ComboBox)
                    {
                        ComboBox comboBox = (ComboBox)c;
                        o = comboBox.SelectedItem;
                    }
                    else
                    {
                        throw new Exception($"Unknown type of Control element:{c}");
                    }
                    property.SetValue(employee, o);

                }
            }
            return employee;
        }

        void ToForm(Employee employee)
        {
            foreach (Control c in splitContainer1.Panel1.Controls)
            {
                int splitterIndex = c.Name.IndexOf(':');
                if (splitterIndex == -1)
                {
                    continue;
                }
                string propertyName = c.Name.Substring(0, splitterIndex);
                PropertyInfo property = typeof(Employee).GetProperty(propertyName);
                if(property == null)
                {
                    continue;
                }
                object value = property.GetValue(employee, null);
                if (c is TextBox)
                {
                    TextBox textBox = (TextBox)c;
                    textBox.Text = value.ToString();
                }
                else if (c is DateTimePicker)
                {
                    DateTimePicker datePicker = (DateTimePicker)c;
                    DateTime dateTime = (DateTime)value;
                    if (dateTime > DateTime.MinValue && dateTime < DateTime.MaxValue)
                    {
                        datePicker.Value = (DateTime)value;
                    }
                }
                else if (c is ComboBox)
                {
                    ComboBox comboBox = (ComboBox)c;
                    comboBox.SelectedItem = value;
                }
            }
        }

        private void SaveButtonClick(object sender, EventArgs eargs)
        {
            try
            {
                Employee employee = FromForm();
                Console.WriteLine(employee.ToString());
                if (employee.Id == 0)
                {
                    int i = 1;
                    if (employees.Count > 0)
                        i += employees[employees.Count - 1].Id;
                    employee.Id = i;
                    employees.Add(employee);
                }
                else
                {
                    int index = employees.FindIndex(e => e.Id == employee.Id);
                    employees[index] = employee;
                }
                SaveData();
            }
            catch (Exception ex)
            {
                UserFeedback(ex.Message);
            }
        }


        private void DeleteButtonClick(object sender, EventArgs eargs)
        {
            try
            {
                Employee employee = FromForm();
                if (employee.Id == 0)
                {
                    throw new Exception("Kein Angestellter ausgewählt!");
                }
                else
                {
                    employees.RemoveAt(employees.FindIndex(e => e.Id == employee.Id));
                    ToForm(new Employee());
                }
                SaveData();
            }
            catch (Exception ex)
            {
                UserFeedback(ex.Message);
            }
        }

        private void UserFeedback(string s)
        {
            textBox1.Text = s;
        }

        private void NewButtonClick(object sender, EventArgs eargs)
        {
            ToForm(new Employee());
        }


            void SaveData()
        {
            using (StreamWriter writer = new StreamWriter(DATABASE))
            {
                foreach (Employee e in employees)
                {
                    writer.WriteLine(e.ToString());
                }
            }
            UpdateList();
        }

        void LoadData()
        {
            if (File.Exists(DATABASE))
            {
                using (StreamReader reader = new StreamReader(DATABASE))
                {
                    string line;

                    while ((line = reader.ReadLine()) != null)
                    {
                        Employee employee = new Employee(line);
                        int i = employees.FindIndex(e => e.Id == employee.Id);
                        if (i < 0)
                        {
                            employees.Add(employee);
                        }
                        else
                        {
                            employees[i] = employee;
                        }
                    }
                }
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs eargs)
        {
            if (listView1.SelectedItems.Count == 1)
            {
                Employee selected = (Employee)listView1.SelectedItems[0].Tag;
                ToForm(selected);
            }
        }
    }

    enum Position
    {
        Manager,
        Assistent,
        Entwickler,
        Designer,
    }

    class Employee
    {
        private int _id = 0;
        public int Id
        {
            get => _id;
            set
            {
                if (value < 0)
                    throw new ArgumentException("ID muss mindestens 0 sein.");
                _id = value;
            }
        }

        private string _name = "";
        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Name darf nicht leer sein.");
                _name = value;
            }
        }

        private string _vorname = "";
        public string Vorname
        {
            get => _vorname;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Vorname darf nicht leer sein.");
                _vorname = value;
            }
        }

        private string _adresse = "";
        public string Adresse
        {
            get => _adresse;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Adresse darf nicht leer sein.");
                _adresse = value;
            }
        }

        private string _telefon = "";
        public string Telefon
        {
            get => _telefon;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Telefon darf nicht leer sein.");

                var telefonRegex = new Regex(@"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$");
                if (!telefonRegex.IsMatch(value))
                    throw new ArgumentException("Ungültiges Telefonformat.");

                _telefon = value;
            }
        }

        private string _email = "";
        public string Email
        {
            get => _email;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Email darf nicht leer sein.");

                var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
                if (!emailRegex.IsMatch(value))
                    throw new ArgumentException("Ungültiges Emailformat.");

                _email = value;
            }
        }

        private Position _position;
        public Position Position
        {
            get => _position;
            set { _position = value; }
        }

        private DateTime _firmeneintritt = DateTime.Now;
        public DateTime Firmeneintritt
        {
            get => _firmeneintritt;
            set { _firmeneintritt = value; }
        }

        private float _gehalt;
        public float Gehalt
        {
            get => _gehalt;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Gehalt darf nicht negativ sein.");
                _gehalt = value;
            }
        }

        private DateTime _geburtsdatum = DateTime.Now;
        public DateTime Geburtsdatum
        {
            get => _geburtsdatum;
            set { _geburtsdatum = value; }
        }

        public Employee() { }

        public Employee(string csvData)
        {
            var properties = this.GetType().GetProperties();
            var values = csvData.Split(';');

            if (values.Length != properties.Length)
            {
                throw new ArgumentException("Die Anzahl der Werte stimmt nicht mit der Anzahl der Eigenschaften überein.");
            }

            for (int i = 0; i < properties.Length; i++)
            {
                var property = properties[i];
                if (property.CanWrite)
                {
                    try
                    {
                        var valueType = property.PropertyType;
                        var converter = TypeDescriptor.GetConverter(valueType);
                        var value = converter.ConvertFromString(null, CultureInfo.InvariantCulture, values[i]);
                        property.SetValue(this, value);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Fehler beim Konvertieren des Wertes für {property.Name}: {ex.Message}");
                    }
                }
            }
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            var properties = this.GetType().GetProperties();

            foreach (var property in properties)
            {
                var value = property.GetValue(this);
                if (value != null)
                {
                    stringBuilder.Append(value.ToString());
                }
                stringBuilder.Append(";");
            }

            return stringBuilder.ToString().TrimEnd(';');
        }
    }
}
