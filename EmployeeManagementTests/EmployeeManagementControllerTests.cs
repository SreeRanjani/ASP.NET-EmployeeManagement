using System.Collections.Generic;
using EmployeeManagement.Controllers;
using EmployeeManagement.DTOs.Employee;
using EmployeeManagement.Models;
using EmployeeManagement.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace EmployeeManagementTests
{
    public class EmployeeManagementControllerTests
    {
        #region Read
        [Fact]
        public async void GetAllEmployees_CallGet_Success_ReturnAllEmployee()
        {
            //Arrange
            var expectedResult = new ServiceResponse<List<GetEmployeeDTO>>()
            {
                Data = GetSampleData()
            };
            var mockEmpService = new Mock<IEmployeeManagerService>();
            mockEmpService.Setup(emp => emp.GetAllEmployees()).ReturnsAsync(expectedResult);
            var EmpController = new EmployeeManagerController(mockEmpService.Object);
            //Act
            var actualResult = await EmpController.GetAllEmployeesAsync();
            //Assert
            (actualResult.Result as OkObjectResult).Value.
            Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void GetSingleEmployee_CallGet_Success_ReturnSelEmployee()
        {
            //Arrange
            var expectedResult = new ServiceResponse<GetEmployeeDTO>()
            {
                Data = GetSampleData()[0]
            };
            var mockEmpService = new Mock<IEmployeeManagerService>();
            mockEmpService.Setup(emp => emp.GetEmployeeById(0)).ReturnsAsync(expectedResult);
            var EmpController = new EmployeeManagerController(mockEmpService.Object);
            //Act
            var actualResult = await EmpController.GetSingleEmployeeAsync(0);
            //Assert
            (actualResult.Result as OkObjectResult).Value.
            Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void GetSingleEmployee_CallGet_Failure_ReturnDiffEmployee()
        {
            //Arrange
            var expectedResult = new ServiceResponse<GetEmployeeDTO>()
            {
                Data = GetSampleData()[0]
            };
            var mockResult = new ServiceResponse<GetEmployeeDTO>()
            {
                Data = GetSampleData()[1]
            };
            var mockEmpService = new Mock<IEmployeeManagerService>();
            mockEmpService.Setup(emp => emp.GetEmployeeById(0)).ReturnsAsync(mockResult);
            var EmpController = new EmployeeManagerController(mockEmpService.Object);
            //Act
            var actualResult = await EmpController.GetSingleEmployeeAsync(0);
            //Assert
            (actualResult.Result as OkObjectResult).Value.
            Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void GetSingleEmployee_CallGet_Success_ReturnNotFo()
        {
            //Arrange
            var expectedResult = new ServiceResponse<GetEmployeeDTO>()
            {
                Data = null
            };

            var mockEmpService = new Mock<IEmployeeManagerService>();
            mockEmpService.Setup(emp => emp.GetEmployeeById(0)).ReturnsAsync(expectedResult);
            var EmpController = new EmployeeManagerController(mockEmpService.Object);
            //Act
            var actualResult = await EmpController.GetSingleEmployeeAsync(0);
            //Assert
            (actualResult.Result as NotFoundObjectResult).Value.
            Should().BeEquivalentTo(expectedResult);
        }
        #endregion

        #region Create
        [Fact]
        public async void AddEmployee_CallPost_Success_ReturnAllEmployee()
        {
            //Arrange
            var newSampleEmp = new AddEmployeeDTO()
            {
                Name = "Shiva",
                MailId = "shiva@gmail.com",
                JobTitle = EmployeeManagement.Models.JobTitle.ProjectEngineer,
                Mission = EmployeeManagement.Models.Mission.GUI,
                ProjectName = "ABC",
                ReportsTo = "Tim"
            };
            var newMockEmp = new GetEmployeeDTO()
            {
                Id = 3,
                Name = newSampleEmp.Name,
                MailId = newSampleEmp.MailId,
                JobTitle = newSampleEmp.JobTitle,
                Mission = newSampleEmp.Mission,
                ProjectName = newSampleEmp.ProjectName,
                ReportsTo = newSampleEmp.ReportsTo
            };
            var expectedResult = new ServiceResponse<List<GetEmployeeDTO>>();
            var sampleData = GetSampleData();
            sampleData.Add(newMockEmp);
            expectedResult.Data = sampleData;

            var mockEmpService = new Mock<IEmployeeManagerService>();
            mockEmpService.Setup(emp => emp.AddEmployee(newSampleEmp)).ReturnsAsync(expectedResult);
            var EmpController = new EmployeeManagerController(mockEmpService.Object);
            //Act
            var actualResult = await EmpController.AddEmployeeAsync(newSampleEmp);
            //Assert
            (actualResult.Result as OkObjectResult).Value.
            Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void AddEmployee_CallPost_Failure_ReturnAllEmployee()
        {
            //Arrange
            var newSampleEmp = new AddEmployeeDTO()
            {
                Name = "Shiva",
                MailId = "shiva@gmail.com",
                JobTitle = EmployeeManagement.Models.JobTitle.ProjectEngineer,
                Mission = EmployeeManagement.Models.Mission.GUI,
                ProjectName = "ABC",
                ReportsTo = "Tim"
            };
            var newMockEmp = new GetEmployeeDTO()
            {
                Id = 3,
                Name = newSampleEmp.Name,
                MailId = newSampleEmp.MailId,
                JobTitle = newSampleEmp.JobTitle,
                Mission = newSampleEmp.Mission,
                ProjectName = newSampleEmp.ProjectName,
                ReportsTo = newSampleEmp.ReportsTo
            };

            var mockResult = new ServiceResponse<List<GetEmployeeDTO>>();
            mockResult.Data = GetSampleData(); ;

            var expectedResult = new ServiceResponse<List<GetEmployeeDTO>>();
            var sampleData = GetSampleData();
            sampleData.Add(newMockEmp);
            expectedResult.Data = sampleData;


            var mockEmpService = new Mock<IEmployeeManagerService>();
            mockEmpService.Setup(emp => emp.AddEmployee(newSampleEmp)).ReturnsAsync(mockResult);
            var EmpController = new EmployeeManagerController(mockEmpService.Object);
            //Act
            var actualResult = await EmpController.AddEmployeeAsync(newSampleEmp);
            //Assert
            (actualResult.Result as OkObjectResult).Value.
            Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void AddEmployee_CallPost_Success_ReturnNotFound()
        {
            //Arrange
            var newSampleEmp = new AddEmployeeDTO()
            {
                Name = "Shiva",
                MailId = "shiva@gmail.com",
                JobTitle = EmployeeManagement.Models.JobTitle.ProjectEngineer,
                Mission = EmployeeManagement.Models.Mission.GUI,
                ProjectName = "ABC",
                ReportsTo = "Tim"
            };

            var expectedResult = new ServiceResponse<List<GetEmployeeDTO>>();
            expectedResult.Data = null;


            var mockEmpService = new Mock<IEmployeeManagerService>();
            mockEmpService.Setup(emp => emp.AddEmployee(newSampleEmp)).ReturnsAsync(expectedResult);
            var EmpController = new EmployeeManagerController(mockEmpService.Object);
            //Act
            var actualResult = await EmpController.AddEmployeeAsync(newSampleEmp);
            //Assert
            (actualResult.Result as OkObjectResult).Value.
            Should().BeEquivalentTo(expectedResult);
        }
        #endregion

        #region Delete
        [Fact]
        public async void DeleteEmployee_CallDelete_Success_ReturnAllEmployee()
        {
            //Arrange
            var expectedResult = new ServiceResponse<List<GetEmployeeDTO>>();
            var sampleData = GetSampleData();
            sampleData.RemoveAt(0);
            expectedResult.Data = sampleData;

            var mockEmpService = new Mock<IEmployeeManagerService>();
            mockEmpService.Setup(emp => emp.DeleteEmployee(0)).ReturnsAsync(expectedResult);
            var EmpController = new EmployeeManagerController(mockEmpService.Object);
            //Act
            var actualResult = await EmpController.DeleteEmployeeAsync(0);
            //Assert
            (actualResult.Result as OkObjectResult).Value.
            Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void DeleteEmployee_CallDelete_Failure_ReturnAllEmployee()
        {
            //Arrange
            var expectedResult = new ServiceResponse<List<GetEmployeeDTO>>();
            var sampleData = GetSampleData();
            sampleData.RemoveAt(0);
            expectedResult.Data = sampleData;

            var mockResult = new ServiceResponse<List<GetEmployeeDTO>>();
            expectedResult.Data = GetSampleData();

            var mockEmpService = new Mock<IEmployeeManagerService>();
            mockEmpService.Setup(emp => emp.DeleteEmployee(0)).ReturnsAsync(mockResult);
            var EmpController = new EmployeeManagerController(mockEmpService.Object);
            //Act
            var actualResult = await EmpController.DeleteEmployeeAsync(0);
            //Assert
            (actualResult.Result as OkObjectResult).Value.
            Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void DeleteEmployee_CallDelete_Success_ReturnNotFound()
        {
            //Arrange
            var expectedResult = new ServiceResponse<List<GetEmployeeDTO>>();
            expectedResult.Data = null;

            var mockEmpService = new Mock<IEmployeeManagerService>();
            mockEmpService.Setup(emp => emp.DeleteEmployee(0)).ReturnsAsync(expectedResult);
            var EmpController = new EmployeeManagerController(mockEmpService.Object);
            //Act
            var actualResult = await EmpController.DeleteEmployeeAsync(0);
            //Assert
            (actualResult.Result as OkObjectResult).Value.
            Should().BeEquivalentTo(expectedResult);
        }
        #endregion

        #region Update
        [Fact]
        public async void UpdateEmployee_CallPut_Success_ReturnUpdatedEmployee()
        {
            //Arrange
            var expectedResult = new ServiceResponse<GetEmployeeDTO>();
            var sampleData = GetSampleData()[0];
            sampleData.JobTitle = JobTitle.ProjectManager;
            sampleData.Mission = Mission.GUI;
            expectedResult.Data = sampleData;

            var updatedEmp = new UpdateEmployeeDTO()
            {
                Name = sampleData.Name,
                MailId = sampleData.MailId,
                JobTitle = sampleData.JobTitle,
                Mission = sampleData.Mission,
                ProjectName = sampleData.ProjectName,
                ReportsTo = sampleData.ReportsTo
            };

            var mockEmpService = new Mock<IEmployeeManagerService>();
            mockEmpService.Setup(emp => emp.UpdateEmployee(0, updatedEmp)).ReturnsAsync(expectedResult);
            var EmpController = new EmployeeManagerController(mockEmpService.Object);
            //Act
            var actualResult = await EmpController.UpdateEmployeeAsync(0, updatedEmp);
            //Assert
            (actualResult.Result as OkObjectResult).Value.
            Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void UpdateEmployee_CallPut_Failure_ReturnEmployeeWithoutUpdate()
        {
            //Arrange
            var expectedEmp = new ServiceResponse<GetEmployeeDTO>();
            var sampleData = GetSampleData()[0];
            sampleData.JobTitle = JobTitle.ProjectManager;
            sampleData.Mission = Mission.GUI;
            expectedEmp.Data = sampleData;

            var updatedEmp = new UpdateEmployeeDTO()
            {
                Name = sampleData.Name,
                MailId = sampleData.MailId,
                JobTitle = sampleData.JobTitle,
                Mission = sampleData.Mission,
                ProjectName = sampleData.ProjectName,
                ReportsTo = sampleData.ReportsTo
            };

            var mockEmp = new ServiceResponse<GetEmployeeDTO>();
            mockEmp.Data = GetSampleData()[0];

            var mockEmpService = new Mock<IEmployeeManagerService>();
            mockEmpService.Setup(emp => emp.UpdateEmployee(0, updatedEmp)).ReturnsAsync(mockEmp);
            var EmpController = new EmployeeManagerController(mockEmpService.Object);
            //Act
            var actualResult = await EmpController.UpdateEmployeeAsync(0, updatedEmp);
            //Assert
            (actualResult.Result as OkObjectResult).Value.
            Should().BeEquivalentTo(expectedEmp);
        }

        [Fact]
        public async void UpdateEmployee_CallPut_Success_ReturnNotFound()
        {
            //Arrange
            var expectedResult = new ServiceResponse<GetEmployeeDTO>();
            expectedResult.Data = null;

            var sampleData = GetSampleData()[0];
            sampleData.JobTitle = JobTitle.ProjectManager;
            sampleData.Mission = Mission.GUI;
            var updatedEmp = new UpdateEmployeeDTO()
            {
                Name = sampleData.Name,
                MailId = sampleData.MailId,
                JobTitle = sampleData.JobTitle,
                Mission = sampleData.Mission,
                ProjectName = sampleData.ProjectName,
                ReportsTo = sampleData.ReportsTo
            };

            var mockEmpService = new Mock<IEmployeeManagerService>();
            mockEmpService.Setup(emp => emp.UpdateEmployee(0, updatedEmp)).ReturnsAsync(expectedResult);
            var EmpController = new EmployeeManagerController(mockEmpService.Object);
            //Act
            var actualResult = await EmpController.UpdateEmployeeAsync(0, updatedEmp);
            //Assert
            (actualResult.Result as NotFoundObjectResult).Value.
            Should().BeEquivalentTo(expectedResult);
        }
        #endregion

        #region HelperMethods
        private List<GetEmployeeDTO> GetSampleData()
        {
            var employees = new List<GetEmployeeDTO>(){
                new GetEmployeeDTO()
                {
                Id = 1,
                Name = "Sree",
                MailId = "sree@gmail.com",
                JobTitle = EmployeeManagement.Models.JobTitle.ProjectLead,
                Mission = EmployeeManagement.Models.Mission.SCV,
                ProjectName = "XYZ",
                ReportsTo = "Jack"
                },
                new GetEmployeeDTO()
                {
                Id = 2,
                Name = "Karthik",
                MailId = "karthik@gmail.com",
                JobTitle = EmployeeManagement.Models.JobTitle.ProjectManager,
                Mission = EmployeeManagement.Models.Mission.D2T,
                ProjectName = "ABC",
                ReportsTo = "Jill"
                }
            };

            return employees;
        }
        #endregion

    }
}