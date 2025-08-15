using System;
using System.Collections.Generic;
using System.Linq;

namespace Assignment3.Q2
{
    // a) Generic repository
    public class Repository<T>
    {
        private readonly List<T> items = new();

        public void Add(T item) => items.Add(item);
        public List<T> GetAll() => new(items);
        public T? GetById(Func<T, bool> predicate) => items.FirstOrDefault(predicate);

        public bool Remove(Func<T, bool> predicate)
        {
            var item = items.FirstOrDefault(predicate);
            if (item != null)
            {
                items.Remove(item);
                return true;
            }
            return false;
        }
    }

    // b) Patient class
    public class Patient
    {
        public int Id;
        public string Name;
        public int Age;
        public string Gender;

        public Patient(int id, string name, int age, string gender)
        {
            Id = id;
            Name = name;
            Age = age;
            Gender = gender;
        }

        public override string ToString() =>
            $"Patient [ID={Id}, Name={Name}, Age={Age}, Gender={Gender}]";
    }

    // c) Prescription class
    public class Prescription
    {
        public int Id;
        public int PatientId;
        public string MedicationName;
        public DateTime DateIssued;

        public Prescription(int id, int patientId, string medicationName, DateTime dateIssued)
        {
            Id = id;
            PatientId = patientId;
            MedicationName = medicationName;
            DateIssued = dateIssued;
        }

        public override string ToString() =>
            $"Prescription [ID={Id}, PatientId={PatientId}, Medication={MedicationName}, DateIssued={DateIssued:d}]";
    }

    // g) HealthSystemApp
    public class HealthSystemApp
    {
        private readonly Repository<Patient> _patientRepo = new();
        private readonly Repository<Prescription> _prescriptionRepo = new();
        private Dictionary<int, List<Prescription>> _prescriptionMap = new();

        public void SeedData()
        {
            _patientRepo.Add(new Patient(1, "John Doe", 30, "Male"));
            _patientRepo.Add(new Patient(2, "Jane Smith", 25, "Female"));
            _patientRepo.Add(new Patient(3, "Samuel Green", 40, "Male"));

            _prescriptionRepo.Add(new Prescription(1, 1, "Amoxicillin", DateTime.Now.AddDays(-10)));
            _prescriptionRepo.Add(new Prescription(2, 1, "Ibuprofen", DateTime.Now.AddDays(-5)));
            _prescriptionRepo.Add(new Prescription(3, 2, "Paracetamol", DateTime.Now.AddDays(-3)));
            _prescriptionRepo.Add(new Prescription(4, 3, "Metformin", DateTime.Now.AddDays(-1)));
            _prescriptionRepo.Add(new Prescription(5, 2, "Vitamin C", DateTime.Now));
        }

        public void BuildPrescriptionMap()
        {
            _prescriptionMap = _prescriptionRepo.GetAll()
                .GroupBy(p => p.PatientId)
                .ToDictionary(g => g.Key, g => g.ToList());
        }

        public void PrintAllPatients()
        {
            Console.WriteLine("\n--- All Patients ---");
            foreach (var patient in _patientRepo.GetAll())
                Console.WriteLine(patient);
        }

        public void PrintPrescriptionsForPatient(int id)
        {
            if (_prescriptionMap.TryGetValue(id, out var prescriptions))
            {
                Console.WriteLine($"\nPrescriptions for Patient ID {id}:");
                foreach (var prescription in prescriptions)
                    Console.WriteLine(prescription);
            }
            else
            {
                Console.WriteLine($"\nNo prescriptions found for Patient ID {id}.");
            }
        }

        public void Run()
        {
            SeedData();
            BuildPrescriptionMap();
            PrintAllPatients();
            PrintPrescriptionsForPatient(2);
        }
    }
}
