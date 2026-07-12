using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace task13;

public static class JsonService
{
    private static readonly JsonSerializerOptions Options = new JsonSerializerOptions
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Converters = { new CustomDateTimeConverter() },
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    public static string Serialize(Student student)
    {
        if (student == null)
        {
            throw new ArgumentNullException(nameof(student));
        }

        return JsonSerializer.Serialize(student, Options);
    }

    public static Student Deserialize(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            throw new ArgumentException("JSON cannot be empty", nameof(json));
        }

        var student = JsonSerializer.Deserialize<Student>(json, Options);

        if (student == null)
        {
            throw new InvalidOperationException("Failed to deserialize student");
        }

        ValidateStudent(student);
        return student;
    }

    public static void SaveToFile(Student student, string filePath)
    {
        if (student == null)
        {
            throw new ArgumentNullException(nameof(student));
        }

        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw new ArgumentException("File path cannot be empty", nameof(filePath));
        }

        var json = Serialize(student);
        File.WriteAllText(filePath, json);
    }

    public static Student LoadFromFile(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw new ArgumentException("File path cannot be empty", nameof(filePath));
        }

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"File not found: {filePath}");
        }

        var json = File.ReadAllText(filePath);
        return Deserialize(json);
    }

    private static void ValidateStudent(Student student)
    {
        if (string.IsNullOrWhiteSpace(student.FirstName))
        {
            throw new ArgumentException("FirstName is required");
        }

        if (string.IsNullOrWhiteSpace(student.LastName))
        {
            throw new ArgumentException("LastName is required");
        }

        if (student.BirthDate == default || student.BirthDate > DateTime.Now)
        {
            throw new ArgumentException("BirthDate is invalid");
        }

        if (student.Grades == null)
        {
            throw new ArgumentException("Grades cannot be null");
        }

        foreach (var subject in student.Grades)
        {
            if (string.IsNullOrWhiteSpace(subject.Name))
            {
                throw new ArgumentException("Subject name is required");
            }

            if (subject.Grade < 0 || subject.Grade > 100)
            {
                throw new ArgumentException($"Grade for '{subject.Name}' must be between 0 and 100");
            }
        }
    }

    public class CustomDateTimeConverter : JsonConverter<DateTime>
    {
        private const string Format = "dd.MM.yyyy";

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateTime.ParseExact(reader.GetString(), Format, null);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(Format));
        }
    }
}