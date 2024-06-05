using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace PetStoreConsole
{
    public class DevTestProgram
    {
        static void Main(string[] args)
        {

            // setup http client.
            // 'using' manages cleanup associated with object once code within {} is done
            // http client is part of the standard library that is used to make request
            using (HttpClient httpClient = new HttpClient()) {
                while (true)
                {
                    // prompt user for input
                    Console.WriteLine("Press A to: view all available pets ");
                    Console.WriteLine("Press B to: view all pets by category in reverse order");

                    // capture input
                    var input = Console.ReadLine();

                    // process input
                    switch (input.ToLower())
                    {
                        case "a":
                            GetAllAvailablePets(httpClient);
                            break;
                        case "b":
                            GetAllPetsByCategoryReversed(httpClient);
                            break;
                        default:
                            Console.WriteLine("Input not recognised. Please enter A or B");
                            break;
                    }

                    // get all available pets
                    static async Task<Pet[]> GetAllAvailablePets(HttpClient httpClient)
                    {
                        // api endpoint for all available pets
                        string apiUrl = "https://petstore.swagger.io/v2/pet/findByStatus?status=available";
                        // await GET request from endpoint. method execution paused.
                        HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
                        // ensure that the GET call succeeded. Throw exception if it doesn't
                        response.EnsureSuccessStatusCode();
                        // turn http response into a string asynchronously 
                        string responseBody = await response.Content.ReadAsStringAsync();
                        // data within responseBody is JSON. Need to turn each entry into a Pet[] object
                        Pet[] pets = JsonConvert.DeserializeObject<Pet[]>(responseBody);
                        // return empty array if no pets are loaded
                        if (pets == null || pets.Length == 0)
                        {
                            return Array.Empty<Pet>(); // Return empty array if no pets provided
                        }
                        // create a new string by turning each pet entry into a string and joining it with a new line
                        string allPetData = string.Join("\n", pets.Select(p => p.ToString()));
                        Console.WriteLine(allPetData);
                        return pets;
                    }

                }
            }
        }

        // get all available pets
        public static async Task<Pet[]> GetAllPetsByCategoryReversed(HttpClient httpClient)
        {
            // api endpoint for all available pets
            string apiUrl = "https://petstore.swagger.io/v2/pet/findByStatus?status=available";
            // await GET request from endpoint. method execution paused.
            HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
            // ensure that the GET call succeeded. Throw exception if it doesn't
            response.EnsureSuccessStatusCode();
            // turn http response into a string asynchronously 
            string responseBody = await response.Content.ReadAsStringAsync();
            // data within responseBody is JSON. Need to turn each entry into a Pet[] object
            Pet[] pets = JsonConvert.DeserializeObject<Pet[]>(responseBody);
            // if there are no pets, return empty array
            if (pets == null || pets.Length == 0)
            {
                return Array.Empty<Pet>(); // Return empty array if no pets provided
            }
            // arrange the pets by category
            Pet[] petsOrderedByCategory = pets.OrderBy(p => p.category?.name ?? string.Empty).ThenByDescending(p => p.name).ToArray();
            // create a new string by turning each pet entry into a string and joining it with a new line
            string newPets = string.Join("\n", pets.Select(p => p.ToString()));
            foreach (Pet p in petsOrderedByCategory)
            {
                if (p.category != null)
                {
                    Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~");
                    Console.WriteLine($"category: {p.category.name}");
                    Console.WriteLine($"name: {p.GetName()}");
                    Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~");

                }
            }
            return pets;
        }
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
}


