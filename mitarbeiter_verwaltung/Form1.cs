using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.IO;
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
            LoadData();
            Employee employee = new Employee()
            {
                Name = "Herbert",
                Vorname = "Landpfand",
                Adresse = "In Herbert sei Schuh",
                Telefon = "07689763784",
                Email = "Herbert@Schuh.pfand"
            };
            string s = employee.ToString();
            Console.WriteLine(s);
            Console.WriteLine(Directory.GetCurrentDirectory());
            SaveEmployee(employee);
        }

        void SaveEmployee(Employee employee)
        {
            if (employee.Id > 0) {
                int index = employees.FindIndex(e  => e.Id == employee.Id);
                employees[index] = employee;
            } else
            {
                int i = 1;
                if (employees.Count > 0) 
                    i += employees[employees.Count - 1].Id;
                employee.Id = i;
                employees.Add(employee);
            }
            SaveData();
        }

        void SaveData()
        {
            using (StreamWriter writer = new StreamWriter(DATABASE))
            {
                foreach (Employee e in  employees)
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
                    } else
                    {
                        employees[i] = employee;
                    }
                }
            }
        }
    }

    enum Position
    {
        CHEF,
        NEDCHEF,
        SKLAVE
    }

    class Employee
    {
        private int _id = 0;
        public int Id
        {
            get => _id;
            set {
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
    }
}
