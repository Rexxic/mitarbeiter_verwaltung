using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
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
            GenerateFields(Employee.GetAttributeNames());
            LoadData();
        }

        void GenerateFields(List<string> fields)
        {
            var k = 0;
            foreach (var field in fields)
            {
                TextBox box = new TextBox();
                box.Location = new System.Drawing.Point(200, 100 + k * 35);
                box.Name = field + ":txt";
                box.Size = new System.Drawing.Size(200, 32);
                if (box.Name.Equals("Id:txt")) {
                    box.Enabled = false;
                }
                this.splitContainer1.Panel1.Controls.Add(box);

                Label label = new Label();
                label.AutoSize = true;
                label.Location = new System.Drawing.Point(50, 103 + k * 35);
                label.Name = field + ":lbl";
                label.Size = new System.Drawing.Size(150, 26);
                label.Text = field;
                this.splitContainer1.Panel1.Controls.Add(label);
                k++;
            }
            Button buttonSave = new Button();
            buttonSave.Location = new System.Drawing.Point(50, 110 + k * 35);
            buttonSave.Name = "save:btn";
            buttonSave.Size = new System.Drawing.Size(150, 40);
            buttonSave.Text = "Speichern";
            buttonSave.UseVisualStyleBackColor = true;
            buttonSave.Click += new EventHandler(SaveButtonClick);
            this.splitContainer1.Panel1.Controls.Add(buttonSave);

            Button buttonDelete = new Button();
            buttonDelete.Location = new System.Drawing.Point(250, 110 + k * 35);
            buttonDelete.Name = "delete:btn";
            buttonDelete.Size = new System.Drawing.Size(150, 40);
            buttonDelete.Text = "Löschen";
            buttonDelete.UseVisualStyleBackColor = true;
            buttonDelete.Click += new EventHandler(DeleteButtonClick);
            this.splitContainer1.Panel1.Controls.Add(buttonDelete);
        }

        private Employee FromForm()
        {
            Employee employee = new Employee();
            foreach (Control c in splitContainer1.Panel1.Controls)
            {
                if (c is TextBox)
                {
                    TextBox textBox = (TextBox)c;
                    string propertyName = textBox.Name.Substring(0, textBox.Name.IndexOf(':'));
                    PropertyInfo property = typeof(Employee).GetProperty(propertyName);
                    Object value;

                    try
                    {
                        if (textBox.Text.Length == 0)
                        {
                            continue;
                        }
                        else if (property.PropertyType is string)
                        {
                            value = textBox.Text;
                        }
                        else
                        {
                            value = Convert.ChangeType(textBox.Text, property.PropertyType);
                        }
                        property.SetValue(employee, value);
                    } catch (Exception ex) {
                        throw new Exception(propertyName + ": " + ex.Message, ex);
                    }
                }
            }
            return employee;
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
                Console.WriteLine(ex.Message);
            }
        }


        private void DeleteButtonClick(object sender, EventArgs eargs)
        {
            try
            {
                Employee employee = FromForm();
                if(employee.Id == 0)
                {
                    throw new Exception("Kein Angestellter ausgewählt!");
                } else
                {
                    employees.RemoveAt(employees.FindIndex(e => e.Id == employee.Id));
                }
                SaveData();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
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
        }

        void LoadData()
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
                if (value < 1)
                    throw new ArgumentException("ID muss mindestens 1 sein.");
                _id = value;
            }
        }

        private string _name;
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

        private string _vorname;
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

        private string _adresse;
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

        private string _telefon;
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

        private string _email;
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

        private DateTime _firmeneintritt;
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

        private DateTime _geburtsdatum;
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
                        throw new ArgumentException($"Fehler beim Konvertieren des Wertes für {property.Name}: {ex.Message}");
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

        public static List<string> GetAttributeNames()
        {
            List<string> names = new List<string>();
            foreach (var property in typeof(Employee).GetProperties())
            {
                names.Add(property.Name);
            }
            return names;
        }
    }
}
