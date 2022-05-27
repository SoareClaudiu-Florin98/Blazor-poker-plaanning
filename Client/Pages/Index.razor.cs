using System;
using Microsoft.AspNetCore.Components;

namespace BlazorPokerPlanning.Client.Pages
{
    public partial class Index
    {
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        private void CreateRoom()
        {
            NavigationManager.NavigateTo($"/room/{Guid.NewGuid()}?dark");
        }
    }
}
