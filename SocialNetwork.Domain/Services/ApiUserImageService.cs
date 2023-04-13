using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SocialNetwork.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Domain.Services
{
    public class ApiUserImageService
    {
        const string apiurl = "https://localhost:7289";
        public async Task<List<UserImage>> GetAllByUserId(Guid userId)
        {
            List<UserImage> userImagesList = new List<UserImage>();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{apiurl}/api/UserImages/UserDetails/{userId}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();

                    userImagesList = JsonConvert.DeserializeObject<List<UserImage>>(apiResponse);
                }
            }
            return userImagesList;
        }

        public async Task<UserImage> GetByImageId(Guid ImageId)
        {
            UserImage userImage = new UserImage();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{apiurl}/api/UserImages/{ImageId}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();

                    userImage = JsonConvert.DeserializeObject<UserImage>(apiResponse);
                }
            }
            return userImage;
        }

        public async Task<UserImage> AddUserImage(UserImage userImage)
        {
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(userImage), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PostAsync($"{apiurl}/api/UserImages", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    userImage = JsonConvert.DeserializeObject<UserImage>(apiResponse);
                }
            }
            return userImage;
        }

        public async Task DeleteUserImage(Guid ImageId)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync($"{apiurl}/api/UserImages/{ImageId}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"{apiResponse}");
                }
            }
        }

        public async Task<bool> UpdateUserImage(UserImage userImage)
        {
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(userImage), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync($"{apiurl}/api/UserImages/{userImage.ImageId}", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var returnedUserImage = JsonConvert.DeserializeObject<UserImage>(apiResponse);
                }
            }
            return true;
        }

    }  
}
