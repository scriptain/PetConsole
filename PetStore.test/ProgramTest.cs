using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using PetStoreConsole;
using Moq;
using Moq.Protected;
namespace PetStore.test

{
    public class ProgramTest
    {

        [Fact]
        public async Task GetAllPetsByCategoryReversed_ValidResponse_ReturnsPetsOrderedByCategory()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(MockPetData.GetUnorderdPets().ToString())
                });
            var httpClient = new HttpClient(mockHttpMessageHandler.Object);

            // Act
            var result = await PetStoreConsole.DevTestProgram.GetAllPetsByCategoryReversed(httpClient);

            Console.WriteLine(result);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Length);
            Assert.Equal("Category1", result[0].category?.name);
            Assert.Equal("Pet3", result[0].name);
            Assert.Equal("Category1", result[1].category?.name);
            Assert.Equal("Pet1", result[1].name);
            Assert.Equal("Category2", result[2].category?.name);
            Assert.Equal("Pet2", result[2].name);
        }

        public class Data
        {
            // using this class to store types.
            // {"id":10002,"category":{"id":1001,"name":"dog"},"name":"gammi","photoUrls":["string"],"tags":[{"id":0,"name":"string"}],"status":"available"}
            public record Category(Int64 id, string name);
            public record Tag(Int64 id, string name);

        }
        public class Pet
        {
            public Int64 id;
            public Data.Category? category;
            public string? name;
            public string[]? photoUrls;
            public Data.Tag[]? tags;
            public string? status;

            public string? GetName()
            {
                return name;
            }

            // Override ToString() to display pet details
            // Override means to redefine the behaviour of a method
            // In this case the old ToString() was logging: PetStoreConsole.Pet which is the 'fully qualified name' of the class
            // it will now return the property names and their respective values for each pet
            public override string ToString()
            {
                string categoryString = category != null ? category.name : "No Category";
                string tagsString = tags != null ? string.Join(", ", tags.Select(tag => tag.name)) : "No Tags";
                return $"ID: {id}, Name: {name}, Category: {categoryString}, Status: {status}, Tags: {tagsString}";
            }
        }

        public class MockPetData()
        {
            // Define a method to generate sample pets
            public static Pet[] GetUnorderdPets()
            {
                Pet[] pets = new Pet[]
                {
                new Pet
                {
                    id = 1,
                    category = new Data.Category(1, "Dog"),
                    name = "Buddy",
                    photoUrls = new string[] { "url1", "url2" },
                    tags = new Data.Tag[] { new Data.Tag(101, "Friendly"), new Data.Tag(102, "Active") },
                    status = "available"
                },
                new Pet
                {
                    id = 2,
                    category = new Data.Category(2, "Cat"),
                    name = "Whiskers",
                    photoUrls = new string[] { "url3", "url4" },
                    tags = new Data.Tag[] { new Data.Tag(103, "Playful"), new Data.Tag(104, "Independent") },
                    status = "available"
                },
                new Pet
                {
                    id = 3,
                    category = new Data.Category(1, "Dog"),
                    name = "Max",
                    photoUrls = new string[] { "url5", "url6" },
                    tags = new Data.Tag[] { new Data.Tag(105, "Loyal"), new Data.Tag(106, "Energetic") },
                    status = "available"
                },
                new Pet
                {
                    id = 4,
                    category = new Data.Category(3, "Bird"),
                    name = "Polly",
                    photoUrls = new string[] { "url7", "url8" },
                    tags = new Data.Tag[] { new Data.Tag(107, "Talkative"), new Data.Tag(108, "Colorful") },
                    status = "available"
                },
                new Pet
                {
                    id = 5,
                    category = new Data.Category(2, "Cat"),
                    name = "Mittens",
                    photoUrls = new string[] { "url9", "url10" },
                    tags = new Data.Tag[] { new Data.Tag(109, "Cuddly"), new Data.Tag(110, "Curious") },
                    status = "available"
                }

                };
                return pets;
            }
            public static Pet[] GetOrderedPets()
            {
                Pet[] pets = new Pet[]
                {
                new Pet
                {
                    id = 1,
                    category = new Data.Category(1, "Cat"),
                    name = "Whiskers",
                    photoUrls = new string[] { "url1", "url2" },
                    tags = new Data.Tag[] { new Data.Tag(101, "Friendly"), new Data.Tag(102, "Active") },
                    status = "available"
                },
                new Pet
                {
                    id = 2,
                    category = new Data.Category(1, "Cat"),
                    name = "Buddy",
                    photoUrls = new string[] { "url3", "url4" },
                    tags = new Data.Tag[] { new Data.Tag(103, "Playful"), new Data.Tag(104, "Independent") },
                    status = "available"
                },
                new Pet
                {
                    id = 3,
                    category = new Data.Category(2, "Dog"),
                    name = "Polly",
                    photoUrls = new string[] { "url5", "url6" },
                    tags = new Data.Tag[] { new Data.Tag(105, "Loyal"), new Data.Tag(106, "Energetic") },
                    status = "available"
                },
                new Pet
                {
                    id = 4,
                    category = new Data.Category(2, "Dog"),
                    name = "Max",
                    photoUrls = new string[] { "url7", "url8" },
                    tags = new Data.Tag[] { new Data.Tag(107, "Talkative"), new Data.Tag(108, "Colorful") },
                    status = "available"
                },
                new Pet
                {
                    id = 5,
                    category = new Data.Category(3, "Snake"),
                    name = "Mittens",
                    photoUrls = new string[] { "url9", "url10" },
                    tags = new Data.Tag[] { new Data.Tag(109, "Cuddly"), new Data.Tag(110, "Curious") },
                    status = "available"
                }

                };
                return pets;
            }

        }
    }

}