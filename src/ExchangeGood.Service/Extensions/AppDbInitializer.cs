using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;
using ExchangeGood.DAO;
using ExchangeGood.Data.Models;
using ExchangeGood.Repository.Builders;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeGood.Service.Extensions
{
    public class AppDbInitializer
    {
        public static async Task Seed(IApplicationBuilder applicationBuilder)
        {
            //get json string
            var memberData = await File.ReadAllTextAsync(  "SeedData/Member.json");
            var categoryData = await File.ReadAllTextAsync("SeedData/Category.json");
            var productData = await File.ReadAllTextAsync("SeedData/Product.json");
            var roleData = await File.ReadAllTextAsync("SeedData/Role.json");
            var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            // deserialize into list obj
            var categories = JsonSerializer.Deserialize<List<Category>>(categoryData, options);
            var roles = JsonSerializer.Deserialize<List<Role>>(roleData, options);
            var members = JsonSerializer.Deserialize<List<Member>>(memberData, options); // let JSON into C-sharp object
            var products = JsonSerializer.Deserialize<List<Product>>(productData, options);


            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<GoodsExchangeContext>();
                context.Database.EnsureCreated();

                if (!context.Roles.Any())
                {
                    foreach (var role in roles)
                    {
                        context.Roles.Add(role);
                    }
                }

                if (!context.Categories.Any())
                {
                    foreach (var category in categories)
                    {
                        context.Categories.Add(category);
                    }
                }
                await context.SaveChangesAsync();

                if (!context.Members.Any())
                {
                    //set up member
                    foreach (var member in members)
                    {
                        var result = MemberBuilder.Empty()
                            .FeId(member.FeId)
                            .RoleId(member.RoleId)
                            .UserName(member.UserName)
                            .PassWord("123456")
                            .Address(member.Address)
                            .Gender(member.Gender)
                            .Email(member.Email)
                            .Phone(member.Phone)
                            .CreatedTime(DateTime.UtcNow)
                            .UpdatedTime(DateTime.UtcNow)
                            .Dob(member.Dob)
                            .Create();
                        context.Members.Add(result);
                    }
                }

                if (!context.Products.Any())
                {
                    foreach (var product in products)
                    {
                        context.Products.Add(product);
                    }
                }
                await context.SaveChangesAsync();

            }
        }
    }
}