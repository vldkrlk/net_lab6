using Microsoft.VisualStudio.TestTools.UnitTesting;
using net_lab5.Classes;

namespace TestLab5;

[TestClass]
public sealed class EmployeeListTests
{
    private const string FilePath = "employees.json";
    private EmployeeList employeeList;

    [TestInitialize]
    public void Setup()
    {
        employeeList = new EmployeeList(
            new JSONFileReader(),
            new JSONFileWriter(),
            FilePath);

        employeeList.Employees.Add(new Employee("Alice", "Developer", 3000, 4));
        employeeList.Employees.Add(new Employee("Bob", "Tester", 2500, 2));
        employeeList.Employees.Add(new Employee("Charlie", "Developer", 4000, 6));
        employeeList.Employees.Add(new Employee("David", "Manager", 5000, 10));
    }

    [TestMethod]
    public void GetEmployeeNames_ShouldReturnNames()
    {
        var result = employeeList.GetEmployeeNames();

        CollectionAssert.AreEqual(
            new List<string> { "Alice", "Bob", "Charlie", "David" },
            result
        );
    }

    [TestMethod]
    public void GroupByPosition_ShouldGroupEmployees()
    {
        var result = employeeList.GroupByPosition();

        Assert.AreEqual(3, result.Count);
        Assert.AreEqual(2, result["Developer"].Count);

        CollectionAssert.AreEqual(
            new List<string> { "Alice", "Charlie" },
            result["Developer"]
        );
    }

    [TestMethod]
    public void GetAverageSalary_ShouldReturnAverage()
    {
        var result = employeeList.GetAverageSalary();

        Assert.AreEqual(3625.0, result);
    }

    [TestMethod]
    public void GetTotalSalary_ShouldReturnTotal()
    {
        var result = employeeList.GetTotalSalary();

        Assert.AreEqual(14500, result);
    }

    [TestMethod]
    public void GetMostExperiencedEmployee_ShouldReturnEmployee()
    {
        var result = employeeList.GetMostExperiencedEmployee();

        Assert.IsNotNull(result);
        Assert.AreEqual("David", result.FullName);
        Assert.AreEqual(10, result.Experience);
    }

    [TestMethod]
    public void AddEmployee_ShouldAddEmployeeToCollection()
    {
        Employee employee = CreateEmployee("Alice", 3000, 4);
        EmployeeList employeeList = CreateEmployeeList();

        employeeList.addEmployee(employee);

        Assert.AreEqual(1, employeeList.Employees.Count);
        Assert.AreSame(employee, employeeList.Employees[0]);
    }

    [TestMethod]
    public void RemoveEmployee_ShouldRemoveEmployeeFromCollection()
    {
        Employee employee = CreateEmployee("Alice", 3000, 4);
        EmployeeList employeeList = CreateEmployeeList();

        employeeList.addEmployee(employee);
        employeeList.removeEmployee(employee);

        Assert.AreEqual(0, employeeList.Employees.Count);
    }

    [TestMethod]
    public void FilterBySalary_ShouldShowEmployeesWithinSalaryLimit()
    {
        Employee lowSalaryEmployee = CreateEmployee("Alice", 3000, 4);
        Employee highSalaryEmployee = CreateEmployee("Bob", 5000, 2);
        EmployeeList employeeList = CreateEmployeeList();

        employeeList.addEmployee(lowSalaryEmployee);
        employeeList.addEmployee(highSalaryEmployee);

        employeeList.FilterBySalary(3000);

        Assert.AreEqual(1, employeeList.Employees.Count);
        Assert.AreSame(lowSalaryEmployee, employeeList.Employees[0]);
    }

    [TestMethod]
    public void ResetFilter_ShouldRestoreAllEmployees()
    {
        Employee lowSalaryEmployee = CreateEmployee("Alice", 3000, 4);
        Employee highSalaryEmployee = CreateEmployee("Bob", 5000, 2);
        EmployeeList employeeList = CreateEmployeeList();

        employeeList.addEmployee(lowSalaryEmployee);
        employeeList.addEmployee(highSalaryEmployee);
        employeeList.FilterBySalary(3000);

        employeeList.ResetFilter();

        CollectionAssert.AreEqual(
            new[] { lowSalaryEmployee, highSalaryEmployee },
            employeeList.Employees.ToArray());
    }

    [TestMethod]
    public void SortByExp_ShouldSortVisibleEmployeesByExperience()
    {
        Employee experiencedEmployee = CreateEmployee("Alice", 3000, 10);
        Employee inexperiencedEmployee = CreateEmployee("Bob", 5000, 2);
        Employee middleExperienceEmployee = CreateEmployee("Charlie", 4000, 5);
        EmployeeList employeeList = CreateEmployeeList();

        employeeList.addEmployee(experiencedEmployee);
        employeeList.addEmployee(inexperiencedEmployee);
        employeeList.addEmployee(middleExperienceEmployee);

        employeeList.SortByExp();

        CollectionAssert.AreEqual(
            new[]
            {
                inexperiencedEmployee,
                middleExperienceEmployee,
                experiencedEmployee
            },
            employeeList.Employees.ToArray());
    }

    private static EmployeeList CreateEmployeeList()
    {
        return new EmployeeList(
            new JSONFileReader(),
            new JSONFileWriter(),
            FilePath);
    }

    private static Employee CreateEmployee(
        string fullName,
        int salary,
        int experience)
    {
        return new Employee(fullName, "Developer", salary, experience);
    }

}