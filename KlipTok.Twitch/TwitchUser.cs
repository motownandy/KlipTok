using System;

namespace KlipTok.Twitch {

  public class TwitchUser {

    public string AccessToken { get; set; } = string.Empty;

    public string RefreshToken { get; set; } = string.Empty;

    public int ExpiresIn { 
      get { return (int)(ExpiresAt-DateTime.Now).TotalSeconds; }
      set { ExpiresAt = DateTime.Now.AddSeconds(value); }
    }

    public DateTime ExpiresAt { get; set; }

    public Uri PictureUri  { get; set; }

    public string TwitchId { get; set; }

    public string DisplayName { get; set; }

    public void Updated() {
      NotifyStateChanged();
    }

    public event Action OnChange;

    private void NotifyStateChanged() => OnChange?.Invoke(); 

  }

}