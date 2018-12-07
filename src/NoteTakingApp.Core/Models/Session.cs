using NoteTakingApp.Core.Common;
using System;

namespace NoteTakingApp.Core.Models
{
    public class Session
    {
        public int SessionId { get; set; }
        public string AccessToken { get; set; }        
        public string Username { get; set; }
        public SessionStatus SessionStatus { get; set; } = SessionStatus.None;
    }

    public enum SessionStatus
    {
        None,
        LoggedIn,        
        Connected,
        Disconnected,
        Invalid
    }
}
