using System;
using System.Linq;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using BlazorPokerPlanning.Client.Services;
using BlazorPokerPlanning.Shared.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorPokerPlanning.Client.Pages
{
    public partial class Room : IAsyncDisposable
    {

        [Inject]
        private IHubService HubService { get; set; }

        [Inject]
        private IJSRuntime IJSRuntime { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        private ILocalStorageService LocalStore { get; set; } 

        [Parameter]
        public string RoomId { get; set; }

        public string VisibilityVoteOptions => State.User.IsAVoter ? "visible-vote-options": "hidden-vote-options";

        public string HiddenVotes => State.User.IsAVoter ? "visible-votes" : "hidden-votes";

        public bool IsDarkMode { get; set; }



        public string GetVotingStatus => State.User.IsAVoter ? "I'm not a voter!" : "I'm a voter!";

        private RoomState State { get; } = new() {User = {IsAVoter = true, Voted = false},
        };

        protected override Task OnInitializedAsync()
        {
            IsDarkMode = false;
            if (NavigationManager.Uri.Contains("?dark"))
            {
                IsDarkMode = true;

            }
            else
            {
                IsDarkMode = false;
            }
            return HubService.InitializeAsync(RoomId, StateHasChanged, State);
        }

        public bool IsConnected => HubService.IsConnected(RoomId);

        public bool CanManageRoom => State.RoomCreatedBy == State.User.Id;

        public bool CanChangePack => CanManageRoom && State.Votes.All(x => string.IsNullOrWhiteSpace(x.Value));

        public bool urlCopiedToClipboard = false;

        public string GetVoteValue(Vote vote)
        {
            bool votesAreVisible = State.State == BlazorPokerPlanning.Shared.Models.RoomState.VotesVisible;
            bool isMyVote = vote.UserId == State.User.Id;

            return votesAreVisible || !State.User.IsAVoter || isMyVote ? vote.Value : "X";
        }

        public Task Reset()
        {
            return HubService.ResetVotes(RoomId);
        }

        public Task Reveal()
        {
            return HubService.RevealVotes(RoomId);
        }

        public Task Vote(string vote)
        {
            if (!State.User.Voted)
            {
                HubService.ChangeVotedStatus(true);
                return HubService.Send(RoomId, vote);
            }
            return Task.CompletedTask; 
        }

        public Task ChangeEstimationPack(ChangeEventArgs e)
        {
            var estimationPackId = int.Parse(e.Value.ToString());

            State.SelectedEstimationPack = State.EstimationPacks.SingleOrDefault(x => x.Id == estimationPackId);
            return HubService.ChangeRoomEstimationPack(RoomId, estimationPackId);
        }

        public Task ChangeIsAVoterStatus()
        {
            HubService.ChangeVotedStatus(false);
            return HubService.ChangeIsAVoterStatus(RoomId);
        }


        public async Task CopyLink()
        {
            var roomUrl = NavigationManager.Uri;

            await IJSRuntime.InvokeVoidAsync("navigator.clipboard.writeText", roomUrl);

            urlCopiedToClipboard = true;
            StateHasChanged();
            await Task.Delay(3000);
            CloseNotification(); 
            StateHasChanged();
        }

        public void CloseNotification()
        {
             urlCopiedToClipboard = false;
        }
        public  void ActivateDarkModeOrLightMode()
        {

            if (NavigationManager.Uri.Contains("?dark"))
            {
                NavigationManager.NavigateTo($"{NavigationManager.Uri.Replace("?dark","")}");
                IsDarkMode = false;

            }
            else
            {
                NavigationManager.NavigateTo($"{NavigationManager.Uri}?dark");
                IsDarkMode = true;
            }
            StateHasChanged();
        }

        public ValueTask DisposeAsync()
        {
            return HubService.DisposeAsync(RoomId);
        }
    }
}
