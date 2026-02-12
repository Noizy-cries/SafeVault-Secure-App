using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace SafeVault.SecurityAudit
{
    public class AuditTool
    {
        public static void ScanForVulnerabilities(string projectPath)
        {
            var files = Directory.GetFiles(projectPath, "*.cs", SearchOption.AllDirectories);
            
            foreach (var file in files)
            {
                var content = File.ReadAllText(file);
                
                // Check for SQL injection
                if (Regex.IsMatch(content, @"ExecuteSqlRaw.*\+", RegexOptions.IgnoreCase))
                    Console.WriteLine($"⚠️ SQL Injection risk: {file}");
                
                // Check for XSS
                if (Regex.IsMatch(content, @"Html\.Raw", RegexOptions.IgnoreCase))
                    Console.WriteLine($"⚠️ XSS risk: {file}");
                
                // Check for hardcoded secrets
                if (Regex.IsMatch(content, @"password.*=.*[""'].+[""']", RegexOptions.IgnoreCase))
                    Console.WriteLine($"⚠️ Hardcoded secret: {file}");
            }
        }
    }
}