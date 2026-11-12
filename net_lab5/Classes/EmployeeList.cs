using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using net_lab5.Interfaces;

namespace net_lab5.Classes
{
    public class EmployeeList
    {
        private readonly IFileLoader _fileReader;
        private readonly IFIleSaver _fileWriter;
        private readonly string _filePath;

        public ObservableCollection<Employee> Employees { get; }

        private List<Employee> _allEmployees;

        public EmployeeList(IFileLoader fileReader, IFIleSaver fileWriter, string filePath)
        {
            _fileReader = fileReader;
            _fileWriter = fileWriter;
            _filePath = filePath;
            Employees = new ObservableCollection<Employee>();
            _allEmployees = new List<Employee>();
        }

        public void addEmployee(Employee employee)
        {
            Employees.Add(employee);
            _allEmployees.Add(employee);
        }

        public void removeEmployee(Employee employee)
        {
            Employees.Remove(employee);
            _allEmployees.Remove(employee);
        }

        public void FilterBySalary(int minSalary)
        {
            var filtered = _allEmployees.Where(e => e.Salary <= minSalary).ToList();
            UpdateVisibleCollection(filtered);
        }

        public void ResetFilter()
        {
            UpdateVisibleCollection(_allEmployees);
        }

        private void UpdateVisibleCollection(List<Employee> newCollection)
        {
            Employees.Clear();
            foreach (var employee in newCollection)
            {
                Employees.Add(employee);
            }
        }

        public void SortByExp()
        {
            var sorted = Employees.OrderBy(e => e.Experience).ToList();
            UpdateVisibleCollection(sorted);
        }

        public List<string> GetEmployeeNames()
        {
            return _allEmployees.Select(e => e.FullName).ToList();
        }

        public Dictionary<string, List<Employee>> GroupByPosition()
        {
            return _allEmployees
                .GroupBy(e => e.Position)
                .ToDictionary(g => g.Key, g => g.ToList());
        }

        public double GetAverageSalary()
        {
            return _allEmployees.Average(e => e.Salary);
        }

        public int GetTotalSalary()
        {
            return _allEmployees.Sum(e => e.Salary);
        }

        public Employee GetMostExperiencedEmployee()
        {
            return _allEmployees.MaxBy(e => e.Experience);
        }

        public async Task SaveAsync()
        {
            await _fileWriter.SaveAsync(_filePath, _allEmployees);
        }

        public async Task LoadAsync()
        {
            _allEmployees = await _fileReader.LoadAsync<Employee>(_filePath);
            UpdateVisibleCollection(_allEmployees);
        }
    }
}
