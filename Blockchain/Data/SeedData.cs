using Blockchain.Models;
using SimpleHashing;

namespace Blockchain.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<BlockchainContext>();

            // Look for users.
            if(context.Users.Any())
                return; // DB has already been seeded.
            context.Users.AddRange(
                new Seller { UserID = 1, Name = "Bob" },
                new Buyer { UserID = 2, Name = "Old Greg", Address = "123 fake street", AnnualIncome = 12345, 
                    ContactNumber = "12345678", DOB="12/09/1990", Employer ="Big Al" },
                new Authority { UserID = 3, Name = "Jeff Winger", Title = "Authority" },
                new Bank { UserID = 4, Name = "Tara Loft", Title = "Banker" }
                );
            context.Logins.AddRange(
                new Login { LoginID = 1, UserID = 1, PasswordHash = PBKDF2.Hash("abc") },
                new Login { LoginID = 2, UserID = 2, PasswordHash = PBKDF2.Hash("abc") },
                new Login { LoginID = 3, UserID = 3, PasswordHash = PBKDF2.Hash("abc") },
                new Login { LoginID = 4, UserID = 4, PasswordHash = PBKDF2.Hash("abc") }
                );
            //default blockchain
     /*       context.Blockchain.AddRange(
                new BlockChain(2)
                );*/

            context.SaveChanges();
        }
    }
}
