using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorPokerPlanning.Client.Shared
{
    public partial class  MainLayout
    {
        [Inject]
        private NavigationManager NavigationManager { get; set; }
        public string IsInARoomClass { get { return   GetRoomClass(); }}


        private  string GetRoomClass() {
            var roomUrl = NavigationManager.Uri;
            if (roomUrl.Contains("/room/" ) && !roomUrl.Contains("dark"))
            {
                return "room-wrapper";
            }

            return ""; 
        }


    }
}
