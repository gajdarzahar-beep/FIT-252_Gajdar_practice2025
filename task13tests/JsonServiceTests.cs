using Xunit;
using task13;
using System;
using System.IO;
using System.Collections.Generic;

namespace task13tests;

public class JsonServiceTests
{
    [Fact]
    public void Serialize_ValidStudent_ReturnsJson()
    {
        var student = new Student
        {
            FirstName = "Иван",
            LastName = "Иванов",
            BirthDate = new DateTime(2000, 1, 1),
            Grades = new List<Subject>
        {
            new Subject { Name = "Математика", Grade = 90 },
            new Subject { Name = "Физика", Grade = 85 }
        }
        };

        var json = JsonService.Serialize(student);

        Assert.Contains("\"firstName\": \"Иван\"", json);
        Assert.Contains("\"lastName\": \"Иванов\"", json);
        Assert.Contains("\"birthDate\": \"01.01.2000\"", json);
        Assert.Contains("\"name\": \"Математика\"", json);
        Assert.Contains("\"grade\": 90", json);
        Assert.Contains("\"name\": \"Физика\"", json);
        Assert.Contains("\"grade\": 85", json);
    }

    [Fact]
    public void Deserialize_ValidJson_ReturnsStudent()
    {
        var json = @"
        {
            ""firstName"": ""Иван"",
            ""lastName"": ""Иванов"",
            ""birthDate"": ""01.01.2000"",
            ""grades"": [
                { ""name"": ""Математика"", ""grade"": 95 },
                { ""name"": ""Физика"", ""grade"": 88 }
            ]
        }";

        var student = JsonService.Deserialize(json);

        Assert.Equal("Иван", student.FirstName);
        Assert.Equal("Иванов", student.LastName);
        Assert.Equal(2000, student.BirthDate.Year);
        Assert.Equal(1, student.BirthDate.Month);
        Assert.Equal(1, student.BirthDate.Day);
        Assert.Equal(2, student.Grades.Count);
        Assert.Equal("Математика", student.Grades[0].Name);
        Assert.Equal(95, student.Grades[0].Grade);
        Assert.Equal("Физика", student.Grades[1].Name);
        Assert.Equal(88, student.Grades[1].Grade);
    }

    [Fact]
    public void SaveToFile_And_LoadFromFile_Works()
    {
        var student = new Student
        {
            FirstName = "Иван",
            LastName = "Иванов",
            BirthDate = new DateTime(2000, 1, 1),
            Grades = new List<Subject>
            {
                new Subject { Name = "Химия", Grade = 78 }
            }
        };

        var tempFile = Path.GetTempFileName();

        JsonService.SaveToFile(student, tempFile);
        var loadedStudent = JsonService.LoadFromFile(tempFile);

        Assert.Equal(student.FirstName, loadedStudent.FirstName);
        Assert.Equal(student.LastName, loadedStudent.LastName);
        Assert.Equal(student.BirthDate, loadedStudent.BirthDate);
        Assert.Equal(student.Grades.Count, loadedStudent.Grades.Count);
        Assert.Equal(student.Grades[0].Name, loadedStudent.Grades[0].Name);
        Assert.Equal(student.Grades[0].Grade, loadedStudent.Grades[0].Grade);

        File.Delete(tempFile);
    }

    [Fact]
    public void Deserialize_InvalidJson_ThrowsException()
    {
        var invalidJson = @"{ ""firstName"": ""Иван"" }";

        Assert.Throws<ArgumentException>(() => JsonService.Deserialize(invalidJson));
    }

    [Fact]
    public void LoadFromFile_FileNotFound_ThrowsException()
    {
        var nonExistentFile = @"C:\non_existent_file_12345.json";
        Assert.Throws<FileNotFoundException>(() => JsonService.LoadFromFile(nonExistentFile));
    }
}