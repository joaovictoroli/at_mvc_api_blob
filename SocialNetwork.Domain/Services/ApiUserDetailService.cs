using SocialNetwork.Domain.Entities;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Text;

namespace SocialNetwork.Domain.Services
{
    public class ApiUserDetailService
    {
        const string apiurl = "https://localhost:7289";

        public async Task<List<UserDetail>> GetAll()
        {           

            List<UserDetail> userDetailsList = new List<UserDetail>();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{apiurl}/api/UserDetails"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();

                    userDetailsList = JsonConvert.DeserializeObject<List<UserDetail>>(apiResponse);
                }
            }
            return userDetailsList;
        }

        public async Task<UserDetail> GetById(Guid id)
        {
            UserDetail userDetail = new UserDetail();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{apiurl}/api/UserDetails/{id}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();

                    userDetail = JsonConvert.DeserializeObject<UserDetail>(apiResponse);
                }
            }
            return userDetail;
        }

        public async Task<UserDetail> AddUserDetail(UserDetail userDetail)
        {
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(userDetail), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PostAsync($"{apiurl}/api/UserDetails", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    userDetail = JsonConvert.DeserializeObject<UserDetail>(apiResponse);
                }
            }
            return userDetail;
        }

        public async Task<bool> UpdateUserDetails(UserDetail userDetails)
        {
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(userDetails), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync($"{apiurl}/api/UserDetails/{userDetails.UserId}", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var userDetail = JsonConvert.DeserializeObject<UserDetail>(apiResponse);
                }
            }
            return true;
        }

        public async Task DeleteUserDetails(Guid UserId)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync($"{apiurl}/api/UserDetails/{UserId}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"{apiResponse}");
                }
            }
        }
    }
}
