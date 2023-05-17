using Client.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic; 
using System.Linq;
using System.Threading.Tasks;


namespace Client.Pages
{
    public partial class Ordershop
    {
        private List<Order> orders =  new();
        [Inject] private HttpClient httpClient {get;set;}
        [Inject] private IConfiguration Config { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var result = await httpClient.GetAsync(Config["apiUri"] + "/api/Customer/GetAll");
            if (result.IsSuccessStatusCode)
            {
                orders = await result.Content.ReadFromJsonAsync<List<Order>>();
            }
            }
    }
}
