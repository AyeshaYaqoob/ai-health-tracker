using System;
using BCrypt.Net;
var passwords = new (string pwd, string label)[] { ("Ayesha28!", "ayesha"), ("Nimra28!", "nimra"), ("Ahmad28!", "ahmad") };
foreach (var (pwd, label) in passwords)
    Console.WriteLine($"{label}: {BCrypt.Net.BCrypt.HashPassword(pwd)}");
