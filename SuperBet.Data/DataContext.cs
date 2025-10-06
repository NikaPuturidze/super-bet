using SuperBet.Core.Models;
using System.IO;
using System.Text.Json;
using System.Transactions;

namespace SuperBet.Data
{
    public class DataContext
    {
        private readonly string UsersFile = "users.json";
        private readonly string TransactionsFile = "transactions.json";
        public List<User> Users { get; private set; } = [];
        public List<UserTransaction> Transactions { get; private set; } = [];

        private static T? LoadFile<T>(string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    File.WriteAllText(path, "[]");
                    return default;
                }

                var json = File.ReadAllText(path);
                return JsonSerializer.Deserialize<T>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] Failed to load {path}: {ex.Message}");
                return default;
            }
        }

        public void LoadData()
        {
            Users = LoadFile<List<User>>(UsersFile) ?? [];
            Transactions = LoadFile<List<UserTransaction>>(TransactionsFile) ?? new();
        }

        public void SaveData()
        {
            File.WriteAllText(UsersFile, JsonSerializer.Serialize(Users));
            File.WriteAllText(TransactionsFile, JsonSerializer.Serialize(Transactions));
        }
    }
}
