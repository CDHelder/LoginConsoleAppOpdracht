using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace LoginConsoleAppOpdracht
{
    public class Program
    {
        private static List<Tuple<string, string>> UserNamesAndPasswords;
        private static string PasswordSalt;
        private static string UsernameSalt;

        static void Main(string[] args)
        {
            PasswordSalt = GenerateSalt();
            UsernameSalt = GenerateSalt();
            UserNamesAndPasswords = new();

            Console.WriteLine("Welcome, please register");
            Console.WriteLine("");
            Console.WriteLine("Enter a username:");
            var username = Console.ReadLine();
            Console.WriteLine("");
            Console.WriteLine("Enter a password:");
            var password = Console.ReadLine();
            Console.WriteLine("");
            CreateUser(username, password);
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            foreach (var user in UserNamesAndPasswords)
            {
                Console.WriteLine($"Username: {user.Item1}, Password: {user.Item2}");
            }
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine("");

            Console.WriteLine("Login using a username");
            var enteredUsername = Console.ReadLine();
            Console.WriteLine("");
            Console.WriteLine("Please enter the password");
            var enteredPassword = Console.ReadLine();

            Console.WriteLine("");

            var loginSucces = ConfirmUserAndPassword(enteredUsername, enteredPassword);
            Console.WriteLine($"Login succesfull is {loginSucces}");
        }

        private static bool ConfirmUserAndPassword(string enteredUsername, string enteredPassword)
        {
            var hashedPassword = ComputeHash(Encoding.UTF8.GetBytes(enteredPassword), Encoding.UTF8.GetBytes(PasswordSalt));
            var hashedUsername = ComputeHash(Encoding.UTF8.GetBytes(enteredUsername), Encoding.UTF8.GetBytes(UsernameSalt));

            var user = UserNamesAndPasswords.Find(a => a.Item1 == hashedUsername && a.Item2 == hashedPassword);

            Console.WriteLine($"Username: {hashedUsername}, Password: {hashedPassword}");
            Console.WriteLine("");

            if (user == null)
            {
                return false;
            }

            return true;
        }

        private static void CreateUser(string username, string password)
        {
            var hashedPassword = ComputeHash(Encoding.UTF8.GetBytes(password), Encoding.UTF8.GetBytes(PasswordSalt));
            var hashedUsername = ComputeHash(Encoding.UTF8.GetBytes(username), Encoding.UTF8.GetBytes(UsernameSalt));
            UserNamesAndPasswords.Add(new Tuple<string, string>(hashedUsername, hashedPassword));
        }

        private static string ComputeHash(byte[] password, byte[] salt)
        {
            var byteResult = new Rfc2898DeriveBytes(password, salt, 10000);
            return Convert.ToBase64String(byteResult.GetBytes(24));
        }

        public static string GenerateSalt()
        {
            var bytes = new byte[128 / 8];
            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }
    }
}
